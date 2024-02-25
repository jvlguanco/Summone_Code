using System.Text;
using System.Text.RegularExpressions;
using Core.Library.RE;

namespace Core.Library;

public class Tokenizer {
    private bool useTokenList = false;
    private StringDFAMatcher stringDfaMatcher;
    private NFAMatcher nfaMatcher;
    private RegExpMatcher regExpMatcher;
    private ReaderBuffer buffer = null;
    private TokenMatch lastMatch = new TokenMatch();
    private Token previousToken = null;
    public Tokenizer(TextReader input)
        : this(input, false) {
    }

    public Tokenizer(TextReader input, bool ignoreCase) {
        this.stringDfaMatcher = new StringDFAMatcher(ignoreCase);
        this.nfaMatcher = new NFAMatcher(ignoreCase);
        this.regExpMatcher = new RegExpMatcher(ignoreCase);
        this.buffer = new ReaderBuffer(input);
    }

    public bool UseTokenList {
        get {
            return useTokenList;
        }
        set {
            useTokenList = value;
        }
    }

    public bool GetUseTokenList() {
        return useTokenList;
    }

    public void SetUseTokenList(bool useTokenList) {
        this.useTokenList = useTokenList;
    }

    public string GetPatternDescription(int id) {
        TokenPattern  pattern;

        pattern = stringDfaMatcher.GetPattern(id);
        if (pattern == null) {
            pattern = nfaMatcher.GetPattern(id);
        }
        if (pattern == null) {
            pattern = regExpMatcher.GetPattern(id);
        }
        return (pattern == null) ? null : pattern.ToShortString();
    }

    public int GetCurrentLine() {
        return buffer.LineNumber;
    }

    public int GetCurrentColumn() {
        return buffer.ColumnNumber;
    }

    public void AddPattern(TokenPattern pattern) {
        switch (pattern.Type) {
        case TokenPattern.PatternType.STRING:
            try {
                stringDfaMatcher.AddPattern(pattern);
            } catch (Exception e) {
                throw new ParserCreationException(
                    ParserCreationException.ErrorType.INVALID_TOKEN,
                    pattern.Name,
                    "error adding string token: " +
                    e.Message);
            }
            break;
        case TokenPattern.PatternType.REGEXP:
            try {
                nfaMatcher.AddPattern(pattern);
            } catch (Exception) {
                try {
                    regExpMatcher.AddPattern(pattern);
                } catch (Exception e) {
                    throw new ParserCreationException(
                        ParserCreationException.ErrorType.INVALID_TOKEN,
                        pattern.Name,
                        "regular expression contains error(s): " +
                        e.Message);
                }
            }
            break;
        default:
            throw new ParserCreationException(
                ParserCreationException.ErrorType.INVALID_TOKEN,
                pattern.Name,
                "pattern type " + pattern.Type +
                " is undefined");
        }
    }

    public void Reset(TextReader input) {
        this.buffer.Dispose();
        this.buffer = new ReaderBuffer(input);
        this.previousToken = null;
        this.lastMatch.Clear();
    }

    public Token Next() {
        Token  token = null;

        do {
            token = NextToken();
            if (token == null) {
                previousToken = null;
                return null;
            }
            if (useTokenList) {
                token.Previous = previousToken;
                previousToken = token;
            }
            if (token.Pattern.Ignore) {
                token = null;
            } else if (token.Pattern.Error) {
                throw new ParseException(
                    ParseException.ErrorType.INVALID_TOKEN,
                    token.Pattern.ErrorMessage,
                    token.StartLine,
                    token.StartColumn);
            }
        } while (token == null);
        return token;
    }

    private Token NextToken() {
        string  str;
        int     line;
        int     column;

        try {
            lastMatch.Clear();
            stringDfaMatcher.Match(buffer, lastMatch);
            nfaMatcher.Match(buffer, lastMatch);
            regExpMatcher.Match(buffer, lastMatch);
            if (lastMatch.Length > 0) {
                line = buffer.LineNumber;
                column = buffer.ColumnNumber;
                str = buffer.Read(lastMatch.Length);
                return NewToken(lastMatch.Pattern, str, line, column);
            } else if (buffer.Peek(0) < 0) {
                return null;
            } else {
                line = buffer.LineNumber;
                column = buffer.ColumnNumber;
                throw new ParseException(
                    ParseException.ErrorType.UNEXPECTED_CHAR,
                    buffer.Read(1),
                    line,
                    column);
            }
        } catch (IOException e) {
            throw new ParseException(ParseException.ErrorType.IO,
                                     e.Message,
                                     -1,
                                     -1);
        }
    }

    protected virtual Token NewToken(TokenPattern pattern,
                                     string image,
                                     int line,
                                     int column) {

        return new Token(pattern, image, line, column);
    }

    public override string ToString() {
        StringBuilder  buffer = new StringBuilder();

        buffer.Append(stringDfaMatcher);
        buffer.Append(nfaMatcher);
        buffer.Append(regExpMatcher);
        return buffer.ToString();
    }
}

internal abstract class TokenMatcher {
    protected TokenPattern[] patterns = new TokenPattern[0];

    protected bool ignoreCase = false;

    public TokenMatcher(bool ignoreCase) {
        this.ignoreCase = ignoreCase;
    }

    public abstract void Match(ReaderBuffer buffer, TokenMatch match);

    public TokenPattern GetPattern(int id) {
        for (int i = 0; i < patterns.Length; i++) {
            if (patterns[i].Id == id) {
                return patterns[i];
            }
        }
        return null;
    }

    public virtual void AddPattern(TokenPattern pattern) {
        Array.Resize(ref patterns, patterns.Length + 1);
        patterns[patterns.Length - 1] = pattern;
    }

    public override string ToString() {
        StringBuilder  buffer = new StringBuilder();

        for (int i = 0; i < patterns.Length; i++) {
            buffer.Append(patterns[i]);
            buffer.Append("\n\n");
        }
        return buffer.ToString();
    }
}

internal class StringDFAMatcher : TokenMatcher {
    private TokenStringDFA automaton = new TokenStringDFA();
    
    public StringDFAMatcher(bool ignoreCase) : base(ignoreCase) {
    }

    public override void AddPattern(TokenPattern pattern) {
        automaton.AddMatch(pattern.Pattern, ignoreCase, pattern);
        base.AddPattern(pattern);
    }

    public override void Match(ReaderBuffer buffer, TokenMatch match) {
        TokenPattern  res = automaton.Match(buffer, ignoreCase);

        if (res != null) {
            match.Update(res.Pattern.Length, res);
        }
    }
}

internal class NFAMatcher : TokenMatcher {
    private TokenNFA automaton = new TokenNFA();

    public NFAMatcher(bool ignoreCase) : base(ignoreCase) {
    }

    public override void AddPattern(TokenPattern pattern) {
        if (pattern.Type == TokenPattern.PatternType.STRING) {
            automaton.AddTextMatch(pattern.Pattern, ignoreCase, pattern);
        } else {
            automaton.AddRegExpMatch(pattern.Pattern, ignoreCase, pattern);
        }
        base.AddPattern(pattern);
    }

    public override void Match(ReaderBuffer buffer, TokenMatch match) {
        automaton.Match(buffer, match);
    }
}

internal class RegExpMatcher : TokenMatcher {
    private REHandler[] regExps = new REHandler[0];

    public RegExpMatcher(bool ignoreCase) : base(ignoreCase) {
    }

    public override void AddPattern(TokenPattern pattern) {
        REHandler  re;

        try {
            re = new GrammaticaRE(pattern.Pattern, ignoreCase);
            pattern.DebugInfo = "Grammatica regexp\n" + re;
        } catch (Exception) {
            re = new SystemRE(pattern.Pattern, ignoreCase);
            pattern.DebugInfo = "native .NET regexp";
        }
        Array.Resize(ref regExps, regExps.Length + 1);
        regExps[regExps.Length - 1] = re;
        base.AddPattern(pattern);
    }

    public override void Match(ReaderBuffer buffer, TokenMatch match) {
        for (int i = 0; i < regExps.Length; i++) {
            int length = regExps[i].Match(buffer);
            if (length > 0) {
                match.Update(length, patterns[i]);
            }
        }
    }
}

internal abstract class REHandler {
    public abstract int Match(ReaderBuffer buffer);
}

internal class GrammaticaRE : REHandler {
    private RegExp regExp;

    private Matcher matcher = null;

    public GrammaticaRE(string regex, bool ignoreCase) {
        regExp = new RegExp(regex, ignoreCase);
    }

    public override int Match(ReaderBuffer buffer) {
        if (matcher == null) {
            matcher = regExp.Matcher(buffer);
        } else {
            matcher.Reset(buffer);
        }
        return matcher.MatchFromBeginning() ? matcher.Length() : 0;
    }
}

internal class SystemRE : REHandler {
    private Regex reg;

    public SystemRE(string regex, bool ignoreCase) {
        if (ignoreCase) {
            reg = new Regex(regex, RegexOptions.IgnoreCase);
        } else {
            reg = new Regex(regex);
        }
    }

    public override int Match(ReaderBuffer buffer) {
        Match  m;

        buffer.Peek(1024 * 16);
        m = reg.Match(buffer.ToString(), buffer.Position);
        
        if (m.Success && m.Index == buffer.Position) {
            return m.Length;
        } else {
            return 0;
        }
    }
}