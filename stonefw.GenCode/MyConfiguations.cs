using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Stonefw.CodeGenerate
{
    /// <summary>
    /// 运行配置信息
    /// </summary>
    public static class MyConfiguations
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static MyConfiguations()
        {
            _ConnectionString =
                System.Configuration.ConfigurationManager.ConnectionStrings["DBForEntity"].ConnectionString;
            _NameSpaceList = new NameValueCollection(256);
            LoadNameSpaceList();
        }

        private static string _ConnectionString = null;

        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get { return _ConnectionString; }
        }

        private static NameValueCollection _NameSpaceList = null;

        /// <summary>
        /// 命名空间列表
        /// </summary>
        public static NameValueCollection NameSpaceList
        {
            get { return _NameSpaceList; }
        }

        #region 公开的函数

        /// <summary>
        /// 保存命名空间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="nameSpace"></param>
        public static void SaveNameSpace(string key, string nameSpace)
        {
            if (_NameSpaceList.AllKeys.Contains(key))
            {
                _NameSpaceList.Set(key, nameSpace);
            }
            else
            {
                _NameSpaceList.Add(key, nameSpace);
            }

            SaveNameSpaceList();
        }

        /// <summary>
        /// 删除命名空间
        /// </summary>
        /// <param name="key"></param>
        public static void DeleteNameSpace(string key)
        {
            if (_NameSpaceList.AllKeys.Contains(key))
            {
                _NameSpaceList.Remove(key);
                SaveNameSpaceList();
            }
        }

        #endregion

        #region 私有的函数

        /// <summary>
        /// 加载命名空间列表
        /// </summary>
        private static void LoadNameSpaceList()
        {
            _NameSpaceList.Clear();

            string filePath = RuntimeInfo.ApplicationFolderPath + "configs\\namespacelist.ini";
            if (!System.IO.File.Exists(filePath))
            {
                return;
            }
            System.IO.StreamReader sr = new System.IO.StreamReader(filePath, Encoding.UTF8);
            string line = null;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim();
                if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                {
                    continue;
                }
                string[] items = line.Split(':');
                if (items == null || items.Length < 2)
                {
                    continue;
                }
                _NameSpaceList.Add(items[0], items[1]);
            }
            sr.Close();
        }

        /// <summary>
        /// 保存命名空间列表
        /// </summary>
        private static void SaveNameSpaceList()
        {
            string filePath = RuntimeInfo.ApplicationFolderPath + "configs\\namespacelist.ini";
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath, false, Encoding.UTF8);
            for (int i = 0; i < _NameSpaceList.Keys.Count; i++)
            {
                sw.Write(string.Format("{0}:{1}\r\n", _NameSpaceList.Keys[i], _NameSpaceList[_NameSpaceList.Keys[i]]));
            }
            sw.Close();
        }

        #endregion
    }
}