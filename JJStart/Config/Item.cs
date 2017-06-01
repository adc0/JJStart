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
        /// Item所在类名
        /// </summary>
        public string Category;

        /// <summary>
        /// 分组集合
        /// </summary>
        public Dictionary<string, ItemSub> Items;   

        /// <summary>
        /// 分组管理
        /// </summary>
        public Item() 
        { 
            this.Items = new Dictionary<string, ItemSub>();
        }

        /// <summary>
        /// 一个分组的文件集合
        /// </summary>
        /// <param name="name">分组名</param>
        public Item(string name) : this() { this.Category = name; }

        /// <summary>
        /// 添加一个栏目
        /// </summary>
        /// <param name="item">文件对象</param>
        public void AddItem(ItemSub item)
        {
            if (this.Items.ContainsKey(item.Name))
            {
                MessageBox.Show("该分组已经存在", "系统警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Items.Add(item.Name, item);
        }

        /// <summary>
        /// 清空Item
        /// </summary>
        /// <param name="name"></param>
        public void ClearItem(string name)
        {
            ItemSub item = null;

            //如果获取到了item
            if(this.Items.TryGetValue(name, out item))
            {
                //清空列表
                item.SubItems.Clear();
            }
        }

        /// <summary>
        /// 删除一个栏目
        /// </summary>
        /// <param name="name">栏目名</param>
        public void DeleteItem(string name)
        {
            this.Items.Remove(name);
        }

        /// <summary>
        /// 判断Item是否存在
        /// </summary>
        /// <returns></returns>
        public bool ExistsItem(string name)
        {
            //如果存在key的话，则返回true
            if (Items.ContainsKey(name))
                return true;
            else
                return false;
        }
    }
}
