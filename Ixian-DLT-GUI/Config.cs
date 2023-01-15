using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ixian_DLT_GUI
{
    public class Config
    {
        public string dataDir = Properties.Settings.Default.dataDir;
        public string dltDir = Properties.Settings.Default.dltDir;
        public string wallet = Properties.Settings.Default.wallet;
        public bool testnet = Properties.Settings.Default.testnet;
        public bool worker = Properties.Settings.Default.worker;
        public bool webstart = Properties.Settings.Default.webstart;
        public bool autorun = Properties.Settings.Default.autorun;
        public string dltpass = Properties.Settings.Default.dltpass;
        public string exIp = Properties.Settings.Default.exIp;
        public string loIp = Properties.Settings.Default.loIp;
        public string dltPort = Properties.Settings.Default.dltPort;
        public string apiPort = Properties.Settings.Default.apiPort;
        public string allowedIp = Properties.Settings.Default.allowedIp;
        public string apiLogin = Properties.Settings.Default.apiLogin;
        public string apiPasswd = Properties.Settings.Default.apiPasswd;
        public string botApi = Properties.Settings.Default.botApi;
        public string chatID = Properties.Settings.Default.chatID;
        public string loglevel = Properties.Settings.Default.loglevel;
        public bool showconsole = Properties.Settings.Default.showConsole;
    }
}
