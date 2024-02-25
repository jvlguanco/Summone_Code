using System.Collections;
using System.Globalization;
using System.Text;
using Core.Library.RE;

namespace Core.Library;

internal class TokenRegExpParser {
    private string pattern;

    private bool ignoreCase;

    private int pos;

    internal NFAState start = new NFAState();

    internal NFAState end = null;

    private int stateCount = 0;

    private int transitionCount = 0;

    private int epsilonCount = 0;

    public TokenRegExpParser(string pattern) : this(pattern, false) {
    }

    public TokenRegExpParser(string pattern, bool ignoreCase) {
        this.pattern = pattern;
        this.ignoreCase = ignoreCase;
        this.pos = 0;
        this.end = ParseExpr(start);
        if (pos < pattern.Length) {
            throw new RegExpException(
                RegExpException.ErrorType.UNEXPECTED_CHARACTER,
                pos,
                pattern);
        }
    }

    public string GetDebugInfo() {
        if (stateCount == 0) {
            UpdateStats(start, new Hashtable());
        }
        return stateCount + " states, " +
               transitionCount + " transitions, " +
               epsilonCount + " epsilons";
    }

    private void UpdateStats(NFAState state, Hashtable visited) {
        if (!visited.ContainsKey(state)) {
            visited.Add(state, state);
            stateCount++;
            for (int i = 0; i < state.outgoing.Length; i++) {
                transitionCount++;
                if (state.outgoing[i] is NFAEpsilonTransition) {
                    epsilonCount++;
                }
                UpdateStats(state.outgoing[i].state, visited);
            }
        }
    }

    private NFAState ParseExpr(NFAState start) {
        NFAState  end = new NFAState();
        NFAState  subStart;
        NFAState  subEnd;

        do {
            if (PeekChar(0) == '|') {
                ReadChar('|');
            }
            subStart = new NFAState();
            subEnd = ParseTerm(subStart);
            if (subStart.incoming.Length == 0) {
                subStart.MergeInto(start);
            } else {
                start.AddOut(new NFAEpsilonTransition(subStart));
            }
            if (subEnd.outgoing.Length == 0 ||
                (!end.HasTransitions() && PeekChar(0) != '|')) {
                subEnd.MergeInto(end);
            } else {
                subEnd.AddOut(new NFAEpsilonTransition(end));
            }
        } while (PeekChar(0) == '|');
        return end;
    }

    private NFAState ParseTerm(NFAState start) {
        NFAState  end;

        end = ParseFact(start);
        while (true) {
            switch (PeekChar(0)) {
            case -1:
            case ')':
            case ']':
            case '{':
            case '}':
            case '?':
            case '+':
            case '|':
                return end;
            default:
                end = ParseFact(end);
                break;
            }
        }
    }

    private NFAState ParseFact(NFAState start) {
        NFAState  placeholder = new NFAState();
        NFAState  end;

        end = ParseAtom(placeholder);
        switch (PeekChar(0)) {
        case '?':
        case '*':
        case '+':
        case '{':
            end = ParseAtomModifier(placeholder, end);
            break;
        }
        if (placeholder.incoming.Length > 0 && start.outgoing.Length > 0) {
            start.AddOut(new NFAEpsilonTransition(placeholder));
            return end;
        } else {
            placeholder.MergeInto(start);
            return (end == placeholder) ? start : end;
        }
    }

    private NFAState ParseAtom(NFAState start) {
        NFAState  end;

        switch (PeekChar(0)) {
        case '.':
            ReadChar('.');
            return start.AddOut(new NFADotTransition(new NFAState()));
        case '(':
            ReadChar('(');
            end = ParseExpr(start);
            ReadChar(')');
            return end;
        case '[':
            ReadChar('[');
            end = ParseCharSet(start);
            ReadChar(']');
            return end;
        case -1:
        case ')':
        case ']':
        case '{':
        case '}':
        case '?':
        case '*':
        case '+':
        case '|':
            throw new RegExpException(
                RegExpException.ErrorType.UNEXPECTED_CHARACTER,
                pos,
                pattern);
        default:
            return ParseChar(start);
        }
    }

    private NFAState ParseAtomModifier(NFAState start, NFAState end) {
        int  min = 0;
        int  max = -1;
        int  firstPos = pos;

        // Read min and max
        switch (ReadChar()) {
        case '?':
            min = 0;
            max = 1;
            break;
        case '*':
            min = 0;
            max = -1;
            break;
        case '+':
            min = 1;
            max = -1;
            break;
        case '{':
            min = ReadNumber();
            max = min;
            if (PeekChar(0) == ',') {
                ReadChar(',');
                max = -1;
                if (PeekChar(0) != '}') {
                    max = ReadNumber();
                }
            }
            ReadChar('}');
            if (max == 0 || (max > 0 && min > max)) {
                throw new RegExpException(
                    RegExpException.ErrorType.INVALID_REPEAT_COUNT,
                    firstPos,
                    pattern);
            }
            break;
        default:
            throw new RegExpException(
                RegExpException.ErrorType.UNEXPECTED_CHARACTER,
                pos - 1,
                pattern);
        }

        // Read possessive or reluctant modifiers
        if (PeekChar(0) == '?') {
            throw new RegExpException(
                RegExpException.ErrorType.UNSUPPORTED_SPECIAL_CHARACTER,
                pos,
                pattern);
        } else if (PeekChar(0) == '+') {
            throw new RegExpException(
                RegExpException.ErrorType.UNSUPPORTED_SPECIAL_CHARACTER,
                pos,
                pattern);
        }

        // Handle supported repeaters
        if (min == 0 && max == 1) {
            return start.AddOut(new NFAEpsilonTransition(end));
        } else if (min == 0 && max == -1) {
            if (end.outgoing.Length == 0) {
                end.MergeInto(start);
            } else {
                end.AddOut(new NFAEpsilonTransition(start));
            }
            return start;
        } else if (min == 1 && max == -1) {
            if (start.outgoing.Length == 1 &&
                end.outgoing.Length == 0 &&
                end.incoming.Length == 1 &&
                start.outgoing[0] == end.incoming[0]) {

                end.AddOut(start.outgoing[0].Copy(end));
            } else {
                end.AddOut(new NFAEpsilonTransition(start));
            }
            return end;
        } else {
            throw new RegExpException(
                RegExpException.ErrorType.INVALID_REPEAT_COUNT,
                firstPos,
                pattern);
        }
    }

    private NFAState ParseCharSet(NFAState start) {
        NFAState                end = new NFAState();
        NFACharRangeTransition  range;
        char                    min;
        char                    max;

        if (PeekChar(0) == '^') {
            ReadChar('^');
            range = new NFACharRangeTransition(true, ignoreCase, end);
        } else {
            range = new NFACharRangeTransition(false, ignoreCase, end);
        }
        start.AddOut(range);
        while (PeekChar(0) > 0) {
            min = (char) PeekChar(0);
            switch (min) {
            case ']':
                return end;
            case '\\':
                range.AddCharacter(ReadEscapeChar());
                break;
            default:
                ReadChar(min);
                if (PeekChar(0) == '-' &&
                    PeekChar(1) > 0 &&
                    PeekChar(1) != ']') {

                    ReadChar('-');
                    max = ReadChar();
                    range.AddRange(min, max);
                } else {
                    range.AddCharacter(min);
                }
                break;
            }
        }
        return end;
    }

    private NFAState ParseChar(NFAState start) {
        switch (PeekChar(0)) {
        case '\\':
            return ParseEscapeChar(start);
        case '^':
        case '$':
            throw new RegExpException(
                RegExpException.ErrorType.UNSUPPORTED_SPECIAL_CHARACTER,
                pos,
                pattern);
        default:
            return start.AddOut(ReadChar(), ignoreCase, new NFAState());
        }
    }

    private NFAState ParseEscapeChar(NFAState start) {
        NFAState  end = new NFAState();

        if (PeekChar(0) == '\\' && PeekChar(1) > 0) {
            switch ((char) PeekChar(1)) {
            case 'd':
                ReadChar();
                ReadChar();
                return start.AddOut(new NFADigitTransition(end));
            case 'D':
                ReadChar();
                ReadChar();
                return start.AddOut(new NFANonDigitTransition(end));
            case 's':
                ReadChar();
                ReadChar();
                return start.AddOut(new NFAWhitespaceTransition(end));
            case 'S':
                ReadChar();
                ReadChar();
                return start.AddOut(new NFANonWhitespaceTransition(end));
            case 'w':
                ReadChar();
                ReadChar();
                return start.AddOut(new NFAWordTransition(end));
            case 'W':
                ReadChar();
                ReadChar();
                return start.AddOut(new NFANonWordTransition(end));
            }
        }
        return start.AddOut(ReadEscapeChar(), ignoreCase, end);
    }

    private char ReadEscapeChar() {
        char    c;
        string  str;
        int     value;

        ReadChar('\\');
        c = ReadChar();
        switch (c) {
        case '0':
            c = ReadChar();
            if (c < '0' || c > '3') {
                throw new RegExpException(
                    RegExpException.ErrorType.UNSUPPORTED_ESCAPE_CHARACTER,
                    pos - 3,
                    pattern);
            }
            value = c - '0';
            c = (char) PeekChar(0);
            if ('0' <= c && c <= '7') {
                value *= 8;
                value += ReadChar() - '0';
                c = (char) PeekChar(0);
                if ('0' <= c && c <= '7') {
                    value *= 8;
                    value += ReadChar() - '0';
                }
            }
            return (char) value;
        case 'x':
            str = ReadChar().ToString() + ReadChar().ToString();
            try {
                value = Int32.Parse(str, NumberStyles.AllowHexSpecifier);
                return (char) value;
            } catch (FormatException) {
                throw new RegExpException(
                    RegExpException.ErrorType.UNSUPPORTED_ESCAPE_CHARACTER,
                    pos - str.Length - 2,
                    pattern);
            }
        case 'u':
            str = ReadChar().ToString() +
                  ReadChar().ToString() +
                  ReadChar().ToString() +
                  ReadChar().ToString();
            try {
                value = Int32.Parse(str, NumberStyles.AllowHexSpecifier);
                return (char) value;
            } catch (FormatException) {
                throw new RegExpException(
                    RegExpException.ErrorType.UNSUPPORTED_ESCAPE_CHARACTER,
                    pos - str.Length - 2,
                    pattern);
            }
        case 't':
            return '\t';
        case 'n':
            return '\n';
        case 'r':
            return '\r';
        case 'f':
            return '\f';
        case 'a':
            return '\u0007';
        case 'e':
            return '\u001B';
        default:
            if (('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z')) {
                throw new RegExpException(
                    RegExpException.ErrorType.UNSUPPORTED_ESCAPE_CHARACTER,
                    pos - 2,
                    pattern);
            }
            return c;
        }
    }

    private int ReadNumber() {
        StringBuilder  buf = new StringBuilder();
        int            c;

        c = PeekChar(0);
        while ('0' <= c && c <= '9') {
            buf.Append(ReadChar());
            c = PeekChar(0);
        }
        if (buf.Length <= 0) {
            throw new RegExpException(
                RegExpException.ErrorType.UNEXPECTED_CHARACTER,
                pos,
                pattern);
        }
        return Int32.Parse(buf.ToString());
    }

    private char ReadChar() {
        int  c = PeekChar(0);

        if (c < 0) {
            throw new RegExpException(
                RegExpException.ErrorType.UNTERMINATED_PATTERN,
                pos,
                pattern);
        } else {
            pos++;
            return (char) c;
        }
    }

    private char ReadChar(char c) {
        if (c != ReadChar()) {
            throw new RegExpException(
                RegExpException.ErrorType.UNEXPECTED_CHARACTER,
                pos - 1,
                pattern);
        }
        return c;
    }

    private int PeekChar(int count) {
        if (pos + count < pattern.Length) {
            return pattern[pos + count];
        } else {
            return -1;
        }
    }
}