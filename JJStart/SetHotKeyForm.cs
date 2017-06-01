using JJPlugin;
using JJStart.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JJStart
{
    public partial class SetHotKeyForm : Form
    {
        private ItemSubInfo fi = null;
        private WinHotKey wHotKey = null;
        private Language lang = new Language();

        public SetHotKeyForm(ItemSubInfo f, WinHotKey w)
        {
            InitializeComponent();

            //相等
            this.fi = f;
            this.wHotKey = w;

            //设置语言
            lang.SetLanguage(this);
        }

        private void SetHotKeyForm_Load(object sender, EventArgs e)
        {
            //Show当前值
            txtHotKey.Text = fi.HotKey == null ? "" : fi.HotKey.ToString();
            txtName.Text = fi.Name;
            txtFullName.Text = path.GetAbsolutePath(Application.ExecutablePath, fi.FullName);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {  
            string fail = lang.GetString("1", "注销热键 {0} 失败！");
            string suss = lang.GetString("2", "设置热键 {0} 失败！");

            //改动的情况，两种，一种设置新的，一种是取消
            //如果为0，说明是取消热键
            if(txtHotKey.TextLength == 0)
            {
                //先判断是否为null
                if(fi.HotKey != null)
                {
                    //取消热键
                    if (wHotKey.UnSetHotKey(fi.HotKey))
                        fi.HotKey = null;
                    else  
                        MessageBox.Show(String.Format(fail, fi.HotKey.ToString()));                   
                }
            }
            else
            {
                //注册新热键，但存在一种情况，即相同，热键没有变化
                if (txtHotKey.HotKey.Key == Keys.None && txtHotKey.HotKey.Modifiers == WinHotKey.KeyModifiers.None)
                {
                    this.Close();
                    return;
                }

                //取消之前的热键
                if (fi.HotKey != null)
                {
                    if (!wHotKey.UnSetHotKey(fi.HotKey))
                    {
                        MessageBox.Show(String.Format(fail, fi.HotKey.ToString()));   
                        return;
                    }
                }

                //新热键
                fi.HotKey = txtHotKey.HotKey;
                fi.HotKey.FullName = fi.FullName;

                //注册
                if (!wHotKey.SetHotKey(fi.HotKey))
                {
                    MessageBox.Show(String.Format(suss, fi.HotKey.ToString()));   
                    return;
                } 
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
