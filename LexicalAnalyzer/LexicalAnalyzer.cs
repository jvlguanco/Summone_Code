using TokenLibrary;

namespace LexicalAnalyzer;

public class Tokens : TokensClass
{

}

public class Analyzer
{
    public List<Tokens> _token = new List<Tokens>();
    public List<int> _tokenLine = new List<int>();
    Boolean _isReserved = false;
    
    public int _invalid = 0;
    public int _valid = 0;
    public int _count = 0;
    public byte _state = 0;
    public int _lines = 0;
    public int _idNum = 1;
    
    Constants td = new Constants();

    public Boolean GetTokenLines(string text, int counter)
    {
        Tokens token = new Tokens();
        Boolean hasLines = false;
        if (text.ElementAt(0) == '\n')
        {
            token = new Tokens();
            token.setTokens("newline");
            token.setLexemes("\\n");
            _token.Add(token);
            _lines++;
            _tokenLine.Add(counter);
            hasLines = true;
            _count = 1;
        }
        else if (text.ElementAt(0) == ' ')
        {
            token = new Tokens();
            token.setTokens("space");
            token.setLexemes("");
            _token.Add(token);
            hasLines = true;
            _count = 1;
        }
        else if (text.ElementAt(0) == '\t')
        {
            token = new Tokens();
            token.setTokens("tab");
            token.setLexemes("\\t");
            _token.Add(token);
            hasLines = true;
            _count = 1;
        }

        return hasLines;
    }
    
    public Boolean GetReservedWords(string text)
    {
        Constants.ReservedWords resWord = new Constants.ReservedWords();
        Constants.ReservedWordsDelims resWordDel = new Constants.ReservedWordsDelims();
        Tokens token = new Tokens();

        List<String> words;
        List<char> delims;
        List<String> temp;
        
        Boolean found = false, hasToken = false, exitFor = false, ifEnd = false, noDelim = true;
        int tempctr = 0, limit = 0;

        if (text.Length != 1)
        {
            while ((text.Length - 1) > tempctr && !isEnd(text[tempctr + 1], resWordDel))
            {
                tempctr++;
            }
            tempctr++;
        }

        for (int i = 0; i < 6; i++)
        {
            _count = 0;
            words = new List<String>();
            delims = new List<char>();
            found = true;
            
            switch (i)
            {
                case 0:
                    words = resWord.rw_1;
                    delims = resWordDel.delim_1;
                    break;
                case 1:
                    words = resWord.rw_2;
                    delims = resWordDel.delim_2;
                    break;
                case 2:
                    words = resWord.rw_3;
                    delims = resWordDel.delim_3;
                    break;
                case 3:
                    words = resWord.rw_4;
                    delims = resWordDel.delim_4;
                    break;
                case 4:
                    words = resWord.rw_5;
                    delims = resWordDel.delim_5;
                    break;
                case 5:
                    words = resWord.rw_6;
                    delims = resWordDel.delim_6;
                    break;
            }

            foreach (char c in text)
            {
                limit = words.Count - 1;
                temp = new List<string>();
                found = false;

                foreach (string w in words)
                {
                    if ((w.Length - 1) >= _count)
                    {
                        if (c == w.ElementAt(_count))
                        {
                            found = true;

                            if (w.Length == tempctr)
                            {
                                if ((tempctr - 1) == _count)
                                {
                                    foreach (char delim in delims)
                                    {
                                        if ((text.Length - 1) > _count)
                                        {
                                            if (text[_count + 1] == delim)
                                            {
                                                hasToken = true;
                                                noDelim = false;

                                                if (w == "buff" || w == "debuff")
                                                {
                                                    token.setTokens("Pool Literal");
                                                }
                                                else
                                                {
                                                    token.setTokens(w);
                                                }

                                                token.setLexemes(w);
                                                _token.Add(token);
                                                _valid++;
                                                break;
                                            }
                                        }
                                        else if (w == words[limit] && hasToken == false)
                                        {
                                            found = false;
                                        }
                                    }

                                    if (hasToken == false)
                                    {
                                        hasToken = true;
                                        noDelim = false;
                                        found = true;
                                        token.setTokens("NoDelim");
                                        token.setLexemes(w);
                                        _token.Add(token);
                                        _invalid++;
                                            
                                    }
                                    else if (noDelim)
                                    {
                                        hasToken = true;
                                        found = true;
                                        token.setTokens("Invalid");
                                        token.setLexemes(w);
                                        _token.Add(token);
                                        _invalid++;
                                        break;
                                    }
                                    
                                    if (hasToken)
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    temp.Add(w);
                                }
                            }

                        }
                    }
                }
                
                _count++;
                words = temp;
                if (found == false)
                {
                    break;
                }
                if (hasToken)
                {
                    exitFor = true;
                    break;
                }
            }
            
            if (exitFor)
            {
                exitFor = false;
                break;
            }
        }
        
        if (found == false)
        {
            hasToken = false;
            
            foreach (var item in resWordDel.delim_end)
            {
                if (text.ElementAt(_count - 1) == item)
                {
                    ifEnd = true;
                }
            }
            
            while (ifEnd != true)
            {
                foreach (var item in resWordDel.delim_end)
                {
                    if ((text.Length) > _count)
                    {
                        if (text.ElementAt(_count) == item)
                        {
                            ifEnd = true;
                            break;
                        }
                    }
                    else
                    {
                        ifEnd = true;
                    }
                }

                if (ifEnd != true)
                {
                    _count++;
                }
            }
        }

        if (!(text.Length >= _count))
        {
            _count--;
        }

        return hasToken;
    }
    
    public Boolean GetReservedSymbols(string text)
    {
        Constants td = new Constants();
        Constants.ReservedSymbols resSym = new Constants.ReservedSymbols();
        Constants.ReservedSymbolsDelims resSymDelim = new Constants.ReservedSymbolsDelims();
        Boolean found = false, hasToken = false, exitFor = false;

        Tokens token = new Tokens();

        List<String> words;
        List<char> delims;
        List<String> temp;
        int tempctr = 0, limit = 0, symctr = 0;

        if (text.Length != 1)
        {
            while ((text.Length - 1) > tempctr && !isEnd(text[tempctr + 1], resSymDelim))
            {
                tempctr++;
            }
            tempctr++;
        }

        for (int i = 0; i < 14; i++)
        {
            symctr = 0;
            words = new List<String>();
            delims = new List<char>();
            found = true;
            
            switch (i)
            {
                case 0:
                    words = resSym.rs_1;
                    delims = resSymDelim.del1;
                    break;
                case 1:
                    words = resSym.rs_2;
                    delims = resSymDelim.del2;
                    break;
                case 2:
                    words = resSym.rs_4;
                    delims = resSymDelim.del4;
                    break;
                case 3:
                    words = resSym.rs_5;
                    delims = resSymDelim.del5;
                    break;
                case 4:
                    words = resSym.rs_6;
                    delims = resSymDelim.del6;
                    break;
                case 5:
                    words = resSym.rs_7;
                    delims = resSymDelim.del7;
                    break;
                case 6:
                    words = resSym.rs_8;
                    delims = resSymDelim.del8;
                    break;
                case 7:
                    words = resSym.rs_9;
                    delims = resSymDelim.del9;
                    break;
                case 8:
                    words = resSym.rs_10;
                    delims = resSymDelim.del10;
                    break;
                case 9:
                    words = resSym.rs_11;
                    delims = resSymDelim.del11;
                    break;
                case 10:
                    words = resSym.rs_12;
                    delims = resSymDelim.del12;
                    break;
                case 11:
                    words = resSym.rs_13;
                    delims = resSymDelim.del13;
                    break;
                case 12:
                    words = resSym.rs_14;
                    delims = resSymDelim.del14;
                    break;
                case 13:
                    words = resSym.rs_3;
                    delims = resSymDelim.del3;
                    break;
            }
            
            foreach (char c in text)
            {
                limit = words.Count - 1;
                temp = new List<string>();
                found = false;
                
                foreach (string w in words)
                {
                    if ((w.Length - 1) >= symctr)
                    {
                        if (c == w.ElementAt(symctr))
                        {
                            found = true;

                            if (w.Length == tempctr)
                            {
                                if ((tempctr - 1) == symctr)
                                {
                                    foreach (char delim in delims)
                                    {
                                        if ((text.Length - 1) > symctr)
                                        {
                                            if (text[symctr + 1] == delim)
                                            {
                                                found = true;
                                                hasToken = true;
                                                token = new Tokens();
                                                token.setTokens(w);
                                                token.setLexemes(w);
                                                _token.Add(token);
                                                _valid++;
                                                break;
                                            }
                                        }
                                        else if (w == words[limit] && hasToken == false)
                                        {
                                            found = false;
                                        }
                                    }

                                    if (hasToken)
                                    {
                                        break;
                                    }

                                }
                                else
                                {
                                    temp.Add(w);
                                }
                            }
                        }
                    }
                }
                
                symctr++;
                words = temp;
                if (found == false)
                {
                    break;
                }
                
                if (hasToken)
                {
                    exitFor = true;
                    break;
                }
            }

            if (exitFor)
            {
                exitFor = false;
                break;
            }
        }

        if (hasToken)
        {
            _count = symctr;
        }
        
        return hasToken;
    }
    
    public Boolean GetLiterals(string text)
    {
        Constants.Literals lit = new Constants.Literals();
        Constants.LiteralsDelims litDelim = new Constants.LiteralsDelims();
        Tokens token = new Tokens();
        
        List<char> delims = new List<char>();
        Boolean hasToken = false, validText = false;
        string literal = "";
        _state = 0;
        int lctr = 0;

        if (text.ElementAt(lctr) == '"')
            _state = 1;
        else if (text.ElementAt(lctr) == '#')
            _state = 2;
        else
        {
            foreach (char num in lit.nums)
            {
                if (text.ElementAt(lctr) == num)
                    _state = 3;
            }
        }

        if (_state != 0)
        {
            switch (_state)
            {
                case 1:
                case 2:
                    delims = litDelim.delim_txt;

                    if (_state == 1)
                    {
                        if (text.Length != 1)
                        {
                            while ((text.Length - 1) > lctr && !(text[lctr + 1] == '"') && !(text[lctr + 1] == '\n'))
                            {
                                literal += text[lctr].ToString();
                                lctr++;
                            }
                            if ((text.Length - 1) == lctr && (text[lctr] != '"'))
                                hasToken = false;
                            else
                            {
                                if (!(lctr == 1 && text[lctr] == '\\'))
                                {
                                    validText = true;
                                    lctr++;
                                    foreach (char c in delims)
                                    {
                                        if ((text.Length - 1) >= (lctr + 1))
                                            if (text[lctr + 1] == c)
                                            {
                                                hasToken = true;
                                                break;
                                            }
                                    }
                                }
                                if (hasToken && validText)
                                {
                                    _valid++;
                                    token = new Tokens();
                                    token.setTokens("Ping Literal");
                                    token.setLexemes(text.Substring(0, (lctr + 1)));
                                    _token.Add(token);
                                    _count = lctr + 1;
                                }
                                else if (!validText)
                                {
                                    _count = lctr + 2;
                                    hasToken = false;
                                }

                            }
                        }
                    }
                    else if(_state == 2) { 
                        if (text.Length != 1)
                        {
                            while ((text.Length - 1) > lctr && !(text[lctr + 1] == '#') && !(text[lctr + 1] == '\n'))
                            {
                                literal += text[lctr].ToString();
                                lctr++;
                            }
                            if ((text.Length - 1) == lctr && (text[lctr] != '#'))
                                hasToken = false;
                            else
                            {
                                if (!(lctr == 1 && text[lctr] == '\\'))
                                {
                                    validText = true;
                                    lctr++;
                                    foreach (char c in delims)
                                    {
                                        if ((text.Length - 1) >= (lctr + 1))
                                            if (text[lctr + 1] == c)
                                            {
                                                hasToken = true;
                                                break;
                                            }
                                    }
                                }
                                
                                if (hasToken && validText)
                                {
                                    _valid++;
                                    token = new Tokens();
                                    token.setTokens("Comment");
                                    token.setLexemes(text.Substring(0, (lctr + 1)));
                                    _token.Add(token);
                                    _count = lctr + 1;
                                }
                                else if (!validText)
                                {
                                    _count = lctr + 2;
                                    hasToken = false;
                                }

                            }
                        }
                    }
                    break;

                case 3:
                    Constants.Identifier id = new Constants.Identifier();
                    delims = litDelim.delim_num;
                    
                    Boolean isNum = true, hasNum = true, hasId = false;
                    List<char> num = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

                    id.id.AddRange(id.delim_caplet);
                    int value = 0;
                    int decValue = 0;

                    if (text.ElementAt(lctr) == '~')
                    {
                        hasNum = false;
                        foreach (char n in num)
                        {
                            if ((text.Length - 1) > lctr)
                                if (text.ElementAt(lctr + 1) == n)
                                { 
                                    hasNum = true;
                                    lctr++;
                                }
                        }
                    }

                    if (hasNum)
                    {
                        while (isNum)
                        {
                            isNum = false;
                            foreach (char n in num)
                            {
                                if ((text.Length - 1) > lctr)
                                    if (text.ElementAt(lctr + 1) == n)
                                    {
                                        value++;
                                        if (value <= 8)
                                        {
                                            lctr++;
                                            isNum = true;
                                        }
                                        else if(value > 8) {
                                            isNum = false;
                                            hasToken = false;
                                        }
                                    }
                            }
                        }
                        
                        Boolean isBloat = false;
                        if ((text.Length - 1) > lctr)
                            if (text.ElementAt(lctr + 1) == '.')
                            {
                                if ((text.Length - 1) > lctr + 1)
                                    foreach (char n in num)
                                    {
                                        if (text.ElementAt(lctr + 2) == n)
                                            isBloat = true;
                                    }
                            }

                        if (isBloat)
                        {
                            lctr++;
                            isNum = true;
                            while (isNum)
                            {
                                isNum = false;
                                foreach (char n in num)
                                {
                                    if ((text.Length - 1) > lctr)
                                        if (text.ElementAt(lctr + 1) == n)
                                        {
                                            decValue++;
                                            if (decValue <= 9)
                                            {
                                                lctr++;
                                                isNum = true;
                                            }
                                            else if(decValue > 9) {
                                                isNum = false;
                                                hasToken = false;
                                            }
                                        }
                                }
                            }

                            foreach (char delim in delims)
                            {
                                if ((text.Length - 1) > lctr)
                                    if (text.ElementAt(lctr + 1) == delim)
                                    {
                                        hasToken = true;
                                        break;
                                    }
                            }

                            if (hasToken)
                            {
                                _valid++;
                                token = new Tokens();
                                token.setTokens("Bloat Literal");
                                token.setLexemes(text.Substring(0, (lctr + 1)));
                                _token.Add(token);
                            }
                            else
                            {
                                foreach (char c in id.id)
                                {
                                    if ((text.Length - 1) > lctr)
                                        if (text.ElementAt(lctr + 1) == c)
                                        {
                                            hasId = true;
                                        }
                                }
                            }

                            if (!hasId)
                            {
                                _count = lctr + 1;
                            }
                        }
                        else
                        {
                            foreach (char delim in delims)
                            {
                                if (text.ElementAt(lctr + 1) == delim)
                                {
                                    hasToken = true;
                                    break;
                                }

                            }

                            if (hasToken)
                            {
                                _valid++;
                                token = new Tokens();
                                token.setTokens("Inter Literal");
                                token.setLexemes(text.Substring(0, (lctr + 1)));
                                _token.Add(token);
                            }
                            else
                            {
                                foreach (char c in id.id)
                                {
                                    if (text.ElementAt(lctr + 1) == c)
                                    {
                                        hasId = true;
                                    }
                                }
                            }

                            if (!hasId)
                            {
                                _count = lctr + 1;
                            }
                        }
                    }
                    break;
            }
        }
        return hasToken;
    }
    
    public Boolean GetIdentifiers(string text)
    {
        Constants.Identifier id = new Constants.Identifier();
        Constants.IdentifierDelims delims = new Constants.IdentifierDelims();
        Boolean hasToken = false, valID = false, isValid = true;
        Tokens token = new Tokens();

        string limitedText = text.Length > 25 ? text.Substring(0, 25) : text;

        id.id.AddRange(id.delim_lowlet);
        id.id.AddRange(id.delim_caplet);

        int ictr = 0;

        foreach (char c in id.id)
        {
            if (text.ElementAt(ictr) == c)
            {
                valID = true;
            }
        }

        id.id.AddRange(id.delim_undscr);
        id.id.AddRange(id.delim_digit);
        
        if (valID)
        {
            isValid = true;
            while (isValid)
            {
                isValid = false;
                foreach (char n in id.id)
                {
                    if ((limitedText.Length - 1) > ictr)
                        if (limitedText.ElementAt(ictr + 1) == n)
                        {
                            ictr++;
                            isValid = true;
                        }
                }
            }

            if(limitedText.ElementAt(ictr) == '_')
            {
                valID = false;
            }

            if (valID)
            {
                foreach (char delim in delims.delim_end)
                {
                    if ((text.Length - 1) > ictr)
                    {
                        if (text.ElementAt(ictr + 1) == delim)
                        {
                            hasToken = true;
                            break;
                        }
                    }
                }
            }

            if (hasToken)
            {
                _valid++;
                token = new Tokens();
                token.setTokens("Identifier");
                token.setLexemes(limitedText.Substring(0, (ictr + 1)));
                _token.Add(token);
                _idNum++;
            }

            _count = ictr + 1;
        }
        return hasToken;
    }

    public Boolean isEnd(char c, Constants.ReservedWordsDelims rwd)
    {
        Boolean result = false;
        foreach (var item in rwd.delim_end)
        {
            if (item == c)
            {
                result = true;
                break;
            }
        }
        return result;
    }
    public Boolean isEnd(char c, Constants.ReservedSymbolsDelims rsd)
    {
        Boolean result = false;
        foreach (var item in rsd.delim_end)
        {
            if (item == c)
            {
                result = true;
                break;
            }
        }
        return result;
    }
}