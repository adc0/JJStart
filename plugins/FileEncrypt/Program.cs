using FileEncrypt.Properties;
using JJPlugin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FileEncrypt
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        [PluginInfo("FileEncrypt", "FileEncrypt", "1.0", "FileEncrypt", "http://FileEncrypt.org")]
        class Plugin : IPlugin
        {
            public bool IsElevationRequired
            {
                get { return false; }
            }

            public Form MainForm
            {
                get { return new Form1(); }
            }

            public Icon GetIcon()
            {
                return Resources.app;
            }

            public void OnDestory()
            {
                throw new NotImplementedException();
            }

            public void OnLoad()
            {
                MainForm.Show();
            }
        }
    }
}
