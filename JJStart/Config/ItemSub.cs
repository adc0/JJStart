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
        /// Item����ʾģʽ
        /// </summary>
        public enum ItemViewMode
        {
            LargeIcon,
            SmallIcon
        }

        /// <summary>
        /// ������
        /// </summary>
        public string Name;

        /// <summary>
        /// �Ƿ��Ѿ�չ��
        /// </summary>
        public bool Expanded;

        /// <summary>
        /// �Ƿ����������ʾ����������ʾ
        /// </summary>
        public bool MultiColumn;

        /// <summary>
        /// ��ʾģʽ
        /// </summary>
        public ItemViewMode ViewMode;

        /// <summary>
        /// ��ҪΪ���ܴ���ظ���
        /// </summary>
        public List<ItemSubInfo> SubItems;

        /// <summary>
        /// 
        /// </summary>
        public ItemSub() 
        {
            //Ĭ��Сͼ��+������ʾ
            this.Expanded = true;
            this.MultiColumn = true;
            this.ViewMode = ItemViewMode.SmallIcon;
           
            //ʵ����
            this.SubItems = new List<ItemSubInfo>(); 
        }

        /// <summary>
        /// һ����Ŀ�ļ���
        /// </summary>
        /// <param name="itemName">��Ŀ</param>
        public ItemSub(string itemName) : this() { this.Name = itemName; }

        /// <summary>
        /// ����Ŀ�����һ���ļ�
        /// </summary>
        /// <param name="file">�ļ�����</param>
        /// <param name="whotkey">�ȼ�����</param>
        public void AddSubItem(ItemSubInfo file, WinHotKey whotkey)
        {
            //����ӵ�ͬʱע���ȼ�����Ҫ�����ڸ���
            if (file.HotKey != null)
            {
                whotkey.SetHotKey(file.HotKey);
            }

            this.SubItems.Add(file);
        }

        /// <summary>
        /// ����Ŀ��ɾ��һ���ļ�
        /// </summary>
        /// <param name="file">�ļ�����</param>
        /// <param name="whotkey">�ȼ�����</param>
        public void DeleteSubItem(ItemSubInfo file, WinHotKey whotkey)
        {
            //��ɾ����ͬʱע�����ȼ�
            if(file.HotKey != null)
            {
                whotkey.UnSetHotKey(file.HotKey);
            }

            this.SubItems.Remove(file);
        }

    }
}
