namespace LexicalAnalyzer;

public class Constants
{
    public class ReservedWords
    {
        public List<string> rw_1 = new List<string> { "base", "hold", "push" };
        public List<string> rw_2 = new List<string> { "comp", "for", "spawn", "up", "tower", "void", "down" };
        public List<string> rw_3 = new List<string> { "buff", "debuff" };
        public List<string> rw_4 = new List<string> { "commit", "destroy" };
        public List<string> rw_5 = new List<string> { "do", "else" };
        public List<string> rw_6 = new List<string> { "if", "while", "recall", "bloat", "inter", "pool", "ping", };
    }

    public class ReservedWordsDelims
    {
        public List<char> delim_1 = new List<char> { '(' };
        public List<char> delim_2 = new List<char> { ' ' };
        public List<char> delim_3 = new List<char> { ' ', '&', '|', ';', ')', ',', '}' };
        public List<char> delim_4 = new List<char> { ';' };
        public List<char> delim_5 = new List<char> { ' ', '\n', '{' };
        public List<char> delim_6 = new List<char> { ' ', '(', '[' };
        public List<char> delim_end = new List<char> { '.', ' ', '\n', '\t','(' , ':', ',', '\'', '[', ']', '#', '%', '\\',
            ')', '"', ';', '^', '~', '`', '_', '!', '<', '>','*', '/', '+', '-','{', '}', '&', '|' , '='};
    }

    public class ReservedSymbols
    {
        public List<string> rs_1 = new List<string> { "+" };
        public List<string> rs_2 = new List<string> { "-", "*", "%" , "/"};
        public List<string> rs_3 = new List<string> { "." };
        public List<string> rs_4 = new List<string> { "||", "&&", "=", "==", "!="};
        public List<string> rs_5 = new List<string> { "!" };
        public List<string> rs_6 = new List<string> { "<=", "<", ">=", ">" };
        public List<string> rs_7 = new List<string> { "(" };
        public List<string> rs_8 = new List<string> { ")" };
        public List<string> rs_9 = new List<string> { "{" };
        public List<string> rs_10 = new List<string> { "}" };
        public List<string> rs_11 = new List<string> { ";" };
        public List<string> rs_12 = new List<string> { "," };
        public List<string> rs_13 = new List<string> { "[" };
        public List<string> rs_14 = new List<string> { "]" };
    }

    public class ReservedSymbolsDelims
    {
        public List<char> del1 = new List<char> { ' ', '"', '(', '~', 
                                                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                                                'n','o','p','q','r','s','t','u','v','w','x','y','z',
                                                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
                                                'A','B','C','D','E','F','G','H','I','J','K','L','M',
                                                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
        
        public List<char> del2 = new List<char> { '(', '~', ' ',
                                                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                                                'n','o','p','q','r','s','t','u','v','w','x','y','z',
                                                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
                                                'A','B','C','D','E','F','G','H','I','J','K','L','M',
                                                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};

        public List<char> del3 = new List<char> { 'a','b','c','d','e','f','g','h','i','j','k','l','m',
                                                'n','o','p','q','r','s','t','u','v','w','x','y','z',
                                                'A','B','C','D','E','F','G','H','I','J','K','L','M',
                                                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
        
        public List<char> del4 = new List<char> { '(', '~', '"', '!', ' ', '{',
                                                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                                                'n','o','p','q','r','s','t','u','v','w','x','y','z',
                                                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
                                                'A','B','C','D','E','F','G','H','I','J','K','L','M',
                                                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
        
        public List<char> del5 = new List<char> { '(', '!',
                                                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                                                'n','o','p','q','r','s','t','u','v','w','x','y','z',
                                                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
                                                'A','B','C','D','E','F','G','H','I','J','K','L','M',
                                                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
        
        public List<char> del6 = new List<char> { '(', '~', ' ',
                                                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                                                'n','o','p','q','r','s','t','u','v','w','x','y','z',
                                                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
                                                'A','B','C','D','E','F','G','H','I','J','K','L','M',
                                                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
        
        public List<char> del7 = new List<char> { '(', '~', ' ', ')', '"', '!',
                                                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                                                'n','o','p','q','r','s','t','u','v','w','x','y','z',
                                                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
                                                'A','B','C','D','E','F','G','H','I','J','K','L','M',
                                                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
        
        public List<char> del8 = new List<char> { ' ', '\n', '{', '=', '>', '<', '!', '&', '|', '+', '-', '/', '*', '%', ';', ')', ']', ',',
                                                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                                                'n','o','p','q','r','s','t','u','v','w','x','y','z',
                                                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
                                                'A','B','C','D','E','F','G','H','I','J','K','L','M',
                                                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
        
        public List<char> del9 = new List<char> { ' ', '\n', '{', '"',
                                                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                                                'n','o','p','q','r','s','t','u','v','w','x','y','z',
                                                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
                                                'A','B','C','D','E','F','G','H','I','J','K','L','M',
                                                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
        
        public List<char> del10 = new List<char> { ' ', '\n', '}', ';', ',', '"',
                                                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                                                'n','o','p','q','r','s','t','u','v','w','x','y','z',
                                                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
                                                'A','B','C','D','E','F','G','H','I','J','K','L','M',
                                                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
        
        public List<char> del11 = new List<char> { ' ', '\n', '#', '}',
                                                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                                                'n','o','p','q','r','s','t','u','v','w','x','y','z',
                                                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
                                                'A','B','C','D','E','F','G','H','I','J','K','L','M',
                                                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
        
        public List<char> del12 = new List<char> { ' ', '~', '!', '{', ']',
                                                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                                                'n','o','p','q','r','s','t','u','v','w','x','y','z',
                                                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
                                                'A','B','C','D','E','F','G','H','I','J','K','L','M',
                                                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
        
        public List<char> del13 = new List<char> { ']', ',', ' ',
                                                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                                                'n','o','p','q','r','s','t','u','v','w','x','y','z',
                                                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
                                                'A','B','C','D','E','F','G','H','I','J','K','L','M',
                                                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
        
        public List<char> del14 = new List<char> { ' ', '[', ')', ';', ',',
                                                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                                                'n','o','p','q','r','s','t','u','v','w','x','y','z',
                                                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
                                                'A','B','C','D','E','F','G','H','I','J','K','L','M',
                                                'N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};

        public List<char> delim_end = new List<char> { '.', ' ', '\n', '\t','(' , ':', ',', '[', ']', '?', '#', '$', '%', '\\',
                                                   ')', ';', '@', '^', '~', '`', '_', '!', '<','"', '>','*', '/', '{', '}', '+', '-', '*', '/', '%',
                                                   'a','b','c','d','e','f','g','h','i','j','k','l','m',
                                                   'n','o','p','q','r','s','t','u','v','w','x','y','z',
                                                   '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                                                   'A','B','C','D','E','F','G','H','I','J','K','L','M',
                                                   'N','O','P','Q','R','S','T','U','V','W','X','Y','Z' };
    }
    
    public class Literals
    {
        public List<char> nums = new List<char> { '~', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    }
    
    public class LiteralsDelims
    {
        public List<char> delim_txt = new List<char> { ' ', '\n', ';', ',', ')', '!', '=', '+', '}' };
        public List<char> delim_num = new List<char> { '+', '-', '*', '/', ' ', '\n', ';', '&', '|', ')', ',', '&', ']', '=', '>', '<', '!', '}', '%' };
    }
    
    public class Identifier
    {
        public List<char> delim_digit = new List<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        public List<char> delim_lowlet = new List<char> { 'a','b','c','d','e','f','g','h','i','j','k','l','m',
                                                   'n','o','p','q','r','s','t','u','v','w','x','y','z' };
        public List<char> delim_caplet = new List<char> {'A','B','C','D','E','F','G','H','I','J','K','L','M',
                                                  'N','O','P','Q','R','S','T','U','V','W','X','Y','Z' };
        public List<char> delim_undscr = new List<char> { '_' };
        public List<char> id = new List<char> {};
    }
    public class IdentifierDelims
    {
        public List<char> delim_end = new List<char> { '.','\n', '=', '\t','(' , ':', ',', '[', ']', '#', '%', '}',
                                                       ')', '"', ';', '^', '~', '!', '<', '>','*', '/', '+', '-',' '};
    }
}