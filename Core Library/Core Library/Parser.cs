using System.Collections;
using System.Text;

namespace Core.Library;

public abstract class Parser {
    private bool initialized = false;

    public SyntaxProductions production = new SyntaxProductions();

    public string GetRecursiveProduction()
    {
        return ("Enter: <StartProgram>\n" + production.GetRecursiveProductions());
    }

    public int GetLastProductionCode()
    {
        return production.GetLastProductionCode();
    }

    public string GetLastProductionState()
    {
        return production.GetLastProductionState();
    }

    public List<string> GetAllProductionState()
    {
        return production.GetAllProductionState();
    }

    public List<int> GetAllProductionCode()
    {
        return production.GetAllProductionCode();
    }

    private Tokenizer tokenizer;

    private Analyzer analyzer;

    private ArrayList patterns = new ArrayList();

    private Hashtable patternIds = new Hashtable(); 

    private ArrayList tokens = new ArrayList();

    private ParserLogException errorLog = new ParserLogException();

    private int errorRecovery = -1;

    internal Parser(TextReader input) : this(input, null) {
    }

    internal Parser(TextReader input, Analyzer analyzer) {
        this.tokenizer = NewTokenizer(input);
        this.analyzer = (analyzer == null) ? NewAnalyzer() : analyzer;
    }

    internal Parser(Tokenizer tokenizer) : this(tokenizer, null) {
    }

    internal Parser(Tokenizer tokenizer, Analyzer analyzer) {
        this.tokenizer = tokenizer;
        this.analyzer = (analyzer == null) ? NewAnalyzer() : analyzer;
    }

    protected virtual Tokenizer NewTokenizer(TextReader input) {
        return new Tokenizer(input);
    }

    protected virtual Analyzer NewAnalyzer() {
        return new Analyzer();
    }

    public Tokenizer Tokenizer {
        get {
            return tokenizer;
        }
    }

    public Analyzer Analyzer {
        get {
            return analyzer;
        }
    }

    public Tokenizer GetTokenizer() {
        return Tokenizer;
    }

    public Analyzer GetAnalyzer() {
        return Analyzer;
    }

    internal void SetInitialized(bool initialized) {
        this.initialized = initialized;
    }

    public virtual void AddPattern(ProductionPattern pattern) {
        if (pattern.Count <= 0) {
            throw new ParserCreationException(
                ParserCreationException.ErrorType.INVALID_PRODUCTION,
                pattern.Name,
                "no production alternatives are present (must have at " +
                "least one)");
        }
        if (patternIds.ContainsKey(pattern.Id)) {
            throw new ParserCreationException(
                ParserCreationException.ErrorType.INVALID_PRODUCTION,
                pattern.Name,
                "another pattern with the same id (" + pattern.Id +
                ") has already been added");
        }
        patterns.Add(pattern);
        patternIds.Add(pattern.Id, pattern);
        SetInitialized(false);
    }

    public virtual void Prepare() {
        if (patterns.Count <= 0) {
            throw new ParserCreationException(
                ParserCreationException.ErrorType.INVALID_PARSER,
                "no production patterns have been added");
        }
        for (int i = 0; i < patterns.Count; i++) {
            CheckPattern((ProductionPattern) patterns[i]);
        }
        SetInitialized(true);
    }

    private void CheckPattern(ProductionPattern pattern) {
        for (int i = 0; i < pattern.Count; i++) {
            CheckAlternative(pattern.Name, pattern[i]);
        }
    }

    private void CheckAlternative(string name,
                                  ProductionPatternAlternative alt) {

        for (int i = 0; i < alt.Count; i++) {
            CheckElement(name, alt[i]);
        }
    }

    private void CheckElement(string name,
                              ProductionPatternElement elem) {

        if (elem.IsProduction() && GetPattern(elem.Id) == null) {
            throw new ParserCreationException(
                ParserCreationException.ErrorType.INVALID_PRODUCTION,
                name,
                "an undefined production pattern id (" + elem.Id +
                ") is referenced");
        }
    }

    public void Reset(TextReader input) {
        this.tokenizer.Reset(input);
        this.analyzer.Reset();
    }

    public void Reset(TextReader input, Analyzer analyzer) {
        this.tokenizer.Reset(input);
        this.analyzer = analyzer;
    }

    public Node Parse() {
        Node  root = null;
       
        // Initialize parser
        if (!initialized) {
            Prepare();
        }
        this.tokens.Clear();
        this.errorLog = new ParserLogException();
        this.errorRecovery = -1;

        // Parse input
        try {
            root = ParseStart();
        } catch (ParseException e) {
            AddError(e, true);
        }

        // Check for errors
        if (errorLog.Count > 0) {
            throw errorLog;
        }
        
        return root;
    }

    protected abstract Node ParseStart();

    protected virtual Production NewProduction(ProductionPattern pattern) {
        return analyzer.NewProduction(pattern);
    }

    internal void AddError(ParseException e, bool recovery) {
        if (errorRecovery <= 0) {
            errorLog.AddError(e);
        }
        if (recovery) {
            errorRecovery = 3;
        }
    }

    internal ProductionPattern GetPattern(int id) {
        return (ProductionPattern) patternIds[id];
    }

    internal ProductionPattern GetStartPattern() {
        if (patterns.Count <= 0) {
            return null;
        } else {
            return (ProductionPattern) patterns[0];
        }
    }

    internal ICollection GetPatterns() {
        return patterns;
    }

    internal void EnterNode(Node node) {
        if (!node.IsHidden() && errorRecovery < 0) {
            try {
                analyzer.Enter(node);
            } catch (ParseException e) {
                AddError(e, false);
            }
        }
    }

    internal Node ExitNode(Node node) {
        if (!node.IsHidden() && errorRecovery < 0) {
            try {
                return analyzer.Exit(node);
            } catch (ParseException e) {
                AddError(e, false);
            }
        }
        return node;
    }

    internal void AddNode(Production node, Node child) {
        if (errorRecovery >= 0) {
            // Do nothing
        } else if (node.IsHidden()) {
            node.AddChild(child);
        } else if (child != null && child.IsHidden()) {
            for (int i = 0; i < child.Count; i++) {
                AddNode(node, child[i]);
            }
        } else {
            try {
                analyzer.Child(node, child);
            } catch (ParseException e) {
                AddError(e, false);
            }
        }
    }

    internal Token NextToken() {
        Token  token = PeekToken(0);

        if (token != null) {
            tokens.RemoveAt(0);
            return token;
        } else {
            throw new ParseException(
                ParseException.ErrorType.UNEXPECTED_EOF,
                null,
                tokenizer.GetCurrentLine(),
                tokenizer.GetCurrentColumn());
        }
    }

    internal Token NextToken(int id) {
        Token      token = NextToken();
        ArrayList  list;

        if (token.Id == id) {
            if (errorRecovery > 0) {
                errorRecovery--;
            }
            return token;
        } else {
            list = new ArrayList(1);
            list.Add(tokenizer.GetPatternDescription(id));
            throw new ParseException(
                ParseException.ErrorType.UNEXPECTED_TOKEN,
                token.ToShortString(),
                list,
                token.StartLine,
                token.StartColumn);
        }
    }

    internal Token PeekToken(int steps) {
        Token  token;

        while (steps >= tokens.Count) {
            try {
                token = tokenizer.Next();
                if (token == null) {
                    return null;
                } else {
                    tokens.Add(token);
                }
            } catch (ParseException e) {
                AddError(e, true);
            }
        }
        return (Token) tokens[steps];
    }

    public override string ToString() {
        StringBuilder  buffer = new StringBuilder();

        for (int i = 0; i < patterns.Count; i++) {
            buffer.Append(ToString((ProductionPattern) patterns[i]));
            buffer.Append("\n");
        }
        return buffer.ToString();
    }

    private string ToString(ProductionPattern prod) {
        StringBuilder  buffer = new StringBuilder();
        StringBuilder  indent = new StringBuilder();
        LookAheadSet   set;
        int            i;

        buffer.Append(prod.Name);
        buffer.Append(" (");
        buffer.Append(prod.Id);
        buffer.Append(") ");
        for (i = 0; i < buffer.Length; i++) {
            indent.Append(" ");
        }
        buffer.Append("= ");
        indent.Append("| ");
        for (i = 0; i < prod.Count; i++) {
            if (i > 0) {
                buffer.Append(indent);
            }
            buffer.Append(ToString(prod[i]));
            buffer.Append("\n");
        }
        for (i = 0; i < prod.Count; i++) {
            set = prod[i].LookAhead;
            if (set.GetMaxLength() > 1) {
                buffer.Append("Using ");
                buffer.Append(set.GetMaxLength());
                buffer.Append(" token look-ahead for alternative ");
                buffer.Append(i + 1);
                buffer.Append(": ");
                buffer.Append(set.ToString(tokenizer));
                buffer.Append("\n");
            }
        }
        return buffer.ToString();
    }

    private string ToString(ProductionPatternAlternative alt) {
        StringBuilder  buffer = new StringBuilder();

        for (int i = 0; i < alt.Count; i++) {
            if (i > 0) {
                buffer.Append(" ");
            }
            buffer.Append(ToString(alt[i]));
        }
        return buffer.ToString();
    }

    private string ToString(ProductionPatternElement elem) {
        StringBuilder  buffer = new StringBuilder();
        int            min = elem.MinCount;
        int            max = elem.MaxCount;

        if (min == 0 && max == 1) {
            buffer.Append("[");
        }
        if (elem.IsToken()) {
            buffer.Append(GetTokenDescription(elem.Id));
        } else {
            buffer.Append(GetPattern(elem.Id).Name);
        }
        if (min == 0 && max == 1) {
            buffer.Append("]");
        } else if (min == 0 && max == Int32.MaxValue) {
            buffer.Append("*");
        } else if (min == 1 && max == Int32.MaxValue) {
            buffer.Append("+");
        } else if (min != 1 || max != 1) {
            buffer.Append("{");
            buffer.Append(min);
            buffer.Append(",");
            buffer.Append(max);
            buffer.Append("}");
        }
        return buffer.ToString();
    }

    internal string GetTokenDescription(int token) {
        if (tokenizer == null) {
            return "";
        } else {
            return tokenizer.GetPatternDescription(token);
        }
    }
}