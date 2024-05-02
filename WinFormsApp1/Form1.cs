using LexicalAnalyzer;
using Syntax_Analyzer;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;

namespace WinFormsApp1;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        syntax.Enabled = false;
        semantic.Enabled = false;
        run.Enabled = false;
    }

    Analyzer lex = new Analyzer();
    string txt;
    Dictionary<int, int> lineMapping = new Dictionary<int, int>();
    string code;
    bool hasError;
    int lineTracker = 2;
    int currentLine = 1;

    private async void button1_Click(object sender, EventArgs e)
    {
        OutputText.Clear();
        LexGrid.Rows.Clear();
        TempGrid.Rows.Clear();
        DataLexicalError.Rows.Clear();
        DataSyntaxError.Rows.Clear();
        semanticError.Rows.Clear();
        run.Enabled = false;

        if (Code.RichTextBox.Text != "")
        {
            lex = new Analyzer();
            Initializer Lexical = new Initializer();
            txt = Code.RichTextBox.Text + " ";

            lex = Lexical.InitializeAnalyzer(txt, lex);

            DisplayTokens(lex);

            if (lex._invalid == 0 && lex._token.Count != 0)
            {
                syntax.Enabled = true;

                SyntaxInitializer syntaxInitializer = new SyntaxInitializer();
                DataSyntaxError.Rows.Clear();

                run.Enabled = false;

                int i = 1;
                string s;

                s = syntaxInitializer.Start(tokenDump(lex._token));

                if (s != "Analyzer has pushed the lane. Proceed!")
                {
                    int errornum = 1;
                    DataSyntaxError.Rows.Clear();

                    if (syntaxInitializer.errors.getColumn() == 1)
                    {
                        syntaxInitializer.errors.setLines(syntaxInitializer.errors.getLines() - 1);
                    }

                    DataSyntaxError.Rows.Add(errornum, syntaxInitializer.errors.getLines(), syntaxInitializer.errors.getErrorMessage());

                    errornum++;

                    tabControl2.SelectTab("tabPage4");
                }
                else
                {
                    DataSyntaxError.Rows.Add(i, "", s);
                    run.Enabled = false;

                    code = TranslateCode();

                    await AnalyzeCodeAsync(code);

                    if (hasError)
                    {
                        run.Enabled = false;
                        tabControl2.SelectTab("tabPage5");
                    }
                    else
                    {
                        run.Enabled = true;
                        semanticError.Rows.Add(0, "No semantic errors found. You may run the code!", 0);
                        tabControl2.SelectTab("tabPage5");
                    }
                }
            }
            else
            {
                tabControl1.SelectTab("tabPage1");
                tabControl2.SelectTab("tabPage3");
                run.Enabled = false;
            }
        }
    }

    private void lexer_Click(object sender, EventArgs e)
    {
        
        syntax.Enabled = false;
        semantic.Enabled = false;
        

        tabControl1.SelectTab("tabPage1");
        tabControl2.SelectTab("tabPage3");

        if (Code.RichTextBox.Text != "")
        {
            lex = new Analyzer();
            Initializer Lexical = new Initializer();
            txt = Code.RichTextBox.Text + " ";

            lex = Lexical.InitializeAnalyzer(txt, lex);

            DisplayTokens(lex);

            if (lex._invalid == 0 && lex._token.Count != 0)
            {
                syntax.Enabled = true;
            }
            else
            {
                syntax.Enabled = false;
                semantic.Enabled = false;
                run.Enabled = false;
            }
        }
    }

    private void syntax_Click(object sender, EventArgs e)
    {
        SyntaxInitializer syntaxInitializer = new SyntaxInitializer();
        DataSyntaxError.Rows.Clear();
        semantic.Enabled = false;
        run.Enabled = false;

        int i = 1;
        string s;

        tabControl2.SelectTab("tabPage4");

        s = syntaxInitializer.Start(tokenDump(lex._token));

        if (s != "Analyzer has pushed the lane. Proceed!")
        {
            int errornum = 1;
            DataSyntaxError.Rows.Clear();

            if (syntaxInitializer.errors.getColumn() == 1)
            {
                syntaxInitializer.errors.setLines(syntaxInitializer.errors.getLines() - 1);
            }

            DataSyntaxError.Rows.Add(errornum, syntaxInitializer.errors.getLines(), syntaxInitializer.errors.getErrorMessage());

            errornum++;
        }
        else
        {
            DataSyntaxError.Rows.Add(i, "", s);
            semantic.Enabled = true;
            run.Enabled = false;
        }
    }

    private void DisplayTokens(Analyzer lex)
    {
        string result = "You may push!";
        int ctr = 0, id = 0, error = 0;
        LexGrid.Rows.Clear();
        DataLexicalError.Rows.Clear();

        if (lex._invalid != 0)
            result = "Pinging " + lex._invalid.ToString() + " error/s. QUEUE AGAIN!";

        DataLexicalError.Rows.Add(id, "Lexical Analyzer: " + result);

        foreach (var token in lex._token)
        {
            if (token.getTokens() == "Invalid")
            {
                error++;
                DataLexicalError.Rows.Add(error, "Invalid lexeme: "
                            + token.getLexemes()
                            + " on line "
                            + token.getLines());
            }
            else if (token.getTokens() == "NoDelim")
            {
                error++;
                DataLexicalError.Rows.Add(error, "Invalid lexeme: "
                            + token.getLexemes()
                            + " on line "
                            + token.getLines());
            }
            else
            {
                id++;
                LexGrid.Rows.Add(id, token.getLexemes(), token.getTokens());

                if (token.getTokens() != "space" && token.getTokens() != "tab")
                {
                    if (token.getTokens() == "Inter Literal" || token.getTokens() == "Bloat Literal")
                    {
                        string temp = token.getLexemes().Replace("~", "-");
                        TempGrid.Rows.Add(id, temp, token.getTokens());
                    }
                    else
                    {
                        TempGrid.Rows.Add(id, token.getLexemes(), token.getTokens());
                    }
                }

            }
            ctr++;
        }
    }

    public List<TokenLibrary.TokensClass> tokenDump(List<Tokens> tokens)
    {
        List<TokenLibrary.TokensClass> token = new List<TokenLibrary.TokensClass>();
        Tokens t = new Tokens();

        foreach (var item in tokens)
        {
            t = new Tokens();
            t.setLexemes(item.getLexemes());
            t.setLines(item.getLines());
            t.setTokens(item.getTokens());
            token.Add(t);
        }

        return token;
    }

    private void clear_Click(object sender, EventArgs e)
    {
        syntax.Enabled = false;
        semantic.Enabled = false;
        run.Enabled = false;
        LexGrid.Rows.Clear();
        DataLexicalError.Rows.Clear();
        DataSyntaxError.Rows.Clear();
        Code.RichTextBox.Clear();
        semanticError.Rows.Clear();
        OutputText.Clear();
        TempGrid.Rows.Clear();
    }

    private void DataLexicalError_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }

    private async void semantic_Click(object sender, EventArgs e)
    {
        code = TranslateCode();
        tabControl2.SelectTab("tabPage5");

        await AnalyzeCodeAsync(code);

        if (hasError)
        {
            run.Enabled = false;
        }
        else
        {
            run.Enabled = true;
            semanticError.Rows.Add(0, "No semantic errors found.", 0);
        }
    }

    private void semanticError_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        // string code = @"using System; class Program { public static void Main() { int a; int b = 12.123; Console.WriteLine(a); Console.ReadLine();} }";
    }

    private async void run_Click(object sender, EventArgs e)
    {
        await ExecuteCodeAsync(code);
    }

    public string TranslateCode()
    {
        OutputText.Text = "";
        string codeTemp = "";
        string tempId = "";
        string datatype = "";
        bool mainFlag = false;
        int lastValue = TempGrid.Rows.Count;
        int count = 0;
        int dimensions = 0;
        int tracker = 0;
        object[] size = new object[2];
        int openBrace = 0;
        int openDo = 0;
        lineTracker = 2;
        currentLine = 1;

        Dictionary<string, string[,]> structArray = new Dictionary<string, string[,]>();
        bool is2D = false;

        lineMapping.Clear();

        while (TempGrid.Rows[count].Cells[2].Value.ToString() != "spawn")
        {
            count++;
        }

        OutputText.Text = "using System; using System.Collections.ObjectModel; class Summoner { \n";

        for (int x = 0; x < count; x++)
        {
            switch (TempGrid.Rows[x].Cells[2].Value.ToString())
            {
                case "comp":
                    codeTemp += "public ";
                    x++;

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "inter")
                    {
                        datatype += "int";
                        x++;
                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                        {
                            tempId = TempGrid.Rows[x].Cells[1].Value.ToString();
                            x++;
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                        {
                            codeTemp += "static readonly " + datatype + "[";
                            x++;
                            datatype = "";

                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != "=")
                            {
                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Inter Literal")
                                {
                                    dimensions++;
                                    size[tracker] = Convert.ToInt32(TempGrid.Rows[x].Cells[1].Value.ToString());
                                    x++;
                                    tracker++;
                                }
                                else
                                {
                                    x++;
                                }
                            }

                            if (dimensions == 1)
                            {
                                codeTemp += "] ";
                            }
                            else if (dimensions == 2)
                            {
                                codeTemp += ",] ";
                            }

                            codeTemp += " " + tempId + " " + " = new int[";

                            tempId = "";

                            if (dimensions == 1)
                            {
                                codeTemp += size[0] + "] ";
                            }
                            else if (dimensions == 2)
                            {
                                codeTemp += size[0] + "," + size[1] + "] ";
                            }

                            tracker = 0;
                            dimensions = 0;

                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != "{")
                            {
                                x++;
                            }

                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                            {
                                codeTemp += ";\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                        }
                        else
                        {
                            codeTemp += "const " + datatype + " " + tempId + " ";
                            tempId = "";
                            datatype = "";
                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                            {
                                codeTemp += ";\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                        }
                    }
                    else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "bloat")
                    {
                        datatype += "double";
                        x++;

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                        {
                            tempId = TempGrid.Rows[x].Cells[1].Value.ToString();
                            x++;
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                        {
                            codeTemp += "static readonly " + datatype + "[";
                            x++;
                            datatype = "";

                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != "=")
                            {
                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Inter Literal")
                                {
                                    dimensions++;
                                    size[tracker] = Convert.ToInt32(TempGrid.Rows[x].Cells[1].Value.ToString());
                                    x++;
                                    tracker++;
                                }
                                else
                                {
                                    x++;
                                }
                            }

                            if (dimensions == 1)
                            {
                                codeTemp += "] ";
                            }
                            else if (dimensions == 2)
                            {
                                codeTemp += ",] ";
                            }

                            codeTemp += " " + tempId + " " + " = new double[";

                            tempId = "";

                            if (dimensions == 1)
                            {
                                codeTemp += size[0] + "] ";
                            }
                            else if (dimensions == 2)
                            {
                                codeTemp += size[0] + "," + size[1] + "] ";
                            }

                            tracker = 0;
                            dimensions = 0;

                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != "{")
                            {
                                x++;
                            }

                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                            {
                                codeTemp += ";\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                        }
                        else
                        {
                            codeTemp += "const " + datatype + " " + tempId + " ";
                            tempId = "";
                            datatype = "";
                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                            {
                                codeTemp += ";\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                        }
                    }
                    else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "ping")
                    {
                        datatype += "string";
                        x++;

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                        {
                            tempId = TempGrid.Rows[x].Cells[1].Value.ToString();
                            x++;
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                        {
                            codeTemp += "static readonly " + datatype + "[";
                            x++;
                            datatype = "";

                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != "=")
                            {
                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Inter Literal")
                                {
                                    dimensions++;
                                    size[tracker] = Convert.ToInt32(TempGrid.Rows[x].Cells[1].Value.ToString());
                                    x++;
                                    tracker++;
                                }
                                else
                                {
                                    x++;
                                }
                            }

                            if (dimensions == 1)
                            {
                                codeTemp += "] ";
                            }
                            else if (dimensions == 2)
                            {
                                codeTemp += ",] ";
                            }

                            codeTemp += " " + tempId + " " + " = new string[";

                            tempId = "";

                            if (dimensions == 1)
                            {
                                codeTemp += size[0] + "] ";
                            }
                            else if (dimensions == 2)
                            {
                                codeTemp += size[0] + "," + size[1] + "] ";
                            }

                            tracker = 0;
                            dimensions = 0;

                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != "{")
                            {
                                x++;
                            }

                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                            {
                                codeTemp += ";\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                        }
                        else
                        {
                            codeTemp += "const " + datatype + " " + tempId + " ";
                            tempId = "";
                            datatype = "";
                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                            {
                                codeTemp += ";\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                        }
                    }
                    else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "pool")
                    {
                        datatype += "bool";
                        x++;

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                        {
                            tempId = TempGrid.Rows[x].Cells[1].Value.ToString();
                            x++;
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                        {
                            codeTemp += "static readonly " + datatype + "[";
                            x++;
                            datatype = "";

                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != "=")
                            {
                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Inter Literal")
                                {
                                    dimensions++;
                                    size[tracker] = Convert.ToInt32(TempGrid.Rows[x].Cells[1].Value.ToString());
                                    x++;
                                    tracker++;
                                }
                                else
                                {
                                    x++;
                                }
                            }

                            if (dimensions == 1)
                            {
                                codeTemp += "] ";
                            }
                            else if (dimensions == 2)
                            {
                                codeTemp += ",] ";
                            }

                            codeTemp += " " + tempId + " " + " = new bool[";

                            tempId = "";

                            if (dimensions == 1)
                            {
                                codeTemp += size[0] + "] ";
                            }
                            else if (dimensions == 2)
                            {
                                codeTemp += size[0] + "," + size[1] + "] ";
                            }

                            tracker = 0;
                            dimensions = 0;

                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != "{")
                            {
                                x++;
                            }

                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                            {
                                codeTemp += ";\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                        }
                        else
                        {
                            codeTemp += "const " + datatype + " " + tempId + " ";
                            tempId = "";
                            datatype = "";
                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                            {
                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Pool Literal")
                                {
                                    if (TempGrid.Rows[x].Cells[1].Value.ToString() == "buff")
                                    {
                                        codeTemp += " true";
                                    }
                                    else
                                    {
                                        codeTemp += " false";
                                    }
                                }
                                else
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                }
                                x++;
                            }

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                            {
                                codeTemp += ";\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                        }
                    }

                    break;
                case "inter":
                    codeTemp += "public static int ";
                    x++;

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                    {
                        tempId = TempGrid.Rows[x].Cells[1].Value.ToString();
                        x++;
                    }

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                    {
                        codeTemp += "[";
                        x++;

                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != "=" && TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                        {
                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Inter Literal")
                            {
                                dimensions++;
                                size[tracker] = Convert.ToInt32(TempGrid.Rows[x].Cells[1].Value.ToString());
                                x++;
                                tracker++;
                            }
                            else
                            {
                                x++;
                            }
                        }

                        if (dimensions == 1)
                        {
                            codeTemp += "] ";
                        }
                        else if (dimensions == 2)
                        {
                            codeTemp += ",] ";
                        }

                        codeTemp += " " + tempId + " " + " = new int[";

                        tempId = "";

                        if (dimensions == 1)
                        {
                            codeTemp += size[0] + "] ";
                        }
                        else if (dimensions == 2)
                        {
                            codeTemp += size[0] + "," + size[1] + "] ";
                        }

                        tracker = 0;
                        dimensions = 0;

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "=")
                        {
                            x++;
                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                        {
                            codeTemp += ";\n";
                            OutputText.Text += codeTemp;
                            codeTemp = "";
                            lineMapping.Add(lineTracker, currentLine);
                            lineTracker++;
                        }
                    }
                    else
                    {
                        codeTemp += tempId + " ";
                        tempId = "";
                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                        {
                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "=")
                            {
                                codeTemp += " = " + TempGrid.Rows[x + 1].Cells[1].Value.ToString();
                                x = x + 2;
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "," && TempGrid.Rows[x - 1].Cells[2].Value.ToString() == "Identifier")
                            {
                                codeTemp += "= default, ";
                                x++;
                            }
                            else
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                        {
                            if (TempGrid.Rows[x - 1].Cells[2].Value.ToString() == "Identifier")
                            {
                                codeTemp += " = default;\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                            else
                            {
                                codeTemp += ";\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                        }
                    }
                    break;
                case "bloat":
                    codeTemp += "public static double ";
                    x++;

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                    {
                        tempId = TempGrid.Rows[x].Cells[1].Value.ToString();
                        x++;
                    }

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                    {
                        codeTemp += "[";
                        x++;

                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != "=" && TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                        {
                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Inter Literal")
                            {
                                dimensions++;
                                size[tracker] = Convert.ToInt32(TempGrid.Rows[x].Cells[1].Value.ToString());
                                x++;
                                tracker++;
                            }
                            else
                            {
                                x++;
                            }
                        }

                        if (dimensions == 1)
                        {
                            codeTemp += "] ";
                        }
                        else if (dimensions == 2)
                        {
                            codeTemp += ",] ";
                        }

                        codeTemp += " " + tempId + " " + " = new double[";

                        tempId = "";

                        if (dimensions == 1)
                        {
                            codeTemp += size[0] + "] ";
                        }
                        else if (dimensions == 2)
                        {
                            codeTemp += size[0] + "," + size[1] + "] ";
                        }

                        tracker = 0;
                        dimensions = 0;

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "=")
                        {
                            x++;
                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                        {
                            codeTemp += ";\n";
                            OutputText.Text += codeTemp;
                            codeTemp = "";
                            lineMapping.Add(lineTracker, currentLine);
                            lineTracker++;
                        }
                    }
                    else
                    {
                        codeTemp += tempId + " ";
                        tempId = "";
                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                        {
                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "=")
                            {
                                codeTemp += " = " + TempGrid.Rows[x + 1].Cells[1].Value.ToString();
                                x = x + 2;
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "," && TempGrid.Rows[x - 1].Cells[2].Value.ToString() == "Identifier")
                            {
                                codeTemp += "= default, ";
                                x++;
                            }
                            else
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                        {
                            if (TempGrid.Rows[x - 1].Cells[2].Value.ToString() == "Identifier")
                            {
                                codeTemp += " = default;\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                            else
                            {
                                codeTemp += ";\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                        }
                    }
                    break;
                case "ping":
                    codeTemp += "public static string ";
                    x++;

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                    {
                        tempId = TempGrid.Rows[x].Cells[1].Value.ToString();
                        x++;
                    }

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                    {
                        codeTemp += "[";
                        x++;

                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != "=" && TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                        {
                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Inter Literal")
                            {
                                dimensions++;
                                size[tracker] = Convert.ToInt32(TempGrid.Rows[x].Cells[1].Value.ToString());
                                x++;
                                tracker++;
                            }
                            else
                            {
                                x++;
                            }
                        }

                        if (dimensions == 1)
                        {
                            codeTemp += "] ";
                        }
                        else if (dimensions == 2)
                        {
                            codeTemp += ",] ";
                        }

                        codeTemp += " " + tempId + " " + " = new string[";

                        tempId = "";

                        if (dimensions == 1)
                        {
                            codeTemp += size[0] + "] ";
                        }
                        else if (dimensions == 2)
                        {
                            codeTemp += size[0] + "," + size[1] + "] ";
                        }

                        tracker = 0;
                        dimensions = 0;

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "=")
                        {
                            x++;
                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                        {
                            codeTemp += ";\n";
                            OutputText.Text += codeTemp;
                            codeTemp = "";
                            lineMapping.Add(lineTracker, currentLine);
                            lineTracker++;
                        }
                    }
                    else
                    {
                        codeTemp += tempId + " ";
                        tempId = "";
                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                        {
                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "=")
                            {
                                codeTemp += " = " + TempGrid.Rows[x + 1].Cells[1].Value.ToString();
                                x = x + 2;
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "," && TempGrid.Rows[x - 1].Cells[2].Value.ToString() == "Identifier")
                            {
                                codeTemp += "= default, ";
                                x++;
                            }
                            else
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                        {
                            if (TempGrid.Rows[x - 1].Cells[2].Value.ToString() == "Identifier")
                            {
                                codeTemp += " = default;\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                            else
                            {
                                codeTemp += ";\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                        }
                    }
                    break;
                case "pool":
                    codeTemp += "public static bool ";
                    x++;

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                    {
                        tempId = TempGrid.Rows[x].Cells[1].Value.ToString();
                        x++;
                    }

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                    {
                        codeTemp += "[";
                        x++;

                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != "=" && TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                        {
                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Inter Literal")
                            {
                                dimensions++;
                                size[tracker] = Convert.ToInt32(TempGrid.Rows[x].Cells[1].Value.ToString());
                                x++;
                                tracker++;
                            }
                            else
                            {
                                x++;
                            }
                        }

                        if (dimensions == 1)
                        {
                            codeTemp += "] ";
                        }
                        else if (dimensions == 2)
                        {
                            codeTemp += ",] ";
                        }

                        codeTemp += " " + tempId + " " + " = new bool[";

                        tempId = "";

                        if (dimensions == 1)
                        {
                            codeTemp += size[0] + "] ";
                        }
                        else if (dimensions == 2)
                        {
                            codeTemp += size[0] + "," + size[1] + "] ";
                        }

                        tracker = 0;
                        dimensions = 0;

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "=")
                        {
                            x++;
                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                        {
                            codeTemp += ";\n";
                            OutputText.Text += codeTemp;
                            codeTemp = "";
                            lineMapping.Add(lineTracker, currentLine);
                            lineTracker++;
                        }
                    }
                    else
                    {
                        codeTemp += tempId + " ";
                        tempId = "";
                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                        {
                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "=")
                            {
                                codeTemp += " = " + TempGrid.Rows[x + 1].Cells[1].Value.ToString();
                                x = x + 2;
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "," && TempGrid.Rows[x - 1].Cells[2].Value.ToString() == "Identifier")
                            {
                                codeTemp += "= default, ";
                                x++;
                            }
                            else
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                        {
                            if (TempGrid.Rows[x - 1].Cells[2].Value.ToString() == "Identifier")
                            {
                                codeTemp += " = default;\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                            else
                            {
                                codeTemp += ";\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                        }
                    }
                    break;
                case "newline":
                    currentLine++;
                    break;
                case "tower":
                    codeTemp += "public struct ";
                    x++;

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                    {
                        codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString() + " {\n";
                        x++;
                        lineMapping.Add(lineTracker, currentLine);
                        lineTracker++;
                    }

                    while (TempGrid.Rows[x].Cells[2].Value.ToString() != "}")
                    {
                        switch (TempGrid.Rows[x].Cells[2].Value.ToString())
                        {
                            case "inter":
                                codeTemp += "public int ";
                                x++;

                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString() + ";\n";
                                    x++;
                                    lineMapping.Add(lineTracker, currentLine);
                                    lineTracker++;
                                }
                                break;
                            case "bloat":
                                codeTemp += "public double ";
                                x++;

                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString() + ";\n";
                                    x++;
                                    lineMapping.Add(lineTracker, currentLine);
                                    lineTracker++;
                                }
                                break;
                            case "pool":
                                codeTemp += "public bool ";
                                x++;

                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString() + ";\n";
                                    x++;
                                    lineMapping.Add(lineTracker, currentLine);
                                    lineTracker++;
                                }
                                break;
                            case "ping":
                                codeTemp += "public string ";
                                x++;

                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString() + ";\n";
                                    x++;
                                    lineMapping.Add(lineTracker, currentLine);
                                    lineTracker++;
                                }
                                break;
                            case "newline":
                                x++;
                                currentLine++;
                                break;
                            default:
                                x++;
                                break;
                        }
                    }

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "}")
                    {
                        codeTemp += "}\n";
                        OutputText.Text += codeTemp;
                        codeTemp = "";
                        lineMapping.Add(lineTracker, currentLine);
                        lineTracker++;
                    }

                    break;
            }
        }

        bool isArray = false;
        string tempData = "";

        for (int x = count; x < TempGrid.Rows.Count; x++)
        {
            switch (TempGrid.Rows[x].Cells[2].Value.ToString())
            {
                case "spawn":
                    codeTemp += "public static ";
                    x++;

                    string returnType = TempGrid.Rows[x].Cells[2].Value.ToString();

                    if (returnType == "void")
                    {
                        codeTemp += "void ";
                        x++;

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "base")
                        {
                            codeTemp += "Main() {\n";

                            OutputText.Text += codeTemp;
                            codeTemp = "";
                            mainFlag = true;
                            x += 3;
                            lineMapping.Add(lineTracker, currentLine);
                            lineTracker++;
                        }
                        else
                        {
                            codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString() + "(";
                            x = x + 2;

                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != ")")
                            {
                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == "inter")
                                {
                                    codeTemp += "int";
                                    x++;
                                }
                                else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "bloat")
                                {
                                    codeTemp += "double";
                                    x++;
                                }
                                else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "ping")
                                {
                                    codeTemp += "string";
                                    x++;
                                }
                                else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "pool")
                                {
                                    codeTemp += "bool";
                                    x++;
                                }
                                else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                                {
                                    codeTemp += "[";
                                    x++;
                                }
                                else if (TempGrid.Rows[x].Cells[2].Value.ToString() == ",")
                                {
                                    codeTemp += ",";
                                    x++;
                                }
                                else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "]")
                                {
                                    codeTemp += "]";
                                    x++;
                                }
                                else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                                {
                                    codeTemp += " " + TempGrid.Rows[x].Cells[1].Value.ToString();
                                    x++;
                                }
                            }

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == ")")
                            {
                                codeTemp += ") {\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                x++;
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                        }
                    }
                    else
                    {
                        if (returnType == "inter")
                        {
                            codeTemp += "int ";
                        }
                        else if (returnType == "bloat")
                        {
                            codeTemp += "double ";
                        }
                        else if (returnType == "ping")
                        {
                            codeTemp += "string ";
                        }
                        else if (returnType == "pool")
                        {
                            codeTemp += "bool ";
                        }

                        x++;

                        codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString() + "(";
                        x = x + 2;

                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != ")")
                        {
                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "inter")
                            {
                                codeTemp += "int";
                                x++;
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "bloat")
                            {
                                codeTemp += "double";
                                x++;
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "ping")
                            {
                                codeTemp += "string";
                                x++;
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "pool")
                            {
                                codeTemp += "bool";
                                x++;
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                            {
                                codeTemp += "[";
                                x++;
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == ",")
                            {
                                codeTemp += ",";
                                x++;
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "]")
                            {
                                codeTemp += "]";
                                x++;
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                            {
                                codeTemp += " " + TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == ")")
                        {
                            codeTemp += ") {\n";
                            OutputText.Text += codeTemp;
                            codeTemp = "";
                            x++;
                            lineMapping.Add(lineTracker, currentLine);
                            lineTracker++;
                        }
                    }

                    break;
                case "comp":
                    if (TempGrid.Rows[x + 3].Cells[2].Value.ToString() == "[")
                    {
                        codeTemp += "ReadOnlyCollection<";
                        isArray = true;
                    }
                    else
                    {
                        codeTemp += "const ";
                    }

                    x++;

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "inter")
                    {
                        codeTemp += "int";
                        tempData = "int";
                    }
                    else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "bloat")
                    {
                        codeTemp += "double";
                        tempData = "double";
                    }
                    else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "ping")
                    {
                        codeTemp += "string";
                        tempData = "string";
                    }
                    else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "pool")
                    {
                        codeTemp += "bool";
                        tempData = "bool";
                    }

                    x++;

                    if (isArray)
                    {
                        codeTemp += "> " + TempGrid.Rows[x].Cells[1].Value.ToString() + " = Array.AsReadOnly(new " + tempData + "[";

                        x++;

                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != "=")
                        {
                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Inter Literal" || TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                            {
                                dimensions++;
                                size[tracker] = TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                                tracker++;
                            }
                            else
                            {
                                x++;
                            }
                        }

                        if (dimensions == 1)
                        {
                            codeTemp += size[0] + "] ";
                        }
                        else if (dimensions == 2)
                        {
                            codeTemp += size[0] + "," + size[1] + "] ";
                        }

                        tracker = 0;
                        dimensions = 0;

                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != "{")
                        {
                            x++;
                        }

                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                        {
                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Pool Literal")
                            {
                                if (TempGrid.Rows[x].Cells[1].Value.ToString() == "buff")
                                {
                                    codeTemp += " true";
                                }
                                else
                                {
                                    codeTemp += " false";
                                }
                            }
                            else
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                            }

                            x++;
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                        {
                            codeTemp += ");\n";
                            OutputText.Text += codeTemp;
                            codeTemp = "";
                            lineMapping.Add(lineTracker, currentLine);
                            lineTracker++;
                        }
                    }
                    else
                    {
                        codeTemp += " " + TempGrid.Rows[x].Cells[1].Value.ToString();
                        x++;
                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                        {
                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Pool Literal")
                            {
                                if (TempGrid.Rows[x].Cells[1].Value.ToString() == "buff")
                                {
                                    codeTemp += " true";
                                }
                                else
                                {
                                    codeTemp += " false";
                                }
                            }
                            else
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                            }
                            x++;
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                        {
                            codeTemp += ";\n";
                            OutputText.Text += codeTemp;
                            codeTemp = "";
                            lineMapping.Add(lineTracker, currentLine);
                            lineTracker++;
                        }
                    }

                    break;
                case "inter":
                case "bloat":
                case "ping":
                case "pool":
                    int openAssign = 0;
                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "inter")
                    {
                        codeTemp += "int";
                        tempData = "int";
                    }
                    else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "bloat")
                    {
                        codeTemp += "double";
                        tempData = "double";
                    }
                    else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "ping")
                    {
                        codeTemp += "string";
                        tempData = "string";
                    }
                    else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "pool")
                    {
                        codeTemp += "bool";
                        tempData = "bool";
                    }

                    x++;

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                    {
                        tempId = TempGrid.Rows[x].Cells[1].Value.ToString();
                        openAssign++;
                        x++;
                    }

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                    {
                        codeTemp += "[";
                        x++;

                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != "=" && TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                        {
                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Inter Literal" || TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                            {
                                dimensions++;
                                size[tracker] = TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                                tracker++;
                            }
                            else
                            {
                                x++;
                            }
                        }

                        if (dimensions == 1)
                        {
                            codeTemp += "] ";
                        }
                        else if (dimensions == 2)
                        {
                            codeTemp += ",] ";
                        }

                        codeTemp += " " + tempId + " " + " = new " + tempData + "[";

                        tempId = "";

                        if (dimensions == 1)
                        {
                            codeTemp += size[0] + "] ";
                        }
                        else if (dimensions == 2)
                        {
                            codeTemp += size[0] + "," + size[1] + "] ";
                        }

                        tracker = 0;
                        dimensions = 0;

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "=")
                        {
                            x++;
                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                            {
                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Pool Literal")
                                {
                                    if (TempGrid.Rows[x].Cells[1].Value.ToString() == "buff")
                                    {
                                        codeTemp += " true";
                                    }
                                    else
                                    {
                                        codeTemp += " false";
                                    }
                                }
                                else
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                }

                                x++;
                            }
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                        {
                            codeTemp += ";\n";
                            OutputText.Text += codeTemp;
                            codeTemp = "";
                            lineMapping.Add(lineTracker, currentLine);
                            lineTracker++;
                        }
                    }
                    else
                    {
                        codeTemp += " " + tempId + " ";
                        tempId = "";
                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                        {
                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "=")
                            {
                                codeTemp += " = ";
                                openAssign--;
                                x++;
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == ",")
                            { 
                                if(openAssign != 0)
                                {
                                    codeTemp += "= default, ";
                                    openAssign--;
                                }
                                else
                                {
                                    codeTemp += ", ";
                                }
                                
                                x++;
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "hold")
                            {
                                codeTemp += "Console.ReadLine()";
                                x = x + 3;
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "inter" || TempGrid.Rows[x].Cells[2].Value.ToString() == "bloat" || TempGrid.Rows[x].Cells[2].Value.ToString() == "ping" || TempGrid.Rows[x].Cells[2].Value.ToString() == "pool")
                            {
                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == "inter")
                                {
                                    tempData = "Int32";
                                }
                                else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "bloat")
                                {
                                    tempData = "Double";
                                }
                                else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "ping")
                                {
                                    tempData = "String";
                                }
                                else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "pool")
                                {
                                    tempData = "Boolean";
                                }

                                codeTemp += "Convert.To" + tempData;
                                x++;

                                while (TempGrid.Rows[x].Cells[2].Value.ToString() != ")")
                                {
                                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "hold")
                                    {
                                        codeTemp += "Console.ReadLine()";
                                        x = x + 3;
                                    }
                                    else
                                    {
                                        codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                        x++;
                                    }
                                }
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Pool Literal")
                            {
                                if (TempGrid.Rows[x].Cells[1].Value.ToString() == "buff")
                                {
                                    codeTemp += " true";
                                }
                                else
                                {
                                    codeTemp += " false";
                                }

                                x++;
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                            {
                                if (TempGrid.Rows[x-1].Cells[2].Value.ToString() != "+" && TempGrid.Rows[x-1].Cells[2].Value.ToString() != "-" && TempGrid.Rows[x-1].Cells[2].Value.ToString() != "*" && TempGrid.Rows[x-1].Cells[2].Value.ToString() != "/" && TempGrid.Rows[x-1].Cells[2].Value.ToString() != "%" && TempGrid.Rows[x-1].Cells[2].Value.ToString() != "=" && TempGrid.Rows[x-1].Cells[2].Value.ToString() != "&&" && TempGrid.Rows[x-1].Cells[2].Value.ToString() != "||" && TempGrid.Rows[x-1].Cells[2].Value.ToString() != "!=" && TempGrid.Rows[x-1].Cells[2].Value.ToString() != "==" && TempGrid.Rows[x-1].Cells[2].Value.ToString() != "!" && TempGrid.Rows[x-1].Cells[2].Value.ToString() != "(")
                                {
                                    openAssign++;
                                }

                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }
                            else
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                        {
                            if (TempGrid.Rows[x - 1].Cells[2].Value.ToString() == "Identifier" && TempGrid.Rows[x - 2].Cells[2].Value.ToString() != "." && openAssign != 0)
                            {
                                codeTemp += " = default;\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                            else
                            {
                                codeTemp += ";\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                            }
                        }
                    }

                    break;
                case "push":
                    x++;
                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "(")
                    {
                        codeTemp += "Console.WriteLine(";

                        do
                        {
                            x++;

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Ping Literal" || TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier" || TempGrid.Rows[x].Cells[2].Value.ToString() == ")" || TempGrid.Rows[x].Cells[2].Value.ToString() == "+" || TempGrid.Rows[x].Cells[2].Value.ToString() == "Pool Literal" || TempGrid.Rows[x].Cells[2].Value.ToString() == "ping")
                            {
                                do
                                {
                                    switch (TempGrid.Rows[x].Cells[2].Value.ToString())
                                    {
                                        case "Ping Literal":
                                            codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                            x++;
                                            break;
                                        case ")":
                                            codeTemp += ")";
                                            x++;
                                            break;
                                        case "Identifier":
                                            codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                            x++;
                                            break;
                                        case "+":
                                            codeTemp += "+";
                                            x++;
                                            break;
                                        case "Pool Literal":
                                            if (TempGrid.Rows[x].Cells[1].Value.ToString() == "buff")
                                            {
                                                codeTemp += "true";
                                            }
                                            else
                                            {
                                                codeTemp += "false";
                                            }

                                            x++;
                                            break;
                                        case "ping":
                                            codeTemp += "Convert.ToString";
                                            x++;
                                            break;
                                        case "inter":
                                            codeTemp += "Convert.ToInt32";
                                            x++;
                                            break;
                                        case "bloat":
                                            codeTemp += "Convert.ToDouble";
                                            x++;
                                            break;
                                        case "pool":
                                            codeTemp += "Convert.ToBoolean";
                                            x++;
                                            break;
                                        case "[":
                                            codeTemp += "[";
                                            x++;

                                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != "]")
                                            {
                                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                                x++;

                                                if (TempGrid.Rows[x + 1].Cells[2].Value.ToString() == "[")
                                                {
                                                    codeTemp += ", ";
                                                    x = x + 2;
                                                }
                                            }

                                            codeTemp += "]";
                                            x++;
                                            break;
                                        default:
                                            codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                            x++;
                                            break;
                                    }
                                } while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";");
                            }
                        } while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";");

                        codeTemp += ";\n";
                        OutputText.Text += codeTemp;
                        codeTemp = "";
                        lineMapping.Add(lineTracker, currentLine);
                        lineTracker++;
                    }

                    break;
                case "tower":
                    x++;

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                    {
                        codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString() + " ";
                        x++;
                    }

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                    {
                        codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                        x++;
                    }

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                    {
                        codeTemp += ";\n";
                        OutputText.Text += codeTemp;
                        x++;
                        codeTemp = "";
                        lineMapping.Add(lineTracker, currentLine);
                        lineTracker++;
                    }
                    break;
                case "Identifier":
                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                    x++;

                    while (TempGrid.Rows[x].Cells[1].Value.ToString() != ";")
                    {
                        if (TempGrid.Rows[x].Cells[1].Value.ToString() == "hold")
                        {
                            codeTemp += "Console.ReadLine()";
                            x = x + 3;
                        }
                        else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "inter" || TempGrid.Rows[x].Cells[2].Value.ToString() == "bloat" || TempGrid.Rows[x].Cells[2].Value.ToString() == "ping" || TempGrid.Rows[x].Cells[2].Value.ToString() == "pool")
                        {
                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "inter")
                            {
                                tempData = "Int32";
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "bloat")
                            {
                                tempData = "Double";
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "ping")
                            {
                                tempData = "String";
                            }
                            else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "pool")
                            {
                                tempData = "Boolean";
                            }

                            codeTemp += "Convert.To" + tempData;
                            x++;

                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != ")")
                            {
                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == "hold")
                                {
                                    codeTemp += "Console.ReadLine()";
                                    x = x + 3;
                                }
                                else
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                    x++;
                                }
                            }
                        }
                        else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Pool Literal")
                        {
                            if (TempGrid.Rows[x].Cells[1].Value.ToString() == "buff")
                            {
                                codeTemp += " true";
                            }
                            else
                            {
                                codeTemp += " false";
                            }
                            x++;
                        }
                        else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                        {
                            codeTemp += "[";
                            x++;

                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != "]")
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;

                                if (TempGrid.Rows[x + 1].Cells[2].Value.ToString() == "[")
                                {
                                    codeTemp += ", ";
                                    x = x + 2;
                                }
                            }
                        }
                        else
                        {
                            codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                            x++;
                        }
                    }

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                    {
                        codeTemp += ";\n";
                        OutputText.Text += codeTemp;
                        codeTemp = "";
                        lineMapping.Add(lineTracker, currentLine);
                        lineTracker++;
                    }

                    break;
                case "hold":
                    codeTemp += "Console.ReadLine();\n";
                    OutputText.Text += codeTemp;
                    codeTemp = "";
                    lineMapping.Add(lineTracker, currentLine);
                    lineTracker++;
                    x = x + 3;
                    break;
                case "for":
                    codeTemp += "for (";
                    x++;

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                    {
                        tempId = TempGrid.Rows[x].Cells[1].Value.ToString();
                        codeTemp += "int " + tempId + " = ";
                        x = x + 2;
                    }

                    while (TempGrid.Rows[x].Cells[2].Value.ToString() != "up" && TempGrid.Rows[x].Cells[2].Value.ToString() != "down")
                    {
                        codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                        x++;
                    }

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "up")
                    {
                        codeTemp += "; " + tempId + " <= ";
                        x++;

                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != "{")
                        {
                            codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                            x++;
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "{")
                        {
                            codeTemp += "; " + tempId + "++) {\n";
                            OutputText.Text += codeTemp;
                            codeTemp = "";
                            lineMapping.Add(lineTracker, currentLine);
                            lineTracker++;
                            openBrace++;
                        }
                    }
                    else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "down")
                    {
                        codeTemp += "; " + tempId + " >= ";
                        x++;

                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != "{")
                        {
                            codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                            x++;
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "{")
                        {
                            codeTemp += "; " + tempId + "--) {\n";
                            OutputText.Text += codeTemp;
                            codeTemp = "";
                            lineMapping.Add(lineTracker, currentLine);
                            lineTracker++;
                            openBrace++;
                        }
                    }


                    break;
                case "do":
                    codeTemp += "do {\n";
                    OutputText.Text += codeTemp;
                    codeTemp = "";
                    lineMapping.Add(lineTracker, currentLine);
                    lineTracker++;
                    openBrace++;
                    openDo++;
                    break;
                case "while":
                    codeTemp += "while (";
                    x = x + 2;

                    while (TempGrid.Rows[x].Cells[2].Value.ToString() != ")")
                    {
                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Pool Literal")
                        {
                            if (TempGrid.Rows[x].Cells[1].Value.ToString() == "buff")
                            {
                                codeTemp += "true";
                            }
                            else
                            {
                                codeTemp += "false";
                            }
                        }
                        else
                        {
                            codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                        }

                        x++;
                    }

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == ")")
                    {
                        codeTemp += ") {\n";
                        OutputText.Text += codeTemp;
                        codeTemp = "";
                        lineMapping.Add(lineTracker, currentLine);
                        lineTracker++;
                        openBrace++;
                    }
                    break;
                case "if":
                    codeTemp += "if (";
                    x = x + 2;

                    while (TempGrid.Rows[x].Cells[2].Value.ToString() != ")")
                    {
                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Pool Literal")
                        {
                            if (TempGrid.Rows[x].Cells[1].Value.ToString() == "buff")
                            {
                                codeTemp += "true";
                            }
                            else
                            {
                                codeTemp += "false";
                            }
                        }
                        else
                        {
                            codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                        }

                        x++;
                    }

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == ")")
                    {
                        codeTemp += ")";
                        x++;
                    }

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "{")
                    {
                        codeTemp += " {\n";
                        OutputText.Text += codeTemp;
                        codeTemp = "";
                        lineMapping.Add(lineTracker, currentLine);
                        lineTracker++;
                        openBrace++;
                    }
                    else
                    {
                        codeTemp += "\n";
                        OutputText.Text += codeTemp;
                        codeTemp = "";
                        lineMapping.Add(lineTracker, currentLine);
                        lineTracker++;
                    }
                    break;
                case "else":
                    codeTemp += "else ";
                    x++;

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "if")
                    {
                        codeTemp += "if (";
                        x = x + 2;

                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != ")")
                        {
                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Pool Literal")
                            {
                                if (TempGrid.Rows[x].Cells[1].Value.ToString() == "buff")
                                {
                                    codeTemp += "true";
                                }
                                else
                                {
                                    codeTemp += "false";
                                }
                            }
                            else
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                            }

                            x++;
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == ")")
                        {
                            codeTemp += ")";
                            x++;
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "{")
                        {
                            codeTemp += " {\n";
                            OutputText.Text += codeTemp;
                            x++;
                            codeTemp = "";
                            lineMapping.Add(lineTracker, currentLine);
                            lineTracker++;
                            openBrace++;
                        }
                        else
                        {
                            codeTemp += "\n";
                            OutputText.Text += codeTemp;
                            codeTemp = "";
                            lineMapping.Add(lineTracker, currentLine);
                            lineTracker++;
                        }
                    }
                    else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "{")
                    {
                        codeTemp += " {\n";
                        OutputText.Text += codeTemp;
                        x++;
                        codeTemp = "";
                        lineMapping.Add(lineTracker, currentLine);
                        lineTracker++;
                        openBrace++;
                    }
                    else
                    {
                        codeTemp += "\n";
                        OutputText.Text += codeTemp;
                        codeTemp = "";
                        lineMapping.Add(lineTracker, currentLine);
                        lineTracker++;
                    }
                    break;
                case "commit":
                    codeTemp += "continue;\n";
                    OutputText.Text += codeTemp;
                    codeTemp = "";
                    lineMapping.Add(lineTracker, currentLine);
                    lineTracker++;
                    x = x + 2;
                    break;
                case "destroy":
                    codeTemp += "break;\n";
                    OutputText.Text += codeTemp;
                    codeTemp = "";
                    lineMapping.Add(lineTracker, currentLine);
                    lineTracker++;
                    x = x + 2;
                    break;
                case "recall":
                    codeTemp += "return ";
                    x++;

                    while (TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                    {
                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Pool Literal")
                        {
                            if (TempGrid.Rows[x].Cells[1].Value.ToString() == "buff")
                            {
                                codeTemp += "true";
                            }
                            else
                            {
                                codeTemp += "false";
                            }
                        }
                        else
                        {
                            codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                        }

                        x++;
                    }

                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                    {
                        codeTemp += ";\n";
                        OutputText.Text += codeTemp;
                        codeTemp = "";
                        lineMapping.Add(lineTracker, currentLine);
                        lineTracker++;
                    }

                    break;
                case "}":
                    if (openBrace == 0 && openDo == 0)
                    {
                        if (x != lastValue - 1)
                        {
                            while (TempGrid.Rows[x].Cells[2].Value.ToString() == "newline")
                            {
                                if (x == lastValue - 1)
                                {
                                    break;
                                }
                                else
                                {
                                    x++;
                                    currentLine++;
                                }
                            }

                            if (x != lastValue - 1)
                            {
                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == "spawn")
                                {
                                    if (mainFlag)
                                    {
                                        OutputText.Text += "Console.ReadLine();}\n";
                                        mainFlag = false;
                                        lineTracker++;
                                    }
                                    else
                                    {
                                        OutputText.Text += "}\n";
                                        lineTracker++;
                                    }
                                }
                                else
                                {
                                    if (mainFlag)
                                    {
                                        OutputText.Text += "Console.ReadLine();}\n";
                                        mainFlag = false;
                                        lineTracker++;
                                    }
                                    else
                                    {
                                        OutputText.Text += "}\n";
                                        lineTracker++;
                                    }
                                }
                            }
                            else
                            {
                                if (mainFlag)
                                {
                                    OutputText.Text += "Console.ReadLine();}\n";
                                    mainFlag = false;
                                    lineTracker++;
                                }
                                else
                                {
                                    OutputText.Text += "}\n";
                                    lineTracker++;
                                }
                            }
                        }
                        else
                        {
                            if (mainFlag)
                            {
                                OutputText.Text += "Console.ReadLine();}\n";
                                mainFlag = false;
                                lineTracker++;
                            }
                            else
                            {
                                OutputText.Text += "}\n";
                                lineTracker++;
                            }
                        }
                    }
                    else
                    {
                        if (openDo != 0)
                        {
                            codeTemp += "} while (";
                            x = x + 3;

                            while (TempGrid.Rows[x].Cells[2].Value.ToString() != ")")
                            {
                                codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                            }

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == ")")
                            {
                                codeTemp += ");\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
                                lineMapping.Add(lineTracker, currentLine);
                                lineTracker++;
                                openBrace--;
                                openDo--;
                            }
                        }
                        else
                        {
                            OutputText.Text += "}\n";
                            lineTracker++;
                            openBrace--;
                        }
                    }

                    break;
                case "newline":
                    currentLine++;
                    break;
            }
        }

        OutputText.Text += "\n}";

        string code = OutputText.Text;
        return code;
    }

    public async Task ExecuteCodeAsync(string code)
    {
        string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);

        try
        {
            string tempFilePath = Path.Combine(tempDir, "Program.cs");
            await File.WriteAllTextAsync(tempFilePath, code);

            string csprojContent = @"<Project Sdk=""Microsoft.NET.Sdk""><PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net8.0</TargetFramework></PropertyGroup></Project>";
            string csprojPath = Path.Combine(tempDir, "TempProject.csproj");
            await File.WriteAllTextAsync(csprojPath, csprojContent);

            ProcessStartInfo startInfo = new ProcessStartInfo("dotnet")
            {
                Arguments = $"run --project \"{csprojPath}\"",
                WorkingDirectory = tempDir,
                UseShellExecute = true,
                CreateNoWindow = false,
            };

            using (Process process = Process.Start(startInfo))
            {
                if (process != null)
                {
                    await process.WaitForExitAsync();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        finally
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
    }

    public async Task AnalyzeCodeAsync(string code)
    {
        string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);
        hasError = false;
        semanticError.Rows.Clear();
        int id = 0;

        try
        {
            string tempFilePath = Path.Combine(tempDir, "Program.cs");
            await File.WriteAllTextAsync(tempFilePath, code);

            string csprojContent = @"<Project Sdk=""Microsoft.NET.Sdk""><PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net8.0</TargetFramework></PropertyGroup></Project>";
            string csprojPath = Path.Combine(tempDir, "TempProject.csproj");
            await File.WriteAllTextAsync(csprojPath, csprojContent);

            ProcessStartInfo startInfo = new ProcessStartInfo("dotnet")
            {
                Arguments = $"run --project \"{csprojPath}\"",
                WorkingDirectory = tempDir,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        string output = e.Data;

                        foreach (KeyValuePair<int, int> pair in lineMapping)
                        {
                            TempGrid.Rows.Add(0, pair.Key, pair.Value);
                        }

                        if (output.Contains("error"))
                        {
                            string pattern = @"\((-?\d+),(-?\d+)\)";
                            string[] parts = output.Split(':');
                            string result;

                            Regex regex = new Regex(pattern);
                            MatchCollection matches = regex.Matches(output);
                            int originalLine = Convert.ToInt32(matches[0].Groups[1].Value);

                            int codeLine = lineMapping.FirstOrDefault(x => x.Key == originalLine).Value;

                            if (parts[2].Contains("error"))
                            {
                                if (parts.Count() == 6)
                                {
                                    result = parts[3] + ": " + parts[4].Replace("[C", "");
                                }
                                else
                                {
                                    result = parts[3].Replace("[C", "");
                                }

                                result = result.Trim();

                                result = result.Replace("bool", "pool");
                                result = result.Replace("int", "inter");
                                result = result.Replace("double", "bloat");
                                result = result.Replace("string", "ping");
                                result = result.Replace(",property or indexer", "");
                                result = result.Replace("The type", "");

                                semanticError.Rows.Add(id, result, codeLine);
                                id++;

                                hasError = true;
                            }
                        }
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                bool exited = await Task.Run(() => process.WaitForExit(1500));
                if (!exited)
                {
                    process.Kill();
                }
            }
        }
        catch (Exception ex)
        {
            semanticError.Rows.Add(id, ex.Message, 0);
        }
        finally
        {
            if (Directory.Exists(tempDir))
            {
                await Task.Delay(100);

                bool deleted = false;
                int attempts = 0;
                const int maxAttempts = 5;

                while (!deleted && attempts < maxAttempts)
                {
                    try
                    {
                        Directory.Delete(tempDir, true);
                        deleted = true;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        attempts++;
                        await Task.Delay(100);
                    }
                    catch (IOException)
                    {
                        attempts++;
                        await Task.Delay(100);
                    }
                }
            }
        }
    }

    private void Code_Load(object sender, EventArgs e)
    {

    }
}