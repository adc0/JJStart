using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace JJStart
{
    [Serializable]

    /// <summary>
    /// 文件类
    /// </summary>
    public class ItemSubInfo
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name;   

        /// <summary>
        /// 保存的名称所对应的文件路径
        /// </summary>
        public string FullName;

        /// <summary>
        /// 热键
        /// </summary>
        public HotKey HotKey;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fullName"></param>
        public ItemSubInfo(string name, string fullName)
        {
            this.Name = name;
            this.FullName = fullName;
        }
    }
}
