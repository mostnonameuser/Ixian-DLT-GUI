using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using System.Security.Policy;
using static System.Net.WebRequestMethods;

namespace Ixian_DLT_GUI
{
    public static class network
    {

        public static async Task<string> update(string url)
        {
            string json;
            HttpClient webewq = new HttpClient();
            webewq.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
            HttpResponseMessage response = await webewq.GetAsync(url);
            response.EnsureSuccessStatusCode();
            json = await response.Content.ReadAsStringAsync();

            return json;
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static async void stopDLT()
        {
            string URL = "http://" + Properties.Settings.Default.loIp + ":" + Properties.Settings.Default.apiPort + "/shutdown";
            HttpClient client = new HttpClient();
            try
            {
                await client.GetAsync(URL);
            }
            catch
            {

            } 
            finally
            {

            }
            client.Dispose();
        }
        public static async Task<string> statusDLT()
        {
            string json = null;
            string URL = "http://" + Properties.Settings.Default.loIp + ":" + Properties.Settings.Default.apiPort + "/status?vv=true";
            try
            {
                HttpClient webewq = new HttpClient();
                try
                {
                    HttpResponseMessage response = await webewq.GetAsync(URL);
                    response.EnsureSuccessStatusCode();
                    json = await response.Content.ReadAsStringAsync();
                }
                catch (SocketException ex) {
                    MessageBox.Show( ex.ToString());
                }
                finally { webewq.Dispose(); }
            }
            finally
            {
            }
            return json;
        }
        public static async Task<string> balanceDLT()
        {
            string json = null;
            string URL = "http://" + Properties.Settings.Default.loIp + ":" + Properties.Settings.Default.apiPort + "/gettotalbalance";
            try
            {
                HttpClient webewq = new HttpClient();
                try
                { 
                    HttpResponseMessage response = await webewq.GetAsync(URL );
                    response.EnsureSuccessStatusCode();
                    json = await response.Content.ReadAsStringAsync();
                }
                catch (WebException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally { webewq.Dispose(); }
            }
            catch (WebException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                
            }
            return json;
        }
        
        public static async Task telegramNotif(string message)
        {
            string URL = "https://api.telegram.org/bot"+ Properties.Settings.Default.botApi +"/sendMessage?chat_id="+ Properties.Settings.Default.chatID + "&text="+message+"&parse_mode=markdown";
            try
            {
                HttpClient webewq = new HttpClient();
                try
                {
                    HttpResponseMessage response = await webewq.GetAsync(URL);
                    response.EnsureSuccessStatusCode();
                    var text = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(text.ToString());
                }
                catch (SocketException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally { webewq.Dispose(); }
            }
            finally
            {
            }
        }
    }
}
