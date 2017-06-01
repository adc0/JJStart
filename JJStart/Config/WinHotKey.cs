using JJPlugin;
using JJStart.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace JJStart
{
    [Serializable]
    public class HotKey
    {
        private int _Id;
        private string _FullName;
        private Keys _Key;
        private WinHotKey.KeyModifiers _Modifiers;

        public HotKey()
        {

        }

        public int Id
        {
            set { _Id = value; }
            get { return _Id; }
        }

        public string FullName
        {
            set { _FullName = value; }
            get { return _FullName; }
        }

        public Keys Key
        {
            set { _Key = value; }
            get { return _Key; }
        }

        public WinHotKey.KeyModifiers Modifiers
        {
            set { _Modifiers = value; }
            get { return _Modifiers; }
        }

        public override string ToString()
        {
            string v = string.Empty;
            if ((this.Modifiers & WinHotKey.KeyModifiers.Control) == WinHotKey.KeyModifiers.Control) v += "Ctrl + ";
            if ((this.Modifiers & WinHotKey.KeyModifiers.Shift) == WinHotKey.KeyModifiers.Shift) v += "Shift + ";
            if ((this.Modifiers & WinHotKey.KeyModifiers.Alt) == WinHotKey.KeyModifiers.Alt) v += "Alt + ";
            v += this.Key.ToString();
            return v;
        }
    }

    [Serializable]
    public class WinHotKey
    {
        private int index = 0x1190;
        private List<HotKey> lstHotKey = null;
        public IntPtr Handle = IntPtr.Zero;

        public WinHotKey()
        {
            lstHotKey = new List<HotKey>();
        }

        public WinHotKey(IntPtr Handle)
        {
            this.Handle = Handle;
            lstHotKey = new List<HotKey>();
        }

        ~WinHotKey()
        {
            foreach(HotKey hotkey in lstHotKey)
            {
                UnregisterHotKey(Handle, hotkey.Id);
            }
        }

        public void Run(int id)
        {
            foreach (HotKey hotkey in lstHotKey)
            {
                if (hotkey.Id == id)
                {
                    process.Start(hotkey.FullName);
                }
            }
        }

        public void ReloadAll()
        {
            foreach (HotKey hotkey in lstHotKey)
            {
                RegisterHotKey(Handle, hotkey.Id, hotkey.Modifiers, hotkey.Key);
            }
        }

        public void UnReloadAll()
        {
            foreach (HotKey hotkey in lstHotKey)
            {
                UnregisterHotKey(Handle, hotkey.Id);
            }
        }

        public bool SetHotKey(HotKey hotKey)
        {
            bool bRet = false;

            if (RegisterHotKey(Handle, index, hotKey.Modifiers, hotKey.Key))
            {
                hotKey.Id = index;
               
                lstHotKey.Add(hotKey);

                index++;

                bRet = true;
            }

            return bRet;
        }

        public bool UnSetHotKey(HotKey hotKey)
        {
            bool bRet = false;

            if (UnregisterHotKey(Handle, hotKey.Id))
            {
                lstHotKey.Remove(hotKey);
                bRet = true;
            }

            return bRet;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        //[Flags()]
        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8,
            // Mask = 0xf
        }
    }
}