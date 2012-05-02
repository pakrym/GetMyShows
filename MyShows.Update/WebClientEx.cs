using System;
using System.Net;
using System.Text;
using NLog;

namespace MyShows.Update
{
    class WebClientEx : WebClient
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public WebClientEx()
        {
            this.Headers.Add(System.Net.HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US) AppleWebKit/A.B (KHTML, like Gecko) Chrome/X.Y.Z.W Safari/A.B.");
            this.Encoding = Encoding.UTF8;
        }


        public string DownloadStringIgnoreAndLog(string url)
        {
            try
            {
                return this.DownloadString(url);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return "";
            }
        }


        public byte[] DownloadDataIgnoreAndLog(string url)
        {
            try
            {
                return this.DownloadData(url);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }

        protected override WebRequest GetWebRequest(Uri address)
        {

            _logger.Debug("Donwloading {0}", address);
            var req = base.GetWebRequest(address);
            req.Timeout = 10000;
            return req;
        }
    }
}