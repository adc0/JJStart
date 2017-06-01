using DevComponents.DotNetBar;
using JJPlugin;
using JJStart.Lib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace JJStart
{
    public class ButtonItemEx : ButtonItem
    {
        /// <summary>
        /// 类型
        /// </summary>
        public enum ButtonItemType
        {
            Normal,
            Plugin
        }

        /// <summary>
        /// Button 类型
        /// </summary>
        private ButtonItemType _ItemType;
        public ButtonItemType ItemType
        {
            get { return _ItemType; }
            set { _ItemType = value; }
        }

        /// <summary>
        /// 是否显示UAC图标
        /// </summary>
        private bool _ShowUACIcon;
        public bool ShowUACIcon
        {
            get { return _ShowUACIcon; }
            set { _ShowUACIcon = value; }
        }

        /// <summary>
        /// 要显示uac的文件的名称
        /// </summary>
        private string _FullName;
        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ButtonItemEx()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sItemName"></param>
        public ButtonItemEx(string sItemName)
        {
            this.Name = sItemName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sItemName"></param>
        /// <param name="sItemText"></param>
        public ButtonItemEx(string sItemName, string sItemText)
        {
            this.Name = sItemName;
            this.Text = sItemText;
        }

        /// <summary>
        /// 重写text属性
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                //这里做下处理
                if (this.ContainerControl != null && this.ContainerControl is SideBarEx)
                {
                    string strText = String.Empty;

                    //正则表达式
                    Regex reg = new Regex(@"[\u4e00-\u9fa5]");

                    if (reg.IsMatch(value))
                    {
                        //转换成char数组
                        byte[] czFileName = Encoding.Unicode.GetBytes(value);

                        //在这里进行一个字符串缩进（前6个汉字（12个字节）+ 3个点）                  
                        if (czFileName.Length > 12)
                            strText = Encoding.Unicode.GetString(czFileName, 0, 12) + "...";
                        else
                            strText = value;
                    }
                    else
                    {
                        //转换成char数组
                        byte[] czFileName = Encoding.ASCII.GetBytes(value);

                        //在这里进行一个字符串缩进（前6个汉字（12个字节）+ 3个点）                 
                        if (czFileName.Length > 12)
                            strText = Encoding.ASCII.GetString(czFileName, 0, 12) + "...";
                        else
                            strText = value;
                    }

                    //接着判断下ItemImageSize
                    //下面主要是为了Button间的对齐
                    //先判断下Panel的显示方式
                    if ((this.ContainerControl as SideBarEx).ExpandedPanel.ItemImageSize == eBarImageSize.Default)
                        base.Text = "<div align=\"left\" width=\"84\">" + strText + "</div>";
                    else
                        base.Text = "<div align=\"center\" width=\"84\">" + strText + "</div>";
                }
                else
                {
                    base.Text = value;
                }
            }
        }

        /// <summary>
        /// 获取TextDrawRect
        /// </summary>
        /// <returns></returns>
        public Rectangle GetTextDrawRect()
        {
            return this.TextDrawRect;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public override void Paint(ItemPaintArgs p)
        {
            base.Paint(p);

            //如果要显示的话
            if (this.ShowUACIcon)
            {
                //Text的显示位置
                Rectangle rect = this.TextDrawRect;

                //判断下显示模式
                if (this.ImagePosition == eImagePosition.Top)
                {
                    //判断下按钮类型和文件
                    if (this.ItemType == ButtonItemType.Plugin || win32.IsElevationRequired(this._FullName))
                    {
                        //uisng及时释放资源
                        //大图标的时候，只采用16x16像素
                        using (Icon icon = new System.Drawing.Icon(Properties.Resources.uac, new Size(16, 16)))
                        {
                            //获取显示的位置
                            Point location = new Point(rect.Left + DisplayRectangle.Location.X + rect.Width - 40, DisplayRectangle.Bottom - rect.Height - 16);

                            //开始画图标
                            p.Graphics.DrawIcon(icon, location.X, location.Y);
                        }
                    }
                }
                else 
                {
                    //判断下按钮类型和文件
                    if (this.ItemType == ButtonItemType.Plugin || win32.IsElevationRequired(this._FullName))
                    {
                        //uisng及时释放资源
                        //小图标的时候，只采用8x8
                        using (Icon icon = new System.Drawing.Icon(Properties.Resources.uac, new Size(8, 8)))
                        {
                            //获取显示的位置
                            Point location = new Point(rect.Left + DisplayRectangle.Location.X - 8, DisplayRectangle.Bottom - 12);

                            //开始画图标
                            p.Graphics.DrawIcon(icon, location.X, location.Y);
                        }
                    }
                }
            }
        }
    }
}
