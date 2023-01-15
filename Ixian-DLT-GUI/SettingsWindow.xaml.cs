using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Diagnostics;
using System.ComponentModel;
namespace Ixian_DLT_GUI
{
    public partial class SettingsWindow : System.Windows.Window
    {
        public bool InstallIsCompleted = false;
        public string CurrentFileBeingExtracted = "";
        public int TotalNumberOfFiles = 1;
        public int NumberOfFilesTransferred = 1;
        public string dltPath = "";
        public string fullDltPath = "";
        public static bool active = false;
        public Config config = new Config();
        public SettingsWindow()
        {
            InitializeComponent();
            dispData.Content = Properties.Settings.Default.dataDir.ToString();
            dispDLT.Content = Properties.Settings.Default.dltDir.ToString();
            dispWal.Content = Properties.Settings.Default.wallet.ToString();
            iftestnet.IsChecked = Properties.Settings.Default.testnet;
            ifworker.IsChecked = Properties.Settings.Default.worker;
            ifwebstart.IsChecked = Properties.Settings.Default.webstart;
            ifautorun.IsChecked = Properties.Settings.Default.autorun;
            walPasswd.Password = Properties.Settings.Default.dltpass.ToString();
            if (Properties.Settings.Default.exIp.ToString() == "")
            {
                string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
                exIp.Text = IPAddress.Parse(externalIpString).ToString();
            }
            else
            {
                exIp.Text = Properties.Settings.Default.exIp.ToString();
            }
            if (Properties.Settings.Default.loIp.ToString() == "")
            {
                loIp.Text = network.GetLocalIPAddress();
            }
            else
            {
                loIp.Text = Properties.Settings.Default.loIp.ToString();
            }
            dPort.Text = Properties.Settings.Default.dltPort.ToString();
            aPort.Text = Properties.Settings.Default.apiPort.ToString();
            allowIp.Text = Properties.Settings.Default.allowedIp.ToString();
            apiLogin.Text = Properties.Settings.Default.apiLogin.ToString();
            apiPass.Password = Properties.Settings.Default.apiPasswd.ToString();
            botApi.Text = Properties.Settings.Default.botApi.ToString();
            chatID.Text = Properties.Settings.Default.chatID.ToString();
            if (Properties.Settings.Default.loglevel != "")
            {
                logLvl.Value = double.Parse(Properties.Settings.Default.loglevel, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                logLvl.Value = (double)0;
            }
        }

        //DLT commands
        public string DLTVersion(string path)
        {
            string version = "";
            string[] par = new string[] { "--version" };
            if (System.IO.File.Exists(@path + "\\IxianDLT.exe"))
            {
                var process = Process.Start(Commands.GetParamsDlt(par, true));
                var output = process.StandardOutput.ReadLine();
                version = output.Split(new char[] { ' ' })[2].Split(new char[] { '-' })[1];
            }
            DirectoryInfo di = new DirectoryInfo(@path);
            foreach (FileInfo file in di.GetFiles(".log"))
            {
                file.Delete();
            }
            return version;
        }
        public async Task UpdateDLT(string link, string path, string fullpath)
        {
            if (!System.IO.File.Exists(fullpath))
            {
                updBtn.IsEnabled = false;
                saveBtn.IsEnabled = false;
                exSetBtn.IsEnabled = false;
                taskProgess.Content = "DLT download";
                taskProgess.Visibility = Visibility.Visible;
                taskProgrBar.Value = 0;
                taskProgrBar.Visibility = Visibility.Visible;
                await GetFileAsync();
            }
            async Task GetFileAsync()
            {
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                    wc.DownloadFileCompleted += wc_DownloadFileCompleted;
                    wc.DownloadFileAsync(new Uri(link), fullpath);
                }
            }
        }

        //Storage commands
        private void Unzipper(string path, string fullpath)
        {
            updBtn.IsEnabled = false;
            saveBtn.IsEnabled = false;
            exSetBtn.IsEnabled = false;
            taskProgess.Content = "DLT unZip";
            taskProgess.Visibility = Visibility.Visible;
            taskProgrBar.Value = 0;
            taskProgrBar.Visibility = Visibility.Visible;
            using (ZipArchive archive = ZipFile.OpenRead(@fullpath))
            {
                double number = archive.Entries.Count;
                double index = 1;
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    index++;
                    string fullPath = System.IO.Path.Combine(@path, entry.FullName).Replace("\\Ixian-DLT", "");
                    if (String.IsNullOrEmpty(entry.Name))
                    {
                        Directory.CreateDirectory(fullPath);
                    }
                    else
                    {
                        entry.ExtractToFile(fullPath);
                    }
                    double percentage = index / number * 100;
                    taskProgrBar.Value = int.Parse(Math.Truncate(percentage).ToString());
                }
            }
            FileInfo fileInf = new FileInfo(fullpath);
            if (fileInf.Exists)
            {
                fileInf.Delete();
            }
            taskProgess.Visibility = Visibility.Hidden;
            taskProgrBar.Visibility = Visibility.Hidden;
            updBtn.IsEnabled = true;
            saveBtn.IsEnabled = true;
            exSetBtn.IsEnabled = true;
        }
        public static void CLeanDir(string path, string name)
        {
            if (Directory.Exists(path))
            {
                var dialog = new Confirm();
                dialog.ResponseText = "Selected " + name + " location\n IS NOT EMPTY!";
                if (dialog.ShowDialog() == true)
                {
                    Directory.Delete(@Properties.Settings.Default.dltDir + "\\data-testnet", true);
                    Directory.Delete(@Properties.Settings.Default.dltDir + "\\data", true);
                    string[] dirs = Directory.GetDirectories(path);
                    foreach (string s in dirs)
                    {
                        Directory.Delete(s, true);
                    }
                    string[] files = Directory.GetFiles(path);
                    foreach (string s in files)
                    {
                        System.IO.File.Delete(s);
                    }
                    MessageBox.Show(@path + "\nwiped");
                }
            }
        }

        //Progress animation
        public void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            taskProgrBar.Value = int.Parse(Math.Truncate(percentage).ToString());
        }
        public void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            taskProgrBar.Value = 0;
            if (e.Cancelled)
            {
                System.Windows.MessageBox.Show("The download has been cancelled");
                taskProgess.Visibility = Visibility.Hidden;
                taskProgrBar.Visibility = Visibility.Hidden;
                updBtn.IsEnabled = true;
                saveBtn.IsEnabled = true;
                exSetBtn.IsEnabled = true;
                return;
            }
            if (e.Error != null) 
            {
                System.Windows.MessageBox.Show("An error ocurred while trying to download file");
                taskProgess.Visibility = Visibility.Hidden;
                taskProgrBar.Visibility = Visibility.Hidden;
                updBtn.IsEnabled = true;
                saveBtn.IsEnabled = true;
                exSetBtn.IsEnabled = true;
                return;
            }
            taskProgess.Visibility = Visibility.Hidden;
            taskProgrBar.Visibility = Visibility.Hidden;
            updBtn.IsEnabled = true;
            saveBtn.IsEnabled = true;
            exSetBtn.IsEnabled = true;
            Unzipper(dltPath, fullDltPath);
        }

        //Window elements
        public async void Update_Click(object sender, RoutedEventArgs e)
        {
            string link = "https://api.github.com/repos/ProjectIxian/Ixian-DLT/releases";
            GitAnswer[]? answer = JsonConvert.DeserializeObject<GitAnswer[]>(await network.update(link));

            string? dltDir = dispDLT.Content.ToString();
            dltPath = dltDir;
            fullDltPath = dltDir + "\\" + answer[0].assets[0].name;
            if (dltDir == "...")
            {
                System.Windows.MessageBox.Show("Choose Dir for DLT");
            }
            else
            {
                if (Directory.EnumerateFiles(@dltDir, "*.*", SearchOption.AllDirectories).Count() == 0)
                {
                    UpdateDLT(answer[0].assets[0].browserDownloadUrl, dltPath, fullDltPath);
                }
                else
                {
                    string version = DLTVersion(dltDir);
                    if (version != answer[0].tagName.Split(new char[] { 'v' })[1])
                    {
                        CLeanDir(dltPath, "DLT-node");
                        UpdateDLT(answer[0].assets[0].browserDownloadUrl, dltPath, fullDltPath);
                    }
                }
            }
        }
        public void Save_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.dataDir = dispData.Content.ToString();
            Properties.Settings.Default.dltDir = dispDLT.Content.ToString();
            Properties.Settings.Default.wallet = dispWal.Content.ToString();
            Properties.Settings.Default.testnet = (bool)iftestnet.IsChecked;
            Properties.Settings.Default.worker = (bool)ifworker.IsChecked;
            Properties.Settings.Default.webstart = (bool)ifwebstart.IsChecked;
            Properties.Settings.Default.autorun = (bool)ifautorun.IsChecked;
            Properties.Settings.Default.dltpass = walPasswd.Password;
            Properties.Settings.Default.exIp = exIp.Text;
            Properties.Settings.Default.loIp = loIp.Text;
            Properties.Settings.Default.dltPort = dPort.Text;
            Properties.Settings.Default.apiPort = aPort.Text;
            Properties.Settings.Default.loglevel = config.loglevel;
            Properties.Settings.Default.allowedIp = allowIp.Text;
            Properties.Settings.Default.apiLogin = apiLogin.Text;
            Properties.Settings.Default.apiPasswd = apiPass.Password;
            Properties.Settings.Default.botApi = botApi.Text;
            Properties.Settings.Default.chatID = chatID.Text;
           

            Properties.Settings.Default.Save();
            System.Windows.MessageBox.Show("Settings saved");
        }
        public void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void btnSelectBCH_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                    dispData.Content = dialog.SelectedPath;
            }

        }
        public void btnSelectDLT_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                    dispDLT.Content = dialog.SelectedPath;
            }
        }
        public void btnOpenWallet_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Wallet files (*.wal)| *.wal";
            if (openFileDialog.ShowDialog() == true)
                dispWal.Content = openFileDialog.FileName;
        }
        public void Log_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            config.loglevel = e.NewValue.ToString();
        }
    }
}
