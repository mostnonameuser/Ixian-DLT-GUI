using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows;
using System.Reflection.Metadata;
using System.Threading;
using System.IO;
using System.IO.Pipes;
using System.Security.Cryptography.X509Certificates;
using static System.Net.WebRequestMethods;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Ixian_DLT_GUI
{
    internal class Commands
    {
        private static ProcessStartInfo Info = null;
        private static Process Proc = null;
        public static ProcessStartInfo startInfo;

        //DLT configuration
        public static ProcessStartInfo GetParamsDlt(string[] args, bool hideWindow)
        {
            string param = string.Join(" ", args);

            startInfo = new ProcessStartInfo
            {
                FileName = @Properties.Settings.Default.dltDir + "\\IxianDLT.exe",  // Путь к приложению
                WorkingDirectory = @Properties.Settings.Default.dltDir,
                UseShellExecute = false,
                Arguments = param,
                RedirectStandardOutput = false,
                RedirectStandardError = false,
                RedirectStandardInput = false,
                CreateNoWindow = hideWindow,
               // WindowStyle = ProcessWindowStyle.Hidden,
            };
            return startInfo;
        }
        public static string[] PutCliParams(string addon)
        {
            List<string> list = new List<string>();
            //string[] cmdparamsPar = new string[] { };
            if (Properties.Settings.Default.wallet != "")
            {
                list.Add("-w " + Properties.Settings.Default.wallet);
            }
            if (Properties.Settings.Default.testnet)
            {
                list.Add("-t");
            }
            if (Properties.Settings.Default.worker)
            {
                list.Add("--worker");
            }

            if (Properties.Settings.Default.dltpass != "")
            {
                list.Add("--walletPassword " + Properties.Settings.Default.dltpass);
            }

            if (Properties.Settings.Default.dltPort != "")
            {
                list.Add("-p " + Properties.Settings.Default.dltPort);
            }
            if (addon != "")
            {
                list.Add(addon);
            }

            string[] cmdparamsPar = list.ToArray();
            return cmdparamsPar;
        }
        public static string makeConfig()
        {
            string config = "";
            if (!Properties.Settings.Default.webstart)
            {
                config = config + "\ndisableWebStart = 1";
            }
            if (Properties.Settings.Default.apiPort != "")
            {
                config = config + "\napiPort  = " + Properties.Settings.Default.apiPort;
            }
            if (Properties.Settings.Default.exIp != "")
            {
                config = config + "\nexternalIp = " + Properties.Settings.Default.exIp;
            }
            if (Properties.Settings.Default.loIp != "" && Properties.Settings.Default.apiPort != "")
            {
                config = config + "\napiBind = http://" + Properties.Settings.Default.loIp + ":" + Properties.Settings.Default.apiPort + "/";
            }
            switch (Properties.Settings.Default.loglevel)
            {
                case "0":
                    config = config + "\nlogVerbosity = 0";
                    break;
                case "1":
                    config = config + "\nlogVerbosity = 1";
                    break;
                case "2":
                    config = config + "\nlogVerbosity = 2";
                    break;
                case "3":
                    config = config + "\nlogVerbosity = 4";
                    break;
                case "4":
                    config = config + "\nlogVerbosity = 8";
                    break;
            }
            if (Properties.Settings.Default.allowedIp != "")
            {
                string[] allowed = Properties.Settings.Default.allowedIp.Split(' ');
                foreach (string ip in allowed)
                {
                    config = config + "\napiAllowIp = " + ip;
                }
            }
            if (Properties.Settings.Default.apiLogin != "" && Properties.Settings.Default.apiPasswd != "")
            {
                config = config + "\naddApiUser = " + Properties.Settings.Default.apiLogin + ":" + Properties.Settings.Default.apiPasswd;
            }

            return config;
        }

        //Powershell commands
        public static async Task<bool> psCom()
        {
            var start = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = "Get-Process ixiandlt -erroraction 'silentlycontinue'",
                CreateNoWindow = true
            };
            using var process = Process.Start(start);
            using var reader = process.StandardOutput;
            process.EnableRaisingEvents = true;
            //string answer = await reader.ReadToEndAsync();
            bool status;
            if (await reader.ReadToEndAsync() != "")
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }
        public static async Task<bool> psKill()
        {
            var start = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = "Get-Process ixiandlt -erroraction 'silentlycontinue' | Stop-Process",
                CreateNoWindow = true
            };
            using var process = Process.Start(start);
            using var reader = process.StandardOutput;
            process.EnableRaisingEvents = true;
            //string answer = await reader.ReadToEndAsync();
            bool status;
            if (await reader.ReadToEndAsync() != "")
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }

    }
}
