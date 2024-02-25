namespace WinFormsApp1
{
    partial class LineNumberRTB
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            richTextBox = new RichTextBox();
            SuspendLayout();
            // 
            // richTextBox
            // 
            richTextBox.Location = new Point(39, 0);
            richTextBox.Name = "richTextBox";
            richTextBox.Size = new Size(728, 476);
            richTextBox.TabIndex = 0;
            richTextBox.Text = "";
            // 
            // LineNumberRTB
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(richTextBox);
            Name = "LineNumberRTB";
            Size = new Size(767, 476);
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox richTextBox;
    }
}
