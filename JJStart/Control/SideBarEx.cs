using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace JJStart
{
    public partial class SideBarEx : SideBar
    {
        private bool _EnableDoubleClickChangeText = false;
        private bool _SideBarPanelItemTextChange = false;

        public bool SideBarPanelItemTextChange
        {
            get { return _SideBarPanelItemTextChange; }
            set { _SideBarPanelItemTextChange = value; }
        }

        public bool EnableDoubleClickChangeText
        {
            get { return _EnableDoubleClickChangeText; }
            set { _EnableDoubleClickChangeText = value; }
        }

        private TextBox _NameEdit;
        private TextBox _ItemTextNameEdit;
        private SideBarPanelItem _SideBarPanelItem;
        private ButtonItemEx _ButtonItemEx;

        /// <summary>
        /// 传递control过来，即“this”
        /// </summary>
        public SideBarEx()
        {
            //创建一个文本输入框
            this._NameEdit = new TextBox();
            this._ItemTextNameEdit = new TextBox();

            //弄个Name
            this._NameEdit.Name = "NameEdit";
            this._ItemTextNameEdit.Name = "ItemTextNameEdit";

            //设置一下字体
            this._NameEdit.Font = this.Font;
            this._ItemTextNameEdit.Font = this.Font;

            //调整成居中显示
            this._NameEdit.TextAlign = HorizontalAlignment.Center;
            this._ItemTextNameEdit.TextAlign = HorizontalAlignment.Center;

            //能多行显示
            this._ItemTextNameEdit.Multiline = true;

            //先隐藏
            this._NameEdit.Hide();
            this._ItemTextNameEdit.Hide();

            //处理KeyDown事件
            this._NameEdit.KeyPress += _NameEdit_KeyPress;
            this._ItemTextNameEdit.KeyPress += _ItemTextNameEdit_KeyPress;

            //处理丢失焦点事件
            this._NameEdit.LostFocus += _NameEdit_LostFocus;
            this._ItemTextNameEdit.LostFocus += _ItemTextNameEdit_LostFocus;

            //添加到里面
            this.Controls.Add(_NameEdit);
            this.Controls.Add(_ItemTextNameEdit);
        }

        /// <summary>
        /// 
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
                if (this._NameEdit.Text.Trim() == "")
                {
                    this._NameEdit.Focus();
                    return;
                }

                //还有一种情况就是该分组已经存在了
                if (this._NameEdit.Modified && this.Panels.Contains(this._NameEdit.Text))
                {
                    this._NameEdit.Focus();
                    return;
                }

                //标志一下是SideBarPanelItem的text改变事件
                this._SideBarPanelItemTextChange = true;

                //改为新的名称
                this._SideBarPanelItem.Text = this._NameEdit.Text;

                //隐藏掉text
                this._NameEdit.Hide();

                //终于把焦点问题给去掉了，擦
                this.Parent.Focus();

                //处理过了
                e.Handled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _ItemTextNameEdit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                //终于把焦点问题给去掉了，擦
                this.Parent.Focus();

                //如果没有输入任何字符串
                if (this._ItemTextNameEdit.Text.Trim() == "")
                {
                    this._ItemTextNameEdit.Focus();
                    return;
                }

                //标志一下是SideBarPanelItem的text改变事件
                this._SideBarPanelItemTextChange = false;

                //改为新的Text
                this._ButtonItemEx.Text = this._ItemTextNameEdit.Text;

                //隐藏掉text
                this._ItemTextNameEdit.Hide();

                //最后加入修改处理
                ItemSubInfo isi = _ButtonItemEx.Tag as ItemSubInfo;

                //保存一下
                isi.Name = this._ItemTextNameEdit.Text;

                //处理过了
                e.Handled = true;
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _ItemTextNameEdit_LostFocus(object sender, EventArgs e)
        {
            this._ItemTextNameEdit.Hide();
        }

        /// <summary>
        /// 重写方法，添加文本框
        /// </summary>
        /// <param name="objItem"></param>
        /// <param name="e"></param>
        protected override void OnItemDoubleClick(BaseItem objItem, MouseEventArgs e)
        {
            //首先判断下是否允许双击更改Text
            if(!EnableDoubleClickChangeText)
            {
                base.OnItemDoubleClick(objItem, e);
                return;
            }

            //下面则是允许
            _SideBarPanelItem = objItem as SideBarPanelItem;

            //判断一下
            if (_SideBarPanelItem == null)
            {
                base.OnItemDoubleClick(objItem, e);
                return;
            }

            Rectangle DisplayRectangle = _SideBarPanelItem.DisplayRectangle;
            Rectangle PanelRect = _SideBarPanelItem.PanelRect;
            Point Location = this.PointToClient(e.Location);

            //处理下特殊情况
            if (Location.Y > DisplayRectangle.Location.Y + PanelRect.Height && Location.Y < PanelRect.Y + DisplayRectangle.Height)
            {
                base.OnItemDoubleClick(objItem, e);
                return;
            }

            //填充相关信息
            this._NameEdit.Clear();
            this._NameEdit.Location = _SideBarPanelItem.DisplayRectangle.Location;
            this._NameEdit.Size = _SideBarPanelItem.PanelRect.Size;
            this._NameEdit.Text = _SideBarPanelItem.Text;
            this._NameEdit.SelectionLength = this._NameEdit.TextLength;
            this._NameEdit.SelectAll();
            this._NameEdit.ScrollToCaret();

            //显现
            this._NameEdit.Show();

            //设置焦点
            this._NameEdit.Focus();

            //添加到最前面
            this.Controls.SetChildIndex(this._NameEdit, 0);

            //base方法
            base.OnItemDoubleClick(objItem, e);
        }

        /// <summary>
        /// 重写MouseClick，主要是实现隐藏掉文本框
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            //隐藏
            if(this._NameEdit.Visible)
            {
                //如果没有输入任何字符串
                if (this._NameEdit.Text.Trim() == "")
                {
                    this._NameEdit.Focus();
                    return;
                }

                //还有一种情况就是该分组已经存在了
                if(this._NameEdit.Modified && this.Panels.Contains(this._NameEdit.Text))
                {
                    this._NameEdit.Focus();
                    return;
                }
 
                //如果有输入的话，则隐藏
                this._NameEdit.Hide();

                //显示修改后的
                this._SideBarPanelItem.Text = this._NameEdit.Text;
            }

            //隐藏
            if (this._ItemTextNameEdit.Visible)
            {
                //如果没有输入任何字符串
                if (this._ItemTextNameEdit.Text.Trim() == "")
                {
                    this._ItemTextNameEdit.Focus();
                    return;
                }

                //这里确定下_ItemTextNameEdit是否已经修改
                if (_ItemTextNameEdit.Modified)
                {
                    //改为新的名称
                    this._ButtonItemEx.Text = this._ItemTextNameEdit.Text;

                    //最后加入修改处理
                    ItemSubInfo isi = _ButtonItemEx.Tag as ItemSubInfo;

                    //保存一下
                    isi.Name = this._ItemTextNameEdit.Text;
                }

                //隐藏掉text
                this._ItemTextNameEdit.Hide();   
            }

            //终于把焦点问题给去掉了，擦
            this.Parent.Focus();

            base.OnMouseClick(e);
        }

        /// <summary>
        /// 开启编辑选项
        /// </summary>
        /// <param name="bie"></param>
        public void BeginItemTextEdit(ButtonItemEx bie)
        {
            Rectangle rect = bie.GetTextDrawRect();
            Rectangle DisplayRectangle = bie.DisplayRectangle;
            ItemSubInfo isi = bie.Tag as ItemSubInfo;

            //保存一下当前修改的bie
            _ButtonItemEx = bie;

            //填充相关信息
            this._ItemTextNameEdit.Clear();

            //这里判断下panel的显示模式
            if(this.ExpandedPanel.LayoutType == eSideBarLayoutType.Default)
            {
                //这里在判断下是小图标还是大图标
                if (this.ExpandedPanel.ItemImageSize == eBarImageSize.Default)
                    this._ItemTextNameEdit.Location = new Point(rect.Location.X, DisplayRectangle.Bottom - rect.Height);
                else if (this.ExpandedPanel.ItemImageSize == eBarImageSize.Medium)
                    this._ItemTextNameEdit.Location = new Point(rect.Location.X, DisplayRectangle.Bottom - rect.Height);
            }
            else if (this.ExpandedPanel.LayoutType == eSideBarLayoutType.MultiColumn)
            {
                //这里在判断下是小图标还是大图标
                if (this.ExpandedPanel.ItemImageSize == eBarImageSize.Default)
                    this._ItemTextNameEdit.Location = new Point(rect.Left + DisplayRectangle.Location.X, DisplayRectangle.Location.Y + rect.Height / 2 - 8);
                else if (this.ExpandedPanel.ItemImageSize == eBarImageSize.Medium)
                    this._ItemTextNameEdit.Location = new Point(rect.Left + DisplayRectangle.Location.X, DisplayRectangle.Bottom - rect.Height);
            }

            //大小
            this._ItemTextNameEdit.Size = rect.Size;

            //显示配置中的名称
            this._ItemTextNameEdit.Text = isi.Name;

            //选中
            this._ItemTextNameEdit.SelectionStart = 0;
            this._ItemTextNameEdit.SelectionLength = this._ItemTextNameEdit.TextLength;
            this._ItemTextNameEdit.SelectAll();
            this._ItemTextNameEdit.ScrollToCaret();

            //显现
            this._ItemTextNameEdit.Show();

            //设置焦点
            this._ItemTextNameEdit.Focus();

            //添加到最前面
            this.Controls.SetChildIndex(this._ItemTextNameEdit, 0);
        }
    }
}