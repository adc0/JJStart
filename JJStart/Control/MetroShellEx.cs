using DevComponents.DotNetBar.Metro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JJStart
{
    public partial class MetroShellEx : MetroShell
    {
        private TextBox _NameEdit;
        private MetroTabItem _MetroTabItem;
        private MetroTabItem _MetroTabItemLastShow;

        /// <summary>
        /// 传递control过来，即“this”
        /// </summary>
        public MetroShellEx()
        {
            //创建一个文本输入框
            this._NameEdit = new TextBox();

            //弄个Name
            this._NameEdit.Name = "NameEdit";

            //设置一下字体
            this._NameEdit.Font = this.Font;

            //调整成居中显示
            this._NameEdit.TextAlign = HorizontalAlignment.Center;

            //先隐藏
            this._NameEdit.Hide();

            //处理KeyPress事件
            this._NameEdit.KeyPress += _NameEdit_KeyPress;

            //处理丢失焦点事件
            this._NameEdit.LostFocus += _NameEdit_LostFocus;

            //添加到里面
            this.Controls.Add(_NameEdit);
        }

        /// <summary>
        /// 重写双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _MetroTabItem_DoubleClick(object sender, EventArgs e)
        {
            //过滤掉“Plugins”
            if (_MetroTabItem.Text == "Plugins")
                return;

            //填充相关信息
            this._NameEdit.Clear();
            this._NameEdit.Location = new Point(_MetroTabItem.DisplayRectangle.Location.X+3, _MetroTabItem.DisplayRectangle.Location.Y + 6);
            this._NameEdit.Size = _MetroTabItem.DisplayRectangle.Size;
            this._NameEdit.Text = _MetroTabItem.Text;
            this._NameEdit.SelectionLength = this._NameEdit.TextLength;
            this._NameEdit.SelectAll();
            this._NameEdit.ScrollToCaret();

            //显现
            this._NameEdit.Show();

            //获取焦点
            this._NameEdit.Focus();

            //添加到最前面
            this.Controls.SetChildIndex(this._NameEdit, 0);

            //记录一下
            this._MetroTabItemLastShow = this._MetroTabItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _MetroTabItem_MouseDown(object sender, MouseEventArgs e)
        {
            //如果是右键的话，则弹出菜单
            if (e.Button == MouseButtons.Right)
            {

            }
        } 

        /// <summary>
        /// 主要对回车的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _NameEdit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                //过滤掉
                e.Handled = true;

                //如果没有输入任何字符串
                if(this._NameEdit.Text.Trim() == "")
                {
                    this._NameEdit.Focus();
                    return;
                }

                //改为新的名称
                this._MetroTabItem.Text = this._NameEdit.Text;

                //隐藏掉text
                this._NameEdit.Hide();

                //把焦点转移到这
                Global.SplitContainer.Panel2.Focus();
               
                //刷新一下
                this.Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _NameEdit_LostFocus(object sender, EventArgs e)
        {
            this._NameEdit.Hide();

            //如果修改了
            if(this._NameEdit.Modified)
            {
                //改为新的名称
                this._MetroTabItem.Text = this._NameEdit.Text;

                //刷新一下
                this.Refresh();
            }
        }

        /// <summary>
        /// 重写方法，添加文本框
        /// </summary>
        /// <param name="objItem"></param>
        /// <param name="e"></param>
        protected override void OnSelectedTabChanged(EventArgs e)
        {
            //如果没有输入任何字符串
            if (this._NameEdit.Text.Trim() != "" && this._NameEdit.Modified)
            {
                //显示修改后的
                this._MetroTabItemLastShow.Text = this._NameEdit.Text;
            }

            //隐藏
            this._NameEdit.Hide();

            //新的
            this._MetroTabItem = this.SelectedTab;

            //先取消在注册
            this._MetroTabItem.DoubleClick -= _MetroTabItem_DoubleClick;
            this._MetroTabItem.DoubleClick += _MetroTabItem_DoubleClick;
            this._MetroTabItem.MouseDown += _MetroTabItem_MouseDown;

            //基类方法
            base.OnSelectedTabChanged(e);
        }    
    }
}
