namespace JJStart
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tsmiOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.metroShell1 = new JJStart.MetroShellEx();
            this.metroTabPanel1 = new DevComponents.DotNetBar.Metro.MetroTabPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBoxDropDown1 = new DevComponents.DotNetBar.Controls.TextBoxDropDown();
            this.sideBar1 = new JJStart.SideBarEx();
            this.imageSmall = new System.Windows.Forms.ImageList(this.components);
            this.imageMedium = new System.Windows.Forms.ImageList(this.components);
            this.metroTabItem1 = new DevComponents.DotNetBar.Metro.MetroTabItem();
            this.btnIAddCate2 = new DevComponents.DotNetBar.ButtonItem();
            this.btnSkin = new DevComponents.DotNetBar.ButtonItem();
            this.styleManager1 = new DevComponents.DotNetBar.StyleManager(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.btnIAddFile = new DevComponents.DotNetBar.ButtonItem();
            this.btnIAddDir = new DevComponents.DotNetBar.ButtonItem();
            this.btnINewCate = new DevComponents.DotNetBar.ButtonItem();
            this.btnINewItem = new DevComponents.DotNetBar.ButtonItem();
            this.btnIView = new DevComponents.DotNetBar.ButtonItem();
            this.btnILargeIcons = new DevComponents.DotNetBar.ButtonItem();
            this.btnISmallIcons = new DevComponents.DotNetBar.ButtonItem();
            this.btnIMultiColumn = new DevComponents.DotNetBar.ButtonItem();
            this.btnIPaste = new DevComponents.DotNetBar.ButtonItem();
            this.btnIClearItem = new DevComponents.DotNetBar.ButtonItem();
            this.btnIDeleteItem = new DevComponents.DotNetBar.ButtonItem();
            this.btnIDeleteCate = new DevComponents.DotNetBar.ButtonItem();
            this.contextMenuBar2 = new DevComponents.DotNetBar.ContextMenuBar();
            this.buttonItem2 = new DevComponents.DotNetBar.ButtonItem();
            this.btnIOpen = new DevComponents.DotNetBar.ButtonItem();
            this.btnIRunasAdmin = new DevComponents.DotNetBar.ButtonItem();
            this.btnIDelete = new DevComponents.DotNetBar.ButtonItem();
            this.btnICut = new DevComponents.DotNetBar.ButtonItem();
            this.btnICopy = new DevComponents.DotNetBar.ButtonItem();
            this.btnIAddToMainPanel = new DevComponents.DotNetBar.ButtonItem();
            this.btnISetHotKey = new DevComponents.DotNetBar.ButtonItem();
            this.btnICreateShortcut = new DevComponents.DotNetBar.ButtonItem();
            this.btnIRename = new DevComponents.DotNetBar.ButtonItem();
            this.btnIExplore = new DevComponents.DotNetBar.ButtonItem();
            this.btnIProperties = new DevComponents.DotNetBar.ButtonItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.metroStatusBar1 = new DevComponents.DotNetBar.Metro.MetroStatusBar();
            this.btnIMenu = new DevComponents.DotNetBar.ButtonItem();
            this.btnIMenuCheckUpdate = new DevComponents.DotNetBar.ButtonItem();
            this.btnIMenuExit = new DevComponents.DotNetBar.ButtonItem();
            this.btnISettings = new DevComponents.DotNetBar.ButtonItem();
            this.btnIJJStartOrg = new DevComponents.DotNetBar.ButtonItem();
            this.contextMenuBar3 = new DevComponents.DotNetBar.ContextMenuBar();
            this.buttonItem3 = new DevComponents.DotNetBar.ButtonItem();
            this.btnIRefreshPlugin = new DevComponents.DotNetBar.ButtonItem();
            this.contextMenuBar4 = new DevComponents.DotNetBar.ContextMenuBar();
            this.buttonItem4 = new DevComponents.DotNetBar.ButtonItem();
            this.btnIMSBOpen = new DevComponents.DotNetBar.ButtonItem();
            this.btnIMSBRemove = new DevComponents.DotNetBar.ButtonItem();
            this.metroShell1.SuspendLayout();
            this.metroTabPanel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar4)).BeginInit();
            this.SuspendLayout();
            // 
            // tsmiOpen
            // 
            this.tsmiOpen.Name = "tsmiOpen";
            this.tsmiOpen.Size = new System.Drawing.Size(175, 24);
            this.tsmiOpen.Text = "打开主窗体";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(172, 6);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(175, 24);
            this.tsmiExit.Text = "退出程序";
            // 
            // metroShell1
            // 
            this.metroShell1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.metroShell1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroShell1.CaptionVisible = true;
            this.metroShell1.Controls.Add(this.metroTabPanel1);
            this.metroShell1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroShell1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.metroShell1.ForeColor = System.Drawing.Color.Black;
            this.metroShell1.HelpButtonText = null;
            this.metroShell1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.metroTabItem1,
            this.btnIAddCate2});
            this.metroShell1.KeyTipsFont = new System.Drawing.Font("Tahoma", 7F);
            this.metroShell1.Location = new System.Drawing.Point(0, 0);
            this.metroShell1.Margin = new System.Windows.Forms.Padding(0);
            this.metroShell1.Name = "metroShell1";
            this.metroShell1.QuickToolbarItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnSkin});
            this.metroShell1.Size = new System.Drawing.Size(468, 683);
            this.metroShell1.SystemText.MaximizeRibbonText = "&Maximize the Ribbon";
            this.metroShell1.SystemText.MinimizeRibbonText = "Mi&nimize the Ribbon";
            this.metroShell1.SystemText.QatAddItemText = "&Add to Quick Access Toolbar";
            this.metroShell1.SystemText.QatCustomizeMenuLabel = "<b>Customize Quick Access Toolbar</b>";
            this.metroShell1.SystemText.QatCustomizeText = "&Customize Quick Access Toolbar...";
            this.metroShell1.SystemText.QatDialogAddButton = "&Add >>";
            this.metroShell1.SystemText.QatDialogCancelButton = "Cancel";
            this.metroShell1.SystemText.QatDialogCaption = "Customize Quick Access Toolbar";
            this.metroShell1.SystemText.QatDialogCategoriesLabel = "&Choose commands from:";
            this.metroShell1.SystemText.QatDialogOkButton = "OK";
            this.metroShell1.SystemText.QatDialogPlacementCheckbox = "&Place Quick Access Toolbar below the Ribbon";
            this.metroShell1.SystemText.QatDialogRemoveButton = "&Remove";
            this.metroShell1.SystemText.QatPlaceAboveRibbonText = "&Place Quick Access Toolbar above the Ribbon";
            this.metroShell1.SystemText.QatPlaceBelowRibbonText = "&Place Quick Access Toolbar below the Ribbon";
            this.metroShell1.SystemText.QatRemoveItemText = "&Remove from Quick Access Toolbar";
            this.metroShell1.TabIndex = 0;
            this.metroShell1.TabStripFont = new System.Drawing.Font("Segoe UI", 10.25F, System.Drawing.FontStyle.Bold);
            this.metroShell1.Text = "metroShell1";
            this.metroShell1.SelectedTabChanged += new System.EventHandler(this.metroShell1_SelectedTabChanged);
            this.metroShell1.SettingsButtonClick += new System.EventHandler(this.metroShell1_SettingsButtonClick);
            this.metroShell1.HelpButtonClick += new System.EventHandler(this.metroShell1_HelpButtonClick);
            // 
            // metroTabPanel1
            // 
            this.metroTabPanel1.AutoScroll = true;
            this.metroTabPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.metroTabPanel1.Controls.Add(this.splitContainer1);
            this.metroTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabPanel1.Location = new System.Drawing.Point(0, 51);
            this.metroTabPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.metroTabPanel1.Name = "metroTabPanel1";
            this.metroTabPanel1.Size = new System.Drawing.Size(468, 632);
            // 
            // 
            // 
            this.metroTabPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.metroTabPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroTabPanel1.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.ForeColor = System.Drawing.Color.Black;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.splitContainer1.Panel1.Controls.Add(this.textBoxDropDown1);
            this.splitContainer1.Panel1.ForeColor = System.Drawing.Color.Black;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(211)))), ((int)(((byte)(211)))));
            this.splitContainer1.Panel2.Controls.Add(this.sideBar1);
            this.splitContainer1.Panel2.ForeColor = System.Drawing.Color.Black;
            this.splitContainer1.Size = new System.Drawing.Size(468, 632);
            this.splitContainer1.SplitterDistance = 25;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 5;
            // 
            // textBoxDropDown1
            // 
            this.textBoxDropDown1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.textBoxDropDown1.BackgroundStyle.Class = "TextBoxBorder";
            this.textBoxDropDown1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textBoxDropDown1.ButtonDropDown.Visible = true;
            this.textBoxDropDown1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDropDown1.ForeColor = System.Drawing.Color.Black;
            this.textBoxDropDown1.Location = new System.Drawing.Point(0, 0);
            this.textBoxDropDown1.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxDropDown1.Name = "textBoxDropDown1";
            this.textBoxDropDown1.Size = new System.Drawing.Size(468, 25);
            this.textBoxDropDown1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.textBoxDropDown1.TabIndex = 20;
            this.textBoxDropDown1.Text = "";
            this.textBoxDropDown1.TextChanged += new System.EventHandler(this.textBoxDropDown1_TextChanged);
            // 
            // sideBar1
            // 
            this.sideBar1.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.sideBar1.AllowDrop = true;
            this.sideBar1.AllowExternalDrop = true;
            this.sideBar1.BackColor = System.Drawing.Color.White;
            this.sideBar1.BorderStyle = DevComponents.DotNetBar.eBorderType.None;
            this.contextMenuBar1.SetContextMenuEx(this.sideBar1, this.buttonItem1);
            this.sideBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sideBar1.EnableDoubleClickChangeText = false;
            this.sideBar1.ExpandedPanel = null;
            this.sideBar1.ForeColor = System.Drawing.Color.Black;
            this.sideBar1.Images = this.imageSmall;
            this.sideBar1.ImagesMedium = this.imageMedium;
            this.sideBar1.Location = new System.Drawing.Point(0, 0);
            this.sideBar1.Margin = new System.Windows.Forms.Padding(0);
            this.sideBar1.Name = "sideBar1";
            this.sideBar1.SideBarPanelItemTextChange = false;
            this.sideBar1.Size = new System.Drawing.Size(468, 606);
            this.sideBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.sideBar1.TabIndex = 4;
            this.sideBar1.Text = "sideBar1";
            this.sideBar1.UseNativeDragDrop = true;
            this.sideBar1.ExpandedChange += new System.EventHandler(this.sideBar1_ExpandedChange);
            this.sideBar1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.sideBar1_MouseDown);
            this.sideBar1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.sideBar1_MouseMove);
            this.sideBar1.ItemTextChanged += new System.EventHandler(this.sideBar1_ItemTextChanged);
            this.sideBar1.DragDrop += new System.Windows.Forms.DragEventHandler(this.sideBar1_DragDrop);
            this.sideBar1.DragEnter += new System.Windows.Forms.DragEventHandler(this.sideBar1_DragEnter);
            this.sideBar1.DragOver += new System.Windows.Forms.DragEventHandler(this.sideBar1_DragOver);
            // 
            // imageSmall
            // 
            this.imageSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageSmall.ImageSize = new System.Drawing.Size(16, 16);
            this.imageSmall.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageMedium
            // 
            this.imageMedium.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageMedium.ImageSize = new System.Drawing.Size(32, 32);
            this.imageMedium.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // metroTabItem1
            // 
            this.metroTabItem1.Checked = true;
            this.metroTabItem1.Name = "metroTabItem1";
            this.metroTabItem1.Panel = this.metroTabPanel1;
            this.metroTabItem1.Text = "&默认";
            // 
            // btnIAddCate2
            // 
            this.btnIAddCate2.Icon = ((System.Drawing.Icon)(resources.GetObject("btnIAddCate2.Icon")));
            this.btnIAddCate2.Name = "btnIAddCate2";
            this.btnIAddCate2.Click += new System.EventHandler(this.btnIAddCate2_Click);
            // 
            // btnSkin
            // 
            this.btnSkin.AutoExpandOnClick = true;
            this.btnSkin.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btnSkin.Image = ((System.Drawing.Image)(resources.GetObject("btnSkin.Image")));
            this.btnSkin.Name = "btnSkin";
            this.btnSkin.ShowSubItems = false;
            // 
            // styleManager1
            // 
            this.styleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Metro;
            this.styleManager1.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(87)))), ((int)(((byte)(154))))));
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipTitle = "JJStart";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.AntiAlias = true;
            this.contextMenuBar1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem1});
            this.contextMenuBar1.Location = new System.Drawing.Point(125, 25);
            this.contextMenuBar1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.contextMenuBar1.Name = "contextMenuBar1";
            this.contextMenuBar1.Size = new System.Drawing.Size(81, 25);
            this.contextMenuBar1.Stretch = true;
            this.contextMenuBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.contextMenuBar1.TabIndex = 6;
            this.contextMenuBar1.TabStop = false;
            this.contextMenuBar1.Text = "contextMenuBar1";
            // 
            // buttonItem1
            // 
            this.buttonItem1.AutoExpandOnClick = true;
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnIAddFile,
            this.btnIAddDir,
            this.btnINewCate,
            this.btnINewItem,
            this.btnIView,
            this.btnIPaste,
            this.btnIClearItem,
            this.btnIDeleteItem,
            this.btnIDeleteCate});
            this.buttonItem1.Text = "buttonItem1";
            this.buttonItem1.PopupOpen += new DevComponents.DotNetBar.DotNetBarManager.PopupOpenEventHandler(this.buttonItem1_PopupOpen);
            // 
            // btnIAddFile
            // 
            this.btnIAddFile.Name = "btnIAddFile";
            this.btnIAddFile.Text = "添加文件";
            this.btnIAddFile.Click += new System.EventHandler(this.btnIAddFile_Click);
            // 
            // btnIAddDir
            // 
            this.btnIAddDir.Name = "btnIAddDir";
            this.btnIAddDir.Text = "添加目录";
            this.btnIAddDir.Click += new System.EventHandler(this.btnIAddDir_Click);
            // 
            // btnINewCate
            // 
            this.btnINewCate.BeginGroup = true;
            this.btnINewCate.Name = "btnINewCate";
            this.btnINewCate.Text = "新建类别";
            this.btnINewCate.Click += new System.EventHandler(this.btnINewCate_Click);
            // 
            // btnINewItem
            // 
            this.btnINewItem.Name = "btnINewItem";
            this.btnINewItem.Text = "新建栏目";
            this.btnINewItem.Click += new System.EventHandler(this.btnINewItem_Click);
            // 
            // btnIView
            // 
            this.btnIView.BeginGroup = true;
            this.btnIView.Name = "btnIView";
            this.btnIView.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnILargeIcons,
            this.btnISmallIcons,
            this.btnIMultiColumn});
            this.btnIView.Text = "查看";
            // 
            // btnILargeIcons
            // 
            this.btnILargeIcons.Name = "btnILargeIcons";
            this.btnILargeIcons.Text = "大图标";
            this.btnILargeIcons.Click += new System.EventHandler(this.btnILargeIcons_Click);
            // 
            // btnISmallIcons
            // 
            this.btnISmallIcons.Name = "btnISmallIcons";
            this.btnISmallIcons.Text = "小图标";
            this.btnISmallIcons.Click += new System.EventHandler(this.btnISmallIcons_Click);
            // 
            // btnIMultiColumn
            // 
            this.btnIMultiColumn.Name = "btnIMultiColumn";
            this.btnIMultiColumn.Text = "允许横排";
            this.btnIMultiColumn.Click += new System.EventHandler(this.btnIMultiColumn_Click);
            // 
            // btnIPaste
            // 
            this.btnIPaste.BeginGroup = true;
            this.btnIPaste.Name = "btnIPaste";
            this.btnIPaste.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlV);
            this.btnIPaste.Text = "粘贴";
            this.btnIPaste.Click += new System.EventHandler(this.btnIPaste_Click);
            // 
            // btnIClearItem
            // 
            this.btnIClearItem.Name = "btnIClearItem";
            this.btnIClearItem.Text = "清空本栏目";
            this.btnIClearItem.Click += new System.EventHandler(this.btnIClearItem_Click);
            // 
            // btnIDeleteItem
            // 
            this.btnIDeleteItem.BeginGroup = true;
            this.btnIDeleteItem.Name = "btnIDeleteItem";
            this.btnIDeleteItem.Text = "删除本栏目";
            this.btnIDeleteItem.Click += new System.EventHandler(this.btnIDeleteItem_Click);
            // 
            // btnIDeleteCate
            // 
            this.btnIDeleteCate.Name = "btnIDeleteCate";
            this.btnIDeleteCate.Text = "删除本类别";
            this.btnIDeleteCate.Click += new System.EventHandler(this.btnIDeleteCate_Click);
            // 
            // contextMenuBar2
            // 
            this.contextMenuBar2.AntiAlias = true;
            this.contextMenuBar2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.contextMenuBar2.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem2});
            this.contextMenuBar2.Location = new System.Drawing.Point(213, 25);
            this.contextMenuBar2.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.contextMenuBar2.Name = "contextMenuBar2";
            this.contextMenuBar2.Size = new System.Drawing.Size(86, 25);
            this.contextMenuBar2.Stretch = true;
            this.contextMenuBar2.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.contextMenuBar2.TabIndex = 7;
            this.contextMenuBar2.TabStop = false;
            this.contextMenuBar2.Text = "contextMenuBar2";
            // 
            // buttonItem2
            // 
            this.buttonItem2.AutoExpandOnClick = true;
            this.buttonItem2.Name = "buttonItem2";
            this.buttonItem2.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnIOpen,
            this.btnIRunasAdmin,
            this.btnIDelete,
            this.btnICut,
            this.btnICopy,
            this.btnIAddToMainPanel,
            this.btnISetHotKey,
            this.btnICreateShortcut,
            this.btnIRename,
            this.btnIExplore,
            this.btnIProperties});
            this.buttonItem2.Text = "buttonItem2";
            this.buttonItem2.PopupOpen += new DevComponents.DotNetBar.DotNetBarManager.PopupOpenEventHandler(this.buttonItem2_PopupOpen);
            // 
            // btnIOpen
            // 
            this.btnIOpen.FontBold = true;
            this.btnIOpen.Name = "btnIOpen";
            this.btnIOpen.Text = "打开(&O)";
            this.btnIOpen.Click += new System.EventHandler(this.tsmibtnOpen_Click);
            // 
            // btnIRunasAdmin
            // 
            this.btnIRunasAdmin.Icon = ((System.Drawing.Icon)(resources.GetObject("btnIRunasAdmin.Icon")));
            this.btnIRunasAdmin.Name = "btnIRunasAdmin";
            this.btnIRunasAdmin.Text = "以管理员权限运行(&A)";
            this.btnIRunasAdmin.Click += new System.EventHandler(this.btnIRunasAdmin_Click);
            // 
            // btnIDelete
            // 
            this.btnIDelete.BeginGroup = true;
            this.btnIDelete.Name = "btnIDelete";
            this.btnIDelete.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.Del);
            this.btnIDelete.Text = "删除";
            this.btnIDelete.Click += new System.EventHandler(this.btnIDelete_Click);
            // 
            // btnICut
            // 
            this.btnICut.Name = "btnICut";
            this.btnICut.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlX);
            this.btnICut.Text = "剪切";
            this.btnICut.Click += new System.EventHandler(this.btnICut_Click);
            // 
            // btnICopy
            // 
            this.btnICopy.Name = "btnICopy";
            this.btnICopy.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlC);
            this.btnICopy.Text = "复制";
            this.btnICopy.Click += new System.EventHandler(this.btnICopy_Click);
            // 
            // btnIAddToMainPanel
            // 
            this.btnIAddToMainPanel.Name = "btnIAddToMainPanel";
            this.btnIAddToMainPanel.Text = "添加到主面板";
            this.btnIAddToMainPanel.Click += new System.EventHandler(this.btnIAddToMainPanel_Click);
            // 
            // btnISetHotKey
            // 
            this.btnISetHotKey.BeginGroup = true;
            this.btnISetHotKey.Name = "btnISetHotKey";
            this.btnISetHotKey.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlH);
            this.btnISetHotKey.Text = "设置热键...";
            this.btnISetHotKey.Click += new System.EventHandler(this.btnISetHotKey_Click);
            // 
            // btnICreateShortcut
            // 
            this.btnICreateShortcut.Name = "btnICreateShortcut";
            this.btnICreateShortcut.Text = "生成快捷方式到桌面";
            this.btnICreateShortcut.Click += new System.EventHandler(this.btnICreateShortcut_Click);
            // 
            // btnIRename
            // 
            this.btnIRename.BeginGroup = true;
            this.btnIRename.Name = "btnIRename";
            this.btnIRename.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.F2);
            this.btnIRename.Text = "重命名";
            this.btnIRename.Click += new System.EventHandler(this.btnIRename_Click);
            // 
            // btnIExplore
            // 
            this.btnIExplore.Name = "btnIExplore";
            this.btnIExplore.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlB);
            this.btnIExplore.Text = "浏览文件/目录...";
            this.btnIExplore.Click += new System.EventHandler(this.btnIExplore_Click);
            // 
            // btnIProperties
            // 
            this.btnIProperties.BeginGroup = true;
            this.btnIProperties.Name = "btnIProperties";
            this.btnIProperties.Text = "属性";
            this.btnIProperties.Click += new System.EventHandler(this.btnIProperties_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // metroStatusBar1
            // 
            this.metroStatusBar1.AllowDrop = true;
            this.metroStatusBar1.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.metroStatusBar1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.metroStatusBar1.ContainerControlProcessDialogKey = true;
            this.contextMenuBar4.SetContextMenuEx(this.metroStatusBar1, this.buttonItem4);
            this.metroStatusBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.metroStatusBar1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.metroStatusBar1.ForeColor = System.Drawing.Color.Black;
            this.metroStatusBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnIMenu,
            this.btnISettings,
            this.btnIJJStartOrg});
            this.metroStatusBar1.Location = new System.Drawing.Point(0, 683);
            this.metroStatusBar1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.metroStatusBar1.Name = "metroStatusBar1";
            this.metroStatusBar1.Size = new System.Drawing.Size(468, 22);
            this.metroStatusBar1.TabIndex = 8;
            this.metroStatusBar1.Text = "metroStatusBar1";
            this.metroStatusBar1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.metroStatusBar1_MouseDown);
            this.metroStatusBar1.DragDrop += new System.Windows.Forms.DragEventHandler(this.metroStatusBar1_DragDrop);
            this.metroStatusBar1.DragEnter += new System.Windows.Forms.DragEventHandler(this.metroStatusBar1_DragEnter);
            // 
            // btnIMenu
            // 
            this.btnIMenu.AutoExpandOnClick = true;
            this.btnIMenu.Icon = ((System.Drawing.Icon)(resources.GetObject("btnIMenu.Icon")));
            this.btnIMenu.Name = "btnIMenu";
            this.btnIMenu.PopupSide = DevComponents.DotNetBar.ePopupSide.Top;
            this.btnIMenu.ShowSubItems = false;
            this.btnIMenu.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnIMenuCheckUpdate,
            this.btnIMenuExit});
            this.btnIMenu.Tooltip = "主菜单";
            // 
            // btnIMenuCheckUpdate
            // 
            this.btnIMenuCheckUpdate.Name = "btnIMenuCheckUpdate";
            this.btnIMenuCheckUpdate.Text = "检测升级";
            this.btnIMenuCheckUpdate.Click += new System.EventHandler(this.btnIMenuCheckUpdate_Click);
            // 
            // btnIMenuExit
            // 
            this.btnIMenuExit.Name = "btnIMenuExit";
            this.btnIMenuExit.Text = "退出";
            this.btnIMenuExit.Click += new System.EventHandler(this.btnIMenuExit_Click);
            // 
            // btnISettings
            // 
            this.btnISettings.Icon = ((System.Drawing.Icon)(resources.GetObject("btnISettings.Icon")));
            this.btnISettings.Name = "btnISettings";
            this.btnISettings.Tooltip = "系统设置";
            this.btnISettings.Click += new System.EventHandler(this.btnISettings_Click);
            // 
            // btnIJJStartOrg
            // 
            this.btnIJJStartOrg.Icon = ((System.Drawing.Icon)(resources.GetObject("btnIJJStartOrg.Icon")));
            this.btnIJJStartOrg.Name = "btnIJJStartOrg";
            this.btnIJJStartOrg.Tooltip = "官网";
            this.btnIJJStartOrg.Click += new System.EventHandler(this.btnIJJStartOrg_Click);
            // 
            // contextMenuBar3
            // 
            this.contextMenuBar3.AntiAlias = true;
            this.contextMenuBar3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuBar3.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem3});
            this.contextMenuBar3.Location = new System.Drawing.Point(302, 25);
            this.contextMenuBar3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contextMenuBar3.Name = "contextMenuBar3";
            this.contextMenuBar3.Size = new System.Drawing.Size(92, 25);
            this.contextMenuBar3.Stretch = true;
            this.contextMenuBar3.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.contextMenuBar3.TabIndex = 9;
            this.contextMenuBar3.TabStop = false;
            this.contextMenuBar3.Text = "contextMenuBar3";
            // 
            // buttonItem3
            // 
            this.buttonItem3.AutoExpandOnClick = true;
            this.buttonItem3.Name = "buttonItem3";
            this.buttonItem3.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnIRefreshPlugin});
            this.buttonItem3.Text = "buttonItem3";
            // 
            // btnIRefreshPlugin
            // 
            this.btnIRefreshPlugin.Name = "btnIRefreshPlugin";
            this.btnIRefreshPlugin.Text = "刷新";
            this.btnIRefreshPlugin.Click += btnIRefreshPlugin_Click;
            // 
            // contextMenuBar4
            // 
            this.contextMenuBar4.AntiAlias = true;
            this.contextMenuBar4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuBar4.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem4});
            this.contextMenuBar4.Location = new System.Drawing.Point(401, 26);
            this.contextMenuBar4.Name = "contextMenuBar4";
            this.contextMenuBar4.Size = new System.Drawing.Size(91, 25);
            this.contextMenuBar4.Stretch = true;
            this.contextMenuBar4.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.contextMenuBar4.TabIndex = 10;
            this.contextMenuBar4.TabStop = false;
            this.contextMenuBar4.Text = "contextMenuBar4";
            this.contextMenuBar4.PopupOpen += new DevComponents.DotNetBar.DotNetBarManager.PopupOpenEventHandler(this.contextMenuBar4_PopupOpen);
            // 
            // buttonItem4
            // 
            this.buttonItem4.AutoExpandOnClick = true;
            this.buttonItem4.Name = "buttonItem4";
            this.buttonItem4.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btnIMSBOpen,
            this.btnIMSBRemove});
            this.buttonItem4.Text = "buttonItem4";
            // 
            // btnIMSBOpen
            // 
            this.btnIMSBOpen.Name = "btnIMSBOpen";
            this.btnIMSBOpen.Text = "打开";
            this.btnIMSBOpen.Click += new System.EventHandler(this.btnIMSBOpen_Click);
            // 
            // btnIMSBRemove
            // 
            this.btnIMSBRemove.Name = "btnIMSBRemove";
            this.btnIMSBRemove.Text = "移除";
            this.btnIMSBRemove.Click += new System.EventHandler(this.btnIMSBRemove_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 705);
            this.Controls.Add(this.contextMenuBar1);
            this.Controls.Add(this.contextMenuBar2);
            this.Controls.Add(this.contextMenuBar3);
            this.Controls.Add(this.contextMenuBar4);
            this.Controls.Add(this.metroShell1);
            this.Controls.Add(this.metroStatusBar1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(0);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "JJStart";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.LocationChanged += new System.EventHandler(this.MainForm_LocationChanged);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.metroShell1.ResumeLayout(false);
            this.metroShell1.PerformLayout();
            this.metroTabPanel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem tsmiOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
        private System.Windows.Forms.ImageList imageSmall;
        private System.Windows.Forms.ImageList imageMedium;
        private DevComponents.DotNetBar.Metro.MetroTabPanel metroTabPanel1;
        private DevComponents.DotNetBar.Metro.MetroTabItem metroTabItem1;
        private DevComponents.DotNetBar.ButtonItem btnSkin;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private DevComponents.DotNetBar.StyleManager styleManager1;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.ButtonItem btnINewCate;
        private DevComponents.DotNetBar.ButtonItem btnINewItem;
        private DevComponents.DotNetBar.ButtonItem btnIView;
        private DevComponents.DotNetBar.ButtonItem btnIDeleteItem;
        private DevComponents.DotNetBar.ButtonItem btnIDeleteCate;
        private DevComponents.DotNetBar.ButtonItem btnILargeIcons;
        private DevComponents.DotNetBar.ButtonItem btnISmallIcons;
        private DevComponents.DotNetBar.ButtonItem btnIMultiColumn;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar2;
        private DevComponents.DotNetBar.ButtonItem buttonItem2;
        private DevComponents.DotNetBar.ButtonItem btnIOpen;
        private DevComponents.DotNetBar.ButtonItem btnIRunasAdmin;
        private DevComponents.DotNetBar.ButtonItem btnIDelete;
        private MetroShellEx metroShell1;
        private SideBarEx sideBar1;
        private DevComponents.DotNetBar.ButtonItem btnIProperties;
        private DevComponents.DotNetBar.ButtonItem btnIClearItem;
        private DevComponents.DotNetBar.ButtonItem btnIExplore;
        private DevComponents.DotNetBar.ButtonItem btnICut;
        private DevComponents.DotNetBar.ButtonItem btnICopy;
        private DevComponents.DotNetBar.ButtonItem btnIPaste;
        private DevComponents.DotNetBar.ButtonItem btnIAddFile;
        private DevComponents.DotNetBar.ButtonItem btnIAddDir;
        private System.Windows.Forms.Timer timer1;
        private DevComponents.DotNetBar.ButtonItem btnIAddCate2;
        private DevComponents.DotNetBar.ButtonItem btnICreateShortcut;
        private DevComponents.DotNetBar.ButtonItem btnISetHotKey;
        private DevComponents.DotNetBar.Metro.MetroStatusBar metroStatusBar1;
        private DevComponents.DotNetBar.ButtonItem btnIMenu;
        private DevComponents.DotNetBar.ButtonItem btnIMenuExit;
        private DevComponents.DotNetBar.ButtonItem btnISettings;
        private DevComponents.DotNetBar.ButtonItem btnIMenuCheckUpdate;
        private DevComponents.DotNetBar.ButtonItem btnIJJStartOrg;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar3;
        private DevComponents.DotNetBar.ButtonItem buttonItem3;
        private DevComponents.DotNetBar.ButtonItem btnIRefreshPlugin;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevComponents.DotNetBar.Controls.TextBoxDropDown textBoxDropDown1;
        private DevComponents.DotNetBar.ButtonItem btnIRename;
        private DevComponents.DotNetBar.ButtonItem btnIAddToMainPanel;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar4;
        private DevComponents.DotNetBar.ButtonItem buttonItem4;
        private DevComponents.DotNetBar.ButtonItem btnIMSBRemove;
        private DevComponents.DotNetBar.ButtonItem btnIMSBOpen;
    }
}

