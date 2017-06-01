using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace JJStart
{
    [Serializable]

    public class Item
    {
        /// <summary>
        /// Item��������
        /// </summary>
        public string Category;

        /// <summary>
        /// ���鼯��
        /// </summary>
        public Dictionary<string, ItemSub> Items;   

        /// <summary>
        /// �������
        /// </summary>
        public Item() 
        { 
            this.Items = new Dictionary<string, ItemSub>();
        }

        /// <summary>
        /// һ��������ļ�����
        /// </summary>
        /// <param name="name">������</param>
        public Item(string name) : this() { this.Category = name; }

        /// <summary>
        /// ���һ����Ŀ
        /// </summary>
        /// <param name="item">�ļ�����</param>
        public void AddItem(ItemSub item)
        {
            if (this.Items.ContainsKey(item.Name))
            {
                MessageBox.Show("�÷����Ѿ�����", "ϵͳ����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Items.Add(item.Name, item);
        }

        /// <summary>
        /// ���Item
        /// </summary>
        /// <param name="name"></param>
        public void ClearItem(string name)
        {
            ItemSub item = null;

            //�����ȡ����item
            if(this.Items.TryGetValue(name, out item))
            {
                //����б�
                item.SubItems.Clear();
            }
        }

        /// <summary>
        /// ɾ��һ����Ŀ
        /// </summary>
        /// <param name="name">��Ŀ��</param>
        public void DeleteItem(string name)
        {
            this.Items.Remove(name);
        }

        /// <summary>
        /// �ж�Item�Ƿ����
        /// </summary>
        /// <returns></returns>
        public bool ExistsItem(string name)
        {
            //�������key�Ļ����򷵻�true
            if (Items.ContainsKey(name))
                return true;
            else
                return false;
        }
    }
}
