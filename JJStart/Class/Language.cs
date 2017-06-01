using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Metro;
using JJPlugin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace JJStart
{
    public class XmlInfo
    {
        private bool tooltip;

        public bool Tooltip
        {
            get { return tooltip; }
            set { tooltip = value; }
        }

        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public XmlInfo()
        {

        }

        public XmlInfo(bool tooltip, string text)
        {
            this.Tooltip = tooltip;
            this.Text = text;
        }
    }

    public class LangInfo
    {
        private string file;

        public string File
        {
            get { return file; }
            set { file = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int lcid;

        public int Lcid
        {
            get { return lcid; }
            set { lcid = value; }
        }

        public LangInfo()
        {

        }

        public LangInfo(string file, string name, int lcid)
        {
            this.file = file;
            this.name = name;
            this.lcid = lcid;
        }
    }

    public class Language
    {
        private bool enable;

        public bool Enable
        {
            get { return enable; }
            set { enable = value; }
        }

        private Dictionary<string, XmlInfo> xmlInfo = new Dictionary<string, XmlInfo>();
        private List<LangInfo> langInfo = new List<LangInfo>();

        public Language()
        {
            if (!Directory.Exists(Global.LanguagesPath))
                this.enable = false;
            else
                this.enable = true;

            if(this.enable)
            {
                foreach (string file in Directory.GetFiles(Global.LanguagesPath))
                {
                    XmlNode xn = null;
                    if (Check(file, out xn))
                    {
                        LangInfo li = new LangInfo();
                        li.File = file;
                        li.Name = xn.Attributes["name"].Value;
                        li.Lcid = Convert.ToInt32(xn.Attributes["lcid"].Value.Replace("0x", ""), 16);
                        langInfo.Add(li);
                    }
                }
            }
        }

        ~Language()
        {
            xmlInfo.Clear();
        }

        public bool Check(string file, out string lcid)
        {
            lcid = "";

            try
            {
                XmlDocument xml_doc = new XmlDocument();
                xml_doc.Load(file);

                XmlNode xml_node = xml_doc.DocumentElement.FirstChild;
                XmlAttribute xa = xml_node.Attributes["name"];
                if (xa == null)
                    return false;

                xa = xml_node.Attributes["version"];
                if (xa == null)
                    return false;

                if (!xa.Value.Equals(Global.Version))
                    return false;

                xa = xml_node.Attributes["lcid"];
                if (xa == null)
                    return false;

                lcid = xa.Value;

                return true;
            }
            catch (Exception ex) { return false; }
        }

        public bool Check(string file, out XmlNode xn)
        {
            xn = null;

            try
            {
                XmlDocument xml_doc = new XmlDocument();
                xml_doc.Load(file);

                XmlNode xml_node = xml_doc.DocumentElement.FirstChild;

                XmlAttribute xa = xml_node.Attributes["name"];
                if (xa == null)
                    return false;

                xa = xml_node.Attributes["version"];
                if (xa == null)
                    return false;

                if (!xa.Value.Equals(Global.Version))
                    return false;

                xa = xml_node.Attributes["lcid"];
                if (xa == null)
                    return false;

                xn = xml_node;

                return true;
            }
            catch (Exception ex) { return false; }
        }

        public bool Load(string name)
        {
            int lcid = Global.LCID;
            if (lcid == 0)
            {
                lcid = System.Globalization.CultureInfo.CurrentUICulture.LCID;
                Global.LCID = lcid;
            }

            foreach(LangInfo li in langInfo)
            {
                if(li.Lcid.Equals(lcid))
                    return Load(li.File, name);
            }
        
            return false;
        }

        public bool Load(string file, string name)
        {
            try
            {
                XmlDocument xml_doc = new XmlDocument();
                xml_doc.Load(file);

                XmlNode xml_node = xml_doc.DocumentElement.FirstChild;

                foreach (XmlNode xml_node_sub in xml_node.ChildNodes)
                {
                    if (xml_node_sub.Name == "Form")
                    {
                        if (xml_node_sub.Attributes["name"].Value != name)
                            continue;

                        xmlInfo.Add(xml_node_sub.Attributes["name"].Value, new XmlInfo(false, xml_node_sub.Attributes["text"].Value));

                        foreach (XmlNode xml_subNode_sub in xml_node_sub.ChildNodes)
                        {
                            switch (xml_subNode_sub.Name)
                            {
                                case "Control":
                                    {
                                        XmlInfo li = new XmlInfo();                                      
                                        XmlAttribute xa = xml_subNode_sub.Attributes["text"];
                                        if (xa == null)
                                        {
                                            li.Tooltip = true;
                                            li.Text = xml_subNode_sub.Attributes["tooltip"].Value;
                                        }
                                        else
                                        {
                                            li.Tooltip = false;
                                            li.Text = xml_subNode_sub.Attributes["text"].Value;
                                        }
                                        xmlInfo.Add(xml_subNode_sub.Attributes["name"].Value, li);
                                    }
                                    break;
                                case "String":
                                    {
                                        XmlInfo li = new XmlInfo();
                                        li.Tooltip = false;
                                        li.Text = xml_subNode_sub.Attributes["text"].Value;
                                        xmlInfo.Add(xml_subNode_sub.Attributes["name"].Value, li);
                                    }
                                    break;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);  
                return false; 
            }
        }

        public void SetBaseItemText(BaseItem bi)
        {
            XmlInfo li = null;

            if (xmlInfo.TryGetValue(bi.Name, out li))
            {
                if(li.Tooltip)
                    bi.Tooltip = li.Text;
                else
                    bi.Text = li.Text;
            }

            if (bi.SubItems == null)
                return;

            foreach (BaseItem bsub in bi.SubItems)
            {
                SetBaseItemText(bsub);
            }
        }

        public void SetControlText(Control ctrl)
        {
            XmlInfo li = null;

            if (xmlInfo.TryGetValue(ctrl.Name, out li))
            {
                ctrl.Text = li.Text;
            }
        }

        public void SetControlsText(Control ctrl)
        {
            SetControlText(ctrl);

            foreach (Control c in ctrl.Controls)
            {
                if(c is TextBoxDropDown)
                {
                    TextBoxDropDown tbdd = c as TextBoxDropDown;
                    foreach (BaseItem bi in tbdd.DropDownItems)
                    {
                        SetBaseItemText(bi);
                    }
                }
                else if (c is ContextMenuBar)
                {
                    ContextMenuBar cmb = c as ContextMenuBar;
                    foreach(BaseItem bi in cmb.Items)
                    {
                        SetBaseItemText(bi);
                    }
                }
                else if(c is MetroStatusBar)
                {
                    MetroStatusBar msb = c as MetroStatusBar;
                    foreach (BaseItem bi in msb.Items)
                    {
                        SetBaseItemText(bi);
                    }
                }
                else
                {
                    SetControlText(c);
                }

                if (c.HasChildren)
                    SetControlsText(c);
            }
        }

        public void SetToolStripText(ToolStrip ms)
        {
            foreach (ToolStripItem tsi in ms.Items)
            {
                XmlInfo li = null;
                if (xmlInfo.TryGetValue(tsi.Name, out li))
                {
                    tsi.Text = li.Text;
                }
            } 
        }

        public string[] GetLanguagesName()
        {
            string[] name = new string[langInfo.Count];

            for (int i = 0; i < langInfo.Count; i++)
            {
                name[i] = langInfo[i].Name;
            }

            return name;
        }

        public string GetLanguagesName(int lcid)
        {
            string name = "";

            for (int i = 0; i < langInfo.Count; i++)
            {
                if(lcid == langInfo[i].Lcid)
                {
                    name = langInfo[i].Name;
                    break;
                }
            }

            return name;
        }

        public int GetLanguagesLcid(string name)
        {
            int lcid = 0;

            for (int i = 0; i < langInfo.Count; i++)
            {
                if(name.Equals(langInfo[i].Name))
                {
                    lcid = langInfo[i].Lcid;
                    break;
                }
            }

            return lcid;
        }

        public string GetString(string name, string default_text)
        {
            string text = "";
            XmlInfo xi = null;

            if (xmlInfo.TryGetValue(name, out xi))
                text = xi.Text;
            else
                text = default_text;

            return text;
        }

        public void SetLanguage(Control ctrl)
        {
            if(this.enable)
            {
                if (this.Load(ctrl.Name))
                {
                    this.SetControlsText(ctrl);
                }
            }
        }

        public void SetLanguage(Control ctrl, params ToolStrip[] args)
        {
            if (this.Enable)
            {
                if (this.Load(ctrl.Name))
                {
                    this.SetControlsText(ctrl);

                    if (args.Length == 0)
                        return;

                    foreach (ToolStrip ms in args)
                    {
                        this.SetToolStripText(ms);
                    }
                }
            }
        }
    }
}