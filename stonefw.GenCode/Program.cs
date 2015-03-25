using System;
using System.Windows.Forms;

namespace stonefw.GenCode
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //获取运行信息
            string applicationPath = Application.ExecutablePath;
            string folder = System.IO.Path.GetDirectoryName(applicationPath);
            if (!folder.EndsWith("\\"))
            {
                folder += "\\";
            }
            RuntimeInfo.ApplicationFolderPath = folder;
            //配置目录
            string configFolder = folder + "configs";
            if (!System.IO.Directory.Exists(configFolder))
            {
                System.IO.Directory.CreateDirectory(configFolder);
            }

            //启动窗口
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
