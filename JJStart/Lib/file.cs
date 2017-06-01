using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace JJStart.Lib
{
 public class file
    {
        /// <summary>
        /// 资源管理器中打开
        /// </summary>
        /// <param name="strPath"></param>
        public static void ExplorerFile(string strPath)
        {
            string strCmdLine = "explorer.exe /select," + strPath;
            win32.WinExec(strCmdLine, win32.SW_SHOW);
        }

        /// <summary>
        /// 获取本地磁盘名称
        /// </summary>
        /// <returns></returns>
        public static string[] GetLogicalDrives()
        {
            return Directory.GetLogicalDrives();
        }       

        /// <summary>
        /// 从文件扩展名得到文件关联图标
        /// </summary>
        /// <param name="fileName">文件名或文件扩展名</param>
        /// <param name="smallIcon">是否是获取小图标，否则是大图标</param>
        /// <returns>图标</returns>
        public static Icon GetFileIcon(string fileName, bool smallIcon)
        {
            win32.SHFILEINFO sfi = new win32.SHFILEINFO();
            Icon icon = null;

            //到这里确定下是否是文件夹
            int nTotal = (int)win32.SHGetFileInfo(fileName, 100, ref sfi, 0, (uint)(smallIcon ? 273 : 272));

            //判断是否成功
            if (nTotal > 0)
                icon = (Icon)Icon.FromHandle(sfi.hIcon).Clone();

            //销毁
            if(sfi.hIcon != IntPtr.Zero)
                win32.DestroyIcon(sfi.hIcon);

            return icon;
        }

        /// <summary>
        /// 从文件扩展名得到文件关联图标
        /// </summary>
        /// <param name="fileName">文件名或文件扩展名</param>
        /// <param name="smallIcon">是否是获取小图标，否则是大图标</param>
        /// <returns>图标</returns>
        public static Icon GetFileIconEx(string fileName, bool smallIcon)
        {
            Icon icon = null;

            //到这里确定下是否是文件夹
            if (!path.IsLogicalDrive(fileName) && Directory.Exists(fileName))
                icon = file.GetFolderIcon(!smallIcon, false);
            else
                icon = file.GetFileIcon(fileName, smallIcon);

            return icon;
        }

        /// <summary>
        /// Used to access system folder icons.
        /// </summary>
        /// <param name="largeIcon">Specify large or small icons.</param>
        /// <param name="openFolder">Specify open or closed FolderType.</param>
        /// <returns>The requested Icon.</returns>
        public static Icon GetFolderIcon(Boolean largeIcon, Boolean openFolder)
        {
            win32.SHFILEINFO sfi = new win32.SHFILEINFO();
            Icon icon = null;

            uint flags = win32.SHGFI_ICON | win32.SHGFI_USEFILEATTRIBUTES;
            flags |= openFolder ? win32.SHGFI_OPENICON : 0;
            flags |= largeIcon ? win32.SHGFI_LARGEICON : win32.SHGFI_SMALLICON;

            //获取Windows文件夹的图标
            string strWinDir = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.System)).FullName;

            //获取Windows目录图标（xp、win7等任何系统都有的）
            int nTotal = (int)win32.SHGetFileInfo(strWinDir, win32.FILE_ATTRIBUTE_DIRECTORY, ref sfi, (uint)Marshal.SizeOf(sfi), flags);

            //判断是否成功
            if (nTotal > 0)
                icon = (Icon)Icon.FromHandle(sfi.hIcon).Clone();

            //销毁
            if (sfi.hIcon != IntPtr.Zero)
                win32.DestroyIcon(sfi.hIcon);

            return icon;
        }

        /// <summary>
        /// 打开文件属性对话框
        /// </summary>
        /// <param name="strPath"></param>
        public static void ShowFileProperty(string strPath)
        {
            win32.SHELLEXECUTEINFO sei = new win32.SHELLEXECUTEINFO();

            //填充结构体
            sei.cbSize = Marshal.SizeOf(sei);
            sei.fMask = win32.SEE_MASK_INVOKEIDLIST | win32.SEE_MASK_NOCLOSEPROCESS | win32.SEE_MASK_FLAG_NO_UI;
            sei.dwHotKey = 0;
            sei.hIcon = new IntPtr(0);
            sei.hInstApp = new IntPtr(0);
            sei.hkeyClass = new IntPtr(0);
            sei.hProcess = new IntPtr(0);
            sei.hwnd = new IntPtr(0);
            sei.lpClass = "";
            sei.lpDirectory = "";
            sei.lpFile = strPath;
            sei.lpIDList = new IntPtr(0);
            sei.lpParameters = "";
            sei.lpVerb = "properties";
            sei.nShow = win32.SW_NORMAL;

            win32.ShellExecuteEx(ref sei);
        }
    }
}
