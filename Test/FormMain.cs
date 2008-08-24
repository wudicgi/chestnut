// $Id$

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WudiStudio.Chestnut.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonTokenize_Click(object sender, EventArgs e)
        {
            Tokenizer tokenizer = Tokenizer.factory(@"D:\Projects\Assemblies\data\_words.lex");
            tokenizer.MatchMode = MatchMode.Normal;
            tokenizer.SingleMode = SingleMode.Normal;
            tokenizer.EnglishMode = EnglishMode.Normal;
            Token[] tokens = tokenizer.Tokenize(textBoxText.Text);
            StringBuilder sb = new StringBuilder();
            foreach (Token token in tokens)
            {
                sb.Append(token.Text);
                sb.Append("/ ");
            }
            textBoxResult.Text = sb.ToString();
        }
    }
}
