using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace JJStart
{
    [Serializable]

    public class ItemSub
    {
        /// <summary>
        /// Item的显示模式
        /// </summary>
        public enum ItemViewMode
        {
            LargeIcon,
            SmallIcon
        }

        /// <summary>
        /// 分组名
        /// </summary>
        public string Name;

        /// <summary>
        /// 是否已经展开
        /// </summary>
        public bool Expanded;

        /// <summary>
        /// 是否允许多列显示，即横排显示
        /// </summary>
        public bool MultiColumn;

        /// <summary>
        /// 显示模式
        /// </summary>
        public ItemViewMode ViewMode;

        /// <summary>
        /// 主要为了能存放重复的
        /// </summary>
        public List<ItemSubInfo> SubItems;

        /// <summary>
        /// 
        /// </summary>
        public ItemSub() 
        {
            //默认小图标+横排显示
            this.Expanded = true;
            this.MultiColumn = true;
            this.ViewMode = ItemViewMode.SmallIcon;
           
            //实例化
            this.SubItems = new List<ItemSubInfo>(); 
        }

        /// <summary>
        /// 一个栏目的集合
        /// </summary>
        /// <param name="itemName">栏目</param>
        public ItemSub(string itemName) : this() { this.Name = itemName; }

        /// <summary>
        /// 向栏目中添加一个文件
        /// </summary>
        /// <param name="file">文件对象</param>
        /// <param name="whotkey">热键管理</param>
        public void AddSubItem(ItemSubInfo file, WinHotKey whotkey)
        {
            //在添加的同时注册热键，主要作用于复制
            if (file.HotKey != null)
            {
                whotkey.SetHotKey(file.HotKey);
            }

            this.SubItems.Add(file);
        }

        /// <summary>
        /// 从栏目中删除一个文件
        /// </summary>
        /// <param name="file">文件对象</param>
        /// <param name="whotkey">热键管理</param>
        public void DeleteSubItem(ItemSubInfo file, WinHotKey whotkey)
        {
            //在删除的同时注销掉热键
            if(file.HotKey != null)
            {
                whotkey.UnSetHotKey(file.HotKey);
            }

            this.SubItems.Remove(file);
        }

    }
}
