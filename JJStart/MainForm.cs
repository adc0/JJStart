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
        //ȫ�ֵģ����ڼ�¼��ǰ����ƶ�����ButtonItem
        private ButtonItemEx m_item = null;
        private Language m_lang = new Language();

        private ButtonItem btnIRun = new ButtonItem("btnIRun", "����");
        private ButtonItem btnISearch = new ButtonItem("btnISearch", "����");
        private ButtonItem btnIBaidu = new ButtonItem("btnIBaidu", "�ٶ�");
        private ButtonItem btnIGoogle = new ButtonItem("btnIGoogle", "�ȸ�");

        /// <summary>
        /// ����ļ��غ�����
        /// </summary>
        #region Plugin
        List<IPlugin> plugins = new List<IPlugin>();
        List<PluginInfoAttribute> piProperties = new List<PluginInfoAttribute>();
        ImageList pluginImageList = new ImageList();

        /// <summary>
        /// �ж�dll�Ƿ���ϲ���淶
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
        /// ��ȡ���
        /// </summary>
        /// <returns></returns>
        private List<string> GetPluginFiles(string PluginsPath)
        {
            List<string> plugins = new List<string>();

            //�ж�����û��Plugins�ļ��д���
            if (!Directory.Exists(PluginsPath))
                return plugins;

            //��ȡ���Ŀ¼�µ������ļ�����Ŀ¼
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
        /// ���в��
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

            //�ж��Ƿ���Ҫ����ԱȨ��
            if(p.IsElevationRequired)
                process.StartByRunas(Application.ExecutablePath, "-loadplugin " + attr.FullName);
            else
                p.OnLoad();
        }

        /// <summary>
        /// �ӡ�Plugins��Ŀ¼�¼��ز��
        /// </summary>
        private void LoadPlugins()
        {
            try
            {
                int i = 0;

                //����
                PluginInfoAttribute typeAttribute = new PluginInfoAttribute();

                //����list
                foreach(string file in GetPluginFiles(Global.PluginsPath))
                {
                    try
                    {
                        Assembly tmp = Assembly.LoadFile(file);
                        Type[] types = tmp.GetTypes();
                        bool ok = false;

                        //����types
                        foreach (Type t in types)
                        {
                            //����ж�
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

                //���ô�С�����
                pluginImageList.ColorDepth = ColorDepth.Depth32Bit;
                pluginImageList.ImageSize = new Size(32, 32);

                //����
                foreach (PluginInfoAttribute pia in piProperties)
                {
                    //��ȡ�ļ��Ĵ�Сͼ��
                    pluginImageList.Images.Add(((IPlugin)plugins[pia.Index]).GetIcon());
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// ˢ�²��
        /// </summary>
        private void ReloadPlugins()
        {
            //�������һЩ����
            plugins.Clear();
            piProperties.Clear();
            pluginImageList.Images.Clear();
            sideBar1.ExpandedPanel.SubItems.Clear();

            //����
            LoadPlugins();

            //��ʾ���
            ShowPlugins();
        }

        /// <summary>
        /// ��ʾ���
        /// </summary>
        private void ShowPlugins()
        {
            for (int i = 0; i < piProperties.Count; i++)
            {
                PluginInfoAttribute pia = piProperties[i];

                //ʵ����һ��button
                ButtonItemEx item = new ButtonItemEx();

                //����ItemType
                item.ItemType = ButtonItemEx.ButtonItemType.Plugin;

                //�ж���Ȩ��
                if (Global.IsAdminRole)
                {
                    //����ǹ���ԱȨ�ޣ�����ʾ
                    item.ShowUACIcon = false;
                }
                else
                {
                    //������ǹ���ԱȨ�ޣ��������ж��²���Ƿ���Ҫuac
                    if (((IPlugin)plugins[pia.Index]).IsElevationRequired)
                        item.ShowUACIcon = true;
                    else
                        item.ShowUACIcon = false;
                }

                //���÷��
                item.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;

                //����ͼ����
                item.ImagePosition = eImagePosition.Top;

                //����ͼƬ����
                item.ImageIndex = i;

                //����tooltip
                item.Tooltip = "Name:" + pia.Name + "\r\nDescription:" + pia.Description + "\r\nVersion:" + pia.Version + "\r\nAuthor:" + pia.Author + "\r\nWebsite:" + pia.Webpage;

                //�����¼�
                item.Click += new EventHandler(RunPlugin);

                //tag
                item.Tag = pia;

                //��ӵ�panels
                sideBar1.ExpandedPanel.SubItems.Add(item);

                //�����������Text
                //��Ϊʵ�����󣬻�û�и��ؼ�֮˵
                item.Text = pia.Name;
            }

            sideBar1.ExpandedPanel.Refresh();
        }

        #endregion

        /// <summary>
        /// ���캯��
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            //ʵ����Щ����
            Global.SettingsManager = new SettingsManager();

            //����������Ϣ
            Global.LoadSettings(this);

            //����ȫ�ֱ���
            Global.Init(this.splitContainer1);

            //����¼�
            btnIRun.Click += btnIRun_Click;
            btnISearch.Click += btnISearch_Click;
            btnIBaidu.Click += btnIBaidu_Click;
            btnIGoogle.Click += btnIGoogle_Click;

            //�Ѱ�ť��ӵ�textBoxDropDown1
            textBoxDropDown1.DropDownItems.AddRange(new BaseItem[] { btnIRun, btnISearch, btnIBaidu, btnIGoogle });

            //��������
            m_lang.SetLanguage(this);

            //��ʼ��
            this.Init();       

            //�ж��Ƿ�������ز��
            if (Global.Settings.LoadPlugin)
            {
                //������ô���������ȴ���tab
                metroShell1.CreateTab("Plugins", "Plugins", metroShell1.Items.Count - 1);

                //���ز����Ϣ
                LoadPlugins();
            }

            //����ȼ�����
            Global.Settings.WinHotKey.Handle = this.Handle;

            //ע�������ȼ�
            Global.Settings.WinHotKey.ReloadAll();

            //����ͼ����Ϣ
            this.Icon = Properties.Resources.JJStart;
            this.notifyIcon1.Icon = Properties.Resources.JJStart;
            this.notifyIcon1.Text = "JJStart " + Global.Version;
        }

        /// <summary>
        /// ��дOnLoad
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            //��ӽ�����ѡ��
            string[] styles = Enum.GetNames(typeof(eStyle));

            //�������
            foreach (string name in styles)
            {
                ButtonItem bi = new ButtonItem(name, name);

                //��ӵ�Ƥ����ť
                this.btnSkin.SubItems.Add(bi);

                //����¼�
                bi.Click += new EventHandler(_item_Click);
            }

            //���Custom���ѡ��
            ColorPickerDropDown colorPickerCustomScheme = new ColorPickerDropDown("ColorPickerCustomScheme", "Custom");
            colorPickerCustomScheme.DisplayMoreColors = false;
            colorPickerCustomScheme.BeginGroup = true;
            this.btnSkin.SubItems.Add(colorPickerCustomScheme);

            //�¼�
            colorPickerCustomScheme.ColorPreview += new ColorPreviewEventHandler(colorPickerCustomScheme_ColorPreview);
            colorPickerCustomScheme.SelectedColorChanged += new EventHandler(colorPickerCustomScheme_SelectedColorChanged);

            ////���BackColor Custom���ѡ��
            //ColorPickerDropDown backColorPickerCustomScheme = new ColorPickerDropDown("BackColorPickerCustomScheme", "BackColor Custom");
            //this.btnSkin.SubItems.Add(backColorPickerCustomScheme);

            ////�¼�
            //backColorPickerCustomScheme.ColorPreview += new ColorPreviewEventHandler(colorPickerCustomScheme_ColorPreview);
            //backColorPickerCustomScheme.SelectedColorChanged += new EventHandler(colorPickerCustomScheme_SelectedColorChanged);

            //��������MainForm_Load
            base.OnLoad(e);
        }

        /// <summary>
        /// ��д��Ϣѭ��
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
        /// ��ʼ��
        /// </summary>
        private void Init()
        {
            try
            {          
                //ɾ����
                //û��������Ϣ�Ļ���������Ĭ�����һ����Ĭ�ϡ���Ŀ
                if (Global.Settings.FirstRun)
                {
                    Global.Settings.FirstRun = false;
                    Global.Settings.Category.AddCategory(new Item(m_lang.GetString("1", "Ĭ��")));
                }

                //���ش�����Ϣ
                this.Size = Global.Settings.Size;

                //�����ж���location
                if (Global.Settings.Location.X == 0 && Global.Settings.Location.Y == 0)
                    this.Location = new Point(52, 52);
                else
                    this.Location = Global.Settings.Location;

                //��������
                this.styleManager1.ManagerStyle = Global.Settings.ManagerStyle;
                this.styleManager1.ManagerColorTint = Global.Settings.ManagerColorTint;

                //�ж�������
                switch (Global.Settings.ButtonCheckedIndex)
                {
                    case 0:
                        btnIRun.Checked = true;
                        //����AutoComplete��ģʽ��Source
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

                //�Ƴ�Ĭ�ϵ�
                metroShell1.Items.RemoveAt(0);

                //����category����
                foreach (string strName in Global.Settings.Category.Items.Keys)
                {
                    //����tab
                    MetroTabItem mti = metroShell1.CreateTab(strName, strName, metroShell1.Items.Count - 1);

                    //�����¼�
                    mti.TextChanged += MetroTabItem_TextChanged;
                }

                //ѡ�е�һ��
                metroShell1.SelectedTab = metroShell1.Items[0] as MetroTabItem;

                //��MetroStatusBar���г�ʼ��
                foreach(ItemSubInfo info in Global.Settings.MainPanelItems)
                {
                    MetroStatusBarAddItem(info, false);
                }
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// �ı��¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetroTabItem_TextChanged(object sender, EventArgs e)
        {
            MetroTabItem mti = sender as MetroTabItem;

            //���ںÿ�
            Dictionary<string, Item> dic = Global.Settings.Category.Items;

            //����
            //��ӵ�category�е�itemlist��
            if (dic.ContainsKey(mti.Name))
            {
                //û���ڵĻ������½�һ���µ�
                Item item = new Item(mti.Text);

                //�õ�ԭʼ��FileList
                item.Items = dic[mti.Name].Items;

                //�Ƴ�
                dic.Remove(mti.Name);

                //��ӵ��µ�
                dic.Add(mti.Text, item);

                //�ı�����
                mti.Name = mti.Text;
            }
        }

        /// <summary>
        /// �½�����
        /// </summary>
        /// <param name="strName"></param>
        private void AddCate(string strName)
        {
            try
            {
                //������˵������tab-"Plugins"
                if (strName.Trim().ToString() == "plugins")
                {
                    MessageBoxEx.Show(this, m_lang.GetString("2", "����������ѱ������ڲ�ʹ�ã��û������½���"));
                    return;
                }

                //�ж����Ƿ��Ѿ�����
                if (Global.Settings.Category.Items.ContainsKey(strName))
                {
                    MessageBoxEx.Show(this, m_lang.GetString("3", "�������Ѿ����ڣ����������룡"));
                    return;
                }

                //ʵ����һ��item
                Item item = new Item(strName);

                //����category
                Global.Settings.Category.AddCategory(item);

                //����tab
                //�ж��Ƿ����˲��
                if (Global.Settings.LoadPlugin)
                {
                    //MetroTabItem
                    MetroTabItem mti = metroShell1.CreateTab(strName, strName, metroShell1.Items.Count - 2);

                    //�����¼�
                    mti.TextChanged += MetroTabItem_TextChanged;

                    //ѡ��
                    metroShell1.SelectedTab = mti;
                }
                else
                {
                    //MetroTabItem
                    MetroTabItem mti = metroShell1.CreateTab(strName, strName, metroShell1.Items.Count - 1);

                    //�����¼�
                    mti.TextChanged += MetroTabItem_TextChanged;

                    //ѡ��
                    metroShell1.SelectedTab = mti;
                }
                
                //ˢ�¿ؼ�
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

            //���
            item.Tag = info;
            item.Tooltip = info.Name;
            item.Icon = file.GetFileIconEx(info.FullName, true);

            //�¼�
            item.Click += MetroStatusBarItem_Click;

            //���
            metroStatusBar1.Items.Add(item);

            //��ӵ�������
            if (add) Global.Settings.MainPanelItems.Add(info);
        }

        /// <summary>
        /// SideBar������Panels
        /// </summary>
        /// <param name="strName"></param>
        private void SideBarAddPanel(string strName)
        {
            //�����ؼ�
            SideBarPanelItem sbpi = new SideBarPanelItem(strName, strName);

            //�������ԣ��Ӷ�Ӧ��Item�л�ȡ
            switch (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[strName].ViewMode)
            {
                case ItemSub.ItemViewMode.LargeIcon:
                    sbpi.ItemImageSize = eBarImageSize.Medium;             
                    break;
                case ItemSub.ItemViewMode.SmallIcon:
                    sbpi.ItemImageSize = eBarImageSize.Default;   
                    break;
            }

            //�����Ƿ����
            if (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[strName].MultiColumn)
            {
                sbpi.LayoutType = eSideBarLayoutType.MultiColumn;
            }

            //���
            sideBar1.Panels.Add(sbpi);

            //����expand����
            //ò�Ʊ���Ҫ��Panels.Add��Ȼ�����Expand
            if (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[strName].Expanded)
            {
                //չ���¼������Ŀ
                sbpi.Expanded = true;
            }
        }

        /// <summary>
        /// ��Panels�����BaseItem
        /// </summary>
        /// <param name="strName"></param>
        private void SideBarPanelAddItem(string strName)
        {
            //���ж���FileInfoList�Ƿ��Ѿ�ʵ����
            if (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[strName].SubItems == null)
                return;
     
            //�����һ���б�
            sideBar1.Panels[strName].SubItems.Clear();

            //���һ��ͼ��
            imageSmall.Images.Clear();
            imageMedium.Images.Clear();

            //����FileList
            foreach (ItemSubInfo f in Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[strName].SubItems)
            {
                //����ļ���Ϣ
                AddItemSub(strName, f);
            }
        }

        /// <summary>
        /// �½���Ŀ����
        /// </summary>
        /// <param name="strName"></param>
        private void AddItem(string strName)
        {
            try
            {
                //������˵������panel-"�������"
                if (strName.Trim() == m_lang.GetString("4", "�������"))
                {
                    MessageBoxEx.Show(this, m_lang.GetString("5", "����Ŀ�����ѱ������ڲ�ʹ�ã��û������½���"));
                    return;
                }

                //�ж����Ƿ��Ѿ�����
                if (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items.ContainsKey(strName))
                {
                    MessageBoxEx.Show(this, m_lang.GetString("6", "����Ŀ�Ѿ����ڣ����������룡"));
                    return;
                }

                //ʵ����
                ItemSub list = new ItemSub(strName);

                //��ӵ�category�е�itemlist��
                Global.Settings.Category.Items[metroShell1.SelectedTab.Name].AddItem(list);

                //���ӵ�SideBar
                SideBarAddPanel(strName);
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// ���Item Sub
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="file"></param>
        private void AddItemSub(string strName, ItemSubInfo file)
        {
            try
            {
                //ʵ����һ��button
                ButtonItemEx item = new ButtonItemEx();

                //���÷��
                item.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;

                //·��ת��һ��
                string strPath = path.GetAbsolutePath(Application.ExecutablePath, file.FullName);

                //��ȡ�ļ��Ĵ�Сͼ��
                imageSmall.Images.Add(JJStart.Lib.file.GetFileIconEx(strPath, true));
                imageMedium.Images.Add(JJStart.Lib.file.GetFileIconEx(strPath, false));

                //����ͼƬ����
                item.ImageIndex = imageSmall.Images.Count - 1;
                
                //��ʾuac
                item.ShowUACIcon = !Global.IsAdminRole;
                item.FullName = file.FullName;

                //����tooltip
                //�ж����ȼ�
                if (file.HotKey == null)
                    item.Tooltip = file.Name + "\r\n" + strPath;
                else
                    item.Tooltip = file.Name + "(" + file.HotKey.ToString() + ")" + "\r\n" + strPath;
                
                //�ж���
                if ((sideBar1.Panels[strName] as SideBarPanelItem).ItemImageSize == eBarImageSize.Default)
                    item.ImagePosition = eImagePosition.Left;
                else
                    item.ImagePosition = eImagePosition.Top;

                //����tagΪ�ļ��ľ���·��
                item.Tag = file;

                //����¼�
                item.Click += this.buttonItem1_Click;
                item.DoubleClick += this.buttonItem1_DoubleClick;

                //��ӵ�ָ����panel
                sideBar1.Panels[strName].SubItems.Add(item);

                //�����������Text
                //��Ϊʵ�����󣬻�û�и��ؼ�֮˵
                item.Text = file.Name;
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// AddItemSub���ٴη�װ
        /// </summary>
        /// <param name="strFilePath"></param>
        private void AddItemSubEx(string strFilePath)
        {
            string strFile = strFilePath;
            string strName = strFilePath;
            string strLinkPath = strFilePath;

            //���ж��¸�ʽ
            if (Path.GetExtension(strFile).ToLower() == ".lnk")
            {
                //link
                vbAccelerator.Components.Shell.ShellLink sl = new vbAccelerator.Components.Shell.ShellLink(strFile);

                //��ȡlink��ָ���exe
                strFile = sl.Target;
            }

            //���Ŀ���ļ���·�������ǵĳ����Ŀ¼�£����ȡ���·��
            if(strFile.Contains(Application.StartupPath))
            {
                //���Ի�ȡ���·��
                strFile = path.GetRelativePath(Application.ExecutablePath, strFile);
            }

            //�ж�һ��
            string name = Path.GetFileNameWithoutExtension(strName);

            //���
            JJStart.ItemSubInfo file = new JJStart.ItemSubInfo((name == "" ? strName : name), strFile);

            //�����ж����Ƿ��Ѿ�����Ŀ������
            if (sideBar1.Panels.Count == 0)
            {
                //���û��������Զ��½�����Ŀ�����ƾ��ǡ��½���Ŀһ��
                AddItem(m_lang.GetString("7", "�½���Ŀһ"));
            }

            //���뵽cate��
            Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].AddSubItem(file, Global.Settings.WinHotKey);

            //����ļ���Ϣ
            AddItemSub(sideBar1.ExpandedPanel.Name, file);
      
            //���ж��¸�ʽ
            if (Path.GetExtension(strLinkPath).ToLower() == ".lnk")
            {
                //�ж���ģʽ
                if (Global.Settings.LinkDragRemove)
                {
                    //���Ϊture����ɾ��link�ļ�
                    File.Delete(strLinkPath);
                }
            }
        }

        /// <summary>
        /// ��ť�����¼�����Ҫ��ʶ��Ctrl
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem1_Click(object sender, EventArgs e)
        {
            try
            {
                //�ж���tag�Ƿ�ΪNULL
                if (((ButtonItem)sender) == null)
                    return;

                //����ת��
                ButtonItem bi = (ButtonItem)sender;

                //���������Ctrl����������ѡ״̬
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

                    //û�н����ѡ״̬�����ж�����ģʽ
                    if(!Global.Settings.DoubleClickRun)
                    {
                        //�����ȡTag�е�����
                        string strPath = ((ItemSubInfo)((ButtonItem)sender).Tag).FullName;

                        //process.Start
                        process.Start(strPath);
                    }
                }
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// ��ť�����¼������г���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //û�н����ѡ״̬�����ж�����ģʽ
                if (Global.Settings.DoubleClickRun)
                {
                    //�ж���tag�Ƿ�ΪNULL
                    if (((ButtonItem)sender).Tag == null)
                        return;

                    //�����ȡTag�е�����
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
                //�����ȡTag�е�����
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

            //����Color
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
        /// Ƥ����ť�¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _item_Click(object sender, EventArgs e)
        {
            ButtonItem item = sender as ButtonItem;
            this.styleManager1.ManagerStyle = (eStyle)Enum.Parse(typeof(eStyle), item.Name);

            //����Style
            Global.Settings.ManagerStyle = this.styleManager1.ManagerStyle;
        }

        /// <summary>
        /// �������¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            //����ȫѡ
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                //���˵�Plugins
                if (metroShell1.SelectedTab.Name == "Plugins")
                    return;

                //�ж��Ƿ����ExpandedPanel
                if (sideBar1.ExpandedPanel != null)
                {
                    //������ڣ���ȫѡSubItems
                    foreach (ButtonItem bi in sideBar1.ExpandedPanel.SubItems)
                    {
                        if (!bi.Checked) bi.Checked = true;
                    }
                }
            }

            //����
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
            {
                //���л�
                if (Global.SettingsManager.Save(Global.Settings))
                    notifyIcon1.BalloonTipText = m_lang.GetString("8", "������Ϣ����ɹ���");
                else
                    notifyIcon1.BalloonTipText = m_lang.GetString("9", "������Ϣ����ʧ�ܡ�");

                //Show Tip
                notifyIcon1.ShowBalloonTip(1000);
            }

            //�ж���
            if(textBoxDropDown1.Text.Length != 0)
            {
                //���ݼ����¼�
                textBoxDropDown1_KeyDown(sender, e);
            }
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //��Ҫ��֪ͨmetroshell���ItemSub
            metroShell1_SelectedTabChanged(sender, e);
        }

        /// <summary>
        /// ��̬����λ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_LocationChanged(object sender, EventArgs e)
        {
            if(Global.Settings != null)
                Global.Settings.Location = this.Location;
        }

        /// <summary>
        /// ��̬�����С
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //�ж�һ���Ƿ�Ϊnull
            if (Global.Settings != null)
                Global.Settings.Size = this.Size;
        }

        /// <summary>
        /// ��������
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
        /// ��tab�ı�ʱ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroShell1_SelectedTabChanged(object sender, EventArgs e)
        {
            try
            {
                if (metroShell1.SelectedTab.Panel != null)
                {
                    //�������Panels
                    sideBar1.Panels.Clear();

                    //����subitem
                    metroShell1.SelectedTab.SubItems.Clear();

                    //SuspendLayout
                    metroShell1.SelectedTab.Panel.SuspendLayout();

                    //��������һЩ��������������״̬���Ű����ϸС����
                    //metroShell1.SelectedTab.Panel.AutoScroll = true;
                    //metroShell1.SelectedTab.Panel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
                    //metroShell1.SelectedTab.Panel.Dock = System.Windows.Forms.DockStyle.Fill;
                    //metroShell1.SelectedTab.Panel.Location = new System.Drawing.Point(0, 51);
                    //metroShell1.SelectedTab.Panel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
                    //metroShell1.SelectedTab.Panel.Padding = new System.Windows.Forms.Padding(2, 0, 2, 3);

                    //��splitContainer1��ӵ�panel           
                    metroShell1.SelectedTab.Panel.Controls.Add(splitContainer1);
                    
                    //���˵�һ��plugins
                    if (metroShell1.SelectedTab.Name.ToLower() == "plugins")
                    {
                        //���ò�����˫������text
                        sideBar1.EnableDoubleClickChangeText = false;

                        //����һ��ͼ��
                        sideBar1.Images = pluginImageList;
                        sideBar1.ImagesMedium = pluginImageList;

                        //�����ؼ�
                        SideBarPanelItem sbpi = new SideBarPanelItem(m_lang.GetString("10", "ȫ��"), m_lang.GetString("10", "ȫ��"));

                        //�������ԣ��Ӷ�Ӧ��Item�л�ȡ
                        sbpi.ItemImageSize = eBarImageSize.Medium;

                        //�����Ƿ����
                        sbpi.LayoutType = eSideBarLayoutType.MultiColumn;

                        //���
                        sideBar1.Panels.Add(sbpi);

                        //��ʾ����б�
                        ShowPlugins();

                        //������Ҫ�ǻָ��˵���Ĭ��
                        this.contextMenuBar1.SetContextMenuEx(this.sideBar1, null);
                        this.contextMenuBar2.SetContextMenuEx(this.sideBar1, null);
                        this.contextMenuBar3.SetContextMenuEx(this.sideBar1, this.buttonItem3);
                    }
                    else
                    {
                        //��������˫������text
                        sideBar1.EnableDoubleClickChangeText = true;

                        //����һ��ͼ��
                        sideBar1.Images = imageSmall;
                        sideBar1.ImagesMedium = imageMedium;

                        //Ȼ���������Ϣ�м���
                        //�����ж�ItemList���Ƿ����
                        if (!Global.Settings.Category.Items.ContainsKey(metroShell1.SelectedTab.Name))               
                            return;

                        //������һ��category�е�item
                        foreach (string strName in Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items.Keys)
                        {
                            //��ӵ�SideBar��
                            SideBarAddPanel(strName);
                        }

                        //������Ҫ�ǻָ��˵���Ĭ��                
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
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroShell1_SettingsButtonClick(object sender, EventArgs e)
        {
            SettingsForm f = new SettingsForm(this);
            f.ShowDialog();
        }

        /// <summary>
        /// �����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIAddCate2_Click(object sender, EventArgs e)
        {
            int index = 1;
            string text = m_lang.GetString("11", "�����");

            //�����ж�һ��index�Ƿ����
            string strName = text + index.ToString();

            //����һ�£����Ƿ����
            while (metroShell1.Items.Contains(strName))
            {
                strName = text + (index++).ToString();
            }
            
            //���
            AddCate(strName);
        }

        /// <summary>
        /// ����
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

                //����AutoComplete��ģʽ��Source
                this.textBoxDropDown1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                this.textBoxDropDown1.AutoCompleteSource = AutoCompleteSource.FileSystem;
            }
        }

        /// <summary>
        /// ����
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

                //����AutoComplete��ģʽ��Source
                this.textBoxDropDown1.AutoCompleteMode = AutoCompleteMode.None;
                this.textBoxDropDown1.AutoCompleteSource = AutoCompleteSource.None;
            }
        }

        /// <summary>
        /// �ٶ�
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

                //����AutoComplete��ģʽ��Source
                this.textBoxDropDown1.AutoCompleteMode = AutoCompleteMode.None;
                this.textBoxDropDown1.AutoCompleteSource = AutoCompleteSource.None;
            }
        }

        /// <summary>
        /// �ȸ�
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

                //����AutoComplete��ģʽ��Source
                this.textBoxDropDown1.AutoCompleteMode = AutoCompleteMode.None;
                this.textBoxDropDown1.AutoCompleteSource = AutoCompleteSource.None;
            }
        }

        /// <summary>
        /// ���ε�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxDropDown1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    //�жϹ�ѡ���ĸ�
                    if (btnIRun.Checked)
                    {
                        process.Start(textBoxDropDown1.Text.Trim());
                        return;
                    }

                    //�ٶ�
                    if (btnIBaidu.Checked)
                    {
                        Process.Start("http://www.baidu.com/s?wd=" + textBoxDropDown1.Text.Trim());
                        return;
                    }

                    //�ȸ�
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
        /// ��Ҫ���ڡ�������ģʽ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxDropDown1_TextChanged(object sender, EventArgs e)
        {
            //����
            if (btnISearch.Checked)
            {
                //��Ӧ֮ǰ�����ж��ı����Ƿ�����
                if (textBoxDropDown1.Text.Length == 0)
                {
                    //�����ڴ��ж��¡�������ģʽ�����Ϊ0�ˣ���ָ�Panel
                    if (btnISearch.Checked)
                    {
                        //�жϵ�ǰpanel�Ƿ��ǡ����������panel
                        if (sideBar1.ExpandedPanel.Name == m_lang.GetString("4","�������"))
                        {
                            //���
                            sideBar1.Panels.Clear();

                            //����ģʽ
                            metroShell1_SelectedTabChanged(sender, e);

                            //ˢ�¿ؼ�
                            if (sideBar1.ExpandedPanel != null)
                                sideBar1.ExpandedPanel.Refresh();
                            else
                                sideBar1.Refresh();                     
                        }
                    }
                }
                else
                {
                    //����
                    string strResultName = m_lang.GetString("4", "�������");

                    //"�������"panel�ķ�������롰��ǰexpanded����panel��񱣳�һ��
                    SideBarPanelItem sbpi = new SideBarPanelItem(strResultName, strResultName);

                    //��������
                    if (sideBar1.ExpandedPanel != null)
                    {
                        sbpi.ItemImageSize = sideBar1.ExpandedPanel.ItemImageSize;
                        sbpi.LayoutType = sideBar1.ExpandedPanel.LayoutType;
                    }
                    
                    //�½�֮ǰ����clear��
                    sideBar1.Panels.Clear();

                    //�½�һ�����Item
                    sideBar1.Panels.Add(sbpi);

                    //����catogory�е�Item�е�List���������Ԫ��
                    //����Item
                    foreach (string strCateName in Global.Settings.Category.Items.Keys)
                    {
                        //������category�е�item
                        foreach (string strItemName in Global.Settings.Category.Items[strCateName].Items.Keys)
                        {
                            //����FileList
                            foreach (ItemSubInfo f in Global.Settings.Category.Items[strCateName].Items[strItemName].SubItems)
                            {
                                //�����޸��£�ֻƥ���ļ�������������չ����
                                if (f.Name.ToLower().Contains(textBoxDropDown1.Text.Trim()))
                                {
                                    AddItemSub(strResultName, f);
                                } 
                            }
                        }
                    }

                    //ˢ�¿ؼ�
                    sideBar1.ExpandedPanel.Refresh();
                }
            }
        }

        /// <summary>
        /// �Ͻ�
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
        /// �Ϸ�
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
                        //ȡ��ѡ��״̬
                        m_item.Checked = false;

                        string fullName = (m_item.Tag as ItemSubInfo).FullName;
                        string[] path = (string[])(e.Data.GetData(DataFormats.FileDrop));

                        //�����Ŀ¼�Ļ������ƽ�ȥ
                        if(Directory.Exists(fullName))
                        {
                            //�����ٴ��ж�ģʽ��������ƶ��Ļ�����move�ļ�
                            //�����ϷŽ������ļ�
                            for (int i = 0; i < path.Length; i++)
                            {
                                //������Ϸŵ�Ŀ¼������Ŀ¼
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
                            //�������̺ʹ������
                            process.Start(fullName, path[0]);
                        }
                    }
                    else
                    {
                        string[] path = (string[])(e.Data.GetData(DataFormats.FileDrop));

                        //�����ϷŽ������ļ�
                        for (int i = 0; i < path.Length; i++)
                        {
                            AddItemSubEx(path[i]);
                        }

                        //ˢ�¿ؼ�
                        sideBar1.ExpandedPanel.Refresh();
                    }
                }
                else
                {
                    //���ж����Ƿ����ڲ��Ϸ�
                    if (e.Data.GetData(typeof(ButtonItemEx)) == null)
                        return;

                    //���
                    bool bSame = true;

                    //���ȱȽ��£��Ƿ��иĶ�
                    for (int i = 0; i < sideBar1.ExpandedPanel.SubItems.Count; i++)
                    {
                        //��ȡTag����ת��
                        ItemSubInfo file = sideBar1.ExpandedPanel.SubItems[i].Tag as ItemSubInfo;

                        //�Ƚ�
                        if(Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].SubItems[i] != file)
                        {
                            //����ȵĻ�������Ϊfalse��������ѭ��
                            bSame = false;
                            break;
                        } 
                    }

                    //�ж���bSame���������ͬ�Ļ���������
                    if (bSame)
                    {
                        //�������ͬ�Ļ��򷵻أ�������֧�����沿��
                        return;
                    }

                    //����б������е�
                    Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].SubItems.Clear();

                    //����SubItems�е�ButtonItem
                    for (int i = 0; i < sideBar1.ExpandedPanel.SubItems.Count; i++)
                    {
                        //��ȡTag����ת��
                        ItemSubInfo file = sideBar1.ExpandedPanel.SubItems[i].Tag as ItemSubInfo;

                        //���¼���
                        Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].SubItems.Add(file);
                    }
                }
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); } 
        }

        /// <summary>
        /// ��Ҫʵ��ѡ�е�Ч��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sideBar1_DragOver(object sender, DragEventArgs e)
        {
            //����ȡ��Checked
            if (m_item != null && m_item.Checked)
            {
                m_item.Checked = false;
            }

            //�ж�һ��
            if (sideBar1.ExpandedPanel != null)
            {
                //��¼��ǰѡ�е�item
                m_item = sideBar1.ExpandedPanel.ItemAtLocation(sideBar1.PointToClient(MousePosition).X, sideBar1.PointToClient(MousePosition).Y) as ButtonItemEx;
            }

            //ѡ��
            if (m_item != null && !m_item.Checked)
            {
                m_item.Checked = true;
            }
        }

        /// <summary>
        /// ��¼���ĸ�itemչ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sideBar1_ExpandedChange(object sender, EventArgs e)
        {
            try
            {
                //ת��������
                SideBarPanelItem sbpi = (SideBarPanelItem)sender;

                //ǰ����metroShell1.SelectedTab���ڣ���Ϊ���ֳ����˳�ʱ��������Ϊnull
                if (metroShell1.SelectedTab != null)
                {
                    //�ж��ĸ�չ��
                    //��Ϊ�ı��ܻ���������һ���رգ�һ��չ��
                    if (sbpi.Expanded)
                    {
                        //��չ����item�����Ƽ�¼��������Ϣ��
                        Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sbpi.Name].Expanded = true;

                        //����BaseItem��Panel��
                        SideBarPanelAddItem(sbpi.Name);
                    }
                    else
                    {
                        //��չ����item�����Ƽ�¼��������Ϣ��
                        Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sbpi.Name].Expanded = false;
                    }
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// ���Text�ı䣬�����������Ϣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sideBar1_ItemTextChanged(object sender, EventArgs e)
        {
            //�����ж�һ����˭��text�ı�
            if (sideBar1.SideBarPanelItemTextChange)
            {
                SideBarPanelItem sbpl = sender as SideBarPanelItem;

                //���ںÿ�
                Dictionary<string, ItemSub> dic = Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items;

                //����
                //��ӵ�category�е�itemlist��
                if (dic.ContainsKey(sbpl.Name))
                {
                    //û���ڵĻ������½�һ���µ�
                    ItemSub file = new ItemSub(sbpl.Text);

                    //�õ�ԭʼ��FileList
                    file.SubItems = dic[sbpl.Name].SubItems;

                    //���������һЩ��Ϣ
                    file.Expanded = dic[sbpl.Name].Expanded;
                    file.MultiColumn = dic[sbpl.Name].MultiColumn;
                    file.ViewMode = dic[sbpl.Name].ViewMode;

                    //�Ƴ�
                    dic.Remove(sbpl.Name);

                    //��ӵ��µ�
                    dic.Add(sbpl.Text, file);

                    //�ı�����
                    sbpl.Name = sbpl.Text;
                }
            }
        }

        /// <summary>
        /// �����Ҽ��˵�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sideBar1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //�ж��ǲ���plugins
                if (metroShell1.SelectedTab.Name.ToLower() == "plugins")
                {
                    this.contextMenuBar1.SetContextMenuEx(this.sideBar1, null);
                    this.contextMenuBar2.SetContextMenuEx(this.sideBar1, null);
                    return;
                }

                //��¼��ǰѡ�е�item
                m_item = sideBar1.ExpandedPanel.ItemAtLocation(sideBar1.PointToClient(MousePosition).X, sideBar1.PointToClient(MousePosition).Y) as ButtonItemEx;

                //����������Ҽ��˵��Ĳ�ͬ����
                if (m_item != null)
                {
                    //�����ﻹҪ�ж����Ƿ����ˡ�Shift��
                    if ((win32.GetKeyState(0x10) & 0x8000) != 0)
                    {
                        //��ȡ��
                        this.contextMenuBar1.SetContextMenuEx(this.sideBar1, null);
                        this.contextMenuBar2.SetContextMenuEx(this.sideBar1, null);

                        //��ʾ��Դ�������Ҽ��˵�
                        ShellContextMenu scm = new ShellContextMenu();
                        List<FileInfo> files = new List<FileInfo>();

                        //���Ŀ��·��
                        files.Add(new FileInfo((m_item.Tag as ItemSubInfo).FullName));

                        //��ʾ�˵�
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
                    //���ѡ����ButtonItem�Ļ�����ȡ��ѡ��
                    foreach (ButtonItem bi in sideBar1.ExpandedPanel.SubItems)
                    {
                        //�ж��Ƿ�checked
                        if (bi.Checked) 
                            bi.Checked = false;
                    }

                    //��ȡ��
                    this.contextMenuBar2.SetContextMenuEx(this.sideBar1, null);
                    this.contextMenuBar1.SetContextMenuEx(this.sideBar1, this.buttonItem1);
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// ����ƶ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sideBar1_MouseMove(object sender, MouseEventArgs e)
        {
            //��¼��ǰѡ�е�item
            m_item = sideBar1.ExpandedPanel.ItemAtLocation(sideBar1.PointToClient(MousePosition).X, sideBar1.PointToClient(MousePosition).Y) as ButtonItemEx;
        }

        /// <summary>
        /// ˫����
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
        /// �����Ҽ��˵�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem1_PopupOpen(object sender, PopupOpenEventArgs e)
        {
            try
            {
                //�ж��ǲ���plugins
                if (metroShell1.SelectedTab.Name.ToLower() == "plugins")
                {
                    this.contextMenuBar1.SetContextMenuEx(this.sideBar1, null);
                    this.contextMenuBar2.SetContextMenuEx(this.sideBar1, null);
                    return;
                }

                //�ж���ExpandedPanel�Ƿ����
                if (sideBar1.ExpandedPanel == null)
                {
                    //���sideBar1.ExpandedPanelΪnull��˵���Ǹս���������Ĭ�ϵ����
                    //������ز˵�ѡ��
                    btnIView.Visible = false;
                    btnIClearItem.Visible = false;
                    btnIDeleteItem.Visible = false;
                    btnIDeleteCate.BeginGroup = true;
                }
                else
                {
                    //�ָ�true
                    btnIView.Visible = true;
                    btnIClearItem.Visible = true;
                    btnIDeleteItem.Visible = true;
                    btnIDeleteCate.BeginGroup = false;

                    //�ж��²˵�
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

                    //�ж����Ƿ��������
                    if (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].MultiColumn)
                        btnIMultiColumn.Checked = true;
                    else
                        btnIMultiColumn.Checked = false;
                }

                //�ж����Ƿ�����ճ��
                if (Clipboard.ContainsData("JStart_FileInfos"))
                    btnIPaste.Enabled = true;
                else
                    btnIPaste.Enabled = false;
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// button2�˵��Ŀ�����ʾ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItem2_PopupOpen(object sender, PopupOpenEventArgs e)
        {
            //���ﴦ������ʾ
            if (!(m_item as ButtonItem).Checked)
            {
                //����Items
                foreach (ButtonItem bi in sideBar1.ExpandedPanel.SubItems)
                {
                    //�ж��Ƿ�checked
                    if (bi.Checked)
                    {
                        bi.Checked = false;
                    }
                }
            }
        }

        /// <summary>
        /// ����ļ�
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
        /// ���Ŀ¼
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
        /// ��ӷ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnINewCate_Click(object sender, EventArgs e)
        {
            InputForm f = new InputForm();

            //������ʾ��Ϣ
            f.lblText.Text = m_lang.GetString("12", "��������������ƣ�");

            if (f.ShowDialog() == DialogResult.OK)
            {
                string strInput = f.txtInput.Text;
                AddCate(strInput);
            }
        }

        /// <summary>
        /// �½���Ŀ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnINewItem_Click(object sender, EventArgs e)
        {
            InputForm f = new InputForm();
            f.lblText.Text = m_lang.GetString("13", "����������Ŀ���ƣ�");

            if (f.ShowDialog() == DialogResult.OK)
            {
                string strInput = f.txtInput.Text;
                AddItem(strInput);
            }
        }

        /// <summary>
        /// ��ͼ��
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
                        //��ӵ�Item��
                        //����Ӹ��жϣ���ֹ�ڡ����������panel�г��ִ���
                        if (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].ExistsItem(sideBar1.ExpandedPanel.Name))
                            Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].ViewMode = ItemSub.ItemViewMode.LargeIcon;

                        //��������
                        sideBar1.ExpandedPanel.ItemImageSize = eBarImageSize.Medium;

                        //��ѡ��ȡ��
                        btnILargeIcons.Checked = true;
                        btnISmallIcons.Checked = false;

                        //�����
                        sideBar1.ExpandedPanel.SubItems.Clear();

                        //���¼�����
                        SideBarPanelAddItem(sideBar1.ExpandedPanel.Name);

                        //ˢ��
                        sideBar1.ExpandedPanel.Refresh();
                    }
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// Сͼ��
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
                        //��ӵ�Item��
                        //����Ӹ��жϣ���ֹ�ڡ����������panel�г��ִ���
                        if (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].ExistsItem(sideBar1.ExpandedPanel.Name))
                            Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].ViewMode = ItemSub.ItemViewMode.SmallIcon;

                        //��������
                        sideBar1.ExpandedPanel.ItemImageSize = eBarImageSize.Default;

                        //��ѡ��ȡ��
                        btnILargeIcons.Checked = false;
                        btnISmallIcons.Checked = true;

                        //�����
                        sideBar1.ExpandedPanel.SubItems.Clear();

                        //���¼�����
                        SideBarPanelAddItem(sideBar1.ExpandedPanel.Name);

                        //ˢ��
                        sideBar1.ExpandedPanel.Refresh();
                    }
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIMultiColumn_Click(object sender, EventArgs e)
        {
            if (btnIMultiColumn.Checked)
            {
                if (sideBar1.ExpandedPanel != null)
                {
                    //��ӵ�Item��
                    //����Ӹ��жϣ���ֹ�ڡ����������panel�г��ִ���
                    if (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].ExistsItem(sideBar1.ExpandedPanel.Name))
                        Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].MultiColumn = false;

                    //��������
                    sideBar1.ExpandedPanel.LayoutType = eSideBarLayoutType.Default;

                    //��������
                    btnIMultiColumn.Checked = false;

                    sideBar1.RecalcLayout();
                }
            }
            else
            {
                //����֮���
                if (sideBar1.ExpandedPanel != null)
                {
                    //��ӵ�Item��
                    //����Ӹ��жϣ���ֹ�ڡ����������panel�г��ִ���
                    if (Global.Settings.Category.Items[metroShell1.SelectedTab.Name].ExistsItem(sideBar1.ExpandedPanel.Name))
                        Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].MultiColumn = true;

                    //��������
                    sideBar1.ExpandedPanel.LayoutType = eSideBarLayoutType.MultiColumn;

                    //��������
                    btnIMultiColumn.Checked = true;

                    sideBar1.RecalcLayout();
                }
            }
        }

        /// <summary>
        /// ճ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIPaste_Click(object sender, EventArgs e)
        {
            try
            {
                //�ж��Ƿ��д˸�ʽ������
                if (Clipboard.ContainsData("JStart_FileInfo"))
                {
                    //�����ж����Ƿ��Ѿ�����Ŀ������
                    if (sideBar1.Panels.Count == 0)
                    {
                        //���û��������Զ��½�����Ŀ�����ƾ��ǡ��½���Ŀһ��
                        AddItem(m_lang.GetString("7", "�½���Ŀһ"));
                    }

                    //����ת��
                    var fi = (JJStart.ItemSubInfo)Clipboard.GetData("JStart_FileInfo");

                    //���뵽cate��
                    Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].AddSubItem(fi, Global.Settings.WinHotKey);

                    //���
                    AddItemSub(sideBar1.ExpandedPanel.Name, fi);

                    //ˢ��һ��
                    sideBar1.ExpandedPanel.Refresh();
                }
                else if (Clipboard.ContainsData("JStart_FileInfos"))
                {
                    //�����ж����Ƿ��Ѿ�����Ŀ������
                    if (sideBar1.Panels.Count == 0)
                    {
                        //���û��������Զ��½�����Ŀ�����ƾ��ǡ��½���Ŀһ��
                        AddItem(m_lang.GetString("7", "�½���Ŀһ"));
                    }

                    //����ת��
                    var lstFInfos = Clipboard.GetData("JStart_FileInfos") as List<ItemSubInfo>;

                    //ΪɶҪ����������Ҫ��Ϊ��˳��һ��
                    for (int i = lstFInfos.Count - 1; i >= 0; i--)
                    {
                        var fi = lstFInfos[i];

                        //���뵽cate��
                        Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].AddSubItem(fi, Global.Settings.WinHotKey);

                        //���
                        AddItemSub(sideBar1.ExpandedPanel.Name, fi);
                    }

                    //ˢ��һ��
                    sideBar1.ExpandedPanel.Refresh();
                }
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// ��ձ���Ŀ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIClearItem_Click(object sender, EventArgs e)
        {
            try
            {
                //����MessageBoxEx
                if (MessageBoxEx.Show(this, m_lang.GetString("14", "��ȷ��Ҫ��ձ���Ŀ��"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    //��Category����յ�ǰ��Ŀ
                    Global.Settings.Category.Items[metroShell1.SelectedTab.Name].ClearItem(sideBar1.ExpandedPanel.Name);

                    //���
                    sideBar1.ExpandedPanel.SubItems.Clear();

                    //ˢ��
                    sideBar1.ExpandedPanel.Refresh();
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// ɾ������Ŀ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIDeleteItem_Click(object sender, EventArgs e)
        {
            try
            {
                //����MessageBoxEx
                if (MessageBoxEx.Show(this, m_lang.GetString("15", "��ȷ��Ҫɾ������Ŀ��"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    //��Category��ɾ����ǰ��Ŀ
                    Global.Settings.Category.Items[metroShell1.SelectedTab.Name].DeleteItem(sideBar1.ExpandedPanel.Name);

                    //��ȡindex
                    int nIndex = sideBar1.Panels.IndexOf(sideBar1.ExpandedPanel.Name);

                    //��sideBar1���Ƴ���Item
                    sideBar1.Panels.Remove(sideBar1.ExpandedPanel.Name);

                    //�������panels
                    if (sideBar1.Panels.Count != 0)
                    {
                        //Ҫչ����index
                        nIndex = nIndex - 1;

                        //�ж�������
                        if (nIndex < 0)
                        {
                            nIndex = nIndex + 1;
                        }

                        //ɾ������չ����һ��panel
                        sideBar1.Panels[nIndex].Expanded = true;
                    }

                    //ˢ��
                    sideBar1.Refresh();
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// ɾ�������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIDeleteCate_Click(object sender, EventArgs e)
        {
            try
            {
                //ȷ����item����Ŀ�����ֻ��һ���ˣ�����ֹ�û�ɾ��
                //�ж����Ƿ����˲��
                if (Global.Settings.LoadPlugin)
                {
                    if (metroShell1.Items.Count == 3)
                    {
                        MessageBoxEx.Show(this, m_lang.GetString("16", "������Ҫ����һ�����"));
                        return;
                    }
                }
                else
                {
                    if (metroShell1.Items.Count == 2)
                    {
                        MessageBoxEx.Show(this, m_lang.GetString("16", "������Ҫ����һ�����"));
                        return;
                    }
                }

                //����MessageBoxEx
                if (MessageBoxEx.Show(this, m_lang.GetString("17", "��ȷ��Ҫɾ���������"), "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    //��Category��ɾ����ǰ��Ŀ
                    Global.Settings.Category.DeleteCategory(metroShell1.SelectedTab.Name);

                    //��ȡindex
                    int nIndex = metroShell1.Items.IndexOf(metroShell1.SelectedTab.Name);

                    //��metroShell1���Ƴ���Item
                    metroShell1.Items.Remove(metroShell1.SelectedTab.Name);

                    //�������Items
                    if (metroShell1.Items.Count != 0)
                    {
                        //Ҫչ����index
                        nIndex = nIndex - 1;

                        //�ж�������
                        if (nIndex < 0)
                        {
                            nIndex = nIndex + 1;
                        }

                        //ɾ����ѡ����һ��tab
                        metroShell1.SelectedTab = (MetroTabItem)metroShell1.Items[nIndex];
                    }

                    //ˢ���¿ؼ�
                    metroShell1.Refresh();
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// ��ͨ�Ĵ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmibtnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                string strPath = "";

                //����Items
                foreach (ButtonItem bi in sideBar1.ExpandedPanel.SubItems)
                {
                    //�ж��Ƿ�checked
                    if (bi.Checked)
                    {
                        //�����ȡTag�е�����
                        strPath = (bi.Tag as ItemSubInfo).FullName;

                        //��
                        process.Start(strPath);
                    }
                }

                //�ж���tag�Ƿ�ΪNULL��������û��checked
                if (m_item == null || m_item.Checked)
                    return;

                //�����ȡTag�е�����
                strPath = ((ItemSubInfo)m_item.Tag).FullName;

                //��
                process.Start(strPath);
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// �Թ���ԱȨ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIRunasAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                string strPath = "";

                //����Items
                foreach (ButtonItem bi in sideBar1.ExpandedPanel.SubItems)
                {
                    //�ж��Ƿ�checked
                    if (bi.Checked)
                    {
                        //�����ȡTag�е�����
                        strPath = (bi.Tag as ItemSubInfo).FullName;

                        //�Թ���ԱȨ�޴�
                        process.StartByRunas(strPath);
                    }
                }

                //�ж���tag�Ƿ�ΪNULL��������û��checked
                if (m_item == null || m_item.Checked)
                    return;

                //�����ȡTag�е�����
                strPath = ((ItemSubInfo)m_item.Tag).FullName;

                //�Թ���ԱȨ�޴�
                process.StartByRunas(strPath);     
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// ɾ��Item�е��������Ŀ�е��ļ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //���ж���ExpandedPanel
                if (sideBar1.ExpandedPanel == null)
                    return;

                int count = 0;
                string strMsg = m_lang.GetString("18", "��ȷ��Ҫɾ�������ݷ�ʽ��");

                //�ж��Ƿ��Ƕ�ѡ
                for (int i = 0; i < sideBar1.ExpandedPanel.SubItems.Count; i++)
                {
                    ButtonItem bi = sideBar1.ExpandedPanel.SubItems[i] as ButtonItem;

                    if (bi.Checked)
                        count++;
                }

                //���û��ѡ�У���buttonItemҲΪNULL���򷵻�
                if (count == 0 && m_item == null)
                {
                    return;
                }
                  
                //���checked����1�Ļ�
                if(count > 1)
                {
                    strMsg = m_lang.GetString("19", "��ȷ��Ҫɾ����Щ��ݷ�ʽ��");
                }

                //����MessageBoxEx
                if (MessageBoxEx.Show(this, strMsg, "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    //����Items
                    for (int i = sideBar1.ExpandedPanel.SubItems.Count - 1; i >= 0; i--)
                    {
                        //�ж��Ƿ�checked
                        ButtonItem bi = sideBar1.ExpandedPanel.SubItems[i] as ButtonItem;

                        if (bi.Checked)
                        {
                            //��List��ɾ����Ӧ���ļ�
                            Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].DeleteSubItem((ItemSubInfo)bi.Tag, Global.Settings.WinHotKey);

                            //�Ƴ���Item
                            sideBar1.ExpandedPanel.SubItems.Remove(bi);
                        }
                    }

                    //�ж���tag�Ƿ�ΪNULL��������û��checked
                    if (m_item == null || m_item.Checked)
                        return;

                    //��List��ɾ����Ӧ���ļ�
                    Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].DeleteSubItem((ItemSubInfo)m_item.Tag, Global.Settings.WinHotKey);

                    //�������
                    int nIndex = sideBar1.ExpandedPanel.SubItems.IndexOf(m_item);

                    //�Ƴ���Item
                    sideBar1.ExpandedPanel.SubItems.RemoveAt(nIndex);
                }  
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnICut_Click(object sender, EventArgs e)
        {
            try
            {
                //��ŵ��б���
                List<ItemSubInfo> items = new List<ItemSubInfo>();

                //�ж���
                if (sideBar1.ExpandedPanel == null)
                    return;

                //����Items
                for (int i = sideBar1.ExpandedPanel.SubItems.Count - 1; i >= 0; i--)
                {
                    //�ж��Ƿ�checked
                    ButtonItem bi = sideBar1.ExpandedPanel.SubItems[i] as ButtonItem;

                    if (bi.Checked)
                    {
                        //FileInfo
                        var fi = (ItemSubInfo)bi.Tag;

                        //��ӵ�list
                        items.Add(fi);

                        //�Ƴ�
                        sideBar1.ExpandedPanel.SubItems.Remove(bi);

                        //���������Ƴ�
                        Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].DeleteSubItem(fi, Global.Settings.WinHotKey);
                    }
                }

                //�ж���tag�Ƿ�ΪNULL��������û��checked
                if (m_item != null && !m_item.Checked)
                {
                    items.Add((ItemSubInfo)m_item.Tag);

                    //�Ƴ�
                    sideBar1.ExpandedPanel.SubItems.Remove(((ButtonItem)m_item));

                    //���������Ƴ�
                    Global.Settings.Category.Items[metroShell1.SelectedTab.Name].Items[sideBar1.ExpandedPanel.Name].DeleteSubItem((ItemSubInfo)m_item.Tag, Global.Settings.WinHotKey);
                }

                //�ж���
                if (items.Count == 0)
                    return;

                //���Ƶ����а�
                Clipboard.SetData("JStart_FileInfos", items);
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnICopy_Click(object sender, EventArgs e)
        {
            try
            {
                //��ŵ��б���
                List<ItemSubInfo> items = new List<ItemSubInfo>();

                //�ж���
                if (sideBar1.ExpandedPanel == null)
                    return;

                //����Items
                for (int i = sideBar1.ExpandedPanel.SubItems.Count - 1; i >= 0; i--)
                {
                    //�ж��Ƿ�checked
                    ButtonItem bi = sideBar1.ExpandedPanel.SubItems[i] as ButtonItem;

                    if (bi.Checked)
                    {
                        //��ӵ�list
                        items.Add((ItemSubInfo)bi.Tag);
                    }
                }

                //�ж���tag�Ƿ�ΪNULL��������û��checked
                if (m_item != null && !m_item.Checked)
                    items.Add((ItemSubInfo)m_item.Tag);

                //�ж���
                if (items.Count == 0)
                    return;

                //���Ƶ����а�
                Clipboard.SetData("JStart_FileInfos", items);
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// ��ӵ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIAddToMainPanel_Click(object sender, EventArgs e)
        {
            //�ж���
            if (sideBar1.ExpandedPanel == null)
                return;

            //����Items
            for (int i = 0; i < sideBar1.ExpandedPanel.SubItems.Count; i++)
            {
                //�ж��Ƿ�checked
                ButtonItem bi = sideBar1.ExpandedPanel.SubItems[i] as ButtonItem;

                if (bi.Checked)
                {
                    MetroStatusBarAddItem(bi.Tag as ItemSubInfo, true);
                }
            }

            //�ж���tag�Ƿ�ΪNULL��������û��checked
            if (m_item != null && !m_item.Checked)
            {
                MetroStatusBarAddItem(m_item.Tag as ItemSubInfo, true);
            }
        }

        /// <summary>
        /// �����ȼ�
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

                //������óɹ�
                if(f.ShowDialog() == DialogResult.OK)
                {
                    //·��ת��һ��
                    string strPath = Path.IsPathRooted(fi.FullName) ? fi.FullName : Application.StartupPath + "\\" + fi.FullName;

                    //����tooltip
                    if (fi.HotKey == null)
                        m_item.Tooltip = fi.Name + "\r\n" + strPath;
                    else
                        m_item.Tooltip = fi.Name + "(" + fi.HotKey.ToString() + ")" + "\r\n" + strPath;
                }
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// ���ɿ�ݷ�ʽ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnICreateShortcut_Click(object sender, EventArgs e)
        {
            try
            {
                //����Items
                for (int i = 0; i < sideBar1.ExpandedPanel.SubItems.Count; i++)
                {
                    //�ж��Ƿ�checked
                    ButtonItem bi = sideBar1.ExpandedPanel.SubItems[i] as ButtonItem;

                    //���ѡ����
                    if (bi.Checked)
                    {
                        ItemSubInfo fi = bi.Tag as ItemSubInfo;

                        //��ӿ�ݼ�
                        using(ShellLink sl = new ShellLink())
                        {
                            //��ȡ����·��
                            string strPath = path.GetAbsolutePath(Application.ExecutablePath, fi.FullName);

                            //���ù���Ŀ¼��Ŀ��exe����Ϣ
                            sl.WorkingDirectory = System.IO.Path.GetDirectoryName(strPath);
                            sl.Target = strPath;

                            //�����ݷ�ʽ
                            sl.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + fi.Name + ".lnk");
                        }
                    }
                }

                //�ж���tag�Ƿ�ΪNULL��������û��checked
                if (m_item == null || m_item.Checked)
                    return;

                //��ӿ�ݼ�
                using (ShellLink sl = new ShellLink())
                {
                    ItemSubInfo fi = m_item.Tag as ItemSubInfo;

                    //��ȡ����·��
                    string strPath = path.GetAbsolutePath(Application.ExecutablePath, fi.FullName);

                    //���ù���Ŀ¼��Ŀ��exe����Ϣ
                    sl.WorkingDirectory = System.IO.Path.GetDirectoryName(strPath);
                    sl.Target = strPath;

                    //�����ݷ�ʽ
                    sl.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + fi.Name + ".lnk");
                }
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// ��ݷ�ʽ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIRename_Click(object sender, EventArgs e)
        {
            //�ж���buttonItem
            if(m_item != null)
            {
                sideBar1.BeginItemTextEdit(m_item);
            }
        }

        /// <summary>
        /// ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIExplore_Click(object sender, EventArgs e)
        {
            foreach (ButtonItem bi in sideBar1.ExpandedPanel.SubItems)
            {
                //�ж��Ƿ�checked
                if (bi.Checked)
                {
                    //�����ԶԻ���
                    file.ExplorerFile((bi.Tag as ItemSubInfo).FullName);
                }
            }

            //�ж���tag�Ƿ�ΪNULL��������û��checked
            if (m_item == null || m_item.Checked)
                return;

            //�����ԶԻ���
            file.ExplorerFile(((ItemSubInfo)m_item.Tag).FullName);
        }

        /// <summary>
        /// �����ԶԻ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIProperties_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ButtonItem bi in sideBar1.ExpandedPanel.SubItems)
                {
                    //�ж��Ƿ�checked
                    if (bi.Checked)
                    {
                        //�����ԶԻ���
                        file.ShowFileProperty((bi.Tag as ItemSubInfo).FullName);
                    }
                }

                //�ж���tag�Ƿ�ΪNULL��������û��checked
                if (m_item == null || m_item.Checked)
                    return;

                //�����ԶԻ���
                file.ShowFileProperty(((ItemSubInfo)m_item.Tag).FullName);
            }
            catch (Exception ex) { MessageBoxEx.Show(this, ex.Message); }
        }

        /// <summary>
        /// ˢ�²��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIRefreshPlugin_Click(object sender, EventArgs e)
        {
            ReloadPlugins();
        }

        /// <summary>
        /// ʱ��ؼ������ڴ������������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            int heigth = this.Height;
            QQFormHide.hide_show(this, heigth);
        }

        /// <summary>
        /// �˳�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIMenuExit_Click(object sender, EventArgs e)
        {
            //�˳�ǰ��������
            Global.SettingsManager.Save(Global.Settings);

            //ע�������ȼ�
            Global.Settings.WinHotKey.UnReloadAll();

            //�˳�
            Application.Exit();
        }

        /// <summary>
        /// �������
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
        /// ����
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
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuBar4_PopupOpen(object sender, PopupOpenEventArgs e)
        {
            //���û��ѡ�У���ȡ��
            if(m_item == null)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIMSBOpen_Click(object sender, EventArgs e)
        {
            MetroStatusBarItem_Click(m_item, e);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIMSBRemove_Click(object sender, EventArgs e)
        {
            if (this.m_item != null)
            {
                if (this.m_item.Parent != null)
                {
                    //��������Ƴ�
                    this.m_item.Parent.SubItems.Remove(m_item);

                    //���������Ƴ�
                    Global.Settings.MainPanelItems.Remove(m_item.Tag as ItemSubInfo);

                    //�Ӹ�������Ͳ����������
                    this.metroStatusBar1.InvalidateLayout();
                }
            }
        }

        /// <summary>
        /// ��Ҫ��Ϊ��ʵ���Ϸ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void metroStatusBar1_DragDrop(object sender, DragEventArgs e)
        {
            ButtonItemEx item = e.Data.GetData(typeof(ButtonItemEx)) as ButtonItemEx;

            if (item == null)
                return;

            //���
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

            //�ж�һ��
            if (item != null)
                m_item = item;
            else
                m_item = null;
        }
    }
}