using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace ITS.App_Code
{
    public static class AppHelper
    {
        public static bool DownloadFile(string address, string fileName)
        {
            try
            {
                WebClient client = new WebClient();
                string userProfileFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string DownloadsFolder = string.Format(userProfileFolder + "\\Downloads\\{0}", fileName);


                client.DownloadFile(address, DownloadsFolder);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}