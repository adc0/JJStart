using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJStart.Lib
{
    class path
    {
        /// <summary>
        /// 获取绝对路径
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public static string GetAbsolutePath(string basePath, string targetPath)
        {
            try
            {
                Uri baseUri = new Uri(basePath, true);
                Uri targetUri = new Uri(baseUri, targetPath, true);
                return targetUri.AbsolutePath.Replace(@"/", @"\");
            }
            catch (Exception ex) { return targetPath; }
        }

        /// <summary>
        /// 获取相对路径
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public static string GetRelativePath(string basePath, string targetPath)
        {
            try
            {
                Uri baseUri = new Uri(basePath, true);
                Uri targetUri = new Uri(targetPath, true);
                return baseUri.MakeRelativeUri(targetUri).ToString().Replace(@"/", @"\");
            }
            catch (Exception ex) { return targetPath; }
        }

        /// <summary>
        /// 判断路径是不是盘符
        /// </summary>
        /// <returns></returns>
        public static bool IsLogicalDrive(string path)
        {
            bool bRet = false;
            string drive = path.ToLower();

            //先判断下path是否合法
            if (path.Length >= 2 && path.Contains(":"))
            {
                //判断下是不是盘符
                string[] drives = file.GetLogicalDrives();

                //比较
                foreach (string d in drives)
                {
                    if (d.ToLower().Contains(drive))
                    {
                        bRet = true;
                        break;
                    }
                }
            }

            return bRet;
        }
    }
}
