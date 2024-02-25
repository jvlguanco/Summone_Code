using System.Collections;
using System.Text;

namespace Core.Library.RE;

internal class CharacterSetElement : Element {
    public static CharacterSetElement DOT =
        new CharacterSetElement(false);

    public static CharacterSetElement DIGIT =
        new CharacterSetElement(false);

    public static CharacterSetElement NON_DIGIT =
        new CharacterSetElement(true);

    public static CharacterSetElement WHITESPACE =
        new CharacterSetElement(false);

    public static CharacterSetElement NON_WHITESPACE =
        new CharacterSetElement(true);

    public static CharacterSetElement WORD =
        new CharacterSetElement(false);

    public static CharacterSetElement NON_WORD =
        new CharacterSetElement(true);

    private bool inverted;

    private ArrayList contents = new ArrayList();

    public CharacterSetElement(bool inverted) {
        this.inverted = inverted;
    }

    public void AddCharacter(char c) {
        contents.Add(c);
    }

    public void AddCharacters(string str) {
        for (int i = 0; i < str.Length; i++) {
            AddCharacter(str[i]);
        }
    }

    public void AddCharacters(StringElement elem) {
        AddCharacters(elem.GetString());
    }

    public void AddRange(char min, char max) {
        contents.Add(new Range(min, max));
    }

    public void AddCharacterSet(CharacterSetElement elem) {
        contents.Add(elem);
    }

    public override object Clone() {
        return this;
    }

    public override int Match(Matcher m,
                              ReaderBuffer buffer,
                              int start,
                              int skip) {

        int  c;

        if (skip != 0) {
            return -1;
        }
        c = buffer.Peek(start);
        if (c < 0) {
            m.SetReadEndOfString();
            return -1;
        }
        if (m.IsCaseInsensitive()) {
            c = (int) Char.ToLower((char) c);
        }
        return InSet((char) c) ? 1 : -1;
    }

    private bool InSet(char c) {
        if (this == DOT) {
            return InDotSet(c);
        } else if (this == DIGIT || this == NON_DIGIT) {
            return InDigitSet(c) != inverted;
        } else if (this == WHITESPACE || this == NON_WHITESPACE) {
            return InWhitespaceSet(c) != inverted;
        } else if (this == WORD || this == NON_WORD) {
            return InWordSet(c) != inverted;
        } else {
            return InUserSet(c) != inverted;
        }
    }

    private bool InDotSet(char c) {
        switch (c) {
        case '\n':
        case '\r':
        case '\u0085':
        case '\u2028':
        case '\u2029':
            return false;
        default:
            return true;
        }
    }

    private bool InDigitSet(char c) {
        return '0' <= c && c <= '9';
    }

    private bool InWhitespaceSet(char c) {
        switch (c) {
        case ' ':
        case '\t':
        case '\n':
        case '\f':
        case '\r':
        case (char) 11:
            return true;
        default:
            return false;
        }
    }

    private bool InWordSet(char c) {
        return ('a' <= c && c <= 'z')
            || ('A' <= c && c <= 'Z')
            || ('0' <= c && c <= '9')
            || c == '_';
    }

    private bool InUserSet(char value) {
        object               obj;
        char                 c;
        Range                r;
        CharacterSetElement  e;

        for (int i = 0; i < contents.Count; i++) {
            obj = contents[i];
            if (obj is char) {
                c = (char) obj;
                if (c == value) {
                    return true;
                }
            } else if (obj is Range) {
                r = (Range) obj;
                if (r.Inside(value)) {
                    return true;
                }
            } else if (obj is CharacterSetElement) {
                e = (CharacterSetElement) obj;
                if (e.InSet(value)) {
                    return true;
                }
            }
        }
        return false;
    }

    public override void PrintTo(TextWriter output, string indent) {
        output.WriteLine(indent + ToString());
    }

    public override string ToString() {
        StringBuilder  buffer;

        // Handle predefined character sets
        if (this == DOT) {
            return ".";
        } else if (this == DIGIT) {
            return "\\d";
        } else if (this == NON_DIGIT) {
            return "\\D";
        } else if (this == WHITESPACE) {
            return "\\s";
        } else if (this == NON_WHITESPACE) {
            return "\\S";
        } else if (this == WORD) {
            return "\\w";
        } else if (this == NON_WORD) {
            return "\\W";
        }

        buffer = new StringBuilder();
        if (inverted) {
            buffer.Append("^[");
        } else {
            buffer.Append("[");
        }
        for (int i = 0; i < contents.Count; i++) {
            buffer.Append(contents[i]);
        }
        buffer.Append("]");

        return buffer.ToString();
    }

    private class Range {
        private char min;

        private char max;

        public Range(char min, char max) {
            this.min = min;
            this.max = max;
        }

        public bool Inside(char c) {
            return min <= c && c <= max;
        }

        public override string ToString() {
            return min + "-" + max;
        }
    }
}