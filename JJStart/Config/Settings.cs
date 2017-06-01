using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using DevComponents.DotNetBar;

namespace JJStart
{
    [Serializable]

    public class Settings
    {
        /// <summary>
        /// 是否是第一次运行
        /// </summary>
        public bool FirstRun;

        /// <summary>
        /// 语言LCID
        /// </summary>
        public int LCID;

        /// <summary>
        /// 是否加载插件
        /// 默认加载插件
        /// </summary>
        public bool LoadPlugin;

        /// <summary>
        /// 标志是否双击打开链接
        /// </summary>
        public bool DoubleClickRun;

        /// <summary>
        /// 对link文件采用移动模式
        /// </summary>
        public bool LinkDragRemove;

        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool TopMost;

        /// <summary>
        /// 窗体的宽度
        /// </summary>
        public Size Size;

        /// <summary>
        /// 窗体位置信息
        /// </summary>
        public Point Location;

        /// <summary>
        /// 保存类别、栏目和文件信息的类
        /// </summary>
        public Category Category;

        /// <summary>
        /// 皮肤信息
        /// </summary>
        public eStyle ManagerStyle;

        /// <summary>
        /// custom自定义颜色信息
        /// </summary>
        public Color ManagerColorTint;

        /// <summary>
        /// 程序隐藏呼出的快捷键
        /// </summary>
        public HotKey HotKey;

        /// <summary>
        /// 热键管理
        /// </summary>
        public WinHotKey WinHotKey;

        /// <summary>
        /// 标记那个按钮被勾选
        /// 0:运行，1:搜索，2:百度，3:谷歌
        /// </summary>
        public int ButtonCheckedIndex;

        /// <summary>
        /// 主面板按钮
        /// </summary>
        public List<ItemSubInfo> MainPanelItems;

        public Settings(IntPtr Handle)
        {
            //设置为默认值
            this.Category = new Category();
            this.FirstRun = true;
            this.Location = new Point(0, 0);
            this.ManagerStyle = eStyle.Metro;
            this.ManagerColorTint = Color.White;
            this.ButtonCheckedIndex = 0;
            this.LoadPlugin = true;
            this.DoubleClickRun = false;
            this.LinkDragRemove = true;
            this.TopMost = true;
            this.LCID = 0;
            this.HotKey = new HotKey();
            this.HotKey.Modifiers = WinHotKey.KeyModifiers.Alt;
            this.HotKey.Key = Keys.D1;
            this.WinHotKey = new WinHotKey(Handle);
            this.WinHotKey.SetHotKey(this.HotKey);
            this.MainPanelItems = new List<ItemSubInfo>();
        }
    }
}
