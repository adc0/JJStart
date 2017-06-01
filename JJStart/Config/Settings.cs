using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using DevComponents.DotNetBar;

namespace JJStart
{
    [Serializable]

    public class Settings
    {
        /// <summary>
        /// �Ƿ��ǵ�һ������
        /// </summary>
        public bool FirstRun;

        /// <summary>
        /// ����LCID
        /// </summary>
        public int LCID;

        /// <summary>
        /// �Ƿ���ز��
        /// Ĭ�ϼ��ز��
        /// </summary>
        public bool LoadPlugin;

        /// <summary>
        /// ��־�Ƿ�˫��������
        /// </summary>
        public bool DoubleClickRun;

        /// <summary>
        /// ��link�ļ������ƶ�ģʽ
        /// </summary>
        public bool LinkDragRemove;

        /// <summary>
        /// �Ƿ��ö�
        /// </summary>
        public bool TopMost;

        /// <summary>
        /// ����Ŀ��
        /// </summary>
        public Size Size;

        /// <summary>
        /// ����λ����Ϣ
        /// </summary>
        public Point Location;

        /// <summary>
        /// ���������Ŀ���ļ���Ϣ����
        /// </summary>
        public Category Category;

        /// <summary>
        /// Ƥ����Ϣ
        /// </summary>
        public eStyle ManagerStyle;

        /// <summary>
        /// custom�Զ�����ɫ��Ϣ
        /// </summary>
        public Color ManagerColorTint;

        /// <summary>
        /// �������غ����Ŀ�ݼ�
        /// </summary>
        public HotKey HotKey;

        /// <summary>
        /// �ȼ�����
        /// </summary>
        public WinHotKey WinHotKey;

        /// <summary>
        /// ����Ǹ���ť����ѡ
        /// 0:���У�1:������2:�ٶȣ�3:�ȸ�
        /// </summary>
        public int ButtonCheckedIndex;

        /// <summary>
        /// ����尴ť
        /// </summary>
        public List<ItemSubInfo> MainPanelItems;

        public Settings(IntPtr Handle)
        {
            //����ΪĬ��ֵ
            this.Category = new Category();
            this.FirstRun = true;
            this.Location = new Point(0, 0);
            this.ManagerStyle = eStyle.Metro;
            this.ManagerColorTint = Color.White;
            this.ButtonCheckedIndex = 0;
            this.LoadPlugin = true;
            this.DoubleClickRun = false;
            this.LinkDragRemove = true;
            this.TopMost = true;
            this.LCID = 0;
            this.HotKey = new HotKey();
            this.HotKey.Modifiers = WinHotKey.KeyModifiers.Alt;
            this.HotKey.Key = Keys.D1;
            this.WinHotKey = new WinHotKey(Handle);
            this.WinHotKey.SetHotKey(this.HotKey);
            this.MainPanelItems = new List<ItemSubInfo>();
        }
    }
}
