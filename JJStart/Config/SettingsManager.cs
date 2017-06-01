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
    public class SettingsManager
    {
        public string ConfigPath;

        public SettingsManager()
        {
            ConfigPath = Application.StartupPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath) + ".cfg";
        }

        public bool ExistsConfigFile()
        {
            if (System.IO.File.Exists(ConfigPath))
                return true;
            else
                return false;
        }

        public Settings LoadConfigFile()
        {
            Settings sc = null;
            FileStream fs = null;

            try
            {
                fs = new FileStream(ConfigPath, FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                sc = (Settings)bf.Deserialize(fs);
            }
            catch (Exception ex)
            {
                sc = null;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

            return sc;
        }

        public bool Save(Settings sc)
        {
            try
            {
                using (FileStream fs = new FileStream(ConfigPath, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, sc);
                    fs.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
