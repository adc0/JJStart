using DevComponents.DotNetBar.Metro;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JJStart
{
    public partial class SettingsForm : Form
    {
        private Language lang = new Language();
        private Form f;

        public SettingsForm(Form f)
        {
            InitializeComponent();

            //设置语言
            lang.SetLanguage(this);

            this.f = f;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.JJStart;

            if(lang.Enable)
            {
                cboLanguage.Items.AddRange(lang.GetLanguagesName());
                cboLanguage.Text = lang.GetLanguagesName(Global.LCID);
            }

            //显示值
            txtHotKey.Text = Global.Settings.HotKey.ToString();
            chkLoadPlugin.Checked = Global.Settings.LoadPlugin;
            chkDblClickRun.Checked = Global.Settings.DoubleClickRun;
            chkLinkDragRemove.Checked = Global.Settings.LinkDragRemove;
            chkTopMost.Checked = Global.Settings.TopMost;

            RegistryKey rkey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            if(rkey != null)
            {
                object objValue = rkey.GetValue("JJStart");
                if (objValue != null)
                {
                    if (objValue.ToString().Equals(Application.ExecutablePath, StringComparison.CurrentCultureIgnoreCase))
                    {
                        //如果是自己的路径的话，则勾选
                        chkAutoRun.Checked = true;
                    }
                }

                rkey.Close();
            }        
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //改动的情况，两种，一种设置新的，一种是取消
            if (txtHotKey.TextLength == 0)
            {
                //必须要设置一个呼出隐藏热键
                txtHotKey.Focus();
                return;
            }

            //注册新热键，但存在一种情况，即相同，热键没有变化
            if (txtHotKey.HotKey.Key != Keys.None && txtHotKey.HotKey.Modifiers != WinHotKey.KeyModifiers.None)
            {
                //取消之前的热键热键
                Global.Settings.WinHotKey.UnSetHotKey(Global.Settings.HotKey);
                Global.Settings.WinHotKey.SetHotKey(txtHotKey.HotKey);
                Global.Settings.HotKey = txtHotKey.HotKey;
            }

            //LCID
            Global.LCID = lang.GetLanguagesLcid(cboLanguage.Text);
            Global.Settings.LCID = Global.LCID;

            //其他赋值
            Global.Settings.LoadPlugin = chkLoadPlugin.Checked;
            Global.Settings.DoubleClickRun = chkDblClickRun.Checked;
            Global.Settings.LinkDragRemove = chkLinkDragRemove.Checked;
            Global.Settings.TopMost = chkTopMost.Checked;

            if (!chkTopMost.Checked)
                this.f.TopMost = false;
            else
                this.f.TopMost = true;

            string strName = Application.ExecutablePath;

            if (chkAutoRun.Checked)
            {
                RegistryKey rkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (rkey == null)
                    rkey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                rkey.SetValue("JJStart", strName);
                rkey.Close();
            }
            else
            {
                RegistryKey rkey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (rkey == null)
                    return;
                rkey.DeleteValue("JJStart", false);
                rkey.Close();
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
