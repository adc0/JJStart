using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace JJStart
{
    [Serializable]

    /// <summary>
    /// �ļ���
    /// </summary>
    public class ItemSubInfo
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name;   

        /// <summary>
        /// �������������Ӧ���ļ�·��
        /// </summary>
        public string FullName;

        /// <summary>
        /// �ȼ�
        /// </summary>
        public HotKey HotKey;

        /// <summary>
        /// ���캯��
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
