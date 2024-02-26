namespace LexicalAnalyzer;

public class Initializer
{
    public int tokens = 0;  
    public Analyzer InitializeAnalyzer(string text, Analyzer lex)
    {
        Boolean hasToken = false;
        Tokens token = new Tokens();
        lex._token.Clear();
        lex._invalid = 0;
        lex._valid = 0;
        while (text != "")
        {
            if (text.ElementAt(0) == '\t')
            {   
                token = new Tokens();
                token.setTokens("tab");
                token.setLexemes("\\t");
                lex._token.Add(token);
                text = text.Remove(0, 1);
                continue;
            }
            else if (hasToken = lex.GetTokenLines(text, tokens))
            {
                text = text.Remove(0, lex._count);
                tokens--;
            }
            else if (hasToken = lex.GetReservedWords(text))
            {
                text = text.Remove(0, lex._count);
            }
            else if (hasToken = lex.GetReservedSymbols(text))
            {
                text = text.Remove(0, lex._count);
            }
            else if (hasToken = lex.GetLiterals(text))
            {
                text = text.Remove(0, lex._count);
            }
            else if (hasToken = lex.GetIdentifiers(text))
            {
                text = text.Remove(0, lex._count);
            }
            else
            {
                token = new Tokens();
                lex._invalid++;
                if (lex._state != 0)
                {
                    switch (lex._state)
                    {
                        case 1:
                            lex._count = GetCtr(text, 1);
                            break;
                    }
                }

                if (lex._count == 0 && text.Length != 1)
                {
                    lex._count = GetCtr(text);
                }
                else if (lex._count == 0 && text.Length == 1)
                {
                    lex._count = 1;
                }
                else if (lex._count >= text.Length)
                {
                    lex._count = text.Length;
                }
                
                token.setTokens("Invalid");
                token.setLexemes(text.Substring(0, lex._count));
                lex._token.Add(token);
                text = text.Remove(0, lex._count);

            }
            
            tokens++;
        }
        
        lex._tokenLine.Add(tokens);
        lex = SetLines(lex);

        return lex;
    }
    
    private Analyzer SetLines(Analyzer lex)
    {
        for (int ctr = 0; ctr < lex._token.Count; ctr++)
        {
            for (int i = 0; i < lex._tokenLine.Count; i++)
            {
                if (ctr + 1 <= lex._tokenLine[i])
                {
                    lex._token[ctr].setLines(i + 1);
                    break;
                }
            }
        }
        
        return lex;
    }

    private int GetCtr(string txt)
    {
        Constants.ReservedWordsDelims rwd = new Constants.ReservedWordsDelims();
        Constants td = new Constants();
        
        Boolean ifEnd = false;
        int ctr = 0;

        foreach (var item in rwd.delim_end)
        {   
            if (txt.ElementAt(ctr - 1) == item)
                ifEnd = true;
        }
        while (ifEnd != true)
        {
            foreach (var item in rwd.delim_end)
            {
                if ((txt.Length) > ctr)
                {
                    if (txt.ElementAt(ctr) == item)
                    {
                        ifEnd = true;
                        break;
                    }
                }
                else ifEnd = true;
            }
            if (ifEnd != true)
                ctr++;
        }

        if (!(txt.Length >= ctr)) ctr--;
        return ctr;
    }
    private int GetCtr(string txt, int ctr)
    {
        Boolean notEnd = true;
        List<char> delims = new List<char>{ '"', '\\', '\n' };
        while (notEnd && (txt.Length - 1) >= ctr)
        {
            foreach (char c in delims)
            {
                if ((txt.Length - 1) > ctr)
                {
                    if (c == txt.ElementAt(ctr))
                    {
                        notEnd = false;
                        if (c == '\\')
                            if (txt.Length - 1 != ctr)
                                ctr++;
                    }
                }
                else
                    notEnd = false;
            }
            ctr++;
        }
        return ctr;
    }
}