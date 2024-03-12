using LexicalAnalyzer;
using Syntax_Analyzer;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace WinFormsApp1;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        syntax.Enabled = false;
        semantic.Enabled = false;
        // run.Enabled = false;
    }

    Analyzer lex = new Analyzer();
    string txt;
    Dictionary<int, int> lineMapping = new Dictionary<int, int>();

    private void lexer_Click(object sender, EventArgs e)
    {
        LexGrid.Rows.Clear();
        TempGrid.Rows.Clear();
        DataLexicalError.Rows.Clear();
        DataSyntaxError.Rows.Clear();
        syntax.Enabled = false;
        semantic.Enabled = false;
        // run.Enabled = false;

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
                // run.Enabled = false;
            }
        }
    }

    private void syntax_Click(object sender, EventArgs e)
    {
        SyntaxInitializer syntaxInitializer = new SyntaxInitializer();
        DataSyntaxError.Rows.Clear();
        semantic.Enabled = false;
        // run.Enabled = false;

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
            // run.Enabled = false;
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

                if(token.getTokens() != "space" && token.getTokens() != "tab")
                {
                    TempGrid.Rows.Add(id, token.getLexemes(), token.getTokens());
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
            // run.Enabled = false;
        LexGrid.Rows.Clear();
        DataLexicalError.Rows.Clear();
        DataSyntaxError.Rows.Clear();
        Code.RichTextBox.Clear();
    }

    private void DataLexicalError_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }

    private void semantic_Click(object sender, EventArgs e)
    {
        
    } 

    private async void semanticError_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        // string code = @"using System; class Program { public static void Main() { int a; int b = 12.123; Console.WriteLine(a); Console.ReadLine();} }";
        string code = TranslateCode();
        
        await AnalyzeCodeAsync(code);
    }

    private async void run_Click(object sender, EventArgs e)
    {
        // string code = @"using System; class Program { public static void Main() { int a; int b = 12.123; Console.WriteLine(a); Console.ReadLine();} }";
        tabControl2.SelectTab("tabPage5");
        
        string code = TranslateCode();

        // await ExecuteCodeAsync(code);
        // await AnalyzeCodeAsync(code);
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
        int[] size = new int[2];
        int lineTracker = 2;
        int currentLine = 1;
        lineMapping.Clear();

        OutputText.Text = "using System; using System.Collections.ObjectModel; class Summoner { \n";

        for (int x = 0; x < TempGrid.Rows.Count; x++)
        {
            while (TempGrid.Rows[x].Cells[2].Value.ToString() != "spawn")
            {
                switch(TempGrid.Rows[x].Cells[2].Value.ToString())
                {
                    case "comp":
                        codeTemp += "public ";
                        x++;
                        count++;

                        if(TempGrid.Rows[x].Cells[2].Value.ToString() == "inter")
                        {
                            datatype += "int";
                            x++;
                            count++;
                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                            {
                                tempId = TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                                count++;
                            }

                            if(TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                            {
                                codeTemp += "readonly " + datatype + "[";
                                x++;
                                count++;
                                datatype = "";

                                while(TempGrid.Rows[x].Cells[2].Value.ToString() != "=")
                                {
                                    if(TempGrid.Rows[x].Cells[2].Value.ToString() == "Inter Literal")
                                    {
                                        dimensions++;
                                        size[tracker] = Convert.ToInt32(TempGrid.Rows[x].Cells[1].Value.ToString());
                                        x++;
                                        count++;
                                        tracker++;
                                    }
                                    else
                                    {
                                        x++;
                                        count++;
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

                                codeTemp += " " + tempId + " " + " = new int[" ;

                                tempId = "";

                                if (dimensions == 1)
                                {
                                    codeTemp += size[0] + "] ";
                                }
                                else if (dimensions == 2)
                                {
                                    codeTemp += size[0] + "," + size[1] +"] ";
                                }

                                tracker = 0;

                                while(TempGrid.Rows[x].Cells[2].Value.ToString() != "{")
                                {
                                    x++;
                                    count++;
                                }

                                while(TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                    x++;
                                    count++;
                                }

                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                                {
                                    codeTemp += ";\n";
                                    OutputText.Text += codeTemp;
                                    codeTemp = "";
                                    lineMapping.Add(currentLine, lineTracker);
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
                                    count++;
                                }

                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                                {
                                    codeTemp += ";\n";
                                    OutputText.Text += codeTemp;
                                    codeTemp = "";
                                    lineMapping.Add(currentLine, lineTracker);
                                    lineTracker++;
                                }
                            }
                        }
                        else if(TempGrid.Rows[x].Cells[2].Value.ToString() == "bloat")
                        {
                            datatype += "double";
                            x++;
                            count++;

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                            {
                                tempId = TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                                count++;
                            }

                            if(TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                            {
                                codeTemp += "readonly " + datatype + "[";
                                x++;
                                count++;
                                datatype = "";

                                while(TempGrid.Rows[x].Cells[2].Value.ToString() != "=")
                                {
                                    if(TempGrid.Rows[x].Cells[2].Value.ToString() == "Inter Literal")
                                    {
                                        dimensions++;
                                        size[tracker] = Convert.ToInt32(TempGrid.Rows[x].Cells[1].Value.ToString());
                                        x++;
                                        count++;
                                        tracker++;
                                    }
                                    else
                                    {
                                        x++;
                                        count++;
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

                                codeTemp += " " + tempId + " " + " = new double[" ;

                                tempId = "";

                                if (dimensions == 1)
                                {
                                    codeTemp += size[0] + "] ";
                                }
                                else if (dimensions == 2)
                                {
                                    codeTemp += size[0] + "," + size[1] +"] ";
                                }

                                tracker = 0;

                                while(TempGrid.Rows[x].Cells[2].Value.ToString() != "{")
                                {
                                    x++;
                                    count++;
                                }

                                while(TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                    x++;
                                    count++;
                                }

                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                                {
                                    codeTemp += ";\n";
                                    OutputText.Text += codeTemp;
                                    codeTemp = "";
                                    lineMapping.Add(currentLine, lineTracker);
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
                                    count++;
                                }

                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                                {
                                    codeTemp += ";\n";
                                    OutputText.Text += codeTemp;
                                    codeTemp = "";
                                    lineMapping.Add(currentLine, lineTracker);
                                    lineTracker++;
                                }
                            }
                        }
                        else if(TempGrid.Rows[x].Cells[2].Value.ToString() == "ping")
                        {
                            datatype += "string";
                            x++;
                            count++;

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                            {
                                tempId = TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                                count++;
                            }

                            if(TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                            {
                                codeTemp += "readonly " + datatype + "[";
                                x++;
                                count++;
                                datatype = "";

                                while(TempGrid.Rows[x].Cells[2].Value.ToString() != "=")
                                {
                                    if(TempGrid.Rows[x].Cells[2].Value.ToString() == "Inter Literal")
                                    {
                                        dimensions++;
                                        size[tracker] = Convert.ToInt32(TempGrid.Rows[x].Cells[1].Value.ToString());
                                        x++;
                                        count++;
                                        tracker++;
                                    }
                                    else
                                    {
                                        x++;
                                        count++;
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

                                codeTemp += " " + tempId + " " + " = new string[" ;

                                tempId = "";

                                if (dimensions == 1)
                                {
                                    codeTemp += size[0] + "] ";
                                }
                                else if (dimensions == 2)
                                {
                                    codeTemp += size[0] + "," + size[1] +"] ";
                                }

                                tracker = 0;

                                while(TempGrid.Rows[x].Cells[2].Value.ToString() != "{")
                                {
                                    x++;
                                    count++;
                                }

                                while(TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                    x++;
                                    count++;
                                }

                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                                {
                                    codeTemp += ";\n";
                                    OutputText.Text += codeTemp;
                                    codeTemp = "";
                                    lineMapping.Add(currentLine, lineTracker);
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
                                    count++;
                                }

                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                                {
                                    codeTemp += ";\n";
                                    OutputText.Text += codeTemp;
                                    codeTemp = "";
                                    lineMapping.Add(currentLine, lineTracker);
                                    lineTracker++;
                                }
                            }
                        }
                        else if(TempGrid.Rows[x].Cells[2].Value.ToString() == "pool")
                        {
                            datatype += "bool";
                            x++;
                            count++;
                            
                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                            {
                                tempId = TempGrid.Rows[x].Cells[1].Value.ToString();
                                x++;
                                count++;
                            }

                            if(TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                            {
                                codeTemp += "readonly " + datatype + "[";
                                x++;
                                count++;
                                datatype = "";

                                while(TempGrid.Rows[x].Cells[2].Value.ToString() != "=")
                                {
                                    if(TempGrid.Rows[x].Cells[2].Value.ToString() == "Inter Literal")
                                    {
                                        dimensions++;
                                        size[tracker] = Convert.ToInt32(TempGrid.Rows[x].Cells[1].Value.ToString());
                                        x++;
                                        count++;
                                        tracker++;
                                    }
                                    else
                                    {
                                        x++;
                                        count++;
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

                                codeTemp += " " + tempId + " " + " = new bool[" ;

                                tempId = "";

                                if (dimensions == 1)
                                {
                                    codeTemp += size[0] + "] ";
                                }
                                else if (dimensions == 2)
                                {
                                    codeTemp += size[0] + "," + size[1] +"] ";
                                }

                                tracker = 0;

                                while(TempGrid.Rows[x].Cells[2].Value.ToString() != "{")
                                {
                                    x++;
                                    count++;
                                }

                                while(TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                    x++;
                                    count++;
                                }

                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                                {
                                    codeTemp += ";\n";
                                    OutputText.Text += codeTemp;
                                    codeTemp = "";
                                    lineMapping.Add(currentLine, lineTracker);
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
                                    if(TempGrid.Rows[x].Cells[2].Value.ToString() == "Pool Literal")
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
                                    else {
                                        codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                    }
                                    x++;
                                    count++;
                                }

                                if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                                {
                                    codeTemp += ";\n";
                                    OutputText.Text += codeTemp;
                                    codeTemp = "";
                                    lineMapping.Add(currentLine, lineTracker);
                                    lineTracker++;
                                }
                            }
                        }

                        break;
                    case "inter":
                        codeTemp += "public static int ";
                        x++;
                        count++;

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                        {
                            tempId = TempGrid.Rows[x].Cells[1].Value.ToString();
                            x++;
                            count++;
                        }

                        if(TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                        {
                            codeTemp += "[";
                            x++;
                            count++;

                            while(TempGrid.Rows[x].Cells[2].Value.ToString() != "=" && TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                            {
                                if(TempGrid.Rows[x].Cells[2].Value.ToString() == "Inter Literal")
                                {
                                    dimensions++;
                                    size[tracker] = Convert.ToInt32(TempGrid.Rows[x].Cells[1].Value.ToString());
                                    x++;
                                    count++;
                                    tracker++;
                                }
                                else
                                {
                                    x++;
                                    count++;
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

                            codeTemp += " " + tempId + " " + " = new int[" ;

                            tempId = "";

                            if (dimensions == 1)
                            {
                                codeTemp += size[0] + "] ";
                            }
                            else if (dimensions == 2)
                            {
                                codeTemp += size[0] + "," + size[1] +"] ";
                            }

                            tracker = 0;

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "=")
                            {
                                x++;
                                count++;
                                while(TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                    x++;
                                    count++;
                                }
                            }

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                            {
                                codeTemp += ";\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
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
                                    codeTemp += " = " + TempGrid.Rows[x+1].Cells[1].Value.ToString();
                                    x = x + 2;
                                    count = count + 2;
                                }
                                else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "," && TempGrid.Rows[x-1].Cells[2].Value.ToString() == "Identifier")
                                {
                                    codeTemp += "= default, ";
                                    x++;
                                    count++;
                                }
                                else
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                    x++;
                                    count++;
                                }
                            }

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                            {
                                if (TempGrid.Rows[x-1].Cells[2].Value.ToString() == "Identifier")
                                {
                                    codeTemp += " = default;\n";
                                    OutputText.Text += codeTemp;
                                    codeTemp = "";
                                    lineMapping.Add(currentLine, lineTracker);
                                    lineTracker++;
                                }
                                else
                                {
                                    codeTemp += ";\n";
                                    OutputText.Text += codeTemp;
                                    codeTemp = "";
                                    lineMapping.Add(currentLine, lineTracker);
                                    lineTracker++;
                                }
                            }
                        }
                        break;
                    case "bloat":
                        codeTemp += "public static double ";
                        x++;
                        count++;

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                        {
                            tempId = TempGrid.Rows[x].Cells[1].Value.ToString();
                            x++;
                            count++;
                        }

                        if(TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                        {
                            codeTemp += "[";
                            x++;
                            count++;

                            while(TempGrid.Rows[x].Cells[2].Value.ToString() != "=" && TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                            {
                                if(TempGrid.Rows[x].Cells[2].Value.ToString() == "Inter Literal")
                                {
                                    dimensions++;
                                    size[tracker] = Convert.ToInt32(TempGrid.Rows[x].Cells[1].Value.ToString());
                                    x++;
                                    count++;
                                    tracker++;
                                }
                                else
                                {
                                    x++;
                                    count++;
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

                            codeTemp += " " + tempId + " " + " = new int[" ;

                            tempId = "";

                            if (dimensions == 1)
                            {
                                codeTemp += size[0] + "] ";
                            }
                            else if (dimensions == 2)
                            {
                                codeTemp += size[0] + "," + size[1] +"] ";
                            }

                            tracker = 0;

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "=")
                            {
                                x++;
                                count++;
                                while(TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                    x++;
                                    count++;
                                }
                            }

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                            {
                                codeTemp += ";\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
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
                                    codeTemp += " = " + TempGrid.Rows[x+1].Cells[1].Value.ToString();
                                    x = x + 2;
                                    count = count + 2;
                                }
                                else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "," && TempGrid.Rows[x-1].Cells[2].Value.ToString() == "Identifier")
                                {
                                    codeTemp += "= default, ";
                                    x++;
                                    count++;
                                }
                                else
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                    x++;
                                    count++;
                                }
                            }

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                            {
                                if (TempGrid.Rows[x-1].Cells[2].Value.ToString() == "Identifier")
                                {
                                    codeTemp += " = default;\n";
                                    OutputText.Text += codeTemp;
                                    codeTemp = "";
                                    lineMapping.Add(currentLine, lineTracker);
                                    lineTracker++;
                                }
                                else
                                {
                                    codeTemp += ";\n";
                                    OutputText.Text += codeTemp;
                                    codeTemp = "";
                                    lineMapping.Add(currentLine, lineTracker);
                                    lineTracker++;
                                }
                            }
                        }
                        break;
                    case "ping":
                        codeTemp += "public static string ";
                        x++;
                        count++;

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                        {
                            tempId = TempGrid.Rows[x].Cells[1].Value.ToString();
                            x++;
                            count++;
                        }

                        if(TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                        {
                            codeTemp += "[";
                            x++;
                            count++;

                            while(TempGrid.Rows[x].Cells[2].Value.ToString() != "=" && TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                            {
                                if(TempGrid.Rows[x].Cells[2].Value.ToString() == "Inter Literal")
                                {
                                    dimensions++;
                                    size[tracker] = Convert.ToInt32(TempGrid.Rows[x].Cells[1].Value.ToString());
                                    x++;
                                    count++;
                                    tracker++;
                                }
                                else
                                {
                                    x++;
                                    count++;
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

                            codeTemp += " " + tempId + " " + " = new int[" ;

                            tempId = "";

                            if (dimensions == 1)
                            {
                                codeTemp += size[0] + "] ";
                            }
                            else if (dimensions == 2)
                            {
                                codeTemp += size[0] + "," + size[1] +"] ";
                            }

                            tracker = 0;

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "=")
                            {
                                x++;
                                count++;
                                while(TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                    x++;
                                    count++;
                                }
                            }

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                            {
                                codeTemp += ";\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
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
                                    codeTemp += " = " + TempGrid.Rows[x+1].Cells[1].Value.ToString();
                                    x = x + 2;
                                    count = count + 2;
                                }
                                else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "," && TempGrid.Rows[x-1].Cells[2].Value.ToString() == "Identifier")
                                {
                                    codeTemp += "= default, ";
                                    x++;
                                    count++;
                                }
                                else
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                    x++;
                                    count++;
                                }
                            }

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                            {
                                if (TempGrid.Rows[x-1].Cells[2].Value.ToString() == "Identifier")
                                {
                                    codeTemp += " = default;\n";
                                    OutputText.Text += codeTemp;
                                    codeTemp = "";
                                    lineMapping.Add(currentLine, lineTracker);
                                    lineTracker++;
                                }
                                else
                                {
                                    codeTemp += ";\n";
                                    OutputText.Text += codeTemp;
                                    codeTemp = "";
                                    lineMapping.Add(currentLine, lineTracker);
                                    lineTracker++;
                                }
                            }
                        }
                        break;
                    case "pool":
                        codeTemp += "public static bool ";
                        x++;
                        count++;

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                        {
                            tempId = TempGrid.Rows[x].Cells[1].Value.ToString();
                            x++;
                            count++;
                        }

                        if(TempGrid.Rows[x].Cells[2].Value.ToString() == "[")
                        {
                            codeTemp += "[";
                            x++;
                            count++;

                            while(TempGrid.Rows[x].Cells[2].Value.ToString() != "=" && TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                            {
                                if(TempGrid.Rows[x].Cells[2].Value.ToString() == "Inter Literal")
                                {
                                    dimensions++;
                                    size[tracker] = Convert.ToInt32(TempGrid.Rows[x].Cells[1].Value.ToString());
                                    x++;
                                    count++;
                                    tracker++;
                                }
                                else
                                {
                                    x++;
                                    count++;
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

                            codeTemp += " " + tempId + " " + " = new int[" ;

                            tempId = "";

                            if (dimensions == 1)
                            {
                                codeTemp += size[0] + "] ";
                            }
                            else if (dimensions == 2)
                            {
                                codeTemp += size[0] + "," + size[1] +"] ";
                            }

                            tracker = 0;

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == "=")
                            {
                                x++;
                                count++;
                                while(TempGrid.Rows[x].Cells[2].Value.ToString() != ";")
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                    x++;
                                    count++;
                                }
                            }

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                            {
                                codeTemp += ";\n";
                                OutputText.Text += codeTemp;
                                codeTemp = "";
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
                                    codeTemp += " = " + TempGrid.Rows[x+1].Cells[1].Value.ToString();
                                    x = x + 2;
                                    count = count + 2;
                                }
                                else if (TempGrid.Rows[x].Cells[2].Value.ToString() == "," && TempGrid.Rows[x-1].Cells[2].Value.ToString() == "Identifier")
                                {
                                    codeTemp += "= default, ";
                                    x++;
                                    count++;
                                }
                                else
                                {
                                    codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString();
                                    x++;
                                    count++;
                                }
                            }

                            if (TempGrid.Rows[x].Cells[2].Value.ToString() == ";")
                            {
                                if (TempGrid.Rows[x-1].Cells[2].Value.ToString() == "Identifier")
                                {
                                    codeTemp += " = default;\n";
                                    OutputText.Text += codeTemp;
                                    codeTemp = "";
                                    lineMapping.Add(currentLine, lineTracker);
                                    lineTracker++;
                                }
                                else
                                {
                                    codeTemp += ";\n";
                                    OutputText.Text += codeTemp;
                                    codeTemp = "";
                                    lineMapping.Add(currentLine, lineTracker);
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
                        count++;

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                        {
                            codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString() + " {\n";
                            x++;
                            count++;
                            lineMapping.Add(currentLine, lineTracker);
                            lineTracker++;
                        }

                        while (TempGrid.Rows[x].Cells[2].Value.ToString() != "}")
                        {
                            switch(TempGrid.Rows[x].Cells[2].Value.ToString())
                            {
                                case "inter":
                                    codeTemp += "public int";
                                    x++;
                                    count++;

                                    if(TempGrid.Rows[x].Cells[2].Value.ToString() == "[" && TempGrid.Rows[x+1].Cells[2].Value.ToString() == "]")
                                    {
                                        codeTemp += "[] ";
                                        x = x + 2;
                                        count++;
                                    }
                                    else
                                    {
                                        codeTemp += " ";
                                    }

                                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                                    {
                                        codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString() + ";\n";
                                        x++;
                                        count++;
                                        lineMapping.Add(currentLine, lineTracker);
                                        lineTracker++;
                                    }
                                    break;
                                case "bloat":
                                    codeTemp += "public double";
                                    x++;
                                    count++;

                                    if(TempGrid.Rows[x].Cells[2].Value.ToString() == "[" && TempGrid.Rows[x+1].Cells[2].Value.ToString() == "]")
                                    {
                                        codeTemp += "[] ";
                                        x = x + 2;
                                        count++;
                                    }
                                    else
                                    {
                                        codeTemp += " ";
                                    }

                                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                                    {
                                        codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString() + ";\n";
                                        x++;
                                        count++;
                                        lineMapping.Add(currentLine, lineTracker);
                                        lineTracker++;
                                    }
                                    break;
                                case "pool":
                                    codeTemp += "public bool";
                                    x++;
                                    count++;

                                    if(TempGrid.Rows[x].Cells[2].Value.ToString() == "[" && TempGrid.Rows[x+1].Cells[2].Value.ToString() == "]")
                                    {
                                        codeTemp += "[] ";
                                        x = x + 2;
                                        count++;
                                    }
                                    else
                                    {
                                        codeTemp += " ";
                                    }

                                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                                    {
                                        codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString() + ";\n";
                                        x++;
                                        count++;
                                        lineMapping.Add(currentLine, lineTracker);
                                        lineTracker++;
                                    }
                                    break;
                                case "ping":
                                    codeTemp += "public ping";
                                    x++;
                                    count++;

                                    if(TempGrid.Rows[x].Cells[2].Value.ToString() == "[" && TempGrid.Rows[x+1].Cells[2].Value.ToString() == "]")
                                    {
                                        codeTemp += "[] ";
                                        x = x + 2;
                                        count++;
                                    }
                                    else
                                    {
                                        codeTemp += " ";
                                    }

                                    if (TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
                                    {
                                        codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString() + ";\n";
                                        x++;
                                        count++;
                                        lineMapping.Add(currentLine, lineTracker);
                                        lineTracker++;
                                    }
                                    break;
                                case "newline":
                                    x++;
                                    count++;
                                    currentLine++;
                                    break;
                                default:
                                    x++;
                                    count++;
                                    break;
                            }
                        }

                        if (TempGrid.Rows[x].Cells[2].Value.ToString() == "}")
                        {
                            codeTemp += "}\n";
                            OutputText.Text += codeTemp;
                            codeTemp = "";
                            lineMapping.Add(currentLine, lineTracker);
                            lineTracker++;
                        }
                    
                        break;
                }

                if (TempGrid.Rows[x].Cells[2].Value.ToString() != "spawn")
                    break;
                if (TempGrid.Rows[x].Cells[2].Value.ToString() == "newline")
                    currentLine++;
            }
        }

        for (int x = count; x < TempGrid.Rows.Count; x++)
        {
            switch(TempGrid.Rows[x].Cells[2].Value.ToString())
            {
                case "spawn":
                    codeTemp += "public static ";
                    x++;

                    if(TempGrid.Rows[x].Cells[2].Value.ToString() == "void")
                    {
                        codeTemp += "void ";
                        x++;

                        if(TempGrid.Rows[x].Cells[2].Value.ToString() == "base")
                        {
                            codeTemp += "Main() {\n";
                            OutputText.Text += codeTemp;
                            codeTemp = "";
                            mainFlag = true;
                            x += 3;
                            lineMapping.Add(currentLine, lineTracker);
                            lineTracker++;
                        }
                        else
                        {
                            codeTemp += TempGrid.Rows[x].Cells[1].Value.ToString() + "(";
                            x = x + 2;

                            while(TempGrid.Rows[x].Cells[2].Value.ToString() != ")")
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
                                lineMapping.Add(currentLine, lineTracker);
                                lineTracker++;
                            }
                        }
                        
                    }
                    else
                    {
                        switch(TempGrid.Rows[x].Cells[2].Value.ToString())
                        {
                            case "inter":
                                codeTemp += "int ";
                                x++;
                                break;
                            case "bloat":
                                codeTemp += "double ";
                                x++;
                                break;
                            case "ping":
                                codeTemp += "string ";
                                x++;
                                break;
                            case "pool":
                                codeTemp += "bool ";
                                x++;
                                break;
                        }
                    }
                    
                    break;
                case "push":
                    x++;
                    if(TempGrid.Rows[x].Cells[2].Value.ToString() == "(")
                    {
                        codeTemp += "Console.WriteLine(";
                        
                        do
                        {
                            x++;

                            if(TempGrid.Rows[x].Cells[2].Value.ToString() == "Ping Literal" || TempGrid.Rows[x].Cells[2].Value.ToString() == "Identifier")
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
                    }

                    break;
                case "}":
                    if (x != lastValue - 1)
                    {
                        while (TempGrid.Rows[x].Cells[2].Value.ToString() == "newline")
                        {
                            if(x == lastValue - 1)
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
                                if(mainFlag)
                                {
                                    OutputText.Text += "Console.ReadLine();}\n";
                                    mainFlag = false;
                                    lineMapping.Add(currentLine, lineTracker);
                                    lineTracker++;
                                }
                                else
                                {
                                    OutputText.Text += "}\n";
                                    lineMapping.Add(currentLine, lineTracker);
                                    lineTracker++;
                                }
                            }
                            else
                            {
                                if(mainFlag)
                                {
                                    OutputText.Text += "Console.ReadLine();}\n";
                                    mainFlag = false;
                                    lineMapping.Add(currentLine, lineTracker);
                                    lineTracker++;
                                }
                                else
                                {
                                    OutputText.Text += "}\n";
                                    lineMapping.Add(currentLine, lineTracker);
                                    lineTracker++;
                                }
                            }
                        }
                        else
                        {
                            if(mainFlag)
                            {
                                OutputText.Text += "Console.ReadLine();}\n";
                                mainFlag = false;
                                lineMapping.Add(currentLine, lineTracker);
                                lineTracker++;
                            }
                            else
                            {
                                OutputText.Text += "}\n";
                                lineMapping.Add(currentLine, lineTracker);
                                lineTracker++;
                            }
                        }
                    }
                    else
                    {
                        if(mainFlag)
                        {
                            OutputText.Text += "Console.ReadLine();}\n";
                            mainFlag = false;
                            lineMapping.Add(currentLine, lineTracker);
                            lineTracker++;
                        }
                        else
                        {
                            OutputText.Text += "}\n";
                            lineMapping.Add(currentLine, lineTracker);
                            lineTracker++;
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
            // Log or display the exception
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
        bool hasError = false;
        codeTextBox.Text = "";
        

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
                        if (output.Contains("error"))
                        {
                            string pattern = @"\((-?\d+),(-?\d+)\)";
                            string[] parts = output.Split(':');
                            string result;

                            Regex regex = new Regex(pattern);
                            MatchCollection matches = regex.Matches(output);
                            int originalLine = Convert.ToInt32(matches[0].Groups[1].Value);

                            int codeLine = lineMapping.FirstOrDefault(x => x.Value == originalLine).Key;

                            if (parts[2].Contains("error"))
                            {
                                result = parts[3].Replace("[C", "");
                                result = result.Trim();
                                
                                codeTextBox.Invoke(new Action(() => codeTextBox.AppendText(result + " ON LINE: " + codeLine + Environment.NewLine + Environment.NewLine)));
                                hasError = true;
                            }
                        }
                    }
                };

                if(hasError)
                {
                    run.Enabled = false;
                }
                else
                {
                    run.Enabled = true;
                }

                process.Start();
                process.BeginOutputReadLine();
                await process.WaitForExitAsync();
            }
        }
        catch (Exception ex)
        {
            codeTextBox.Invoke(new Action(() => codeTextBox.AppendText($"An error occurred: {ex.Message}" + Environment.NewLine)));
        }
        finally
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
    }
}