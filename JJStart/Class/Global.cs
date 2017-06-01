using JJPlugin;
using JJStart.Lib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace JJStart
{
    class Global
    {
        public static bool IsAdminRole;
        public static int LCID = 0;
        public static string Version = "0.8";
        public static string LanguagesPath = Application.StartupPath + "\\languages";
        public static string PluginsPath = Application.StartupPath + "\\plugins";
        public static Settings Settings = null;
        public static SettingsManager SettingsManager = null;
        public static SplitContainer SplitContainer = null;

        public static void Init(SplitContainer sc)
        {
            Global.IsAdminRole = uac.IsAdminRole();
            Global.LCID = Global.Settings.LCID;
            Global.SplitContainer = sc;
        }

        public static void LoadSettings(Form f)
        {
            try
            {
                Global.Settings = Global.SettingsManager.LoadConfigFile();
                if (Global.Settings == null)
                {
                    Global.Settings = new Settings(f.Handle);
                    Global.Settings.Size = f.Size;
                    Global.Settings.Location = f.Location;
                }

                if (!Global.Settings.TopMost)
                    f.TopMost = false;
                else
                    f.TopMost = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
