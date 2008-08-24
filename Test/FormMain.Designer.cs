namespace WudiStudio.Chestnut.Test
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelText = new System.Windows.Forms.Label();
            this.textBoxText = new System.Windows.Forms.TextBox();
            this.labelResult = new System.Windows.Forms.Label();
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.labelLexicon = new System.Windows.Forms.Label();
            this.textBoxLexicon = new System.Windows.Forms.TextBox();
            this.labelMatchMode = new System.Windows.Forms.Label();
            this.labelSingleMode = new System.Windows.Forms.Label();
            this.labelEnglishMode = new System.Windows.Forms.Label();
            this.panelMatchMode = new System.Windows.Forms.Panel();
            this.radioButtonMatchModeFast = new System.Windows.Forms.RadioButton();
            this.radioButtonMatchModeNormal = new System.Windows.Forms.RadioButton();
            this.panelSingleMode = new System.Windows.Forms.Panel();
            this.radioButtonSingleModeDualize = new System.Windows.Forms.RadioButton();
            this.radioButtonSingleModeConcat = new System.Windows.Forms.RadioButton();
            this.radioButtonSingleModeThrowaway = new System.Windows.Forms.RadioButton();
            this.panelEnglishMode = new System.Windows.Forms.Panel();
            this.radioButtonEnglishModeThrowaway = new System.Windows.Forms.RadioButton();
            this.radioButtonEnglishModeNormal = new System.Windows.Forms.RadioButton();
            this.buttonLexicon = new System.Windows.Forms.Button();
            this.buttonTokenize = new System.Windows.Forms.Button();
            this.groupBoxOptions = new System.Windows.Forms.GroupBox();
            this.panelMatchMode.SuspendLayout();
            this.panelSingleMode.SuspendLayout();
            this.panelEnglishMode.SuspendLayout();
            this.groupBoxOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelText
            // 
            this.labelText.AutoSize = true;
            this.labelText.Location = new System.Drawing.Point(12, 12);
            this.labelText.Name = "labelText";
            this.labelText.Size = new System.Drawing.Size(35, 12);
            this.labelText.TabIndex = 0;
            this.labelText.Text = "文本:";
            // 
            // textBoxText
            // 
            this.textBoxText.Location = new System.Drawing.Point(12, 27);
            this.textBoxText.Multiline = true;
            this.textBoxText.Name = "textBoxText";
            this.textBoxText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxText.Size = new System.Drawing.Size(608, 100);
            this.textBoxText.TabIndex = 1;
            this.textBoxText.Text = "这使他迸发出旺盛活力。第一次登上海拔八千米的山。";
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Location = new System.Drawing.Point(12, 138);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(35, 12);
            this.labelResult.TabIndex = 2;
            this.labelResult.Text = "结果:";
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(12, 153);
            this.textBoxResult.Multiline = true;
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxResult.Size = new System.Drawing.Size(608, 130);
            this.textBoxResult.TabIndex = 3;
            // 
            // labelLexicon
            // 
            this.labelLexicon.AutoSize = true;
            this.labelLexicon.Location = new System.Drawing.Point(9, 22);
            this.labelLexicon.Name = "labelLexicon";
            this.labelLexicon.Size = new System.Drawing.Size(59, 12);
            this.labelLexicon.TabIndex = 0;
            this.labelLexicon.Text = "词库文件:";
            // 
            // textBoxLexicon
            // 
            this.textBoxLexicon.Location = new System.Drawing.Point(81, 19);
            this.textBoxLexicon.Name = "textBoxLexicon";
            this.textBoxLexicon.Size = new System.Drawing.Size(242, 21);
            this.textBoxLexicon.TabIndex = 1;
            // 
            // labelMatchMode
            // 
            this.labelMatchMode.AutoSize = true;
            this.labelMatchMode.Location = new System.Drawing.Point(9, 51);
            this.labelMatchMode.Name = "labelMatchMode";
            this.labelMatchMode.Size = new System.Drawing.Size(59, 12);
            this.labelMatchMode.TabIndex = 4;
            this.labelMatchMode.Text = "匹配模式:";
            // 
            // labelSingleMode
            // 
            this.labelSingleMode.AutoSize = true;
            this.labelSingleMode.Location = new System.Drawing.Point(9, 79);
            this.labelSingleMode.Name = "labelSingleMode";
            this.labelSingleMode.Size = new System.Drawing.Size(59, 12);
            this.labelSingleMode.TabIndex = 5;
            this.labelSingleMode.Text = "单字模式:";
            // 
            // labelEnglishMode
            // 
            this.labelEnglishMode.AutoSize = true;
            this.labelEnglishMode.Location = new System.Drawing.Point(9, 108);
            this.labelEnglishMode.Name = "labelEnglishMode";
            this.labelEnglishMode.Size = new System.Drawing.Size(59, 12);
            this.labelEnglishMode.TabIndex = 6;
            this.labelEnglishMode.Text = "英文模式:";
            // 
            // panelMatchMode
            // 
            this.panelMatchMode.Controls.Add(this.radioButtonMatchModeFast);
            this.panelMatchMode.Controls.Add(this.radioButtonMatchModeNormal);
            this.panelMatchMode.Location = new System.Drawing.Point(81, 46);
            this.panelMatchMode.Name = "panelMatchMode";
            this.panelMatchMode.Size = new System.Drawing.Size(320, 22);
            this.panelMatchMode.TabIndex = 7;
            // 
            // radioButtonMatchModeFast
            // 
            this.radioButtonMatchModeFast.AutoSize = true;
            this.radioButtonMatchModeFast.Location = new System.Drawing.Point(122, 3);
            this.radioButtonMatchModeFast.Name = "radioButtonMatchModeFast";
            this.radioButtonMatchModeFast.Size = new System.Drawing.Size(125, 16);
            this.radioButtonMatchModeFast.TabIndex = 1;
            this.radioButtonMatchModeFast.TabStop = true;
            this.radioButtonMatchModeFast.Text = "快速 (仅逆向匹配)";
            this.radioButtonMatchModeFast.UseVisualStyleBackColor = true;
            // 
            // radioButtonMatchModeNormal
            // 
            this.radioButtonMatchModeNormal.AutoSize = true;
            this.radioButtonMatchModeNormal.Location = new System.Drawing.Point(3, 3);
            this.radioButtonMatchModeNormal.Name = "radioButtonMatchModeNormal";
            this.radioButtonMatchModeNormal.Size = new System.Drawing.Size(113, 16);
            this.radioButtonMatchModeNormal.TabIndex = 0;
            this.radioButtonMatchModeNormal.TabStop = true;
            this.radioButtonMatchModeNormal.Text = "标准 (双向匹配)";
            this.radioButtonMatchModeNormal.UseVisualStyleBackColor = true;
            // 
            // panelSingleMode
            // 
            this.panelSingleMode.Controls.Add(this.radioButtonSingleModeDualize);
            this.panelSingleMode.Controls.Add(this.radioButtonSingleModeConcat);
            this.panelSingleMode.Controls.Add(this.radioButtonSingleModeThrowaway);
            this.panelSingleMode.Location = new System.Drawing.Point(81, 74);
            this.panelSingleMode.Name = "panelSingleMode";
            this.panelSingleMode.Size = new System.Drawing.Size(320, 22);
            this.panelSingleMode.TabIndex = 8;
            // 
            // radioButtonSingleModeDualize
            // 
            this.radioButtonSingleModeDualize.AutoSize = true;
            this.radioButtonSingleModeDualize.Location = new System.Drawing.Point(80, 3);
            this.radioButtonSingleModeDualize.Name = "radioButtonSingleModeDualize";
            this.radioButtonSingleModeDualize.Size = new System.Drawing.Size(83, 16);
            this.radioButtonSingleModeDualize.TabIndex = 2;
            this.radioButtonSingleModeDualize.TabStop = true;
            this.radioButtonSingleModeDualize.Text = "二元法切分";
            this.radioButtonSingleModeDualize.UseVisualStyleBackColor = true;
            // 
            // radioButtonSingleModeConcat
            // 
            this.radioButtonSingleModeConcat.AutoSize = true;
            this.radioButtonSingleModeConcat.Location = new System.Drawing.Point(3, 3);
            this.radioButtonSingleModeConcat.Name = "radioButtonSingleModeConcat";
            this.radioButtonSingleModeConcat.Size = new System.Drawing.Size(71, 16);
            this.radioButtonSingleModeConcat.TabIndex = 1;
            this.radioButtonSingleModeConcat.TabStop = true;
            this.radioButtonSingleModeConcat.Text = "连接成词";
            this.radioButtonSingleModeConcat.UseVisualStyleBackColor = true;
            // 
            // radioButtonSingleModeThrowaway
            // 
            this.radioButtonSingleModeThrowaway.AutoSize = true;
            this.radioButtonSingleModeThrowaway.Location = new System.Drawing.Point(169, 3);
            this.radioButtonSingleModeThrowaway.Name = "radioButtonSingleModeThrowaway";
            this.radioButtonSingleModeThrowaway.Size = new System.Drawing.Size(95, 16);
            this.radioButtonSingleModeThrowaway.TabIndex = 0;
            this.radioButtonSingleModeThrowaway.TabStop = true;
            this.radioButtonSingleModeThrowaway.Text = "丢弃所有单字";
            this.radioButtonSingleModeThrowaway.UseVisualStyleBackColor = true;
            // 
            // panelEnglishMode
            // 
            this.panelEnglishMode.Controls.Add(this.radioButtonEnglishModeThrowaway);
            this.panelEnglishMode.Controls.Add(this.radioButtonEnglishModeNormal);
            this.panelEnglishMode.Location = new System.Drawing.Point(81, 102);
            this.panelEnglishMode.Name = "panelEnglishMode";
            this.panelEnglishMode.Size = new System.Drawing.Size(320, 22);
            this.panelEnglishMode.TabIndex = 9;
            // 
            // radioButtonEnglishModeThrowaway
            // 
            this.radioButtonEnglishModeThrowaway.AutoSize = true;
            this.radioButtonEnglishModeThrowaway.Location = new System.Drawing.Point(56, 4);
            this.radioButtonEnglishModeThrowaway.Name = "radioButtonEnglishModeThrowaway";
            this.radioButtonEnglishModeThrowaway.Size = new System.Drawing.Size(119, 16);
            this.radioButtonEnglishModeThrowaway.TabIndex = 1;
            this.radioButtonEnglishModeThrowaway.TabStop = true;
            this.radioButtonEnglishModeThrowaway.Text = "丢弃所有非中文词";
            this.radioButtonEnglishModeThrowaway.UseVisualStyleBackColor = true;
            // 
            // radioButtonEnglishModeNormal
            // 
            this.radioButtonEnglishModeNormal.AutoSize = true;
            this.radioButtonEnglishModeNormal.Location = new System.Drawing.Point(3, 4);
            this.radioButtonEnglishModeNormal.Name = "radioButtonEnglishModeNormal";
            this.radioButtonEnglishModeNormal.Size = new System.Drawing.Size(47, 16);
            this.radioButtonEnglishModeNormal.TabIndex = 0;
            this.radioButtonEnglishModeNormal.TabStop = true;
            this.radioButtonEnglishModeNormal.Text = "标准";
            this.radioButtonEnglishModeNormal.UseVisualStyleBackColor = true;
            // 
            // buttonLexicon
            // 
            this.buttonLexicon.Location = new System.Drawing.Point(329, 17);
            this.buttonLexicon.Name = "buttonLexicon";
            this.buttonLexicon.Size = new System.Drawing.Size(72, 23);
            this.buttonLexicon.TabIndex = 10;
            this.buttonLexicon.Text = "浏览...";
            this.buttonLexicon.UseVisualStyleBackColor = true;
            // 
            // buttonTokenize
            // 
            this.buttonTokenize.Location = new System.Drawing.Point(486, 350);
            this.buttonTokenize.Name = "buttonTokenize";
            this.buttonTokenize.Size = new System.Drawing.Size(75, 23);
            this.buttonTokenize.TabIndex = 11;
            this.buttonTokenize.Text = "分词";
            this.buttonTokenize.UseVisualStyleBackColor = true;
            this.buttonTokenize.Click += new System.EventHandler(this.buttonTokenize_Click);
            // 
            // groupBoxOptions
            // 
            this.groupBoxOptions.Controls.Add(this.labelLexicon);
            this.groupBoxOptions.Controls.Add(this.textBoxLexicon);
            this.groupBoxOptions.Controls.Add(this.buttonLexicon);
            this.groupBoxOptions.Controls.Add(this.labelMatchMode);
            this.groupBoxOptions.Controls.Add(this.panelEnglishMode);
            this.groupBoxOptions.Controls.Add(this.labelSingleMode);
            this.groupBoxOptions.Controls.Add(this.panelSingleMode);
            this.groupBoxOptions.Controls.Add(this.labelEnglishMode);
            this.groupBoxOptions.Controls.Add(this.panelMatchMode);
            this.groupBoxOptions.Location = new System.Drawing.Point(12, 299);
            this.groupBoxOptions.Name = "groupBoxOptions";
            this.groupBoxOptions.Size = new System.Drawing.Size(412, 132);
            this.groupBoxOptions.TabIndex = 12;
            this.groupBoxOptions.TabStop = false;
            this.groupBoxOptions.Text = "选项";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 446);
            this.Controls.Add(this.groupBoxOptions);
            this.Controls.Add(this.buttonTokenize);
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.textBoxText);
            this.Controls.Add(this.labelText);
            this.Name = "Form1";
            this.Text = "Test";
            this.panelMatchMode.ResumeLayout(false);
            this.panelMatchMode.PerformLayout();
            this.panelSingleMode.ResumeLayout(false);
            this.panelSingleMode.PerformLayout();
            this.panelEnglishMode.ResumeLayout(false);
            this.panelEnglishMode.PerformLayout();
            this.groupBoxOptions.ResumeLayout(false);
            this.groupBoxOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelText;
        private System.Windows.Forms.TextBox textBoxText;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.Label labelLexicon;
        private System.Windows.Forms.TextBox textBoxLexicon;
        private System.Windows.Forms.Label labelMatchMode;
        private System.Windows.Forms.Label labelSingleMode;
        private System.Windows.Forms.Label labelEnglishMode;
        private System.Windows.Forms.Panel panelMatchMode;
        private System.Windows.Forms.RadioButton radioButtonMatchModeFast;
        private System.Windows.Forms.RadioButton radioButtonMatchModeNormal;
        private System.Windows.Forms.Panel panelSingleMode;
        private System.Windows.Forms.RadioButton radioButtonSingleModeDualize;
        private System.Windows.Forms.RadioButton radioButtonSingleModeConcat;
        private System.Windows.Forms.RadioButton radioButtonSingleModeThrowaway;
        private System.Windows.Forms.Panel panelEnglishMode;
        private System.Windows.Forms.RadioButton radioButtonEnglishModeThrowaway;
        private System.Windows.Forms.RadioButton radioButtonEnglishModeNormal;
        private System.Windows.Forms.Button buttonLexicon;
        private System.Windows.Forms.Button buttonTokenize;
        private System.Windows.Forms.GroupBox groupBoxOptions;
    }
}

