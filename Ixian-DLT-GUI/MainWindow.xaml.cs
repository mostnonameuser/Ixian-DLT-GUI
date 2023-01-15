using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using Microsoft.VisualBasic.Logging;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using static System.Windows.Forms.LinkLabel;
using static System.Net.Mime.MediaTypeNames;
using System.Globalization;
using System.IO.Compression;
using System.Security.Principal;

namespace Ixian_DLT_GUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ifConsole.IsChecked = Properties.Settings.Default.showConsole;
            if (Properties.Settings.Default.autorun == true)
            {
                initStart();
            };
        }

        //variables
        [DllImport("kernel32.dll")]
        static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, SymbolicLink dwFlags);
        public Status? dltStat = new Status();
        public Balance? dltBal = new Balance();
        public static bool active = false;
        public static bool aMon = false;
        public static bool hide = false;
        private static Thread m = null;
        private static Thread t = null;
        private static Thread f = null;
        private static Thread stat = null;
        public static string ConOut;
        public static string wallet;
        public static FileStream fstream;
        public enum SymbolicLink
        {
            File = 0,
            Directory = 1
        }

        //Common functions
        private async void initStart()
        {
            if (!File.Exists(Properties.Settings.Default.dltDir.ToString() + "\\IxianDLT.exe") || Properties.Settings.Default.dltpass == "")
            {
                MessageBox.Show("Please make initial settings before starting DLT");
            }
            else
            {
                startBtn.Visibility = Visibility.Hidden;
                setBtn.Visibility = Visibility.Hidden;
                stopBtn.Visibility = Visibility.Visible;
                ifConsole.Visibility = Visibility.Hidden;
                repBtn.Visibility = Visibility.Visible;
                if (ifConsole.IsChecked == true)
                {
                    hide = false;
                }
                else
                {
                    hide = true;
                }
                active = await Commands.psCom();
                _ = checkIfActive();
                _ = startNode("");
                checkStatus();
            }
        }
        private async Task startNode(string param)
        {
            if (!active)
            {
                if (File.Exists(@Properties.Settings.Default.dltDir + "\\IxianDLT.exe"))
                {
                    await prepConfig();
                    string symbolicLink = "";
                    if (Properties.Settings.Default.testnet == true)
                    {
                        DirectoryInfo pathInfo = new DirectoryInfo(@Properties.Settings.Default.dltDir + "\\data-testnet");
                        if (Directory.Exists(@Properties.Settings.Default.dltDir + "\\data-testnet"))
                        {
                            if (pathInfo.ResolveLinkTarget(true) == null || pathInfo.ResolveLinkTarget(true).ToString() != @Properties.Settings.Default.dataDir)
                            {
                                Directory.Delete(@Properties.Settings.Default.dltDir + "\\data-testnet", true);
                            }
                        }
                        symbolicLink = @Properties.Settings.Default.dltDir + "\\data-testnet";
                        string fileName = @Properties.Settings.Default.dataDir;
                        CreateSymbolicLink(symbolicLink, fileName, SymbolicLink.Directory);
                    }
                    else
                    {
                        DirectoryInfo pathInfo = new DirectoryInfo(@Properties.Settings.Default.dltDir + "\\data");
                        if (Directory.Exists(@Properties.Settings.Default.dltDir + "\\data"))
                        {
                            if (pathInfo.ResolveLinkTarget(true) == null || pathInfo.ResolveLinkTarget(true).ToString() != @Properties.Settings.Default.dataDir)
                            {
                                Directory.Delete(@Properties.Settings.Default.dltDir + "\\data", true);
                            }
                        }
                        symbolicLink = @Properties.Settings.Default.dltDir + "\\data";
                        string fileName = @Properties.Settings.Default.dataDir;
                        CreateSymbolicLink(symbolicLink, fileName, SymbolicLink.Directory);
                    }
                    Process.Start(Commands.GetParamsDlt(Commands.PutCliParams(param), hide));
                    active = true;
                    getlogStart();
                    if (LogExp.IsExpanded == true)
                    {
                        aMon = true;
                        //    getlogStart();
                        mStart();
                    }
                }
                else
                {
                    MessageBox.Show("IxianDLT.exe is missing!\nReload through Settings window");
                }
            }
            else
            {
                getlogStart();
                MessageBox.Show("Another instance of IxianDLT.exe is running...");
                if (LogExp.IsExpanded == true)
                {
                    aMon = true;
                    //  getlogStart();
                    mStart();
                }
            }


        }
        private async Task prepConfig()
        {
            File.Delete(@Properties.Settings.Default.dltDir + "\\ixian.cfg");
            File.WriteAllText(@Properties.Settings.Default.dltDir + "\\ixian.cfg", Commands.makeConfig());
        }
        public static bool IsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        //window elements
        private void Console_CheckedChanged(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.showConsole = (bool)ifConsole.IsChecked;

            Properties.Settings.Default.Save();
        }
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            LogExp.Header = "DLT Log";
            if (active)
            {
                LogExp.IsExpanded = true;
                aMon = true;
                getlogStart();
                mStart();
            }
            else
            {
                Dispatcher.Invoke(() => ConsText.Text = "");
            }

        }
        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            LogExp.Header = "DLT Log (Disabled)";
            LogExp.IsExpanded = false;
            aMon = false;
        }
        private async void Start_Click(object sender, RoutedEventArgs e)
        {   
            if (IsAdministrator())
            {
                initStart();
            }
            else
            {
                MessageBox.Show("Please restart software as Administrator");
            }
            
        }
        private async void Stop_Click(object sender, RoutedEventArgs e)
        {
            stopBtn.IsEnabled = false;
            int count = 0;
            network.stopDLT();
            while (active == true )
            {
                Thread.Sleep(100);
                if ( count > 100 )
                {
                    await Commands.psKill();
                    Thread.Sleep(500);
                }
                active = await Commands.psCom();
                count++;
            }
            stopBtn.IsEnabled = true;
            stopBtn.Visibility = Visibility.Hidden;
            startBtn.Visibility = Visibility.Visible;
            setBtn.Visibility = Visibility.Visible;
            ifConsole.Visibility = Visibility.Visible;
            repBtn.Visibility = Visibility.Hidden;
           
            if (LogExp.IsExpanded == true)
            {
                aMon = false;
                getlogStart();
                mStart();
            }
            ConsText.Text = "DLT off";
            Dispatcher.Invoke(() => apiLink.Text = "n/a");
            Dispatcher.Invoke(() => connections.Content = "n/a");
            Dispatcher.Invoke(() => blockdif.Content = "n/a");
            Dispatcher.Invoke(() => blockdif.ToolTip = "n/a");
            Dispatcher.Invoke(() => syncProgrBar.Value = (double)0);
            Dispatcher.Invoke(() => balance.Content = "n/a");
            Dispatcher.Invoke(() => spow.Content = "n/a");
            Dispatcher.Invoke(() => presences.Content = "n/a");
            Dispatcher.Invoke(() => status.Content = "DLT Status: Offline");
            //MessageBox.Show("Node stopped");
        }
        private async void Report_Click(object sender, RoutedEventArgs e)
        {
            string savedName = "ErrorReport_" + dltStat.result.nodeVer + ".zip";
            bool ok = false;

            while (!ok)
            {
                try
                {
                    using FileStream sourceStream = new FileStream(@Properties.Settings.Default.dltDir + "\\ixian.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                   // string savedName = "ErrorReport_" + dltStat.result.nodeVer + ".zip";
                    using FileStream targetStream = File.Create(Properties.Settings.Default.dataDir + "\\" + savedName);
                    using GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress);
                    await sourceStream.CopyToAsync(compressionStream);
                    ok = true;
                }
                finally
                {
                }
            }
            MessageBox.Show($"Report prepare {Properties.Settings.Default.dltDir + "\\ixian.log"} complete.\nSaved to {Properties.Settings.Default.dataDir + "\\" + savedName}");
             
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            bool isWindowOpen = false;

            foreach (Window w in System.Windows.Application.Current.Windows)
            {
                if (w is SettingsWindow)
                {
                    isWindowOpen = true;
                    w.Activate();
                }
            }
            if (!isWindowOpen)
            {
                SettingsWindow setWin = new SettingsWindow();
                setWin.Show();
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            aMon = false;
            active = false;
            Environment.Exit(0);
        }
        public void hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", "http://" + Properties.Settings.Default.loIp + ":" + Properties.Settings.Default.apiPort + "/");

        }
      

        //threads
        private void getlogStart()
        {
            t = new Thread(getlogStartProcess);
            t.Start();
        }
        private static void getlogStartProcess()
        {
            while (aMon)
            {
                try
                {
                    fstream = new FileStream(@Properties.Settings.Default.dltDir + "\\ixian.log", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader sr = new StreamReader(fstream);

                    //////////////////////// найти в логе КОШЕЛЁК
                    ConOut = string.Join("\n", sr.ReadToEnd().Split("\n", StringSplitOptions.None).Reverse().Take(13).Reverse().Take(12).ToArray());
                    fstream.Close();
                }
                finally
                {
                }
                Thread.Sleep(200);
            }
        }
        private void mStart()
        {
            m = new Thread(MonProc);
            m.Start();
        }
        private void MonProc()
        {
            Thread.Sleep(1000);
            while (aMon)
            {
                try
                {
                    if (ConOut != null)
                    {
                        Dispatcher.Invoke(() => ConsText.Text = ConOut.ToString() + "\n");
                    }
                }
                finally { }
                //MessageBox.Show("Mon is running...");
                Thread.Sleep(300);
            }
        }
        private async Task checkIfActive()
        {
            f = new Thread(failureCheck);
            f.Start();
        }
        private static async void failureCheck()
        {
            while (true)
            {
                active = await Commands.psCom();
                Thread.Sleep(5000);
            }
        }
        private void checkStatus()
        {
            m = new Thread(statusProc);
            m.Start();
        }
        private async void statusProc()
        {
            Thread.Sleep(1000);
            //TextBox textBox = (TextBox)status;
            while (active)
            {
                try
                {
                    dltBal = JsonConvert.DeserializeObject<Balance>(await network.balanceDLT());
                    dltStat = JsonConvert.DeserializeObject<Status>(await network.statusDLT());
                }
                catch
                {
                    Dispatcher.Invoke(() => apiLink.Text = "n/a");
                    Dispatcher.Invoke(() => connections.Content = "n/a");
                    Dispatcher.Invoke(() => blockdif.Content = "n/a");
                    Dispatcher.Invoke(() => blockdif.ToolTip = "n/a");
                    Dispatcher.Invoke(() => syncProgrBar.Value = (double)0);
                    Dispatcher.Invoke(() => balance.Content = "n/a");
                    Dispatcher.Invoke(() => spow.Content = "n/a");
                    Dispatcher.Invoke(() => presences.Content = "n/a");
                }
                finally
                {
                    if ( dltStat.result.update == "" )
                    {
                        Dispatcher.Invoke(() => status.Content = "DLT " + dltStat.result.nodeVer + " Status: " + dltStat.result.status);
                    }
                    else
                    {
                        Dispatcher.Invoke(() => status.Content = "(NEED UPDATE) " + dltStat.result.nodeVer + " to " + dltStat.result.update + " Status: " + dltStat.result.status);
                    }
                    Dispatcher.Invoke(() => apiLink.Text = "http://" + Properties.Settings.Default.loIp + ":" + Properties.Settings.Default.apiPort);
                    Dispatcher.Invoke(() => connections.Content = dltStat.result.netClients.Length.ToString() + "/" + dltStat.result.netServers.Length.ToString());
                    Dispatcher.Invoke(() => blockdif.Content = (dltStat.result.netBlock - dltStat.result.curblock).ToString());
                    Dispatcher.Invoke(() => blockdif.ToolTip = dltStat.result.curblock.ToString() + " / " + dltStat.result.netBlock.ToString());
                    Dispatcher.Invoke(() => syncProgrBar.Value = (double)100 - (double)((float)((dltStat.result.netBlock - dltStat.result.curblock) * (float)100 / (float)11000)));
                    Dispatcher.Invoke(() => balance.Content = (float)Math.Round(float.Parse(dltBal.result, CultureInfo.InvariantCulture), 2) + " IXI");
                    Dispatcher.Invoke(() => spow.Content = (dltStat.result.spow / 1024).ToString() + " Kh");
                    Dispatcher.Invoke(() => presences.Content = (dltStat.result.presences).ToString());
                    if (dltStat.result.status == "ErrorLongTimeNoBlock" && (int)(dltStat.result.netBlock - dltStat.result.curblock) > (int)15)
                    {
                        if (Properties.Settings.Default.chatID != "" && Properties.Settings.Default.botApi != "")
                        {
                            noBlockReaction();
                        }
                    }
                }
                
              
                Thread.Sleep(10000);
            }
        }
       
        //status reactions
        private async void noBlockReaction()
        {
            int count = 0;
            _ = network.telegramNotif("DLT at " + Properties.Settings.Default.exIp + " / " + Properties.Settings.Default.loIp + ":" + Properties.Settings.Default.dltPort + " Failed with ErrorNoBlock status");
            network.stopDLT();
            while (active == true)
            {
                Thread.Sleep(100);
                if (count > 200)
                {
                    await Commands.psKill();
                    Thread.Sleep(500);
                }
                active = await Commands.psCom();
                count++;
            }
            _ = startNode("--lastGoodBlock " + ((int)dltStat.result.curblock - (int)1000).ToString());
        }
    }
}
