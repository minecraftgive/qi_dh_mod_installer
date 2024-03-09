using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace qi_dh_mod_installer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        string path = ":\\SteamLibrary\\steamapps\\common\\Dread Hunger\\DreadHunger\\Content\\Paks";
        string version = null;
        string game_exe_path = null;
        public MainWindow()
        {
            InitializeComponent();
            //if (MessageBox.Show("text", "title", MessageBoxButton.YesNo) == MessageBoxResult.Yes) { MessageBox.Show("yes"); }

        }

        public void ReadVersionFile(String resource)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            BufferedStream input = new BufferedStream(assembly.GetManifestResourceStream(resource));
            version = input.ToString();
            //FileStream output = new FileStream(path, FileMode.Create);
            //byte[] data = new byte[1024];
            //int lengthEachRead;
            //while ((lengthEachRead = input.Read(data, 0, data.Length)) > 0)
            //{
            //    //output.Write(data, 0, lengthEachRead);
            //    version[lengthEachRead] = data;
            //}
        }
        public int install()
        {
            //string work_path=path+"\\~mods";
            if (!Directory.Exists(path + "\\~mods")) { tb_t.Text += "no ~mods";  Directory.CreateDirectory(path + "\\~mods"); /*work_path = path+"\\~mods";*/ }
            ExtractFile("qi_dh_mod_installer.Resources.Te_TotemMod_Client_p.pak", path+ "\\~mods\\Te_TotemMod_Client_p.pak");
            ExtractFile("qi_dh_mod_installer.Resources.Te_TotemMod_Client_p.sig", path+ "\\~mods\\Te_TotemMod_Client_p.sig");
            //MD5.Create();
            MessageBox.Show("安装完成!!! \n Installed successfully!");
            return 0;
        }
        ///copy engineer
        /// <summary>
        /// 释放内嵌资源至指定位置
        /// </summary>
        /// name="resource_o"嵌入的资源，此参数写作：命名空间.文件夹名.文件名.扩展名
        /// 
        /// <param name="resource">嵌入的资源，此参数写作：文件名.扩展名</param>
        /// <param name="path">释放到位置</param>
        private void ExtractFile(String resource, String path)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            BufferedStream input = new BufferedStream(assembly.GetManifestResourceStream(resource));
            FileStream output = new FileStream(path, FileMode.Create);
            byte[] data = new byte[1024];
            int lengthEachRead;
            while ((lengthEachRead = input.Read(data, 0, data.Length)) > 0)
            {
                output.Write(data, 0, lengthEachRead);
            }
            output.Flush();
            output.Close();
        }

        public bool isInstall()
        {
            if(File.Exists(path+ "\\~mods\\Te_TotemMod_Client_p.pak")|File.Exists(path+ "\\~mods\\Te_TotemMod_Client_p.sig"))
            {
                //MessageBox aaa = MessageBox.Show("text", "title", MessageBoxButton.YesNo);
                //MessageBox.Show("text", "title", MessageBoxButton.YesNo);
                if (MessageBox.Show("重装mod? \nreinstall mods?", "已安装 \nInstalled", MessageBoxButton.YesNo) == MessageBoxResult.No) { MessageBox.Show("已取消"); return false; }
            }
            return true;
        }
        public bool Check_And_Auto_Find_Path()
        {
            for (char i = 'A'; i <= 'Z'; i++)
            {
                if (i == 'C')
                {
                    if (Directory.Exists("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Dread Hunger\\DreadHunger\\Content\\Paks"))
                    { B_install.IsEnabled = true; path = i + path; return true; }
                    if (Directory.Exists("C:\\Program Files\\Steam\\steamapps\\common\\Dread Hunge\\DreadHunger\\Content\\Paks"))
                    { B_install.IsEnabled = true; path = i + path; return true; }

                }

                tb_t.Text += i + path + '\n';

                if (Directory.Exists(i + path))
                { B_install.IsEnabled = true; path = i + path; return true; }
            }

            return false;
        }
        public bool Manually_Get_Path()
        {
            //bool select = false;
            OpenFileDialog game_exe_path_dia = new OpenFileDialog();
            game_exe_path_dia.Filter = "DreadHunger.exe|DreadHunger.exe";

            //if ((bool)game_exe_path_dia.ShowDialog()) { game_exe_path = game_exe_path_dia.FileName; MessageBox.Show(game_exe_path.Remove(game_exe_path.Length-16,16)); }
            //else { MessageBox.Show("no"); }
            //game_exe_path_dia.ShowDialog();
            //select = (bool)DialogResult;
            //if (select) { MessageBox.Show("selected!"); }
            //else { MessageBox.Show("no"); }
            if ((bool)game_exe_path_dia.ShowDialog()) 
            {
                game_exe_path = game_exe_path_dia.FileName;
                try
                {
                    path = game_exe_path.Remove(game_exe_path.Length - 16, 16);
                    return true;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("目录错误! \n若目录正确请反馈bug \nWrong Directory! \nIf you got the right path please contact to report this bug");
                }
                /*MessageBox.Show(game_exe_path);*/ 
            }
            else { if (MessageBox.Show("重新选择游戏目录? \nManually select game path?", "自动获取目录失败! \nAuto Get Path Failed!", MessageBoxButton.YesNo) == MessageBoxResult.No) { MessageBox.Show("已取消,程序即将关闭 \nCanceled,Exting..."); Environment.Exit(0); }
                else { return false; }
            }

            return false;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            
            //string path = ":\\AMD";
            //string path = ":\\SteamLibrary\\steamapps\\common\\Dread Hunger";
            B_install.IsEnabled = false;
            if (Check_And_Auto_Find_Path()) { tb_t.Text += "good:" + path; }
            else { if (MessageBox.Show("手动选择游戏目录? \nManually select game path?", "自动获取目录失败! \nAuto Get Path Failed!", MessageBoxButton.YesNo) == MessageBoxResult.Yes){ if (!Manually_Get_Path()) { Manually_Get_Path(); }  } }
            if (File.Exists(path + "\\~mods\\Te_TotemMod_Client_p.pak") | File.Exists(path + "\\~mods\\Te_TotemMod_Client_p.sig"))
                {B_install.Content = "重装 \nReinstall";}
            //ReadVersionFile("qi_dh_mod_installer.Resources.version");
            //tb_version.Text = version;
            //ShowDialog();

        }

        private void B_install_Click(object sender, RoutedEventArgs e)
        {
            //tb_t.Text = path;
            if (isInstall())
            {

                try
                { 
                    install();
                    if (File.Exists(path + "\\~mods\\Te_TotemMod_Client_p.pak") | File.Exists(path + "\\~mods\\Te_TotemMod_Client_p.sig"))
                    { B_install.Content = "重装 \nReinstall"; }
                }
                catch (Exception ex)
                {/* MessageBox.Show(ex.ToString()); */}
            }
        }

        private void b_log_Click(object sender, RoutedEventArgs e)
        {
            if (tb_t.Visibility==Visibility.Hidden) tb_t.Visibility = Visibility.Visible;
            else tb_t.Visibility = Visibility.Hidden;
            if (File.Exists(path + "\\~mods\\Te_TotemMod_Client_p.pak") | File.Exists(path + "\\~mods\\Te_TotemMod_Client_p.sig"))
            { B_install.Content = "重装 \nReinstall"; }
        }
    }
}
