using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace JJPlugin
{
    /// <summary> 
    /// 本程序的插件必须实现这个接口 
    /// </summary> 
    public interface IPlugin
    {
        /// <summary>
        /// 是否需要uac权限
        /// </summary>
        bool IsElevationRequired
        {
            get;
        }

        /// <summary>
        /// 主程序窗体
        /// </summary>
        Form MainForm
        {
            get;
        }

        /// <summary>
        /// 加载
        /// </summary>
        void OnLoad();

        /// <summary>
        /// 销毁
        /// </summary>
        void OnDestory();

        /// <summary>
        /// 获取图标
        /// </summary>
        /// <returns></returns>
        Icon GetIcon();
    }

    /// <summary> 
    ///  
    /// </summary> 
    public class PluginInfoAttribute : System.Attribute
    {
        /// <summary>
        /// 插件名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description;

        /// <summary>
        /// 版本
        /// </summary>
        public string Version;

        /// <summary>
        /// 作者
        /// </summary>
        public string Author;

        /// <summary>
        /// 作者博客首页
        /// </summary>
        public string Webpage;

        /// <summary>
        /// Tag
        /// </summary>
        public object Tag = null;

        /// <summary>
        /// 索引
        /// </summary>
        public int Index = 0;

        /// <summary>
        /// 对应的插件绝对路径
        /// </summary>
        public string FullName;

        /// <summary> 
        ///默认构造函数
        /// </summary> 
        public PluginInfoAttribute() { }

        /// <summary>
        /// 要使用的构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="version"></param>
        /// <param name="author"></param>
        /// <param name="webpage"></param>
        public PluginInfoAttribute(string name, string description, string version, string author, string webpage)
        {
            this.Name = name;
            this.Description = description;
            this.Version = version;
            this.Author = author;
            this.Webpage = webpage;
        }
    }
}