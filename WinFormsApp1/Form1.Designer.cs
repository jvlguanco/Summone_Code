namespace WinFormsApp1;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        label1 = new Label();
        lexer = new Button();
        syntax = new Button();
        semantic = new Button();
        clear = new Button();
        Code = new LineNumberRTB();
        tabControl1 = new TabControl();
        tabPage1 = new TabPage();
        LexGrid = new DataGridView();
        ID = new DataGridViewTextBoxColumn();
        Lexeme = new DataGridViewTextBoxColumn();
        Token = new DataGridViewTextBoxColumn();
        tabControl2 = new TabControl();
        tabPage3 = new TabPage();
        DataLexicalError = new DataGridView();
        dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
        TempGrid = new DataGridView();
        dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
        OutputText = new RichTextBox();
        tabPage4 = new TabPage();
        DataSyntaxError = new DataGridView();
        dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
        Column1 = new DataGridViewTextBoxColumn();
        Column3 = new DataGridViewTextBoxColumn();
        tabPage5 = new TabPage();
        semanticError = new DataGridView();
        dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
        Line = new DataGridViewTextBoxColumn();
        run = new Button();
        button1 = new Button();
        tabControl1.SuspendLayout();
        tabPage1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)LexGrid).BeginInit();
        tabControl2.SuspendLayout();
        tabPage3.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DataLexicalError).BeginInit();
        ((System.ComponentModel.ISupportInitialize)TempGrid).BeginInit();
        tabPage4.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DataSyntaxError).BeginInit();
        tabPage5.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)semanticError).BeginInit();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new Font("Tahoma", 27.75F, FontStyle.Bold);
        label1.Location = new Point(43, 13);
        label1.Name = "label1";
        label1.Size = new Size(335, 45);
        label1.TabIndex = 0;
        label1.Text = "⚔️SUMMONER⚔️";
        // 
        // lexer
        // 
        lexer.Location = new Point(103, 134);
        lexer.Name = "lexer";
        lexer.Size = new Size(142, 40);
        lexer.TabIndex = 1;
        lexer.Text = "Lexical Analyzer";
        lexer.UseVisualStyleBackColor = true;
        lexer.Click += lexer_Click;
        // 
        // syntax
        // 
        syntax.Location = new Point(103, 239);
        syntax.Name = "syntax";
        syntax.Size = new Size(142, 40);
        syntax.TabIndex = 2;
        syntax.Text = "Syntax Analyzer";
        syntax.UseVisualStyleBackColor = true;
        syntax.Click += syntax_Click;
        // 
        // semantic
        // 
        semantic.Location = new Point(103, 331);
        semantic.Name = "semantic";
        semantic.Size = new Size(142, 40);
        semantic.TabIndex = 3;
        semantic.Text = "Semantic Analyzer";
        semantic.UseVisualStyleBackColor = true;
        semantic.Click += semantic_Click;
        // 
        // clear
        // 
        clear.Location = new Point(337, 70);
        clear.Name = "clear";
        clear.Size = new Size(142, 40);
        clear.TabIndex = 4;
        clear.Text = "Clear";
        clear.UseVisualStyleBackColor = true;
        clear.Click += clear_Click;
        // 
        // Code
        // 
        Code.BackColor = SystemColors.Window;
        Code.BorderStyle = BorderStyle.Fixed3D;
        Code.Font = new Font("Segoe UI", 12F);
        Code.Location = new Point(41, 117);
        Code.Margin = new Padding(4);
        Code.Name = "Code";
        Code.Size = new Size(767, 476);
        Code.TabIndex = 5;
        Code.Load += Code_Load;
        // 
        // tabControl1
        // 
        tabControl1.Controls.Add(tabPage1);
        tabControl1.Font = new Font("Segoe UI", 11F);
        tabControl1.Location = new Point(817, 120);
        tabControl1.Name = "tabControl1";
        tabControl1.SelectedIndex = 0;
        tabControl1.Size = new Size(349, 477);
        tabControl1.TabIndex = 6;
        // 
        // tabPage1
        // 
        tabPage1.Controls.Add(LexGrid);
        tabPage1.Controls.Add(lexer);
        tabPage1.Controls.Add(syntax);
        tabPage1.Controls.Add(semantic);
        tabPage1.Location = new Point(4, 29);
        tabPage1.Name = "tabPage1";
        tabPage1.Padding = new Padding(3);
        tabPage1.Size = new Size(341, 444);
        tabPage1.TabIndex = 0;
        tabPage1.Text = "Lexical Result";
        tabPage1.UseVisualStyleBackColor = true;
        // 
        // LexGrid
        // 
        LexGrid.AllowUserToAddRows = false;
        LexGrid.AllowUserToDeleteRows = false;
        LexGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        LexGrid.Columns.AddRange(new DataGridViewColumn[] { ID, Lexeme, Token });
        LexGrid.Location = new Point(0, 0);
        LexGrid.Name = "LexGrid";
        LexGrid.ReadOnly = true;
        LexGrid.RowHeadersVisible = false;
        LexGrid.Size = new Size(341, 444);
        LexGrid.TabIndex = 0;
        // 
        // ID
        // 
        ID.HeaderText = "ID";
        ID.Name = "ID";
        ID.ReadOnly = true;
        ID.Width = 35;
        // 
        // Lexeme
        // 
        Lexeme.HeaderText = "Lexeme";
        Lexeme.Name = "Lexeme";
        Lexeme.ReadOnly = true;
        Lexeme.Width = 140;
        // 
        // Token
        // 
        Token.HeaderText = "Token";
        Token.Name = "Token";
        Token.ReadOnly = true;
        Token.Width = 140;
        // 
        // tabControl2
        // 
        tabControl2.Controls.Add(tabPage3);
        tabControl2.Controls.Add(tabPage4);
        tabControl2.Controls.Add(tabPage5);
        tabControl2.Font = new Font("Segoe UI", 12F);
        tabControl2.Location = new Point(1, 603);
        tabControl2.Name = "tabControl2";
        tabControl2.SelectedIndex = 0;
        tabControl2.Size = new Size(1165, 156);
        tabControl2.TabIndex = 7;
        // 
        // tabPage3
        // 
        tabPage3.Controls.Add(DataLexicalError);
        tabPage3.Controls.Add(TempGrid);
        tabPage3.Location = new Point(4, 30);
        tabPage3.Name = "tabPage3";
        tabPage3.Padding = new Padding(3);
        tabPage3.Size = new Size(1157, 122);
        tabPage3.TabIndex = 0;
        tabPage3.Text = "Lexical Status";
        tabPage3.UseVisualStyleBackColor = true;
        // 
        // DataLexicalError
        // 
        DataLexicalError.AllowUserToAddRows = false;
        DataLexicalError.AllowUserToDeleteRows = false;
        DataLexicalError.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        DataLexicalError.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5 });
        DataLexicalError.Location = new Point(0, 0);
        DataLexicalError.Name = "DataLexicalError";
        DataLexicalError.ReadOnly = true;
        DataLexicalError.RowHeadersVisible = false;
        DataLexicalError.Size = new Size(1157, 119);
        DataLexicalError.TabIndex = 2;
        DataLexicalError.CellContentClick += DataLexicalError_CellContentClick;
        // 
        // dataGridViewTextBoxColumn4
        // 
        dataGridViewTextBoxColumn4.HeaderText = "ID";
        dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
        dataGridViewTextBoxColumn4.ReadOnly = true;
        dataGridViewTextBoxColumn4.Width = 70;
        // 
        // dataGridViewTextBoxColumn5
        // 
        dataGridViewTextBoxColumn5.HeaderText = "Message";
        dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
        dataGridViewTextBoxColumn5.ReadOnly = true;
        dataGridViewTextBoxColumn5.Width = 1075;
        // 
        // TempGrid
        // 
        TempGrid.AllowUserToAddRows = false;
        TempGrid.AllowUserToDeleteRows = false;
        TempGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        TempGrid.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn7, dataGridViewTextBoxColumn8 });
        TempGrid.Location = new Point(203, 19);
        TempGrid.Name = "TempGrid";
        TempGrid.ReadOnly = true;
        TempGrid.RowHeadersVisible = false;
        TempGrid.Size = new Size(313, 68);
        TempGrid.TabIndex = 10;
        // 
        // dataGridViewTextBoxColumn3
        // 
        dataGridViewTextBoxColumn3.HeaderText = "ID";
        dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
        dataGridViewTextBoxColumn3.ReadOnly = true;
        dataGridViewTextBoxColumn3.Width = 35;
        // 
        // dataGridViewTextBoxColumn7
        // 
        dataGridViewTextBoxColumn7.HeaderText = "Lexeme";
        dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
        dataGridViewTextBoxColumn7.ReadOnly = true;
        dataGridViewTextBoxColumn7.Width = 140;
        // 
        // dataGridViewTextBoxColumn8
        // 
        dataGridViewTextBoxColumn8.HeaderText = "Token";
        dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
        dataGridViewTextBoxColumn8.ReadOnly = true;
        dataGridViewTextBoxColumn8.Width = 140;
        // 
        // OutputText
        // 
        OutputText.Location = new Point(599, 243);
        OutputText.Name = "OutputText";
        OutputText.Size = new Size(392, 111);
        OutputText.TabIndex = 9;
        OutputText.Text = "";
        // 
        // tabPage4
        // 
        tabPage4.Controls.Add(DataSyntaxError);
        tabPage4.Location = new Point(4, 30);
        tabPage4.Name = "tabPage4";
        tabPage4.Padding = new Padding(3);
        tabPage4.Size = new Size(1157, 122);
        tabPage4.TabIndex = 1;
        tabPage4.Text = "Syntax Result";
        tabPage4.UseVisualStyleBackColor = true;
        // 
        // DataSyntaxError
        // 
        DataSyntaxError.AllowUserToAddRows = false;
        DataSyntaxError.AllowUserToDeleteRows = false;
        DataSyntaxError.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        DataSyntaxError.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn6, Column1, Column3 });
        DataSyntaxError.Location = new Point(0, 0);
        DataSyntaxError.Name = "DataSyntaxError";
        DataSyntaxError.ReadOnly = true;
        DataSyntaxError.RowHeadersVisible = false;
        DataSyntaxError.Size = new Size(1157, 119);
        DataSyntaxError.TabIndex = 3;
        // 
        // dataGridViewTextBoxColumn6
        // 
        dataGridViewTextBoxColumn6.HeaderText = "ID";
        dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
        dataGridViewTextBoxColumn6.ReadOnly = true;
        dataGridViewTextBoxColumn6.Width = 50;
        // 
        // Column1
        // 
        Column1.HeaderText = "Line";
        Column1.Name = "Column1";
        Column1.ReadOnly = true;
        Column1.Width = 70;
        // 
        // Column3
        // 
        Column3.HeaderText = "Message";
        Column3.Name = "Column3";
        Column3.ReadOnly = true;
        Column3.Width = 1025;
        // 
        // tabPage5
        // 
        tabPage5.Controls.Add(semanticError);
        tabPage5.Location = new Point(4, 30);
        tabPage5.Name = "tabPage5";
        tabPage5.Padding = new Padding(3);
        tabPage5.Size = new Size(1157, 122);
        tabPage5.TabIndex = 2;
        tabPage5.Text = "Semantic Error";
        tabPage5.UseVisualStyleBackColor = true;
        // 
        // semanticError
        // 
        semanticError.AllowUserToAddRows = false;
        semanticError.AllowUserToDeleteRows = false;
        semanticError.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        semanticError.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, Line });
        semanticError.Location = new Point(0, 0);
        semanticError.Name = "semanticError";
        semanticError.ReadOnly = true;
        semanticError.RowHeadersVisible = false;
        semanticError.Size = new Size(1157, 121);
        semanticError.TabIndex = 4;
        semanticError.CellContentClick += semanticError_CellContentClick;
        // 
        // dataGridViewTextBoxColumn1
        // 
        dataGridViewTextBoxColumn1.HeaderText = "ID";
        dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
        dataGridViewTextBoxColumn1.ReadOnly = true;
        dataGridViewTextBoxColumn1.Width = 50;
        // 
        // dataGridViewTextBoxColumn2
        // 
        dataGridViewTextBoxColumn2.HeaderText = "Message";
        dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
        dataGridViewTextBoxColumn2.ReadOnly = true;
        dataGridViewTextBoxColumn2.Width = 1000;
        // 
        // Line
        // 
        Line.HeaderText = "Line";
        Line.Name = "Line";
        Line.ReadOnly = true;
        // 
        // run
        // 
        run.Location = new Point(189, 70);
        run.Name = "run";
        run.Size = new Size(142, 40);
        run.TabIndex = 8;
        run.Text = "Run";
        run.UseVisualStyleBackColor = true;
        run.Click += run_Click;
        // 
        // button1
        // 
        button1.Location = new Point(41, 70);
        button1.Name = "button1";
        button1.Size = new Size(142, 40);
        button1.TabIndex = 9;
        button1.Text = "Analyze";
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1170, 759);
        Controls.Add(button1);
        Controls.Add(run);
        Controls.Add(OutputText);
        Controls.Add(tabControl2);
        Controls.Add(tabControl1);
        Controls.Add(Code);
        Controls.Add(clear);
        Controls.Add(label1);
        Name = "Form1";
        Text = "Form1";
        tabControl1.ResumeLayout(false);
        tabPage1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)LexGrid).EndInit();
        tabControl2.ResumeLayout(false);
        tabPage3.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)DataLexicalError).EndInit();
        ((System.ComponentModel.ISupportInitialize)TempGrid).EndInit();
        tabPage4.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)DataSyntaxError).EndInit();
        tabPage5.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)semanticError).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label label1;
    private Button lexer;
    private Button syntax;
    private Button semantic;
    private Button clear;
    private LineNumberRTB Code;
    private TabControl tabControl1;
    private TabPage tabPage1;
    private DataGridView LexGrid;
    private DataGridViewTextBoxColumn ID;
    private DataGridViewTextBoxColumn Lexeme;
    private DataGridViewTextBoxColumn Token;
    private TabControl tabControl2;
    private TabPage tabPage3;
    private DataGridView DataLexicalError;
    private TabPage tabPage4;
    private TabPage tabPage5;
    private DataGridView DataSyntaxError;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    private DataGridViewTextBoxColumn Column1;
    private DataGridViewTextBoxColumn Column3;
    private DataGridView semanticError;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn Line;
    private RichTextBox OutputText;
    private Button run;
    private DataGridView TempGrid;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
    private Button button1;
}