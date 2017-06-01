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
        /// 分组集合
        /// </summary>
        public Dictionary<string, Item> Items;   

        /// <summary>
        /// 分组管理
        /// </summary>
        public Category() { this.Items = new Dictionary<string, Item>(); }

        /// <summary>
        /// 添加Category
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddCategory(Item item)
        {
            this.Items.Add(item.Category, item);
            return true;
        }

        /// <summary>
        /// 删除一个Category
        /// </summary>
        /// <param name="name">Category名</param>
        public void DeleteCategory(string name)
        {
            this.Items.Remove(name);
        }
    }
}
