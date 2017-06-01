using JJPlugin;
using JJStart.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace JJStart
{
    /*
    *	ProgramInfo
    *
    *	This class is just for getting information about the application.
    *	Each assembly has a GUID, and that GUID is useful to us in this application,
    *	so the most important thing in this class is the AssemblyGuid property.
    *
    *	GetEntryAssembly() is used instead of GetExecutingAssembly(), so that you
    *	can put this code into a class library and still get the results you expect.
    *	(Otherwise it would return info on the DLL assembly instead of your application.)
    */
    public static class ProgramInfo
    {
        public static string AssemblyGuid
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
                if (attributes.Length == 0)
                {
                    return String.Empty;
                }
                return ((System.Runtime.InteropServices.GuidAttribute)attributes[0]).Value;
            }
        }
        public static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().CodeBase);
            }
        }
    }

    /* All of the code below can optionally be put in a class library and reused with all your applications. */

    /*
    *	SingeInstance
    *
    *	This is where the magic happens.
    *
    *	Start() tries to create a mutex.
    *	If it detects that another instance is already using the mutex, then it returns FALSE.
    *	Otherwise it returns TRUE.
    *	(Notice that a GUID is used for the mutex name, which is a little better than using the application name.)
    *
    *	If another instance is detected, then you can use ShowFirstInstance() to show it
    *	(which will work as long as you override WndProc as shown above).
    *
    *	ShowFirstInstance() broadcasts a message to all windows.
    *	The message is WM_SHOWFIRSTINSTANCE.
    *	(Notice that a GUID is used for WM_SHOWFIRSTINSTANCE.
    *	That allows you to reuse this code in multiple applications without getting
    *	strange results when you run them all at the same time.)
    *
    */
    public static class SingleInstance
    {
        public static readonly uint WM_SHOWFIRSTINSTANCE = win32.RegisterWindowMessage(ProgramInfo.AssemblyGuid.ToString());
        static Mutex mutex;

        static public bool Start()
        {
            bool onlyInstance = false;
            string mutexName = String.Format("Local\\{0}", ProgramInfo.AssemblyGuid);

            // if you want your app to be limited to a single instance
            // across ALL SESSIONS (multiple users & terminal services), then use the following line instead:
            // string mutexName = String.Format("Global\\{0}", ProgramInfo.AssemblyGuid);

            mutex = new Mutex(true, mutexName, out onlyInstance);
            return onlyInstance;
        }

        static public void ShowFirstInstance()
        {
            win32.SendMessage((IntPtr)win32.HWND_BROADCAST, WM_SHOWFIRSTINSTANCE, IntPtr.Zero, IntPtr.Zero);
        }

        static public void Stop()
        {
            mutex.ReleaseMutex();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    static class Program 
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            /*放弃对XP的支持*/
            //判断一下系统平台
            Version currentVersion = Environment.OSVersion.Version;
            Version compareVersion = new Version("6.0");

            if (currentVersion.CompareTo(compareVersion) < 0)
            {
                MessageBox.Show("Unsupported operating system platforms.");
                return;
            }

            //首先判断有无参数
            if(args.Length == 2)
            {
                if (args[0].ToLower().Equals("-loadplugin"))
                {
                    try
                    {
                        Assembly asm = Assembly.LoadFile(args[1]);
                        foreach (Type t in asm.GetTypes())
                        {
                            if (MainForm.IsValidPlugin(t))
                            {
                                Application.EnableVisualStyles();
                                Application.SetCompatibleTextRenderingDefault(false);
                                Application.Run(((IPlugin)asm.CreateInstance(t.FullName)).MainForm);
                                break;
                            }
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }

                return;
            }

            //单独运行一个实例
            if (!SingleInstance.Start())
            {
                SingleInstance.ShowFirstInstance();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            SingleInstance.Stop();   
        }
    }
}