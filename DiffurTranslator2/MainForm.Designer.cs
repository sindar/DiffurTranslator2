namespace DiffurTranslator2
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TranslateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LegendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.CodeRTextBox = new System.Windows.Forms.RichTextBox();
            this.SaveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.DebugRTextBox = new System.Windows.Forms.RichTextBox();
            this.LexRTextBox = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.MainMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.SettingsToolStripMenuItem,
            this.TranslateToolStripMenuItem});
            this.MainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.Size = new System.Drawing.Size(532, 24);
            this.MainMenuStrip.TabIndex = 0;
            this.MainMenuStrip.Text = "menuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.FileToolStripMenuItem.Text = "Файл";
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.OpenToolStripMenuItem.Text = "Открыть";
            this.OpenToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.saveToolStripMenuItem.Text = "Сохранить";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.ExitToolStripMenuItem.Text = "Выход";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // TranslateToolStripMenuItem
            // 
            this.TranslateToolStripMenuItem.Name = "TranslateToolStripMenuItem";
            this.TranslateToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
            this.TranslateToolStripMenuItem.Text = "Транслировать";
            this.TranslateToolStripMenuItem.Click += new System.EventHandler(this.TranslateToolStripMenuItem_Click);
            // 
            // SettingsToolStripMenuItem
            // 
            this.SettingsToolStripMenuItem.Checked = true;
            this.SettingsToolStripMenuItem.CheckOnClick = true;
            this.SettingsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LegendToolStripMenuItem});
            this.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem";
            this.SettingsToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.SettingsToolStripMenuItem.Text = "Настройки";
            // 
            // LegendToolStripMenuItem
            // 
            this.LegendToolStripMenuItem.Checked = true;
            this.LegendToolStripMenuItem.CheckOnClick = true;
            this.LegendToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LegendToolStripMenuItem.Name = "LegendToolStripMenuItem";
            this.LegendToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.LegendToolStripMenuItem.Text = "Легенда на графике";
            this.LegendToolStripMenuItem.CheckedChanged += new System.EventHandler(this.LegendToolStripMenuItem_CheckedChanged);
            // 
            // OpenFileDialog1
            // 
            this.OpenFileDialog1.FileName = "openFileDialog1";
            this.OpenFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFileDialog_FileOk);
            // 
            // CodeRTextBox
            // 
            this.CodeRTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CodeRTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CodeRTextBox.Location = new System.Drawing.Point(0, 0);
            this.CodeRTextBox.Name = "CodeRTextBox";
            this.CodeRTextBox.Size = new System.Drawing.Size(508, 344);
            this.CodeRTextBox.TabIndex = 1;
            this.CodeRTextBox.Text = "";
            // 
            // SaveFileDialog1
            // 
            this.SaveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.SaveFileDialog_FileOk);
            // 
            // DebugRTextBox
            // 
            this.DebugRTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DebugRTextBox.ForeColor = System.Drawing.Color.MediumBlue;
            this.DebugRTextBox.Location = new System.Drawing.Point(0, 0);
            this.DebugRTextBox.Name = "DebugRTextBox";
            this.DebugRTextBox.Size = new System.Drawing.Size(508, 131);
            this.DebugRTextBox.TabIndex = 2;
            this.DebugRTextBox.Text = "";
            // 
            // LexRTextBox
            // 
            this.LexRTextBox.Location = new System.Drawing.Point(662, 60);
            this.LexRTextBox.Name = "LexRTextBox";
            this.LexRTextBox.Size = new System.Drawing.Size(79, 344);
            this.LexRTextBox.TabIndex = 4;
            this.LexRTextBox.Text = "";
            this.LexRTextBox.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 39);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.CodeRTextBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.DebugRTextBox);
            this.splitContainer1.Size = new System.Drawing.Size(508, 479);
            this.splitContainer1.SplitterDistance = 344;
            this.splitContainer1.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 530);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.LexRTextBox);
            this.Controls.Add(this.MainMenuStrip);
            this.Name = "MainForm";
            this.Text = "Транслятор диффуров";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.RichTextBox CodeRTextBox;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog1;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog1;
        private System.Windows.Forms.RichTextBox LexRTextBox;
        public System.Windows.Forms.RichTextBox DebugRTextBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem TranslateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LegendToolStripMenuItem;
    }
}

