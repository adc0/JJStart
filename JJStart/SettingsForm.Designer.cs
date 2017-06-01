namespace JJStart
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chkLoadPlugin = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.chkDblClickRun = new System.Windows.Forms.CheckBox();
            this.chkAutoRun = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboLanguage = new System.Windows.Forms.ComboBox();
            this.txtHotKey = new JJStart.TextBoxEx();
            this.chkLinkDragRemove = new System.Windows.Forms.CheckBox();
            this.chkTopMost = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkLoadPlugin
            // 
            this.chkLoadPlugin.AutoSize = true;
            this.chkLoadPlugin.Location = new System.Drawing.Point(19, 83);
            this.chkLoadPlugin.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkLoadPlugin.Name = "chkLoadPlugin";
            this.chkLoadPlugin.Size = new System.Drawing.Size(78, 17);
            this.chkLoadPlugin.TabIndex = 0;
            this.chkLoadPlugin.Text = "加载插件";
            this.chkLoadPlugin.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(186, 204);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(56, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(126, 204);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(56, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "呼出/隐藏:";
            // 
            // chkDblClickRun
            // 
            this.chkDblClickRun.AutoSize = true;
            this.chkDblClickRun.Location = new System.Drawing.Point(19, 108);
            this.chkDblClickRun.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkDblClickRun.Name = "chkDblClickRun";
            this.chkDblClickRun.Size = new System.Drawing.Size(104, 17);
            this.chkDblClickRun.TabIndex = 0;
            this.chkDblClickRun.Text = "双击打开链接";
            this.chkDblClickRun.UseVisualStyleBackColor = true;
            // 
            // chkAutoRun
            // 
            this.chkAutoRun.AutoSize = true;
            this.chkAutoRun.Location = new System.Drawing.Point(19, 132);
            this.chkAutoRun.Name = "chkAutoRun";
            this.chkAutoRun.Size = new System.Drawing.Size(136, 17);
            this.chkAutoRun.TabIndex = 4;
            this.chkAutoRun.Text = "开机自动启动JJStart";
            this.chkAutoRun.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "界面语言:";
            // 
            // cboLanguage
            // 
            this.cboLanguage.FormattingEnabled = true;
            this.cboLanguage.Location = new System.Drawing.Point(88, 17);
            this.cboLanguage.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.Size = new System.Drawing.Size(154, 21);
            this.cboLanguage.TabIndex = 6;
            // 
            // txtHotKey
            // 
            this.txtHotKey.Location = new System.Drawing.Point(88, 47);
            this.txtHotKey.Name = "txtHotKey";
            this.txtHotKey.Size = new System.Drawing.Size(154, 22);
            this.txtHotKey.TabIndex = 2;
            // 
            // chkLinkDragRemove
            // 
            this.chkLinkDragRemove.AutoSize = true;
            this.chkLinkDragRemove.Location = new System.Drawing.Point(19, 155);
            this.chkLinkDragRemove.Name = "chkLinkDragRemove";
            this.chkLinkDragRemove.Size = new System.Drawing.Size(169, 17);
            this.chkLinkDragRemove.TabIndex = 4;
            this.chkLinkDragRemove.Text = "对快捷方式采用移动模式";
            this.chkLinkDragRemove.UseVisualStyleBackColor = true;
            // 
            // chkTopMost
            // 
            this.chkTopMost.AutoSize = true;
            this.chkTopMost.Location = new System.Drawing.Point(19, 178);
            this.chkTopMost.Name = "chkTopMost";
            this.chkTopMost.Size = new System.Drawing.Size(78, 17);
            this.chkTopMost.TabIndex = 4;
            this.chkTopMost.Text = "始终置顶";
            this.chkTopMost.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 238);
            this.Controls.Add(this.cboLanguage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkTopMost);
            this.Controls.Add(this.chkLinkDragRemove);
            this.Controls.Add(this.chkAutoRun);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtHotKey);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.chkDblClickRun);
            this.Controls.Add(this.chkLoadPlugin);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "系统设置";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkLoadPlugin;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label2;
        private TextBoxEx txtHotKey;
        private System.Windows.Forms.CheckBox chkDblClickRun;
        private System.Windows.Forms.CheckBox chkAutoRun;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboLanguage;
        private System.Windows.Forms.CheckBox chkLinkDragRemove;
        private System.Windows.Forms.CheckBox chkTopMost;
    }
}