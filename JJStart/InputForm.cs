using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;
using JJStart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JJStart
{
    public partial class InputForm : Form
    {
        private Language lang = new Language();

        public InputForm()
        {
            InitializeComponent();

            //设置语言
            lang.SetLanguage(this);
        }

        private void InputForm_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.JJStart;

            //设置输入焦点
            this.txtInput.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.txtInput.Text == "")
            {
                this.txtInput.Focus();
                return;
            }

            this.Close();
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}