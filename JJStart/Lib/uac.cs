using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace JJStart.Lib
{
    class uac
    {
        [DllImport("Shell32.dll", SetLastError = false)]
        public static extern Int32 SHGetStockIconInfo(SHSTOCKICONID siid, SHGSI uFlags, ref SHSTOCKICONINFO psii);

        public enum SHSTOCKICONID : uint
        {
            SIID_SHIELD = 77
        }

        [Flags]
        public enum SHGSI : uint
        {
            SHGSI_ICON = 0x000000100,
            SHGSI_SMALLICON = 0x000000001
        }

        [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SHSTOCKICONINFO
        {
            public UInt32 cbSize;
            public IntPtr hIcon;
            public Int32 iSysIconIndex;
            public Int32 iIcon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szPath;
        }

        /// <summary>
        /// 获取系统uac图标
        /// </summary>
        /// <returns></returns>
        public static Icon GetUacIcon()
        {
            SHSTOCKICONINFO iconResult = new SHSTOCKICONINFO();

            iconResult.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(iconResult);

            SHGetStockIconInfo(SHSTOCKICONID.SIID_SHIELD, SHGSI.SHGSI_ICON | SHGSI.SHGSI_SMALLICON, ref iconResult);

            return Icon.FromHandle(iconResult.hIcon);
        }

        /// <summary>
        /// uac权限判断
        /// </summary>
        /// <returns></returns>
        public static bool IsAdminRole()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);

            //检测是否具有管理员权限
            if (principal.IsInRole(WindowsBuiltInRole.Administrator))
                return true;
            else
                return false;
        }
    }
}
