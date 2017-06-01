using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace JJStart
{
    public class QQFormHide
    {
        /// <summary>
        /// QQ吸附窗体
        /// </summary>
        /// <from>http://www.cnblogs.com/feiyangqingyun/archive/2010/12/04/1896444.html</from>
        /// <author>原作者：刘典武</author>
        /// <time>2010-12-01 </time>
        /// <param name="f">要吸附边缘的窗体</param>
        /// <param name="height">窗体的高度</param>
        /// <param name="timer">定时器控件</param>
        /// 用法：在对应窗体timer控件的Tick事件中写代码 int height = this.Height; QQFormHide.hide_show(this, height);
        public static void hide_show(Form f, int height)
        {
            if (f.WindowState != FormWindowState.Minimized)
            {
                if (Cursor.Position.X > f.Left - 1 && Cursor.Position.X < f.Right && Cursor.Position.Y > f.Top - 1 && Cursor.Position.Y < f.Bottom)
                {
                    if (f.Top <= 0 && f.Left > 5 && f.Left < Screen.PrimaryScreen.WorkingArea.Width - f.Width)
                    {
                        f.Top = 0;
                    }
                    else if (f.Left <= 0)
                    {
                        f.Left = 0;
                    }
                    else if (f.Left + f.Width > Screen.PrimaryScreen.WorkingArea.Width)
                    {
                        f.Left = Screen.PrimaryScreen.WorkingArea.Width - f.Width;
                    }
                    else
                    {
                        if (height > 0)
                        {
                            f.Height = height;
                            height = 0;
                        }
                    }
                }
                else
                {
                    if (height < 1)
                    {
                        height = f.Height;
                    }
                    if (f.Top <= 4 && f.Left > 5 && f.Left < Screen.PrimaryScreen.WorkingArea.Width - f.Width)
                    {
                        f.Top = 3 - f.Height;
                        if (f.Left <= 4)
                        {
                            f.Left = -5;
                        }
                        else if (f.Left + f.Width >= Screen.PrimaryScreen.WorkingArea.Width - 4)
                        {
                            f.Left = Screen.PrimaryScreen.WorkingArea.Width - f.Width + 5;
                        }
                    }
                    else if (f.Left <= 4)
                    {
                        f.Left = 3 - f.Width;
                    }
                    else if (f.Left + f.Width >= Screen.PrimaryScreen.WorkingArea.Width - 4)
                    {
                        f.Left = Screen.PrimaryScreen.WorkingArea.Width - 3;
                    }
                }
            }
        }
    }
}
