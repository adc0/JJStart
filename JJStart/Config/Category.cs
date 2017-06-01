using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace JJStart
{
    [Serializable]

    public class Category
    {
        /// <summary>
        /// ���鼯��
        /// </summary>
        public Dictionary<string, Item> Items;   

        /// <summary>
        /// �������
        /// </summary>
        public Category() { this.Items = new Dictionary<string, Item>(); }

        /// <summary>
        /// ���Category
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddCategory(Item item)
        {
            this.Items.Add(item.Category, item);
            return true;
        }

        /// <summary>
        /// ɾ��һ��Category
        /// </summary>
        /// <param name="name">Category��</param>
        public void DeleteCategory(string name)
        {
            this.Items.Remove(name);
        }
    }
}
