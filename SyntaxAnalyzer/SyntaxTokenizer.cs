using Core.Library;

namespace Syntax_Analyzer;

public class SyntaxTokenizer : Tokenizer {
    public SyntaxTokenizer(TextReader input)
        : base(input, false) {

        CreatePatterns();
    }
    
    private void CreatePatterns() {
        TokenPattern  pattern;

        pattern = new TokenPattern((int) SyntaxConstants.SPAWN,
                                    "SPAWN",
                                    TokenPattern.PatternType.STRING,
                                    "spawn");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.BASE,
                                    "BASE",
                                    TokenPattern.PatternType.STRING,
                                    "base");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.PUSH,
                                    "PUSH",
                                    TokenPattern.PatternType.STRING,
                                    "push");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.HOLD,
                                    "HOLD",
                                    TokenPattern.PatternType.STRING,
                                    "hold");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.COMP,
                                    "COMP",
                                    TokenPattern.PatternType.STRING,
                                    "comp");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.RECALL,
                                    "RECALL",
                                    TokenPattern.PatternType.STRING,
                                    "recall");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.DESTROY,
                                    "DESTROY",
                                    TokenPattern.PatternType.STRING,
                                    "destroy");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.COMMIT,
                                    "COMMIT",
                                    TokenPattern.PatternType.STRING,
                                    "commit");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.FOR,
                                    "FOR",
                                    TokenPattern.PatternType.STRING,
                                    "for");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.TO,
                                    "TO",
                                    TokenPattern.PatternType.STRING,
                                    "to");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.IF,
                                    "IF",
                                    TokenPattern.PatternType.STRING,
                                    "if");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.ELSE,
                                    "ELSE",
                                    TokenPattern.PatternType.STRING,
                                    "else");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.DO,
                                    "DO",
                                    TokenPattern.PatternType.STRING,
                                    "do");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.WHILE,
                                    "WHILE",
                                    TokenPattern.PatternType.STRING,
                                    "while");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.TOWER,
                                    "TOWER",
                                    TokenPattern.PatternType.STRING,
                                    "tower");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.VOID,
                                    "VOID",
                                    TokenPattern.PatternType.STRING,
                                    "void");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.INTER,
                                    "INTER",
                                    TokenPattern.PatternType.STRING,
                                    "inter");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.POOL,
                                    "POOL",
                                    TokenPattern.PatternType.STRING,
                                    "pool");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.PING,
                                    "PING",
                                    TokenPattern.PatternType.STRING,
                                    "ping");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.BLOAT,
                                    "BLOAT",
                                    TokenPattern.PatternType.STRING,
                                    "bloat");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.BUFF,
                                    "BUFF",
                                    TokenPattern.PatternType.STRING,
                                    "buff");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.DEBUFF,
                                    "DEBUFF",
                                    TokenPattern.PatternType.STRING,
                                    "debuff");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.PLUS,
                                    "PLUS",
                                    TokenPattern.PatternType.STRING,
                                    "+");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.MINUS,
                                    "MINUS",
                                    TokenPattern.PatternType.STRING,
                                    "-");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.MULTI,
                                    "MULTI",
                                    TokenPattern.PatternType.STRING,
                                    "*");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.DIV,
                                    "DIV",
                                    TokenPattern.PatternType.STRING,
                                    "/");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.MOD,
                                    "MOD",
                                    TokenPattern.PatternType.STRING,
                                    "%");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.O_PAREN,
                                    "O_PAREN",
                                    TokenPattern.PatternType.STRING,
                                    "(");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.C_PAREN,
                                    "C_PAREN",
                                    TokenPattern.PatternType.STRING,
                                    ")");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.O_BRACE,
                                    "O_BRACE",
                                    TokenPattern.PatternType.STRING,
                                    "{");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.C_BRACE,
                                    "C_BRACE",
                                    TokenPattern.PatternType.STRING,
                                    "}");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.O_SQR,
                                    "O_SQR",
                                    TokenPattern.PatternType.STRING,
                                    "[");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.C_SQR,
                                    "C_SQR",
                                    TokenPattern.PatternType.STRING,
                                    "]");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.OR,
                                    "OR",
                                    TokenPattern.PatternType.STRING,
                                    "||");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.AND,
                                    "AND",
                                    TokenPattern.PatternType.STRING,
                                    "&&");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.NOT,
                                    "NOT",
                                    TokenPattern.PatternType.STRING,
                                    "!");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.ASSIGN,
                                    "ASSIGN",
                                    TokenPattern.PatternType.STRING,
                                    "=");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.EQUAL_EQUAL,
                                    "EQUAL_EQUAL",
                                    TokenPattern.PatternType.STRING,
                                    "==");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.NOT_EQUAL,
                                    "NOT_EQUAL",
                                    TokenPattern.PatternType.STRING,
                                    "!=");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.SEMICOL,
                                    "SEMICOL",
                                    TokenPattern.PatternType.STRING,
                                    ";");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.COMMA,
                                    "COMMA",
                                    TokenPattern.PatternType.STRING,
                                    ",");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.PER,
                                    "PER",
                                    TokenPattern.PatternType.STRING,
                                    ".");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.GREAT,
                                    "GREAT",
                                    TokenPattern.PatternType.STRING,
                                    ">");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.GREAT_E,
                                    "GREAT_E",
                                    TokenPattern.PatternType.STRING,
                                    ">=");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.LESS,
                                    "LESS",
                                    TokenPattern.PatternType.STRING,
                                    "<");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.LESS_E,
                                    "LESS_E",
                                    TokenPattern.PatternType.STRING,
                                    "<=");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.INT_LIT,
                                    "INT_LIT",
                                    TokenPattern.PatternType.STRING,
                                    "Inter Literal");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.FLOAT_LIT,
                                    "FLOAT_LIT",
                                    TokenPattern.PatternType.STRING,
                                    "Bloat Literal");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.STRING_LIT,
                                    "STRING_LIT",
                                    TokenPattern.PatternType.STRING,
                                    "Ping Literal");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.BOOL_LIT,
                                    "BOOL_LIT",
                                    TokenPattern.PatternType.STRING,
                                    "Pool Literal");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.COMMENT,
                                    "COMMENT",
                                    TokenPattern.PatternType.STRING,
                                    "Comment");
        pattern.Ignore = true;
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.NEWLINE,
                                    "NEWLINE",
                                    TokenPattern.PatternType.STRING,
                                    "newline");
        pattern.Ignore = true;
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.TAB,
                                    "TAB",
                                    TokenPattern.PatternType.STRING,
                                    "tab");
        pattern.Ignore = true;
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.SPACE,
                                    "SPACE",
                                    TokenPattern.PatternType.STRING,
                                    "space");
        pattern.Ignore = true;
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.IDEN,
                                    "IDEN",
                                    TokenPattern.PatternType.STRING,
                                    "Identifier");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.FUNC_NAME,
                                    "FUNC_NAME",
                                    TokenPattern.PatternType.STRING,
                                    "FuncId");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.TOWER_NAME,
                                    "TOWER_NAME",
                                    TokenPattern.PatternType.STRING,
                                    "TowerId");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.TOWER_ID,
                                    "TOWER_ID",
                                    TokenPattern.PatternType.STRING,
                                    "TowerLoc");
        AddPattern(pattern);

        pattern = new TokenPattern((int) SyntaxConstants.WHITESPACE,
                                    "WHITESPACE",
                                    TokenPattern.PatternType.REGEXP,
                                    "[ \\t\\n\\r]+");
        pattern.Ignore = true;
        AddPattern(pattern);
    }
}