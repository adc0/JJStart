using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using JJStart;
using System.Media;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using DevComponents.DotNetBar.Metro;
using DevComponents.DotNetBar;
using System.Runtime.Serialization.Formatters.Binary;
using JJPlugin;
using System.Reflection;
using vbAccelerator.Components.Shell;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Metro.ColorTables;
using DevComponents.DotNetBar.Metro.Rendering;
using Microsoft.VisualBasic.FileIO;
using Peter;
using JJStart.Lib;

namespace JJStart
{
    public partial class MainForm : MetroAppForm
    {
        //全局的，用于记录当前鼠标移动到的ButtonItem
        private ButtonItemEx m_item = null;
        private Language m_lang = new Language();

        private ButtonItem btnIRun = new ButtonItem("btnIRun", "运行");
        private ButtonItem btnISearch = new ButtonItem("btnISearch", "搜索");
        private ButtonItem btnIBaidu = new ButtonItem("btnIBaidu", "百度");
        private ButtonItem btnIGoogle = new ButtonItem("btnIGoogle", "谷歌");

        /// <summary>
        /// 插件的加载和运行
        /// </summary>
        #region Plugin
        List<IPlugin> plugins = new List<IPlugin>();
        List<PluginInfoAttribute> piProperties = new List<PluginInfoAttribute>();
        ImageList pluginImageList = new ImageList();

        /// <summary>
        /// 判断dll是否符合插件规范
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsValidPlugin(Type t)
        {
            bool ret = false;
            Type[] interfaces = t.GetInterfaces();

            foreach (Type theInterface in interfaces)
            {
                if (theInterface.FullName == "JJPlugin.IPlugin")
                {
                    ret = true;
                    break;
                }
            }

            return ret;
        }

        /// <summary>
        /// 获取插件
        /// </summary>
        /// <returns></returns>
        private List<string> GetPluginFiles(string PluginsPath)
        {
            List<string> plugins = new List<string>();

            //判断下有没有Plugins文件夹存在
            if (!Directory.Exists(PluginsPath))
                return plugins;

            //获取插件目录下的所有文件和子目录
            string[] files = Directory.GetFiles(PluginsPath);
            string[] dirs = Directory.GetDirectories(PluginsPath);

            foreach (string dir in dirs)
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    FileInfo fi = new FileInfo(file);
                    if (!fi.Extension.ToLower().Equals(".dll"))
                        continue;
                    plugins.Add(file);
                }
            }

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (!fi.Extension.ToLower().Equals(".dll"))
                    continue;
                plugins.Add(file);
            }

            return plugins;
        }

        /// <summary>
        /// 运行插件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunPlugin(object sender, EventArgs e)
        {
            ButtonItemEx bie = sender as ButtonItemEx;

            //PluginInfoAttribute
            PluginInfoAttribute attr = bie.Tag as PluginInfoAttribute;

            //IPlugin
            IPlugin p = (IPlugin)plugins[attr.Index];

            //判断是否需要管理员权限
            if(p.IsElevationRequired)
                process.StartByRunas(Application.ExecutablePath, "-loadplugin " + attr.FullName);
            else
                p.OnLoad();
        }

        /// <summary>
        /// 从“Plugins”目录下加载插件
        /// </summary>
        private void LoadPlugins()
        {
            try
            {
                int i = 0;

                //属性
                PluginInfoAttribute typeAttribute = new PluginInfoAttribute();

                //遍历list
                foreach(string file in GetPluginFiles(Global.PluginsPath))
                {
                    try
                    {
                        Assembly tmp = Assembly.LoadFile(file);
                        Type[] types = tmp.GetTypes();
                        bool ok = false;

                        //遍历types
                        foreach (Type t in types)
                        {
                            //插件判断
                            if (IsValidPlugin(t))
                            {
                                plugins.Add((IPlugin)tmp.CreateInstance(t.FullName));

                                object[] attbs = t.GetCustomAttributes(typeAttribute.GetType(), false);
                                PluginInfoAttribute attribute = null;

                                foreach (object attb in attbs)
                                {
                                    if (attb is PluginInfoAttribute)
                                    {
                                        attribute = (PluginInfoAttribute)attb;
                                        attribute.Index = i;
                                        attribute.FullName = file;
                                        i++;
                                        ok = true;
                                        break;
                                    }
                                }

                                if (attribute != null)
                                    this.piProperties.Add(attribute);

                                if (ok)
                                    break;
                            }
                        }
                    }
                    catch (Exception ex) {}
                }

                //设置大小和深度
                pluginImageList.ColorDepth = ColorDepth.Depth32Bit;
                pluginImageList.ImageSize = new Size(32, 32);

                //遍历
                foreach (PluginInfoAttribute pia in piProperties)
                {
                    //获取文件的大小图标
                    pluginImageList.Images.Add(((IPlugin)plugins[pia.Index]).GetIcon());
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 刷新插件
        /// </summary>
        private void ReloadPlugins()
        {
            //首先清空一些东西
            plugins.Clear();
            piProperties.Clear();
            pluginImageList.Images.Clear();
            sideBar1.ExpandedPanel.SubItems.Clear();

            //加载
            LoadPlugins();

            //显示插件
            ShowPlugins();
        }

        /// <summary>
        /// 显示插件
        /// </summary>
        private void ShowPlugins()
        {
            for (int i = 0; i < piProperties.Count; i++)
            {
                PluginInfoAttribute pia = piProperties[i];

                //实例化一个button
                ButtonItemEx item = new ButtonItemEx();

                //设置ItemType
                item.ItemType = ButtonItemEx.ButtonItemType.Plugin;

                //判断下权限
                if (Global.IsAdminRole)
                {
                    //如果是管理员权限，则不显示
                    item.ShowUACIcon = false;
                }
                else
                {
                    //如果不是管理员权限，这里则判断下插件是否需要uac
                    if (((IPlugin)plugins[pia.Index]).IsElevationRequired)
                        item.ShowUACIcon = true;
                    else
                        item.ShowUACIcon = false;
                }

                //设置风格
                item.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;

                //设置图标风格
                item.ImagePosition = eImagePosition.Top;

                //设置图片索引
                item.ImageIndex = i;

                //设置tooltip
                item.Tooltip = "Name:" + pia.Name + "\r\nDescription:" + pia.Description + "\r\nVersion:" + pia.Version + "\r\nAuthor:" + pia.Author + "\r\nWebsite:" + pia.Webpage;

                //设置事件
                item.Click += new EventHandler(RunPlugin);

                //tag
                item.Tag = pia;

                //添加到panels
                sideBar1.ExpandedPanel.SubItems.Add(item);

                //必须最后设置Text
                //因为实例化后，还没有父控件之说
                item.Text = pia.Name;
            }

            sideBar1.ExpandedPanel.Refresh();
        }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            //实例化些变量
            Global.SettingsManager = new SettingsManager();

            //加载配置信息
            Global.LoadSettings(this);

            //设置全局变量
            Global.Init(this.splitContainer1);

            //填充事件
            btnIRun.Click += btnIRun_Click;
            btnISearch.Click += btnISearch_Click;
            btnIBaidu.Click += btnIBaidu_Click;
            btnIGoogle.Click += btnIGoogle_Click;

            //把按钮添加到textBoxDropDown1
            textBoxDropDown1.DropDownItems.AddRange(new BaseItem[] { btnIRun, btnISearch, btnIBaidu, btnIGoogle });

            //设置语言
            m_lang.SetLanguage(this);

            //初始化
            this.Init();       

            //判断是否允许加载插件
            if (Global.Settings.LoadPlugin)
            {
                //不管怎么样，反正先创建tab
                metroShell1.CreateTab("Plugins", "Plugins", metroShell1.Items.Count - 1);

                //加载插件信息
                LoadPlugins();
            }

            //添加热键管理
            Global.Settings.WinHotKey.Handle = this.Handle;

            //注册所有热键
            Global.Settings.WinHotKey.ReloadAll();

            //设置图标信息
            this.Icon = Properties.Resources.JJStart;
            this.notifyIcon1.Icon = Properties.Resources.JJStart;
            this.notifyIcon1.Text = "JJStart " + Global.Version;
        }

        /// <summary>
        /// 重写OnLoad
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            //添加界面风格选项
            string[] styles = Enum.GetNames(typeof(eStyle));

            //遍历风格
            foreach (string name in styles)
            {
                ButtonItem bi = new ButtonItem(name, name);

                //添加到皮肤按钮
                this.btnSkin.SubItems.Add(bi);

                //添加事件
                bi.Click += new EventHandler(_item_Click);
            }

            //添加Custom风格选项
            ColorPickerDropDown colorPickerCustomScheme = new ColorPickerDropDown("ColorPickerCustomScheme", "Custom");
            colorPickerCustomScheme.DisplayMoreColors = false;
            colorPickerCustomScheme.BeginGroup = true;
            this.btnSkin.SubItems.Add(colorPickerCustomScheme);

            //事件
            colorPickerCustomScheme.ColorPreview += new ColorPreviewEventHandler(colorPickerCustomScheme_ColorPreview);
            colorPickerCustomScheme.SelectedColorChanged += new EventHandler(colorPickerCustomScheme_SelectedColorChanged);

            ////添加BackColor Custom风格选项
            //ColorPickerDropDown backColorPickerCustomScheme = new ColorPickerDropDown("BackColorPickerCustomScheme", "BackColor Custom");
            //this.btnSkin.SubItems.Add(backColorPickerCustomScheme);

            ////事件
            //backColorPickerCustomScheme.ColorPreview += new ColorPreviewEventHandler(colorPickerCustomScheme_ColorPreview);
            //backColorPickerCustomScheme.SelectedColorChanged += new EventHandler(colorPickerCustomScheme_SelectedColorChanged);

            //继续允许MainForm_Load
            base.OnLoad(e);
        }

        /// <summary>
        /// 重写消息循环
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                if (!this.Visible)
                    this.Show();
            }

            switch (m.Msg)
            {
                case win32.WM_HOTKEY:
                    {
                        int id = m.WParam.ToInt32();
                        if (id == Global.Settings.HotKey.Id)
                        {
                            if (this.Visible)
                                this.Hide();
                            else
                                this.Show();
                            return;
                        }

                        Global.Settings.WinHotKey.Run(id);
                    }
                    break;
                case win32.WM_SYSCOMMAND:
                    switch (m.WParam.ToInt32())
                    {
                        case win32.SC_CLOSE:
                            {
                                btnIMenuExit_Click(null, null);
                            }
                            break;
                        case win32.SC_MINIMIZE:
                            {
                                this.Visible = false;
                                return;
                            }
                    }
                    break;
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            try
            {          
                //删除的
                //没有配置信息的话，这里则默认添加一个“默认”栏目
                if (Global.Settings.FirstRun)
                {
                    Global.Settings.FirstRun = false;
                    Global.Settings.Category.AddCategory(new Item(m_lang.GetString("1", "默认")));
                }

                //加载窗体信息
                this.Size = Global.Settings.Size;

                //这里判断下location
                if (Global.Settings.Location.X == 0 && Global.Settings.Location.Y == 0)
                    this.Location = new Point(52, 52);
                else
                    this.Location = Global.Settings.Location;

                //其他设置
                this.styleManager1.ManagerStyle = Global.Settings.ManagerStyle;
                this.styleManager1.ManagerColorTint = Global.Settings.ManagerColorTint;

                //判断下索引
                switch (Global.Settings.ButtonCheckedIndex)
                {
                    case 0:
                        btnIRun.Checked = true;
                        //更新AutoComplete的模式和Source
                        this.textBoxDropDown1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        this.textBoxDropDown1.AutoCompleteSource = AutoCompleteSource.FileSystem;
                        break;
                    case 1:
                        btnISearch.Checked = true;
                        break;
                    case 2:
                        btnIBaidu.Checked = true;
                        break;
                    case 3:
                        btnIGoogle.Checked = true;
                        break;
                }

                //移除默认的
                metroShell1.Items.RemoveAt(0);

                //遍历category名称
                foreach (string strName in Global.Settings.Category.Items.Keys)
                {
                    //创建tab
                    MetroTabItem mti = metroShell1.CreateTab(strName, strName, metroShell1.Items.Count - 1);

                    //设置事件
                    mti.TextChanged += MetroTabItem_TextChanged;
                }

                //选中第一个
                metroShell1.SelectedTab = metroShell1.Items[0] as MetroTabItem;

                //对MetroStatusBar进行初始化
                foreach(ItemSubInfo info in Global.Settings.MainPanelItems)
                {
                    MetroStatusBarAddItem(info, false);
                }
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// 改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroTabItem_TextChanged(object sender, EventArgs e)
        {
            MetroTabItem mti = sender as MetroTabItem;

            //用于好看
            Dictionary<string, Item> dic = Global.Settings.Category.Items;

            //更新
            //添加到category中的itemlist中
            if (dic.ContainsKey(mti.Name))
            {
                //没存在的话，则新建一个新的
                Item item = new Item(mti.Text);

                //得到原始的FileList
                item.Items = dic[mti.Name].Items;

                //移除
                dic.Remove(mti.Name);

                //添加到新的
                dic.Add(mti.Text, item);

                //改变名称
                mti.Name = mti.Text;
            }
        }

        /// <summary>
        /// 新建类名
        /// </summary>
        /// <param name="strName"></param>
        private void AddCate(string strName)
        {
            try
            {
                //这里过滤掉特殊的tab-"Plugins"
                if (strName.Trim().ToString() == "plugins")
                {
                    MessageBoxEx.Show(this, m_lang.GetString("2", "该类别名称已被程序内部使用，用户不能新建！"));
                    return;
                }

                //判断下是否已经存在
                if (Global.Settings.Category.Items.ContainsKey(strName))
                {
                    MessageBoxEx.Show(this, m_lang.GetString("3", "该类名已经存在，请重新输入！"));
                    return;
                }

                //实例化一个item
                Item item = new Item(strName);

                //加入category
                Global.Settings.Category.AddCategory(item);

                //创建tab
                //判断是否开启了插件
                if (Global.Settings.LoadPlugin)
                {
                    //MetroTabItem
                    MetroTabItem mti = metroShell1.CreateTab(strName, strName, metroShell1.Items.Count - 2);

                    //设置事件
                    mti.TextChanged += MetroTabItem_TextChanged;

                    //选中
                    metroShell1.SelectedTab = mti;
                }
                else
                {
                    //MetroTabItem
                    MetroTabItem mti = metroShell1.CreateTab(strName, strName, metroShell1.Items.Count - 1);

                    //设置事件
                    mti.TextChanged += MetroTabItem_TextChanged;

                    //选中
                    metroShell1.SelectedTab = mti;
                }
                
                //刷新控件
                metroShell1.Refresh();
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="add"></param>
        private void MetroStatusBarAddItem(ItemSubInfo info, bool add)
        {
            ButtonItemEx item = new ButtonItemEx(info.Name);

            //填充
            item.Tag = info;
            item.Tooltip = info.Name;
            item.Icon = file.GetFileIconEx(info.FullName, true);

            //事件
            item.Click += MetroStatusBarItem_Click;

            //添加
            metroStatusBar1.Items.Add(item);

            //添加到配置中
            if (add) Global.Settings.MainPanelItems.Add(info);
        }

        /// <summary>
        /// SideBar中增加Panels
        /// </summary>
        /// <param name="strName"></param>
        private void SideBarAddPanel(string strName)
        {
            //创建控件
            SideBarPanelItem sbpi = new SideBarPanelItem(strName, strName);

            //设置属性，从对应的Item中获取
            switch (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[strName].ViewMode)
            {
                case ItemSub.ItemViewMode.LargeIcon:
                    sbpi.ItemImageSize = eBarImageSize.Medium;             
                    break;
                case ItemSub.ItemViewMode.SmallIcon:
                    sbpi.ItemImageSize = eBarImageSize.Default;   
                    break;
            }

            //设置是否横排
            if (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[strName].MultiColumn)
            {
                sbpi.LayoutType = eSideBarLayoutType.MultiColumn;
            }

            //添加
            sideBar1.Panels.Add(sbpi);

            //设置expand属性
            //貌似必须要先Panels.Add，然后才能Expand
            if (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[strName].Expanded)
            {
                //展开新加入的栏目
                sbpi.Expanded = true;
            }
        }

        /// <summary>
        /// 向Panels中添加BaseItem
        /// </summary>
        /// <param name="strName"></param>
        private void SideBarPanelAddItem(string strName)
        {
            //先判断下FileInfoList是否已经实例化
            if (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[strName].SubItems == null)
                return;
     
            //先清空一下列表
            sideBar1.Panels[strName].SubItems.Clear();

            //清空一下图标
            imageSmall.Images.Clear();
            imageMedium.Images.Clear();

            //遍历FileList
            foreach (ItemSubInfo f in Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[strName].SubItems)
            {
                //添加文件信息
                AddItemSub(strName, f);
            }
        }

        /// <summary>
        /// 新建栏目函数
        /// </summary>
        /// <param name="strName"></param>
        private void AddItem(string strName)
        {
            try
            {
                //这里过滤掉特殊的panel-"搜索结果"
                if (strName.Trim() == m_lang.GetString("4", "搜索结果"))
                {
                    MessageBoxEx.Show(this, m_lang.GetString("5", "该栏目名称已被程序内部使用，用户不能新建！"));
                    return;
                }

                //判断下是否已经存在
                if (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items.ContainsKey(strName))
                {
                    MessageBoxEx.Show(this, m_lang.GetString("6", "该栏目已经存在，请重新输入！"));
                    return;
                }

                //实例化
                ItemSub list = new ItemSub(strName);

                //添加到category中的itemlist中
                Global.Settings.Category.Items[metroShell1.SelectedTab.Name].AddItem(list);

                //增加到SideBar
                SideBarAddPanel(strName);
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 添加Item Sub
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="file"></param>
        private void AddItemSub(string strName, ItemSubInfo file)
        {
            try
            {
                //实例化一个button
                ButtonItemEx item = new ButtonItemEx();

                //设置风格
                item.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;

                //路径转换一下
                string strPath = path.GetAbsolutePath(Application.ExecutablePath, file.FullName);

                //获取文件的大小图标
                imageSmall.Images.Add(JJStart.Lib.file.GetFileIconEx(strPath, true));
                imageMedium.Images.Add(JJStart.Lib.file.GetFileIconEx(strPath, false));

                //设置图片索引
                item.ImageIndex = imageSmall.Images.Count - 1;
                
                //显示uac
                item.ShowUACIcon = !Global.IsAdminRole;
                item.FullName = file.FullName;

                //设置tooltip
                //判断下热键
                if (file.HotKey == null)
                    item.Tooltip = file.Name + "\r\n" + strPath;
                else
                    item.Tooltip = file.Name + "(" + file.HotKey.ToString() + ")" + "\r\n" + strPath;
                
                //判断下
                if ((sideBar1.Panels[strName] as SideBarPanelItem).ItemImageSize == eBarImageSize.Default)
                    item.ImagePosition = eImagePosition.Left;
                else
                    item.ImagePosition = eImagePosition.Top;

                //设置tag为文件的绝对路径
                item.Tag = file;

                //添加事件
                item.Click += this.buttonItem1_Click;
                item.DoubleClick += this.buttonItem1_DoubleClick;

                //添加到指定的panel
                sideBar1.Panels[strName].SubItems.Add(item);

                //必须最后设置Text
                //因为实例化后，还没有父控件之说
                item.Text = file.Name;
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// AddItemSub的再次封装
        /// </summary>
        /// <param name="strFilePath"></param>
        private void AddItemSubEx(string strFilePath)
        {
            string strFile = strFilePath;
            string strName = strFilePath;
            string strLinkPath = strFilePath;

            //先判断下格式
            if (Path.GetExtension(strFile).ToLower() == ".lnk")
            {
                //link
                vbAccelerator.Components.Shell.ShellLink sl = new vbAccelerator.Components.Shell.ShellLink(strFile);

                //获取link所指向的exe
                strFile = sl.Target;
            }

            //如果目标文件的路径在我们的程序根目录下，则获取相对路径
            if(strFile.Contains(Application.StartupPath))
            {
                //尝试获取相对路径
                strFile = path.GetRelativePath(Application.ExecutablePath, strFile);
            }

            //判断一下
            string name = Path.GetFileNameWithoutExtension(strName);

            //添加
            JJStart.ItemSubInfo file = new JJStart.ItemSubInfo((name == "" ? strName : name), strFile);

            //这里判断下是否已经有栏目存在了
            if (sideBar1.Panels.Count == 0)
            {
                //如果没有这里就自动新建个栏目，名称就是“新建栏目一”
                AddItem(m_lang.GetString("7", "新建栏目一"));
            }

            //加入到cate中
            Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].AddSubItem(file, Global.Settings.WinHotKey);

            //添加文件信息
            AddItemSub(sideBar1.ExpandedPanel.Name, file);
      
            //先判断下格式
            if (Path.GetExtension(strLinkPath).ToLower() == ".lnk")
            {
                //判断下模式
                if (Global.Settings.LinkDragRemove)
                {
                    //如果为ture，则删除link文件
                    File.Delete(strLinkPath);
                }
            }
        }

        /// <summary>
        /// 按钮单击事件，主要是识别Ctrl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //判断下tag是否为NULL
                if (((ButtonItem)sender) == null)
                    return;

                //类型转换
                ButtonItem bi = (ButtonItem)sender;

                //如果按下了Ctrl键，则进入多选状态
                if (win32.GetAsyncKeyState(win32.VK_CONTROL) < 0)
                {
                    if (bi.Checked)
                        bi.Checked = false;
                    else
                        bi.Checked = true;
                }
                else
                {
                    foreach (ButtonItem b in sideBar1.ExpandedPanel.SubItems)
                    {
                        b.Checked = false;
                    }

                    //没有进入多选状态，则判断运行模式
                    if(!Global.Settings.DoubleClickRun)
                    {
                        //这里获取Tag中的内容
                        string strPath = ((ItemSubInfo)((ButtonItem)sender).Tag).FullName;

                        //process.Start
                        process.Start(strPath);
                    }
                }
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// 按钮单击事件，运行程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //没有进入多选状态，则判断运行模式
                if (Global.Settings.DoubleClickRun)
                {
                    //判断下tag是否为NULL
                    if (((ButtonItem)sender).Tag == null)
                        return;

                    //这里获取Tag中的内容
                    string strPath = ((ItemSubInfo)((ButtonItem)sender).Tag).FullName;

                    //process.Start
                    process.Start(strPath);
                }
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroStatusBarItem_Click(object sender, EventArgs e)
        {
            try
            {
                //这里获取Tag中的内容
                string strPath = ((ItemSubInfo)((ButtonItem)sender).Tag).FullName;

                //process.Start
                process.Start(strPath);
            }
            catch (Exception ex) { MessageBoxEx.Show(ex.Message); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void colorPickerCustomScheme_SelectedColorChanged(object sender, EventArgs e)
        {
            ColorPickerDropDown cpcs = sender as ColorPickerDropDown;
            this.styleManager1.ManagerColorTint = cpcs.SelectedColor;

            //保存Color
            Global.Settings.ManagerColorTint = this.styleManager1.ManagerColorTint;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void colorPickerCustomScheme_ColorPreview(object sender, ColorPreviewEventArgs e)
        {
            this.styleManager1.ManagerColorTint = e.Color;

            //ColorPickerDropDown cpdd = sender as ColorPickerDropDown;

            //if (cpdd.Text == "Custom")
            //{
            //    MetroColorGeneratorParameters theme = new MetroColorGeneratorParameters(this.styleManager1.MetroColorParameters.CanvasColor, e.Color);
            //    StyleManager.MetroColorGeneratorParameters = theme;
            //}
            //else
            //{
            //    MetroColorGeneratorParameters theme = new MetroColorGeneratorParameters(e.Color, this.styleManager1.MetroColorParameters.BaseColor);
            //    StyleManager.MetroColorGeneratorParameters = theme;
            //}
        }

        /// <summary>
        /// 皮肤按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _item_Click(object sender, EventArgs e)
        {
            ButtonItem item = sender as ButtonItem;
            this.styleManager1.ManagerStyle = (eStyle)Enum.Parse(typeof(eStyle), item.Name);

            //保存Style
            Global.Settings.ManagerStyle = this.styleManager1.ManagerStyle;
        }

        /// <summary>
        /// 处理按键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            //处理全选
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                //过滤掉Plugins
                if (metroShell1.SelectedTab.Name == "Plugins")
                    return;

                //判断是否存在ExpandedPanel
                if (sideBar1.ExpandedPanel != null)
                {
                    //如果存在，则全选SubItems
                    foreach (ButtonItem bi in sideBar1.ExpandedPanel.SubItems)
                    {
                        if (!bi.Checked) bi.Checked = true;
                    }
                }
            }

            //保存
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
            {
                //序列化
                if (Global.SettingsManager.Save(Global.Settings))
                    notifyIcon1.BalloonTipText = m_lang.GetString("8", "配置信息保存成功。");
                else
                    notifyIcon1.BalloonTipText = m_lang.GetString("9", "配置信息保存失败。");

                //Show Tip
                notifyIcon1.ShowBalloonTip(1000);
            }

            //判断下
            if(textBoxDropDown1.Text.Length != 0)
            {
                //传递键盘事件
                textBoxDropDown1_KeyDown(sender, e);
            }
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //主要是通知metroshell添加ItemSub
            metroShell1_SelectedTabChanged(sender, e);
        }

        /// <summary>
        /// 动态保存位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_LocationChanged(object sender, EventArgs e)
        {
            if(Global.Settings != null)
                Global.Settings.Location = this.Location;
        }

        /// <summary>
        /// 动态保存大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //判断一下是否为null
            if (Global.Settings != null)
                Global.Settings.Size = this.Size;
        }

        /// <summary>
        /// 帮助功能
        /// </summary>
        private void metroShell1_HelpButtonClick(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://jjstart.org");
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// 当tab改变时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroShell1_SelectedTabChanged(object sender, EventArgs e)
        {
            try
            {
                if (metroShell1.SelectedTab.Panel != null)
                {
                    //首先清空Panels
                    sideBar1.Panels.Clear();

                    //清理subitem
                    metroShell1.SelectedTab.SubItems.Clear();

                    //SuspendLayout
                    metroShell1.SelectedTab.Panel.SuspendLayout();

                    //必须得填充一些参数，否则会造成状态栏排版出现细小问题
                    //metroShell1.SelectedTab.Panel.AutoScroll = true;
                    //metroShell1.SelectedTab.Panel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
                    //metroShell1.SelectedTab.Panel.Dock = System.Windows.Forms.DockStyle.Fill;
                    //metroShell1.SelectedTab.Panel.Location = new System.Drawing.Point(0, 51);
                    //metroShell1.SelectedTab.Panel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
                    //metroShell1.SelectedTab.Panel.Padding = new System.Windows.Forms.Padding(2, 0, 2, 3);

                    //把splitContainer1添加到panel           
                    metroShell1.SelectedTab.Panel.Controls.Add(splitContainer1);
                    
                    //过滤掉一下plugins
                    if (metroShell1.SelectedTab.Name.ToLower() == "plugins")
                    {
                        //设置不允许双击更改text
                        sideBar1.EnableDoubleClickChangeText = false;

                        //设置一下图标
                        sideBar1.Images = pluginImageList;
                        sideBar1.ImagesMedium = pluginImageList;

                        //创建控件
                        SideBarPanelItem sbpi = new SideBarPanelItem(m_lang.GetString("10", "全部"), m_lang.GetString("10", "全部"));

                        //设置属性，从对应的Item中获取
                        sbpi.ItemImageSize = eBarImageSize.Medium;

                        //设置是否横排
                        sbpi.LayoutType = eSideBarLayoutType.MultiColumn;

                        //添加
                        sideBar1.Panels.Add(sbpi);

                        //显示插件列表
                        ShowPlugins();

                        //这里主要是恢复菜单到默认
                        this.contextMenuBar1.SetContextMenuEx(this.sideBar1, null);
                        this.contextMenuBar2.SetContextMenuEx(this.sideBar1, null);
                        this.contextMenuBar3.SetContextMenuEx(this.sideBar1, this.buttonItem3);
                    }
                    else
                    {
                        //设置允许双击更改text
                        sideBar1.EnableDoubleClickChangeText = true;

                        //设置一下图标
                        sideBar1.Images = imageSmall;
                        sideBar1.ImagesMedium = imageMedium;

                        //然后从配置信息中加载
                        //首先判断ItemList中是否存在
                        if (!Global.Settings.Category.Items.ContainsKey(metroShell1.SelectedTab.Name))               
                            return;

                        //遍历第一个category中的item
                        foreach (string strName in Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items.Keys)
                        {
                            //添加到SideBar中
                            SideBarAddPanel(strName);
                        }

                        //这里主要是恢复菜单到默认                
                        this.contextMenuBar1.SetContextMenuEx(this.sideBar1, this.buttonItem1);
                        this.contextMenuBar2.SetContextMenuEx(this.sideBar1, null);
                        this.contextMenuBar3.SetContextMenuEx(this.sideBar1, null);
                    }

                    //ResumeLayout
                    metroShell1.SelectedTab.Panel.ResumeLayout();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroShell1_SettingsButtonClick(object sender, EventArgs e)
        {
            SettingsForm f = new SettingsForm(this);
            f.ShowDialog();
        }

        /// <summary>
        /// 新类别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIAddCate2_Click(object sender, EventArgs e)
        {
            int index = 1;
            string text = m_lang.GetString("11", "新类别");

            //首先判断一下index是否存在
            string strName = text + index.ToString();

            //遍历一下，看是否存在
            while (metroShell1.Items.Contains(strName))
            {
                strName = text + (index++).ToString();
            }
            
            //添加
            AddCate(strName);
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIRun_Click(object sender, EventArgs e)
        {
            if(!btnIRun.Checked)
            {
                btnIRun.Checked = true;
                btnISearch.Checked = false;
                btnIBaidu.Checked = false;
                btnIGoogle.Checked = false;

                Global.Settings.ButtonCheckedIndex = 0;

                //更新AutoComplete的模式和Source
                this.textBoxDropDown1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                this.textBoxDropDown1.AutoCompleteSource = AutoCompleteSource.FileSystem;
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnISearch_Click(object sender, EventArgs e)
        {
            if (!btnISearch.Checked)
            {
                btnIRun.Checked = false;
                btnISearch.Checked = true;
                btnIBaidu.Checked = false;
                btnIGoogle.Checked = false;

                Global.Settings.ButtonCheckedIndex = 1;

                //更新AutoComplete的模式和Source
                this.textBoxDropDown1.AutoCompleteMode = AutoCompleteMode.None;
                this.textBoxDropDown1.AutoCompleteSource = AutoCompleteSource.None;
            }
        }

        /// <summary>
        /// 百度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIBaidu_Click(object sender, EventArgs e)
        {
            if (!btnIBaidu.Checked)
            {
                btnIRun.Checked = false;
                btnISearch.Checked = false;
                btnIBaidu.Checked = true;
                btnIGoogle.Checked = false;

                Global.Settings.ButtonCheckedIndex = 2;

                //更新AutoComplete的模式和Source
                this.textBoxDropDown1.AutoCompleteMode = AutoCompleteMode.None;
                this.textBoxDropDown1.AutoCompleteSource = AutoCompleteSource.None;
            }
        }

        /// <summary>
        /// 谷歌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIGoogle_Click(object sender, EventArgs e)
        {
            if (!btnIGoogle.Checked)
            {
                btnIRun.Checked = false;
                btnISearch.Checked = false;
                btnIBaidu.Checked = false;
                btnIGoogle.Checked = true;

                Global.Settings.ButtonCheckedIndex = 3;

                //更新AutoComplete的模式和Source
                this.textBoxDropDown1.AutoCompleteMode = AutoCompleteMode.None;
                this.textBoxDropDown1.AutoCompleteSource = AutoCompleteSource.None;
            }
        }

        /// <summary>
        /// 屏蔽掉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxDropDown1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    //判断勾选了哪个
                    if (btnIRun.Checked)
                    {
                        process.Start(textBoxDropDown1.Text.Trim());
                        return;
                    }

                    //百度
                    if (btnIBaidu.Checked)
                    {
                        Process.Start("http://www.baidu.com/s?wd=" + textBoxDropDown1.Text.Trim());
                        return;
                    }

                    //谷歌
                    if (btnIGoogle.Checked)
                    {
                        //Process.Start
                        Process.Start("http://www.google.com/search?q=" + textBoxDropDown1.Text.Trim());
                        return;
                    }
                }    
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 主要用于“搜索”模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxDropDown1_TextChanged(object sender, EventArgs e)
        {
            //搜索
            if (btnISearch.Checked)
            {
                //响应之前，先判断文本框是否输入
                if (textBoxDropDown1.Text.Length == 0)
                {
                    //这里在此判断下“搜索”模式，如果为0了，则恢复Panel
                    if (btnISearch.Checked)
                    {
                        //判断当前panel是否是“搜索结果”panel
                        if (sideBar1.ExpandedPanel.Name == m_lang.GetString("4","搜索结果"))
                        {
                            //清空
                            sideBar1.Panels.Clear();

                            //触发模式
                            metroShell1_SelectedTabChanged(sender, e);

                            //刷新控件
                            if (sideBar1.ExpandedPanel != null)
                                sideBar1.ExpandedPanel.Refresh();
                            else
                                sideBar1.Refresh();                     
                        }
                    }
                }
                else
                {
                    //名称
                    string strResultName = m_lang.GetString("4", "搜索结果");

                    //"搜索结果"panel的风格设置与“当前expanded”的panel风格保持一致
                    SideBarPanelItem sbpi = new SideBarPanelItem(strResultName, strResultName);

                    //设置属性
                    if (sideBar1.ExpandedPanel != null)
                    {
                        sbpi.ItemImageSize = sideBar1.ExpandedPanel.ItemImageSize;
                        sbpi.LayoutType = sideBar1.ExpandedPanel.LayoutType;
                    }
                    
                    //新建之前，先clear掉
                    sideBar1.Panels.Clear();

                    //新建一个结果Item
                    sideBar1.Panels.Add(sbpi);

                    //遍历catogory中的Item中的List里面的所有元素
                    //遍历Item
                    foreach (string strCateName in Global.Settings.Category.Items.Keys)
                    {
                        //遍历第category中的item
                        foreach (string strItemName in Global.Settings.Category.Items[strCateName].Items.Keys)
                        {
                            //遍历FileList
                            foreach (ItemSubInfo f in Global.Settings.Category.Items[strCateName].Items[strItemName].SubItems)
                            {
                                //这里修复下，只匹配文件名（不包括扩展名）
                                if (f.Name.ToLower().Contains(textBoxDropDown1.Text.Trim()))
                                {
                                    AddItemSub(strResultName, f);
                                } 
                            }
                        }
                    }

                    //刷新控件
                    sideBar1.ExpandedPanel.Refresh();
                }
            }
        }

        /// <summary>
        /// 拖进
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sideBar1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// 拖放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sideBar1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    if(m_item != null)
                    {
                        //取消选中状态
                        m_item.Checked = false;

                        string fullName = (m_item.Tag as ItemSubInfo).FullName;
                        string[] path = (string[])(e.Data.GetData(DataFormats.FileDrop));

                        //如果是目录的话，则复制进去
                        if(Directory.Exists(fullName))
                        {
                            //这里再次判断模式，如果是移动的话，则move文件
                            //遍历拖放进来的文件
                            for (int i = 0; i < path.Length; i++)
                            {
                                //如果是拖放的目录，则复制目录
                                if (Directory.Exists(path[i]))
                                {
                                    DirectoryInfo di = new DirectoryInfo(path[i]);

                                    //Directory.Move(path[i], fullName + "\\" + di.Name);
                                    FileSystem.CopyDirectory(path[i], fullName + "\\" + di.Name, UIOption.AllDialogs, UICancelOption.DoNothing);
                                }
                                else
                                {
                                    //File.Move(path[i], fullName + "\\" + Path.GetFileName(path[i]));
                                    FileSystem.CopyFile(path[i], fullName + "\\" + Path.GetFileName(path[i]), UIOption.AllDialogs, UICancelOption.DoNothing);
                                }    
                            }
                        }
                        else
                        {
                            //开启进程和传入参数
                            process.Start(fullName, path[0]);
                        }
                    }
                    else
                    {
                        string[] path = (string[])(e.Data.GetData(DataFormats.FileDrop));

                        //遍历拖放进来的文件
                        for (int i = 0; i < path.Length; i++)
                        {
                            AddItemSubEx(path[i]);
                        }

                        //刷新控件
                        sideBar1.ExpandedPanel.Refresh();
                    }
                }
                else
                {
                    //先判断下是否是内部拖放
                    if (e.Data.GetData(typeof(ButtonItemEx)) == null)
                        return;

                    //如果
                    bool bSame = true;

                    //首先比较下，是否有改动
                    for (int i = 0; i < sideBar1.ExpandedPanel.SubItems.Count; i++)
                    {
                        //获取Tag，并转换
                        ItemSubInfo file = sideBar1.ExpandedPanel.SubItems[i].Tag as ItemSubInfo;

                        //比较
                        if(Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].SubItems[i] != file)
                        {
                            //不相等的话，则标记为false，并跳出循环
                            bSame = false;
                            break;
                        } 
                    }

                    //判断下bSame，如果都相同的话，则跳出
                    if (bSame)
                    {
                        //如果都相同的话则返回，不继续支持下面部分
                        return;
                    }

                    //清空列表中已有的
                    Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].SubItems.Clear();

                    //遍历SubItems中的ButtonItem
                    for (int i = 0; i < sideBar1.ExpandedPanel.SubItems.Count; i++)
                    {
                        //获取Tag，并转换
                        ItemSubInfo file = sideBar1.ExpandedPanel.SubItems[i].Tag as ItemSubInfo;

                        //重新加入
                        Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].SubItems.Add(file);
                    }
                }
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); } 
        }

        /// <summary>
        /// 主要实现选中的效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sideBar1_DragOver(object sender, DragEventArgs e)
        {
            //试着取消Checked
            if (m_item != null && m_item.Checked)
            {
                m_item.Checked = false;
            }

            //判断一下
            if (sideBar1.ExpandedPanel != null)
            {
                //记录当前选中的item
                m_item = sideBar1.ExpandedPanel.ItemAtLocation(sideBar1.PointToClient(MousePosition).X, sideBar1.PointToClient(MousePosition).Y) as ButtonItemEx;
            }

            //选中
            if (m_item != null && !m_item.Checked)
            {
                m_item.Checked = true;
            }
        }

        /// <summary>
        /// 记录下哪个item展开了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sideBar1_ExpandedChange(object sender, EventArgs e)
        {
            try
            {
                //转换下类型
                SideBarPanelItem sbpi = (SideBarPanelItem)sender;

                //前提是metroShell1.SelectedTab存在，因为发现程序退出时，这玩意为null
                if (metroShell1.SelectedTab != null)
                {
                    //判断哪个展开
                    //因为改变总会有两个，一个关闭，一个展开
                    if (sbpi.Expanded)
                    {
                        //把展开的item的名称记录到配置信息中
                        Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sbpi.Name].Expanded = true;

                        //增加BaseItem到Panel中
                        SideBarPanelAddItem(sbpi.Name);
                    }
                    else
                    {
                        //把展开的item的名称记录到配置信息中
                        Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sbpi.Name].Expanded = false;
                    }
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 如果Text改变，则更新配置信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sideBar1_ItemTextChanged(object sender, EventArgs e)
        {
            //这里判断一下是谁的text改变
            if (sideBar1.SideBarPanelItemTextChange)
            {
                SideBarPanelItem sbpl = sender as SideBarPanelItem;

                //用于好看
                Dictionary<string, ItemSub> dic = Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items;

                //更新
                //添加到category中的itemlist中
                if (dic.ContainsKey(sbpl.Name))
                {
                    //没存在的话，则新建一个新的
                    ItemSub file = new ItemSub(sbpl.Text);

                    //得到原始的FileList
                    file.SubItems = dic[sbpl.Name].SubItems;

                    //填充其他的一些信息
                    file.Expanded = dic[sbpl.Name].Expanded;
                    file.MultiColumn = dic[sbpl.Name].MultiColumn;
                    file.ViewMode = dic[sbpl.Name].ViewMode;

                    //移除
                    dic.Remove(sbpl.Name);

                    //添加到新的
                    dic.Add(sbpl.Text, file);

                    //改变名称
                    sbpl.Name = sbpl.Text;
                }
            }
        }

        /// <summary>
        /// 控制右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sideBar1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //判断是不是plugins
                if (metroShell1.SelectedTab.Name.ToLower() == "plugins")
                {
                    this.contextMenuBar1.SetContextMenuEx(this.sideBar1, null);
                    this.contextMenuBar2.SetContextMenuEx(this.sideBar1, null);
                    return;
                }

                //记录当前选中的item
                m_item = sideBar1.ExpandedPanel.ItemAtLocation(sideBar1.PointToClient(MousePosition).X, sideBar1.PointToClient(MousePosition).Y) as ButtonItemEx;

                //在这里控制右键菜单的不同显现
                if (m_item != null)
                {
                    //在这里还要判断下是否按下了“Shift”
                    if ((win32.GetKeyState(0x10) & 0x8000) != 0)
                    {
                        //先取消
                        this.contextMenuBar1.SetContextMenuEx(this.sideBar1, null);
                        this.contextMenuBar2.SetContextMenuEx(this.sideBar1, null);

                        //显示资源管理器右键菜单
                        ShellContextMenu scm = new ShellContextMenu();
                        List<FileInfo> files = new List<FileInfo>();

                        //添加目标路径
                        files.Add(new FileInfo((m_item.Tag as ItemSubInfo).FullName));

                        //显示菜单
                        scm.ShowContextMenu(files.ToArray(), Cursor.Position);
                    } 
                    else
                    {
                        this.contextMenuBar1.SetContextMenuEx(this.sideBar1, null);
                        this.contextMenuBar2.SetContextMenuEx(this.sideBar1, this.buttonItem2);
                    }
                }
                else
                {
                    //如果选中了ButtonItem的话，则取消选中
                    foreach (ButtonItem bi in sideBar1.ExpandedPanel.SubItems)
                    {
                        //判断是否checked
                        if (bi.Checked) 
                            bi.Checked = false;
                    }

                    //先取消
                    this.contextMenuBar2.SetContextMenuEx(this.sideBar1, null);
                    this.contextMenuBar1.SetContextMenuEx(this.sideBar1, this.buttonItem1);
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sideBar1_MouseMove(object sender, MouseEventArgs e)
        {
            //记录当前选中的item
            m_item = sideBar1.ExpandedPanel.ItemAtLocation(sideBar1.PointToClient(MousePosition).X, sideBar1.PointToClient(MousePosition).Y) as ButtonItemEx;
        }

        /// <summary>
        /// 双击打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible)
                this.Hide();
            else
                this.Show();
        }

        /// <summary>
        /// 控制右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem1_PopupOpen(object sender, PopupOpenEventArgs e)
        {
            try
            {
                //判断是不是plugins
                if (metroShell1.SelectedTab.Name.ToLower() == "plugins")
                {
                    this.contextMenuBar1.SetContextMenuEx(this.sideBar1, null);
                    this.contextMenuBar2.SetContextMenuEx(this.sideBar1, null);
                    return;
                }

                //判断下ExpandedPanel是否存在
                if (sideBar1.ExpandedPanel == null)
                {
                    //如果sideBar1.ExpandedPanel为null，说明是刚建立的类别或默认的类别
                    //隐藏相关菜单选项
                    btnIView.Visible = false;
                    btnIClearItem.Visible = false;
                    btnIDeleteItem.Visible = false;
                    btnIDeleteCate.BeginGroup = true;
                }
                else
                {
                    //恢复true
                    btnIView.Visible = true;
                    btnIClearItem.Visible = true;
                    btnIDeleteItem.Visible = true;
                    btnIDeleteCate.BeginGroup = false;

                    //判断下菜单
                    switch (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].ViewMode)
                    {
                        case ItemSub.ItemViewMode.LargeIcon:
                            btnILargeIcons.Checked = true;
                            btnISmallIcons.Checked = false;
                            break;
                        case ItemSub.ItemViewMode.SmallIcon:
                            btnILargeIcons.Checked = false;
                            btnISmallIcons.Checked = true;
                            break;
                    }

                    //判断下是否允许横排
                    if (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].MultiColumn)
                        btnIMultiColumn.Checked = true;
                    else
                        btnIMultiColumn.Checked = false;
                }

                //判断下是否允许粘贴
                if (Clipboard.ContainsData("JStart_FileInfos"))
                    btnIPaste.Enabled = true;
                else
                    btnIPaste.Enabled = false;
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// button2菜单的控制显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem2_PopupOpen(object sender, PopupOpenEventArgs e)
        {
            //这里处理下显示
            if (!(m_item as ButtonItem).Checked)
            {
                //遍历Items
                foreach (ButtonItem bi in sideBar1.ExpandedPanel.SubItems)
                {
                    //判断是否checked
                    if (bi.Checked)
                    {
                        bi.Checked = false;
                    }
                }
            }
        }

        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIAddFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = true;
                ofd.Filter = "Exe Files(*.exe)|*.exe|All Files(*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (string strFile in ofd.FileNames)
                    {
                        AddItemSubEx(strFile);
                    }
                }
            }

            sideBar1.ExpandedPanel.Refresh();
        }

        /// <summary>
        /// 添加目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIAddDir_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.RootFolder = Environment.SpecialFolder.Desktop;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    AddItemSubEx(fbd.SelectedPath);
                }
            }

            sideBar1.ExpandedPanel.Refresh();
        }

        /// <summary>
        /// 添加分组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnINewCate_Click(object sender, EventArgs e)
        {
            InputForm f = new InputForm();

            //设置提示信息
            f.lblText.Text = m_lang.GetString("12", "请输入新类别名称：");

            if (f.ShowDialog() == DialogResult.OK)
            {
                string strInput = f.txtInput.Text;
                AddCate(strInput);
            }
        }

        /// <summary>
        /// 新建栏目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnINewItem_Click(object sender, EventArgs e)
        {
            InputForm f = new InputForm();
            f.lblText.Text = m_lang.GetString("13", "请输入新栏目名称：");

            if (f.ShowDialog() == DialogResult.OK)
            {
                string strInput = f.txtInput.Text;
                AddItem(strInput);
            }
        }

        /// <summary>
        /// 大图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnILargeIcons_Click(object sender, EventArgs e)
        {
            try
            {
                if (!btnILargeIcons.Checked)
                {
                    if (sideBar1.ExpandedPanel != null)
                    {
                        //添加到Item中
                        //这里加个判断，防止在“搜索结果”panel中出现错误
                        if (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].ExistsItem(sideBar1.ExpandedPanel.Name))
                            Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].ViewMode = ItemSub.ItemViewMode.LargeIcon;

                        //设置属性
                        sideBar1.ExpandedPanel.ItemImageSize = eBarImageSize.Medium;

                        //勾选和取消
                        btnILargeIcons.Checked = true;
                        btnISmallIcons.Checked = false;

                        //先清空
                        sideBar1.ExpandedPanel.SubItems.Clear();

                        //重新加载下
                        SideBarPanelAddItem(sideBar1.ExpandedPanel.Name);

                        //刷新
                        sideBar1.ExpandedPanel.Refresh();
                    }
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 小图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnISmallIcons_Click(object sender, EventArgs e)
        {
            try
            {
                if (!btnISmallIcons.Checked)
                {
                    if (sideBar1.ExpandedPanel != null)
                    {
                        //添加到Item中
                        //这里加个判断，防止在“搜索结果”panel中出现错误
                        if (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].ExistsItem(sideBar1.ExpandedPanel.Name))
                            Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].ViewMode = ItemSub.ItemViewMode.SmallIcon;

                        //设置属性
                        sideBar1.ExpandedPanel.ItemImageSize = eBarImageSize.Default;

                        //勾选和取消
                        btnILargeIcons.Checked = false;
                        btnISmallIcons.Checked = true;

                        //先清空
                        sideBar1.ExpandedPanel.SubItems.Clear();

                        //重新加载下
                        SideBarPanelAddItem(sideBar1.ExpandedPanel.Name);

                        //刷新
                        sideBar1.ExpandedPanel.Refresh();
                    }
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 允许横排
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIMultiColumn_Click(object sender, EventArgs e)
        {
            if (btnIMultiColumn.Checked)
            {
                if (sideBar1.ExpandedPanel != null)
                {
                    //添加到Item中
                    //这里加个判断，防止在“搜索结果”panel中出现错误
                    if (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].ExistsItem(sideBar1.ExpandedPanel.Name))
                        Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].MultiColumn = false;

                    //设置属性
                    sideBar1.ExpandedPanel.LayoutType = eSideBarLayoutType.Default;

                    //设置属性
                    btnIMultiColumn.Checked = false;

                    sideBar1.RecalcLayout();
                }
            }
            else
            {
                //横排之类的
                if (sideBar1.ExpandedPanel != null)
                {
                    //添加到Item中
                    //这里加个判断，防止在“搜索结果”panel中出现错误
                    if (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].ExistsItem(sideBar1.ExpandedPanel.Name))
                        Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].MultiColumn = true;

                    //设置属性
                    sideBar1.ExpandedPanel.LayoutType = eSideBarLayoutType.MultiColumn;

                    //设置属性
                    btnIMultiColumn.Checked = true;

                    sideBar1.RecalcLayout();
                }
            }
        }

        /// <summary>
        /// 粘贴
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIPaste_Click(object sender, EventArgs e)
        {
            try
            {
                //判断是否有此格式的数据
                if (Clipboard.ContainsData("JStart_FileInfo"))
                {
                    //这里判断下是否已经有栏目存在了
                    if (sideBar1.Panels.Count == 0)
                    {
                        //如果没有这里就自动新建个栏目，名称就是“新建栏目一”
                        AddItem(m_lang.GetString("7", "新建栏目一"));
                    }

                    //类型转换
                    var fi = (JJStart.ItemSubInfo)Clipboard.GetData("JStart_FileInfo");

                    //加入到cate中
                    Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].AddSubItem(fi, Global.Settings.WinHotKey);

                    //添加
                    AddItemSub(sideBar1.ExpandedPanel.Name, fi);

                    //刷新一下
                    sideBar1.ExpandedPanel.Refresh();
                }
                else if (Clipboard.ContainsData("JStart_FileInfos"))
                {
                    //这里判断下是否已经有栏目存在了
                    if (sideBar1.Panels.Count == 0)
                    {
                        //如果没有这里就自动新建个栏目，名称就是“新建栏目一”
                        AddItem(m_lang.GetString("7", "新建栏目一"));
                    }

                    //类型转换
                    var lstFInfos = Clipboard.GetData("JStart_FileInfos") as List<ItemSubInfo>;

                    //为啥要倒过来，主要是为了顺序一致
                    for (int i = lstFInfos.Count - 1; i >= 0; i--)
                    {
                        var fi = lstFInfos[i];

                        //加入到cate中
                        Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].AddSubItem(fi, Global.Settings.WinHotKey);

                        //添加
                        AddItemSub(sideBar1.ExpandedPanel.Name, fi);
                    }

                    //刷新一下
                    sideBar1.ExpandedPanel.Refresh();
                }
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// 清空本栏目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIClearItem_Click(object sender, EventArgs e)
        {
            try
            {
                //弹个MessageBoxEx
                if (MessageBoxEx.Show(this, m_lang.GetString("14", "你确定要清空本栏目吗？"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    //从Category中清空当前栏目
                    Global.Settings.Category.Items[metroShell1.SelectedTab.Name].ClearItem(sideBar1.ExpandedPanel.Name);

                    //清空
                    sideBar1.ExpandedPanel.SubItems.Clear();

                    //刷新
                    sideBar1.ExpandedPanel.Refresh();
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 删除本栏目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIDeleteItem_Click(object sender, EventArgs e)
        {
            try
            {
                //弹个MessageBoxEx
                if (MessageBoxEx.Show(this, m_lang.GetString("15", "你确定要删除本栏目吗？"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    //从Category中删除当前栏目
                    Global.Settings.Category.Items[metroShell1.SelectedTab.Name].DeleteItem(sideBar1.ExpandedPanel.Name);

                    //获取index
                    int nIndex = sideBar1.Panels.IndexOf(sideBar1.ExpandedPanel.Name);

                    //从sideBar1中移除改Item
                    sideBar1.Panels.Remove(sideBar1.ExpandedPanel.Name);

                    //如果还有panels
                    if (sideBar1.Panels.Count != 0)
                    {
                        //要展开的index
                        nIndex = nIndex - 1;

                        //判断下索引
                        if (nIndex < 0)
                        {
                            nIndex = nIndex + 1;
                        }

                        //删除后，则展开上一个panel
                        sideBar1.Panels[nIndex].Expanded = true;
                    }

                    //刷新
                    sideBar1.Refresh();
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 删除本类别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIDeleteCate_Click(object sender, EventArgs e)
        {
            try
            {
                //确定下item的数目，如果只有一个了，则阻止用户删除
                //判断下是否开启了插件
                if (Global.Settings.LoadPlugin)
                {
                    if (metroShell1.Items.Count == 3)
                    {
                        MessageBoxEx.Show(this, m_lang.GetString("16", "至少需要保留一个类别！"));
                        return;
                    }
                }
                else
                {
                    if (metroShell1.Items.Count == 2)
                    {
                        MessageBoxEx.Show(this, m_lang.GetString("16", "至少需要保留一个类别！"));
                        return;
                    }
                }

                //弹个MessageBoxEx
                if (MessageBoxEx.Show(this, m_lang.GetString("17", "你确定要删除本类别吗？"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    //从Category中删除当前栏目
                    Global.Settings.Category.DeleteCategory(metroShell1.SelectedTab.Name);

                    //获取index
                    int nIndex = metroShell1.Items.IndexOf(metroShell1.SelectedTab.Name);

                    //从metroShell1中移除改Item
                    metroShell1.Items.Remove(metroShell1.SelectedTab.Name);

                    //如果还有Items
                    if (metroShell1.Items.Count != 0)
                    {
                        //要展开的index
                        nIndex = nIndex - 1;

                        //判断下索引
                        if (nIndex < 0)
                        {
                            nIndex = nIndex + 1;
                        }

                        //删除后，选中上一个tab
                        metroShell1.SelectedTab = (MetroTabItem)metroShell1.Items[nIndex];
                    }

                    //刷新下控件
                    metroShell1.Refresh();
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 普通的打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmibtnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                string strPath = "";

                //遍历Items
                foreach (ButtonItem bi in sideBar1.ExpandedPanel.SubItems)
                {
                    //判断是否checked
                    if (bi.Checked)
                    {
                        //这里获取Tag中的内容
                        strPath = (bi.Tag as ItemSubInfo).FullName;

                        //打开
                        process.Start(strPath);
                    }
                }

                //判断下tag是否为NULL，或者有没有checked
                if (m_item == null || m_item.Checked)
                    return;

                //这里获取Tag中的内容
                strPath = ((ItemSubInfo)m_item.Tag).FullName;

                //打开
                process.Start(strPath);
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// 以管理员权限运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIRunasAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                string strPath = "";

                //遍历Items
                foreach (ButtonItem bi in sideBar1.ExpandedPanel.SubItems)
                {
                    //判断是否checked
                    if (bi.Checked)
                    {
                        //这里获取Tag中的内容
                        strPath = (bi.Tag as ItemSubInfo).FullName;

                        //以管理员权限打开
                        process.StartByRunas(strPath);
                    }
                }

                //判断下tag是否为NULL，或者有没有checked
                if (m_item == null || m_item.Checked)
                    return;

                //这里获取Tag中的内容
                strPath = ((ItemSubInfo)m_item.Tag).FullName;

                //以管理员权限打开
                process.StartByRunas(strPath);     
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// 删除Item中的子项，即栏目中的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //先判断下ExpandedPanel
                if (sideBar1.ExpandedPanel == null)
                    return;

                int count = 0;
                string strMsg = m_lang.GetString("18", "你确定要删除这个快捷方式吗？");

                //判断是否是多选
                for (int i = 0; i < sideBar1.ExpandedPanel.SubItems.Count; i++)
                {
                    ButtonItem bi = sideBar1.ExpandedPanel.SubItems[i] as ButtonItem;

                    if (bi.Checked)
                        count++;
                }

                //如果没有选中，且buttonItem也为NULL，则返回
                if (count == 0 && m_item == null)
                {
                    return;
                }
                  
                //如果checked大于1的话
                if(count > 1)
                {
                    strMsg = m_lang.GetString("19", "你确定要删除这些快捷方式吗？");
                }

                //弹个MessageBoxEx
                if (MessageBoxEx.Show(this, strMsg, "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    //遍历Items
                    for (int i = sideBar1.ExpandedPanel.SubItems.Count - 1; i >= 0; i--)
                    {
                        //判断是否checked
                        ButtonItem bi = sideBar1.ExpandedPanel.SubItems[i] as ButtonItem;

                        if (bi.Checked)
                        {
                            //从List中删除对应的文件
                            Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].DeleteSubItem((ItemSubInfo)bi.Tag, Global.Settings.WinHotKey);

                            //移除此Item
                            sideBar1.ExpandedPanel.SubItems.Remove(bi);
                        }
                    }

                    //判断下tag是否为NULL，或者有没有checked
                    if (m_item == null || m_item.Checked)
                        return;

                    //从List中删除对应的文件
                    Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].DeleteSubItem((ItemSubInfo)m_item.Tag, Global.Settings.WinHotKey);

                    //获得索引
                    int nIndex = sideBar1.ExpandedPanel.SubItems.IndexOf(m_item);

                    //移除此Item
                    sideBar1.ExpandedPanel.SubItems.RemoveAt(nIndex);
                }  
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// 剪切
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnICut_Click(object sender, EventArgs e)
        {
            try
            {
                //存放到列表中
                List<ItemSubInfo> items = new List<ItemSubInfo>();

                //判断下
                if (sideBar1.ExpandedPanel == null)
                    return;

                //遍历Items
                for (int i = sideBar1.ExpandedPanel.SubItems.Count - 1; i >= 0; i--)
                {
                    //判断是否checked
                    ButtonItem bi = sideBar1.ExpandedPanel.SubItems[i] as ButtonItem;

                    if (bi.Checked)
                    {
                        //FileInfo
                        var fi = (ItemSubInfo)bi.Tag;

                        //添加到list
                        items.Add(fi);

                        //移除
                        sideBar1.ExpandedPanel.SubItems.Remove(bi);

                        //从配置中移除
                        Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].DeleteSubItem(fi, Global.Settings.WinHotKey);
                    }
                }

                //判断下tag是否为NULL，或者有没有checked
                if (m_item != null && !m_item.Checked)
                {
                    items.Add((ItemSubInfo)m_item.Tag);

                    //移除
                    sideBar1.ExpandedPanel.SubItems.Remove(((ButtonItem)m_item));

                    //从配置中移除
                    Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].DeleteSubItem((ItemSubInfo)m_item.Tag, Global.Settings.WinHotKey);
                }

                //判断下
                if (items.Count == 0)
                    return;

                //复制到剪切板
                Clipboard.SetData("JStart_FileInfos", items);
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnICopy_Click(object sender, EventArgs e)
        {
            try
            {
                //存放到列表中
                List<ItemSubInfo> items = new List<ItemSubInfo>();

                //判断下
                if (sideBar1.ExpandedPanel == null)
                    return;

                //遍历Items
                for (int i = sideBar1.ExpandedPanel.SubItems.Count - 1; i >= 0; i--)
                {
                    //判断是否checked
                    ButtonItem bi = sideBar1.ExpandedPanel.SubItems[i] as ButtonItem;

                    if (bi.Checked)
                    {
                        //添加到list
                        items.Add((ItemSubInfo)bi.Tag);
                    }
                }

                //判断下tag是否为NULL，或者有没有checked
                if (m_item != null && !m_item.Checked)
                    items.Add((ItemSubInfo)m_item.Tag);

                //判断下
                if (items.Count == 0)
                    return;

                //复制到剪切板
                Clipboard.SetData("JStart_FileInfos", items);
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// 添加到主面板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIAddToMainPanel_Click(object sender, EventArgs e)
        {
            //判断下
            if (sideBar1.ExpandedPanel == null)
                return;

            //遍历Items
            for (int i = 0; i < sideBar1.ExpandedPanel.SubItems.Count; i++)
            {
                //判断是否checked
                ButtonItem bi = sideBar1.ExpandedPanel.SubItems[i] as ButtonItem;

                if (bi.Checked)
                {
                    MetroStatusBarAddItem(bi.Tag as ItemSubInfo, true);
                }
            }

            //判断下tag是否为NULL，或者有没有checked
            if (m_item != null && !m_item.Checked)
            {
                MetroStatusBarAddItem(m_item.Tag as ItemSubInfo, true);
            }
        }

        /// <summary>
        /// 设置热键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnISetHotKey_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_item == null)
                    return;

                ItemSubInfo fi = m_item.Tag as ItemSubInfo;
                SetHotKeyForm f = new SetHotKeyForm(fi, Global.Settings.WinHotKey);

                //如果设置成功
                if(f.ShowDialog() == DialogResult.OK)
                {
                    //路径转换一下
                    string strPath = Path.IsPathRooted(fi.FullName) ? fi.FullName : Application.StartupPath + "\\" + fi.FullName;

                    //设置tooltip
                    if (fi.HotKey == null)
                        m_item.Tooltip = fi.Name + "\r\n" + strPath;
                    else
                        m_item.Tooltip = fi.Name + "(" + fi.HotKey.ToString() + ")" + "\r\n" + strPath;
                }
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// 生成快捷方式到桌面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnICreateShortcut_Click(object sender, EventArgs e)
        {
            try
            {
                //遍历Items
                for (int i = 0; i < sideBar1.ExpandedPanel.SubItems.Count; i++)
                {
                    //判断是否checked
                    ButtonItem bi = sideBar1.ExpandedPanel.SubItems[i] as ButtonItem;

                    //如果选中了
                    if (bi.Checked)
                    {
                        ItemSubInfo fi = bi.Tag as ItemSubInfo;

                        //添加快捷键
                        using(ShellLink sl = new ShellLink())
                        {
                            //获取绝对路径
                            string strPath = path.GetAbsolutePath(Application.ExecutablePath, fi.FullName);

                            //设置工作目录、目标exe等信息
                            sl.WorkingDirectory = System.IO.Path.GetDirectoryName(strPath);
                            sl.Target = strPath;

                            //保存快捷方式
                            sl.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + fi.Name + ".lnk");
                        }
                    }
                }

                //判断下tag是否为NULL，或者有没有checked
                if (m_item == null || m_item.Checked)
                    return;

                //添加快捷键
                using (ShellLink sl = new ShellLink())
                {
                    ItemSubInfo fi = m_item.Tag as ItemSubInfo;

                    //获取绝对路径
                    string strPath = path.GetAbsolutePath(Application.ExecutablePath, fi.FullName);

                    //设置工作目录、目标exe等信息
                    sl.WorkingDirectory = System.IO.Path.GetDirectoryName(strPath);
                    sl.Target = strPath;

                    //保存快捷方式
                    sl.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + fi.Name + ".lnk");
                }
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// 快捷方式重命名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIRename_Click(object sender, EventArgs e)
        {
            //判断下buttonItem
            if(m_item != null)
            {
                sideBar1.BeginItemTextEdit(m_item);
            }
        }

        /// <summary>
        /// 浏览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIExplore_Click(object sender, EventArgs e)
        {
            foreach (ButtonItem bi in sideBar1.ExpandedPanel.SubItems)
            {
                //判断是否checked
                if (bi.Checked)
                {
                    //打开属性对话框
                    file.ExplorerFile((bi.Tag as ItemSubInfo).FullName);
                }
            }

            //判断下tag是否为NULL，或者有没有checked
            if (m_item == null || m_item.Checked)
                return;

            //打开属性对话框
            file.ExplorerFile(((ItemSubInfo)m_item.Tag).FullName);
        }

        /// <summary>
        /// 打开属性对话框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIProperties_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ButtonItem bi in sideBar1.ExpandedPanel.SubItems)
                {
                    //判断是否checked
                    if (bi.Checked)
                    {
                        //打开属性对话框
                        file.ShowFileProperty((bi.Tag as ItemSubInfo).FullName);
                    }
                }

                //判断下tag是否为NULL，或者有没有checked
                if (m_item == null || m_item.Checked)
                    return;

                //打开属性对话框
                file.ShowFileProperty(((ItemSubInfo)m_item.Tag).FullName);
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// 刷新插件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIRefreshPlugin_Click(object sender, EventArgs e)
        {
            ReloadPlugins();
        }

        /// <summary>
        /// 时间控件，由于窗体的贴边隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            int heigth = this.Height;
            QQFormHide.hide_show(this, heigth);
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIMenuExit_Click(object sender, EventArgs e)
        {
            //退出前，做保存
            Global.SettingsManager.Save(Global.Settings);

            //注销所有热键
            Global.Settings.WinHotKey.UnReloadAll();

            //退出
            Application.Exit();
        }

        /// <summary>
        /// 检测升级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIMenuCheckUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://jjstart.org");
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnISettings_Click(object sender, EventArgs e)
        {
            metroShell1_SettingsButtonClick(sender, e);
        }

        /// <summary>
        /// JJstart.org
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIJJStartOrg_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://jjstart.org");
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// 控制显现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuBar4_PopupOpen(object sender, PopupOpenEventArgs e)
        {
            //如果没有选中，则取消
            if(m_item == null)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIMSBOpen_Click(object sender, EventArgs e)
        {
            MetroStatusBarItem_Click(m_item, e);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIMSBRemove_Click(object sender, EventArgs e)
        {
            if (this.m_item != null)
            {
                if (this.m_item.Parent != null)
                {
                    //从面板中移除
                    this.m_item.Parent.SubItems.Remove(m_item);

                    //从配置中移除
                    Global.Settings.MainPanelItems.Remove(m_item.Tag as ItemSubInfo);

                    //加个这玩意就不会出问题了
                    this.metroStatusBar1.InvalidateLayout();
                }
            }
        }

        /// <summary>
        /// 主要是为了实现拖放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroStatusBar1_DragDrop(object sender, DragEventArgs e)
        {
            ButtonItemEx item = e.Data.GetData(typeof(ButtonItemEx)) as ButtonItemEx;

            if (item == null)
                return;

            //添加
            MetroStatusBarAddItem(item.Tag as ItemSubInfo, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroStatusBar1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(ButtonItemEx)) == null)
                e.Effect = DragDropEffects.None;
            else
                e.Effect = DragDropEffects.Copy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroStatusBar1_MouseDown(object sender, MouseEventArgs e)
        {
            ButtonItemEx item = sender as ButtonItemEx;

            //判断一下
            if (item != null)
                m_item = item;
            else
                m_item = null;
        }
    }
}