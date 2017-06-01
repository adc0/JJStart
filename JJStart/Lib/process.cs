using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JJStart.Lib
{
    class process
    {
        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="fileName"></param>
        public static void Start(string fileName)
        {
            try
            {
                //解决相对路径问题
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = fileName;
                psi.WorkingDirectory = Path.GetDirectoryName(fileName);
                Process.Start(psi);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        public static void Start(string fileName, string arguments)
        {
            try
            {
                Process.Start(fileName, arguments);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// 以管理员权限打开
        /// </summary>
        /// <param name="fileName"></param>
        public static void StartByRunas(string fileName)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.Verb = "runas";
                psi.FileName = fileName;
                Process.Start(psi);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// 以管理员权限打开
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        public static void StartByRunas(string fileName, string arguments)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.Verb = "runas";
                psi.FileName = fileName;
                psi.Arguments = arguments;
                Process.Start(psi);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
