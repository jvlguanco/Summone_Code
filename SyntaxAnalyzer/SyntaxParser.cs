using Core.Library;

namespace Syntax_Analyzer;

public class SyntaxParser : RecursiveDescentParser {
    private enum SynteticPatterns {
    }

    public SyntaxParser(TextReader input)
        : base(input) {

        CreatePatterns();
    }

    public SyntaxParser(TextReader input, SyntaxAnalyzer analyzer)
        : base(input, analyzer) {

        CreatePatterns();
    }

    protected override Tokenizer NewTokenizer(TextReader input) {
        return new SyntaxTokenizer(input);
    }

    private void CreatePatterns() {
        ProductionPattern             pattern;
        ProductionPatternAlternative  alt;

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_PROGRAM,
                                        "Prod_program");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_GLOBAL_DECLARATION, 0, 1);
        alt.AddToken((int) SyntaxConstants.SPAWN, 1, 1);
        alt.AddToken((int) SyntaxConstants.VOID, 1, 1);
        alt.AddToken((int) SyntaxConstants.BASE, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_BASE_PROD, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_USER_FUNCTION, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GLOBAL_DECLARATION,
                                        "Prod_global_declaration");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_GLOBAL_VAR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GLOBAL_DECLARATION, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_GLOBAL_COMP, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GLOBAL_DECLARATION, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_GLOBAL_TOWER, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GLOBAL_DECLARATION, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GLOBAL_VAR,
                                        "Prod_global_var");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.INTER, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GV_INTER, 1, 1);
        alt.AddToken((int) SyntaxConstants.SEMICOL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.BLOAT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GV_BLOAT, 1, 1);
        alt.AddToken((int) SyntaxConstants.SEMICOL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.PING, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GV_PING, 1, 1);
        alt.AddToken((int) SyntaxConstants.SEMICOL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.POOL, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GV_POOL, 1, 1);
        alt.AddToken((int) SyntaxConstants.SEMICOL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GV_INTER,
                                        "Prod_gv_inter");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GV_INTER_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GV_INTER_TAIL,
                                        "Prod_gv_inter_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GV_INTER_TAIL_ADD, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_G_INTER_ARRAY_DEC, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GV_INTER_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_INTER_ARRAY_DEC,
                                        "Prod_G_inter_array_dec");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_INTER_1D_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_INTER_1D_TAIL,
                                        "Prod_G_inter_1D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_INTER_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_INTER_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_INTER_2D_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_INTER_ELEMENT,
                                        "Prod_G_inter_element");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_ADD_INTER_1D,
                                        "Prod_G_add_inter_1D");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_INTER_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_INTER_1D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_INTER_2D_TAIL,
                                        "Prod_G_inter_2D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_INTER_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_INTER_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_INTER_2D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_ADD_INTER_2D,
                                        "Prod_G_add_inter_2D");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_INTER_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_INTER_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_INTER_2D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_GV_INTER_TAIL,
                                        "Prod_add_gv_inter_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GV_INTER_VAL_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_GV_INTER_VAL_TAIL,
                                        "Prod_add_gv_inter_val_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GV_INTER_TAIL_ADD, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GV_INTER_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GV_INTER_TAIL_ADD,
                                        "Prod_gv_inter_tail_add");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GV_INTER_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GV_BLOAT,
                                        "Prod_gv_bloat");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GV_BLOAT_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GV_BLOAT_TAIL,
                                        "Prod_gv_bloat_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.FLOAT_LIT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GV_BLOAT_TAIL_ADD, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_G_BLOAT_ARRAY_DEC, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GV_BLOAT_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_BLOAT_ARRAY_DEC,
                                        "Prod_G_bloat_array_dec");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_BLOAT_1D_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_BLOAT_1D_TAIL,
                                        "Prod_G_bloat_1D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_BLOAT_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_BLOAT_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_BLOAT_2D_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_BLOAT_ELEMENT,
                                        "Prod_G_bloat_element");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.FLOAT_LIT, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_ADD_BLOAT_1D,
                                        "Prod_G_add_bloat_1D");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_BLOAT_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_BLOAT_1D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_BLOAT_2D_TAIL,
                                        "Prod_G_bloat_2D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_BLOAT_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_BLOAT_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_BLOAT_2D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_ADD_BLOAT_2D,
                                        "Prod_G_add_bloat_2D");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_BLOAT_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_BLOAT_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_BLOAT_2D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_GV_BLOAT_TAIL,
                                        "Prod_add_gv_bloat_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GV_BLOAT_VAL_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_GV_BLOAT_VAL_TAIL,
                                        "Prod_add_gv_bloat_val_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.FLOAT_LIT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GV_BLOAT_TAIL_ADD, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GV_BLOAT_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GV_BLOAT_TAIL_ADD,
                                        "Prod_gv_bloat_tail_add");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GV_BLOAT_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GV_PING,
                                        "Prod_gv_ping");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_PING_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_PING_TAIL,
                                        "Prod_g_ping_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.STRING_LIT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GV_PING_TAIL_ADD, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_G_PING_ARRAY_DEC, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GV_PING_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_PING_ARRAY_DEC,
                                        "Prod_G_ping_array_dec");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_PING_1D_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_PING_1D_TAIL,
                                        "Prod_G_ping_1D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_PING_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_PING_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_PING_2D_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_PING_ELEMENT,
                                        "Prod_G_ping_element");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.STRING_LIT, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_ADD_PING_1D,
                                        "Prod_G_add_ping_1D");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_PING_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_PING_1D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_PING_2D_TAIL,
                                        "Prod_G_ping_2D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_PING_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_PING_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_PING_2D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_ADD_PING_2D,
                                        "Prod_G_add_ping_2D");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_PING_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_PING_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_PING_2D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_GV_PING_TAIL,
                                        "Prod_add_gv_ping_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GV_PING_VAL_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_GV_PING_VAL_TAIL,
                                        "Prod_add_gv_ping_val_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.STRING_LIT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GV_PING_TAIL_ADD, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GV_PING_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GV_PING_TAIL_ADD,
                                        "Prod_gv_ping_tail_add");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GV_PING_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GV_POOL,
                                        "Prod_gv_pool");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_POOL_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_POOL_TAIL,
                                        "Prod_g_pool_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.BOOL_LIT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GV_POOL_TAIL_ADD, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_G_POOL_ARRAY_DEC, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GV_POOL_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_POOL_ARRAY_DEC,
                                        "Prod_G_pool_array_dec");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_POOL_1D_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_POOL_1D_TAIL,
                                        "Prod_G_pool_1D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_POOL_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_POOL_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_POOL_2D_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_POOL_ELEMENT,
                                        "Prod_G_pool_element");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.BOOL_LIT, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_ADD_POOL_1D,
                                        "Prod_G_add_pool_1D");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_POOL_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_POOL_1D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_POOL_2D_TAIL,
                                        "Prod_G_pool_2D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_POOL_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_POOL_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_POOL_2D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_G_ADD_POOL_2D,
                                        "Prod_G_add_pool_2D");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_POOL_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_POOL_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_POOL_2D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_GV_POOL_TAIL,
                                        "Prod_add_gv_pool_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GV_POOL_VAL_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_GV_POOL_VAL_TAIL,
                                        "Prod_add_gv_pool_val_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.BOOL_LIT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GV_POOL_TAIL_ADD, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GV_POOL_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GV_POOL_TAIL_ADD,
                                        "Prod_gv_pool_tail_add");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GV_POOL_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GLOBAL_COMP,
                                        "Prod_global_comp");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMP, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GC_DATATYPE, 1, 1);
        alt.AddToken((int) SyntaxConstants.SEMICOL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GC_DATATYPE,
                                        "Prod_gc_datatype");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.INTER, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GC_INTER_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.BLOAT, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GC_BLOAT_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.POOL, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GC_POOL_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.PING, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GC_PING_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GC_INTER_TAIL,
                                        "Prod_gc_inter_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GC_INTER_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_GC_INTER_ARRAY_DEC, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_GC_INTER_TAIL,
                                        "Prod_add_gc_inter_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GC_INTER_VAL_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_GC_INTER_VAL_TAIL,
                                        "Prod_add_gc_inter_val_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GC_INTER_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GC_INTER_ARRAY_DEC,
                                        "Prod_GC_inter_array_dec");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GC_INTER_1D_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GC_INTER_1D_TAIL,
                                        "Prod_gc_inter_1D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_INTER_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_INTER_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GC_INTER_2D_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GC_INTER_2D_TAIL,
                                        "Prod_gc_inter_2D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_INTER_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_INTER_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_INTER_2D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GC_BLOAT_TAIL,
                                        "Prod_gc_bloat_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.FLOAT_LIT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GC_BLOAT_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_GC_BLOAT_ARRAY_DEC, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_GC_BLOAT_TAIL,
                                        "Prod_add_gc_bloat_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GC_BLOAT_VAL_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_GC_BLOAT_VAL_TAIL,
                                        "Prod_add_gc_bloat_val_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.FLOAT_LIT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GC_BLOAT_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GC_BLOAT_ARRAY_DEC,
                                        "Prod_GC_bloat_array_dec");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GC_BLOAT_1D_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GC_BLOAT_1D_TAIL,
                                        "Prod_gc_bloat_1D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_BLOAT_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_BLOAT_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GC_BLOAT_2D_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GC_BLOAT_2D_TAIL,
                                        "Prod_gc_bloat_2D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_BLOAT_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_BLOAT_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_BLOAT_2D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GC_PING_TAIL,
                                        "Prod_gc_ping_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.STRING_LIT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GC_PING_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_GC_PING_ARRAY_DEC, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_GC_PING_TAIL,
                                        "Prod_add_gc_ping_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GC_PING_VAL_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_GC_PING_VAL_TAIL,
                                        "Prod_add_gc_ping_val_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.STRING_LIT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GC_PING_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GC_PING_ARRAY_DEC,
                                        "Prod_GC_ping_array_dec");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GC_PING_1D_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GC_PING_1D_TAIL,
                                        "Prod_gc_ping_1D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_PING_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_PING_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GC_PING_2D_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GC_PING_2D_TAIL,
                                        "Prod_gc_ping_2D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_PING_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_PING_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_PING_2D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GC_POOL_TAIL,
                                        "Prod_gc_pool_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.BOOL_LIT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GC_POOL_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_GC_POOL_ARRAY_DEC, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_GC_POOL_TAIL,
                                        "Prod_add_gc_pool_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GC_POOL_VAL_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_GC_POOL_VAL_TAIL,
                                        "Prod_add_gc_pool_val_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.BOOL_LIT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_GC_POOL_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GC_POOL_ARRAY_DEC,
                                        "Prod_GC_pool_array_dec");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GC_POOL_1D_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GC_POOL_1D_TAIL,
                                        "Prod_gc_pool_1D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GC_POOL_2D_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_POOL_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_POOL_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GC_POOL_2D_TAIL,
                                        "Prod_gc_pool_2D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_POOL_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_POOL_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_G_ADD_POOL_2D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GLOBAL_TOWER,
                                        "Prod_global_tower");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.TOWER, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_TOWER_VAR, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_TOWER_VAR,
                                        "Prod_tower_var");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_GT_DATA_TYPE, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddToken((int) SyntaxConstants.SEMICOL, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_TOWER_VAR, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_TOWER_VAR,
                                        "Prod_add_tower_var");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_TOWER_VAR, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GT_DATA_TYPE,
                                        "Prod_gt_data_type");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.INTER, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.BLOAT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.PING, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.POOL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_BASE_PROD,
                                        "Prod_base_prod");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_LOCAL_DEC, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_BASE_PROD, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_STATEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_BASE_PROD, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LOCAL_DEC,
                                        "Prod_local_dec");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_LOCAL_VAR, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_LOCAL_COMP, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_LOCAL_TOWER, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LOCAL_VAR,
                                        "Prod_local_var");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.INTER, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_INTER, 1, 1);
        alt.AddToken((int) SyntaxConstants.SEMICOL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.BLOAT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_BLOAT, 1, 1);
        alt.AddToken((int) SyntaxConstants.SEMICOL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.PING, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_PING, 1, 1);
        alt.AddToken((int) SyntaxConstants.SEMICOL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.POOL, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_POOL, 1, 1);
        alt.AddToken((int) SyntaxConstants.SEMICOL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LV_INTER,
                                        "Prod_lv_inter");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_INT_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LV_INT_TAIL,
                                        "Prod_lv_int_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_INTER_VALUE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_INTER_TAIL_ADD, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_L_INTER_ARRAY_DEC, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LV_INTER_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LV_INTER_VALUE,
                                        "Prod_lv_inter_value");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_MATH_EXPRESSION, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_MATH_EXPRESSION,
                                        "Prod_math_expression");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_MATH_OPERAND, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_MATH_TAIL_EXPRESSION, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_MATH_OPERAND,
                                        "Prod_math_operand");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_MATH_EXPRESSION, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.INTER, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_INTER_CONVERSION_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.BLOAT, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_BLOAT_CONVERSION_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.FLOAT_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_VALUE_TYPE, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_MATH_TAIL_EXPRESSION,
                                        "Prod_math_tail_expression");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_MATH_OPERATOR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_MATH_OPERAND, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_MATH_TAIL_EXPRESSION, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_MATH_OPERATOR,
                                        "Prod_math_operator");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.PLUS, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.MINUS, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.DIV, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.MOD, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.MULTI, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_INTER_CONVERSION_VALUE,
                                        "Prod_inter_conversion_value");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.STRING_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_MATH_EXPRESSION, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.HOLD, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_VALUE_TYPE,
                                        "Prod_value_type");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_2D_VALUE_TYPE, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.PER, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ARGUMENT, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_INDEX_VALUE,
                                        "Prod_index_value");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_MATH_EXPRESSION, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_2D_VALUE_TYPE,
                                        "Prod_2D_value_type");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ARGUMENT,
                                        "Prod_argument");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_GENERAL_EXPRESSION, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADDITIONAL_ARGS, 0, 1);
        pattern.AddAlternative(alt);
        // alt = new ProductionPatternAlternative();
        // alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_VALUE_TYPE, 0, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_ADDITIONAL_ARGS, 0, 1);
        // pattern.AddAlternative(alt);
        // alt = new ProductionPatternAlternative();
        // alt.AddProduction((int) SyntaxConstants.PROD_BUILTIN_FUNC_CALL, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_ADDITIONAL_ARGS, 0, 1);
        // pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LITERAL_VALUE,
                                        "Prod_literal_value");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.FLOAT_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.STRING_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.BOOL_LIT, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADDITIONAL_ARGS,
                                        "Prod_additional_args");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ARGUMENT, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_BUILTIN_FUNC_CALL,
                                        "Prod_builtin_func_call");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.INTER, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_INTER_CONVERSION_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.BLOAT, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_BLOAT_CONVERSION_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.POOL, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_POOL_CONVERSION_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.PING, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_PING_CONVERSION_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_INTER_ARRAY_DEC,
                                        "Prod_L_inter_array_dec");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_INTER_1D_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_INTER_1D_TAIL,
                                        "Prod_L_inter_1D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_INTER_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_INTER_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_INTER_2D_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_INTER_ELEMENT,
                                        "Prod_L_inter_element");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_LV_INTER_VALUE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_ADD_INTER_1D,
                                        "Prod_L_add_inter_1D");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_INTER_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_INTER_1D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_INTER_2D_TAIL,
                                        "Prod_L_inter_2D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_INTER_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_INTER_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_INTER_2D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_ADD_INTER_2D,
                                        "Prod_L_add_inter_2D");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_INTER_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_INTER_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_INTER_2D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_LV_INTER_TAIL,
                                        "Prod_add_lv_inter_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LV_INTER_VAL_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_LV_INTER_VAL_TAIL,
                                        "Prod_add_lv_inter_val_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_INTER_VALUE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_INTER_TAIL_ADD, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LV_INTER_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LV_INTER_TAIL_ADD,
                                        "Prod_lv_inter_tail_add");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LV_INTER_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LV_BLOAT,
                                        "Prod_lv_bloat");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_BLOAT_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LV_BLOAT_TAIL,
                                        "Prod_lv_bloat_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_BLOAT_VALUE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_BLOAT_TAIL_ADD, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_BLOAT_ARRAY_DEC, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LV_BLOAT_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LV_BLOAT_VALUE,
                                        "Prod_lv_bloat_value");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_MATH_EXPRESSION, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_BLOAT_CONVERSION_VALUE,
                                        "Prod_bloat_conversion_value");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.STRING_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_MATH_EXPRESSION, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.HOLD, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_BLOAT_ARRAY_DEC,
                                        "Prod_bloat_array_dec");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_BLOAT_1D_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_BLOAT_1D_TAIL,
                                        "Prod_L_bloat_1D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_BLOAT_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_BLOAT_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_BLOAT_2D_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_BLOAT_ELEMENT,
                                        "Prod_L_bloat_element");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_LV_BLOAT_VALUE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_ADD_BLOAT_1D,
                                        "Prod_L_add_bloat_1D");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_BLOAT_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_BLOAT_1D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_BLOAT_2D_TAIL,
                                        "Prod_L_bloat_2D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_BLOAT_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_BLOAT_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_BLOAT_2D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_ADD_BLOAT_2D,
                                        "Prod_L_add_bloat_2D");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_BLOAT_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_BLOAT_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_BLOAT_2D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_LV_BLOAT_TAIL,
                                        "Prod_add_lv_bloat_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LV_BLOAT_VAL_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_LV_BLOAT_VAL_TAIL,
                                        "Prod_add_lv_bloat_val_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_BLOAT_VALUE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_BLOAT_TAIL_ADD, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LV_BLOAT_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LV_BLOAT_TAIL_ADD,
                                        "Prod_lv_bloat_tail_add");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LV_BLOAT_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LV_PING,
                                        "Prod_lv_ping");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_PING_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LV_PING_TAIL,
                                        "Prod_lv_ping_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_PING_VALUE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_PING_TAIL_ADD, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_L_BLOAT_ARRAY_DEC, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LV_PING_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LV_PING_VALUE,
                                        "Prod_lv_ping_value");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_STRING_CONCAT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.HOLD, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_PING_CONVERSION_VALUE,
                                        "Prod_ping_conversion_value");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.FLOAT_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.BOOL_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_STRING_CONCAT, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_STRING_CONCAT,
                                        "Prod_string_concat");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_STRING_VALUE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_STRING_TAIL_CONCAT, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_STRING_VALUE,
                                        "Prod_string_value");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_VALUE_TYPE, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.STRING_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.PING, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_PING_CONVERSION_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_STRING_TAIL_CONCAT,
                                        "Prod_string_tail_concat");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.PLUS, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_STRING_CONCAT, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_BLOAT_ARRAY_DEC,
                                        "Prod_L_bloat_array_dec");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_PING_1D_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_PING_1D_TAIL,
                                        "Prod_L_ping_1D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_PING_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_PING_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_PING_2D_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_PING_ELEMENT,
                                        "Prod_L_ping_element");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_LV_PING_VALUE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_ADD_PING_1D,
                                        "Prod_L_add_ping_1D");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_PING_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_PING_1D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_PING_2D_TAIL,
                                        "Prod_L_ping_2D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_PING_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_PING_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_PING_2D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_ADD_PING_2D,
                                        "Prod_L_add_ping_2D");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_PING_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_PING_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_PING_2D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_LV_PING_TAIL,
                                        "Prod_add_lv_ping_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LV_PING_VAL_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_LV_PING_VAL_TAIL,
                                        "Prod_add_lv_ping_val_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_PING_VALUE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_PING_TAIL_ADD, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LV_PING_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LV_PING_TAIL_ADD,
                                        "Prod_lv_ping_tail_add");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LV_PING_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LV_POOL,
                                        "Prod_lv_pool");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_POOL_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LV_POOL_TAIL,
                                        "Prod_lv_pool_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_POOL_VALUE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_POOL_TAIL_ADD, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_L_POOL_ARRAY_DEC, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LV_POOL_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LV_POOL_VALUE,
                                        "Prod_lv_pool_value");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_GENERAL_EXPRESSION, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_POOL_CONVERSION_VALUE,
                                        "Prod_pool_conversion_value");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.STRING_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.BOOL_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_VALUE_TYPE, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.HOLD, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_POOL_CONVERT,
                                        "Prod_pool_convert");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.STRING_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.BOOL_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_VALUE_TYPE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GENERAL_EXPRESSION,
                                        "Prod_general_expression");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_GENERAL_OPERAND, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GENERAL_TAIL_EXPRESSION, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GENERAL_OPERAND,
                                        "Prod_general_operand");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GENERAL_EXPRESSION, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.NOT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GENERAL_OPERAND, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.INTER, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_INTER_CONVERSION_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.BLOAT, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_BLOAT_CONVERSION_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.FLOAT_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.STRING_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.BOOL_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_VALUE_TYPE, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.POOL, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_POOL_CONVERSION_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.PING, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_PING_CONVERSION_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GENERAL_TAIL_EXPRESSION,
                                        "Prod_general_tail_expression");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_GENERAL_OPERATOR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GENERAL_OPERAND, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_GENERAL_TAIL_EXPRESSION, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_GENERAL_OPERATOR,
                                        "Prod_general_operator");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_MATH_OPERATOR, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.AND, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.OR, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.EQUAL_EQUAL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.NOT_EQUAL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.GREAT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.GREAT_E, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.LESS, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.LESS_E, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_POOL_ARRAY_DEC,
                                        "Prod_L_pool_array_dec");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_POOL_1D_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_POOL_ELEMENT,
                                        "Prod_L_pool_element");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_LV_POOL_VALUE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_POOL_1D_TAIL,
                                        "Prod_L_pool_1D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_POOL_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_POOL_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_POOL_2D_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_ADD_POOL_1D,
                                        "Prod_L_add_pool_1D");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_POOL_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_POOL_1D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_POOL_2D_TAIL,
                                        "Prod_L_pool_2D_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_POOL_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_POOL_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_POOL_2D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_L_ADD_POOL_2D,
                                        "Prod_L_add_pool_2D");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_POOL_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_POOL_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_POOL_2D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_LV_POOL_TAIL,
                                        "Prod_add_lv_pool_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LV_POOL_VAL_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_LV_POOL_VAL_TAIL,
                                        "Prod_add_lv_pool_val_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_POOL_VALUE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_POOL_TAIL_ADD, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LV_POOL_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LV_POOL_TAIL_ADD,
                                        "Prod_lv_pool_tail_add");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LV_POOL_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LOCAL_COMP,
                                        "Prod_local_comp");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMP, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LC_DATA_TYPE, 1, 1);
        alt.AddToken((int) SyntaxConstants.SEMICOL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LC_DATA_TYPE,
                                        "Prod_lc_data_type");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.INTER, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LC_INTER_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.BLOAT, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LC_BLOAT_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.PING, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LC_PING_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.POOL, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LC_POOL_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LC_INTER_TAIL,
                                        "Prod_lc_inter_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_INTER_VALUE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LC_INTER_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        // alt = new ProductionPatternAlternative();
        // alt.AddProduction((int) SyntaxConstants.PROD_LC_INTER_ARRAY_DEC, 1, 1);
        // pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_LC_INTER_TAIL,
                                        "Prod_add_lc_inter_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LC_INTER_VAL_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_LC_INTER_VAL_TAIL,
                                        "Prod_add_lc_inter_val_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_INTER_VALUE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LC_INTER_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        // pattern = new ProductionPattern((int) SyntaxConstants.PROD_LC_INTER_ARRAY_DEC,
        //                                 "Prod_lc_inter_array_dec");
        // alt = new ProductionPatternAlternative();
        // alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        // alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_LC_INTER_1D_TAIL, 1, 1);
        // pattern.AddAlternative(alt);
        // AddPattern(pattern);

        // pattern = new ProductionPattern((int) SyntaxConstants.PROD_LC_INTER_1D_TAIL,
        //                                 "Prod_lc_inter_1D_tail");
        // alt = new ProductionPatternAlternative();
        // alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        // alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_INTER_ELEMENT, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_INTER_1D, 0, 1);
        // alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        // pattern.AddAlternative(alt);
        // alt = new ProductionPatternAlternative();
        // alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        // alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_LC_INTER_2D_TAIL, 1, 1);
        // pattern.AddAlternative(alt);
        // AddPattern(pattern);

        // pattern = new ProductionPattern((int) SyntaxConstants.PROD_LC_INTER_2D_TAIL,
        //                                 "Prod_lc_inter_2D_tail");
        // alt = new ProductionPatternAlternative();
        // alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        // alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        // alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_INTER_ELEMENT, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_INTER_1D, 0, 1);
        // alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_INTER_2D, 1, 1);
        // alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        // pattern.AddAlternative(alt);
        // AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LC_BLOAT_TAIL,
                                        "Prod_lc_bloat_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_BLOAT_VALUE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LC_BLOAT_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        // alt = new ProductionPatternAlternative();
        // alt.AddProduction((int) SyntaxConstants.PROD_LC_BLOAT_ARRAY_DEC, 1, 1);
        // pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_LC_BLOAT_TAIL,
                                        "Prod_add_lc_bloat_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LC_BLOAT_VAL_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_LC_BLOAT_VAL_TAIL,
                                        "Prod_add_lc_bloat_val_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_BLOAT_VALUE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LC_BLOAT_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        // pattern = new ProductionPattern((int) SyntaxConstants.PROD_LC_BLOAT_ARRAY_DEC,
        //                                 "Prod_lc_bloat_array_dec");
        // alt = new ProductionPatternAlternative();
        // alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        // alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_LC_BLOAT_1D_TAIL, 1, 1);
        // pattern.AddAlternative(alt);
        // AddPattern(pattern);

        // pattern = new ProductionPattern((int) SyntaxConstants.PROD_LC_BLOAT_1D_TAIL,
        //                                 "Prod_lc_bloat_1D_tail");
        // alt = new ProductionPatternAlternative();
        // alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        // alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_BLOAT_ELEMENT, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_BLOAT_1D, 0, 1);
        // alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        // pattern.AddAlternative(alt);
        // alt = new ProductionPatternAlternative();
        // alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        // alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_LC_BLOAT_2D_TAIL, 1, 1);
        // pattern.AddAlternative(alt);
        // AddPattern(pattern);

        // pattern = new ProductionPattern((int) SyntaxConstants.PROD_LC_BLOAT_2D_TAIL,
        //                                 "Prod_lc_bloat_2D_tail");
        // alt = new ProductionPatternAlternative();
        // alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        // alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        // alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_BLOAT_ELEMENT, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_BLOAT_1D, 0, 1);
        // alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_BLOAT_2D, 1, 1);
        // alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        // pattern.AddAlternative(alt);
        // AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LC_PING_TAIL,
                                        "Prod_lc_ping_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_PING_VALUE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LC_PING_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        // alt = new ProductionPatternAlternative();
        // alt.AddProduction((int) SyntaxConstants.PROD_LC_PING_ARRAY_DEC, 1, 1);
        // pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_LC_PING_TAIL,
                                        "Prod_add_lc_ping_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LC_PING_VAL_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_LC_PING_VAL_TAIL,
                                        "Prod_add_lc_ping_val_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_PING_VALUE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LC_PING_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        // pattern = new ProductionPattern((int) SyntaxConstants.PROD_LC_PING_ARRAY_DEC,
        //                                 "Prod_lc_ping_array_dec");
        // alt = new ProductionPatternAlternative();
        // alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        // alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_LC_PING_1D_TAIL, 1, 1);
        // pattern.AddAlternative(alt);
        // AddPattern(pattern);

        // pattern = new ProductionPattern((int) SyntaxConstants.PROD_LC_PING_1D_TAIL,
        //                                 "Prod_lc_ping_1D_tail");
        // alt = new ProductionPatternAlternative();
        // alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        // alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_PING_ELEMENT, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_PING_1D, 0, 1);
        // alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        // pattern.AddAlternative(alt);
        // alt = new ProductionPatternAlternative();
        // alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        // alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_LC_PING_2D_TAIL, 1, 1);
        // pattern.AddAlternative(alt);
        // AddPattern(pattern);

        // pattern = new ProductionPattern((int) SyntaxConstants.PROD_LC_PING_2D_TAIL,
        //                                 "Prod_lc_ping_2D_tail");
        // alt = new ProductionPatternAlternative();
        // alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        // alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        // alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_PING_ELEMENT, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_PING_1D, 0, 1);
        // alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_PING_2D, 0, 1);
        // alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        // pattern.AddAlternative(alt);
        // AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LC_POOL_TAIL,
                                        "Prod_lc_pool_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_POOL_VALUE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LC_POOL_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        // alt = new ProductionPatternAlternative();
        // alt.AddProduction((int) SyntaxConstants.PROD_LC_POOL_ARRAY_DEC, 1, 1);
        // pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_LC_POOL_TAIL,
                                        "Prod_add_lc_pool_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LC_POOL_VAL_TAIL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_LC_POOL_VAL_TAIL,
                                        "Prod_add_lc_pool_val_tail");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_LV_POOL_VALUE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_LC_POOL_TAIL, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        // pattern = new ProductionPattern((int) SyntaxConstants.PROD_LC_POOL_ARRAY_DEC,
        //                                 "Prod_lc_pool_array_dec");
        // alt = new ProductionPatternAlternative();
        // alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        // alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_LC_POOL_1D_TAIL, 1, 1);
        // pattern.AddAlternative(alt);
        // AddPattern(pattern);

        // pattern = new ProductionPattern((int) SyntaxConstants.PROD_LC_POOL_1D_TAIL,
        //                                 "Prod_lc_pool_1D_tail");
        // alt = new ProductionPatternAlternative();
        // alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        // alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_POOL_ELEMENT, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_POOL_1D, 0, 1);
        // alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        // pattern.AddAlternative(alt);
        // alt = new ProductionPatternAlternative();
        // alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        // alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_LC_POOL_2D_TAIL, 1, 1);
        // pattern.AddAlternative(alt);
        // AddPattern(pattern);

        // pattern = new ProductionPattern((int) SyntaxConstants.PROD_LC_POOL_2D_TAIL,
        //                                 "Prod_lc_pool_2D_tail");
        // alt = new ProductionPatternAlternative();
        // alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        // alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        // alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_POOL_ELEMENT, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_POOL_1D, 0, 1);
        // alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        // alt.AddProduction((int) SyntaxConstants.PROD_L_ADD_POOL_2D, 1, 1);
        // alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        // pattern.AddAlternative(alt);
        // AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LOCAL_TOWER,
                                        "Prod_local_tower");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.TOWER, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddToken((int) SyntaxConstants.SEMICOL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_STATEMENT,
                                        "Prod_statement");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_STM_TYPE, 1, 1);
        alt.AddToken((int) SyntaxConstants.SEMICOL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_LOOP_STM, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_COND_STM, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.PUSH, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_PUSH_VALUE, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        alt.AddToken((int) SyntaxConstants.SEMICOL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.RECALL, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_RECALL_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.SEMICOL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_STM_TYPE,
                                        "Prod_stm_type");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ASSIGN_VALUE_TYPE, 0, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ASSIGNMENT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ARGUMENT, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ASSIGN_VALUE_TYPE,
                                        "Prod_assign_value_type");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_INDEX_VALUE, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_2D_VALUE_TYPE, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.PER, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ASSIGNMENT,
                                        "Prod_assignment");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ASSIGN_VALUE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ASSIGN_VALUE,
                                        "Prod_assign_value");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.HOLD, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_GENERAL_EXPRESSION, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_1D_2D_ARRAY, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_1D_2D_ARRAY,
                                        "Prod_1D_2D_array");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_ASSIGN_ARRAY_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_ASSIGN_1D, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ASSIGN_ARRAY_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_ASSIGN_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_ASSIGN_2D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ASSIGN_ARRAY_ELEMENT,
                                        "Prod_assign_array_element");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_GENERAL_EXPRESSION, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_ASSIGN_1D,
                                        "Prod_add_assign_1D");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ASSIGN_ARRAY_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_ASSIGN_1D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADD_ASSIGN_2D,
                                        "Prod_add_assign_2D");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ASSIGN_ARRAY_ELEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_ASSIGN_1D, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADD_ASSIGN_2D, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LOOP_STM,
                                        "Prod_loop_stm");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.FOR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_INIT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_FOR_KEYWORD, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_END, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_CONTENT, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.WHILE, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_CONDITION, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_CONTENT, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.DO, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_CONTENT, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        alt.AddToken((int) SyntaxConstants.WHILE, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_CONDITION, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_INIT,
                                        "Prod_init");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddToken((int) SyntaxConstants.ASSIGN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_INIT_VALUE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_INIT_VALUE,
                                        "Prod_init_value");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_VALUE_TYPE, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_FOR_KEYWORD,
                                        "Prod_for_keyword");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.UP, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.DOWN, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_END,
                                        "Prod_end");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.INT_LIT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_VALUE_TYPE, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_CONTENT,
                                        "Prod_content");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_LOCAL_DEC, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_CONTENT, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_STATEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_CONTENT, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_LOOP_TERMINATOR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_CONTENT, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_CONDITION,
                                        "Prod_condition");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_GENERAL_EXPRESSION, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_LOOP_TERMINATOR,
                                        "Prod_loop_terminator");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.DESTROY, 1, 1);
        alt.AddToken((int) SyntaxConstants.SEMICOL, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMIT, 1, 1);
        alt.AddToken((int) SyntaxConstants.SEMICOL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_COND_STM,
                                        "Prod_cond_stm");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.IF, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_CONDITION, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_BODY, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ELSE_CLAUSE, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_BODY,
                                        "Prod_body");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_CONTENT_ONELINE, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_CONTENT, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_CONTENT_ONELINE,
                                        "Prod_content_oneline");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_LOCAL_DEC, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_STATEMENT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_LOOP_TERMINATOR, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ELSE_CLAUSE,
                                        "Prod_else_clause");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.ELSE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_BODY, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_PUSH_VALUE,
                                        "Prod_push_value");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_STRING_CONCAT, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_RECALL_VALUE,
                                        "Prod_recall_value");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_GENERAL_EXPRESSION, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_USER_FUNCTION,
                                        "Prod_user_function");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.SPAWN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_SPAWN_TAIL, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_USER_FUNCTION, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_SPAWN_TAIL,
                                        "Prod_spawn_tail");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_SPAWN_DATA_TYPE, 1, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_PAREN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_PARAMETER, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_PAREN, 1, 1);
        alt.AddToken((int) SyntaxConstants.O_BRACE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_USER_BODY, 1, 1);
        alt.AddToken((int) SyntaxConstants.C_BRACE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_SPAWN_DATA_TYPE,
                                        "Prod_spawn_data_type");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.VOID, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_DATA_TYPE, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_DATA_TYPE,
                                        "Prod_data_type");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.INTER, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.BLOAT, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.PING, 1, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.POOL, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_PARAMETER,
                                        "Prod_parameter");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_DATA_TYPE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_OPTIONAL_ARRAY, 0, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADDITIONAL_PARAM, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_OPTIONAL_ARRAY,
                                        "Prod_optional_array");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.O_SQR, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_2D_ARRAY, 0, 1);
        alt.AddToken((int) SyntaxConstants.C_SQR, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_2D_ARRAY,
                                        "Prod_2D_array");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_ADDITIONAL_PARAM,
                                        "Prod_additional_param");
        alt = new ProductionPatternAlternative();
        alt.AddToken((int) SyntaxConstants.COMMA, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_DATA_TYPE, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_OPTIONAL_ARRAY, 0, 1);
        alt.AddToken((int) SyntaxConstants.IDEN, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_ADDITIONAL_PARAM, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);

        pattern = new ProductionPattern((int) SyntaxConstants.PROD_USER_BODY,
                                        "Prod_user_body");
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_LOCAL_DEC, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_USER_BODY, 0, 1);
        pattern.AddAlternative(alt);
        alt = new ProductionPatternAlternative();
        alt.AddProduction((int) SyntaxConstants.PROD_STATEMENT, 1, 1);
        alt.AddProduction((int) SyntaxConstants.PROD_USER_BODY, 0, 1);
        pattern.AddAlternative(alt);
        AddPattern(pattern);
    }
}