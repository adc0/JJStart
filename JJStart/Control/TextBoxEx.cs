using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace JJStart
{
    public partial class TextBoxEx : TextBox
    {
        public TextBoxEx()
        {
            this.KeyDown += TextBoxEx_KeyDown;
            this.KeyPress += TextBoxEx_KeyPress;
            this.KeyUp += TextBoxEx_KeyUp;
        }

        public HotKey HotKey = new HotKey();

        /// <summary>
        /// 键盘按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxEx_KeyDown(object sender, KeyEventArgs e)
        {
            this.Text = string.Empty;

            if (e.Control)
            {
                HotKey.Modifiers |= WinHotKey.KeyModifiers.Control;
                this.Text += "Ctrl + ";
            }
            if (e.Alt)
            {
                HotKey.Modifiers |= WinHotKey.KeyModifiers.Alt;
                this.Text += "Alt + ";
            }
            if (e.Shift)
            {
                HotKey.Modifiers |= WinHotKey.KeyModifiers.Shift;
                this.Text += "Shift + ";
            }

            if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
            {
                if (e.Modifiers != Keys.None)
                {
                    this.Text += e.KeyCode.ToString();
                    HotKey.Key = e.KeyCode;
                }
            }
            else if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
            {
                if (e.Modifiers != Keys.None)
                {
                    this.Text += e.KeyCode.ToString();
                    HotKey.Key = e.KeyCode;
                }
            }
            else if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
            {
                if (e.Modifiers != Keys.None)
                {
                    this.Text += e.KeyCode.ToString().Replace("Pad", "");
                    HotKey.Key = e.KeyCode;
                }
            }
            else if (e.KeyCode >= Keys.F1 && e.KeyCode <= Keys.F12)
            {
                this.Text += e.KeyCode.ToString();
                HotKey.Key = e.KeyCode;
            }
            else if (e.KeyValue == 18 | e.KeyValue == 17 | e.KeyValue == 16)
            {
                HotKey.Key = Keys.None;
            }
        }

        /// <summary>
        /// Press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxEx_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxEx_KeyUp(object sender, KeyEventArgs e)
        {
            if (HotKey.Key == Keys.None)
            {
                string v = string.Empty;
                HotKey.Modifiers = WinHotKey.KeyModifiers.None;

                if (e.Control)
                {
                    HotKey.Modifiers |= WinHotKey.KeyModifiers.Control;
                    v += "Ctrl + ";
                }
                if (e.Shift)
                {
                    HotKey.Modifiers |= WinHotKey.KeyModifiers.Shift;
                    v += "Shift + ";
                }
                if (e.Alt)
                {
                    HotKey.Modifiers |= WinHotKey.KeyModifiers.Alt;
                    v += "Alt + ";
                }

                if (string.IsNullOrEmpty(v))
                    this.Text = string.Empty;
                else
                    this.Text = v;
            }
        }
    }
}