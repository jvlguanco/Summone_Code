using Syntax_Analyzer;
using TokenLibrary;
using Core.Library;

namespace Semantic_Analyzer;

public class SemanticInitializer : SyntaxAnalyzer
{
    public class Tokens : TokensClass
    {
        public List<string> attribute = new List<string>();

        public void addAttribute(string attribute)
        {
            this.attribute.Add(attribute);
        }

        public List<string> getAttribute()
        {
            return this.attribute;
        }
    }
    
    public string error = "";
    public List<Tokens> tokens;
    public List<Tokens> ID = new List<Tokens>();
    public List<Tokens> globalID = new List<Tokens>();

    public SemanticInitializer() : this(new List<Tokens>()) { }

    public SemanticInitializer(List<Tokens> tokens)
    {
        this.tokens = tokens;
    }

    public string Start()
    {
        string tokenstream = "";
        string result = "Semantics Analyzer Failed...\n";
        int line = 1;
        int linejump = 0;
        foreach (var t in tokens)
        {
            if (t.getLines() != line)
            {
                linejump = t.getLines() - line;
                line = t.getLines();
                for (int i = 0; i < linejump; i++)
                {
                    tokenstream += "\n";
                }
            }
            tokenstream += t.getTokens() + " ";
        }
        tokenstream = tokenstream.TrimEnd();

        Parser p;
        p = CreateParser(tokenstream);

        try
        {
            p.Parse();
            if (error == "")
                result = "Semantics Analyzer Succeeded...";
        }
        catch (ParserCreationException e)
        {
            result = "Semantics Analyzer Halted due to Syntax Analyzer Error...";
        }
        catch (ParserLogException e)
        {
            result = "Semantics Analyzer Halted due to Syntax Log Error...";
        }
        
        return result;
    }
    
    private Parser CreateParser(string input)
    {
        SyntaxParser parser = null;
        try
        {
            parser = new SyntaxParser(new StringReader(input), this);
            parser.Prepare();
        }
        catch (ParserCreationException e)
        {
            throw new Exception(e.Message);
        }
        return parser;
    }
    
    public Tokens GetTokens(int line, int column)
    {
        List<Tokens> t = new List<Tokens>();
        Tokens token = new Tokens();
        int endline = 0;
        foreach (var item in tokens)
        {
            if (item.getLines() == line)
                t.Add(item);
        }
        foreach (var item in t)
        {
            endline += item.getTokens().Length + 1;
            if (column <= endline)
            {
                token = item;
                break;
            }
        }
        return token;
    }
    
    private void hasGlobalID(Tokens token)
    {
        Boolean isdeclared = false;
        if (globalID.Count != 0)
        {
            foreach (var item in globalID)
            {
                if (token.getLexemes() == item.getLexemes())
                {
                    error += "Semantics Error (Ln" + token.getLines() + "): " + token.getLexemes() + " is already declared.\n";
                    isdeclared = true;
                    break;
                }
            }
        }

        if (!isdeclared)
        {
            globalID.Add(token);
        }
    }
    
    private string getDtype(string dtype)
    {
        switch (dtype)
        {
            case "INTER":
                dtype = "inter";
                break;
            case "BLOAT":
                dtype = "bloat";
                break;
            case "PING":
                dtype = "Ping";
                break;
            case "Pool":
                dtype = "Pool";
                break;
            case "VOID":
                dtype = "Void";
                break;
        }
        return dtype;
    }
    
    public virtual Node ExitProdStart(Production node) {
        return node;
    }
    
    public virtual Node ExitProdProgram(Production node) {
        return node;
    }
    
    public virtual Node ExitProdGlobalDeclaration(Production node) {
        return node;
    }
}