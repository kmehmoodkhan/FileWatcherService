using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;


namespace Watcher.Business
{
    public class FileWatcher : IFileWatcher
    {
        private DateTime lastPollDate = DateTime.Now;
        public FileWatcher()
        {
        }
        public List<string> GetFiles(DateTime startDate)
        {
            lastPollDate = startDate;
            var allFiles = new DirectoryInfo(ApplicationSettings.WatchFolderPath).GetFiles();
            List<string> chosenFiles = new List<string>();

            foreach(var f in allFiles)
            {
                if(f.LastAccessTime > lastPollDate)
                {
                    chosenFiles.Add(f.FullName);
                }
            }
            //var chosenFiles = allFiles
            //                  .Where(t => t.LastAccessTime > lastPollDate).Select(t => t.FullName).ToList();



            

            return chosenFiles;
        }

        public List<string> UploadFiles(List<string> files)
        {
            List<string> result = new List<string>();
            foreach (string f in files)
            {
                string fileName = Path.GetFileName(f);

                try
                {
                    WebClient wc = new WebClient();
                    byte[] bytes = wc.DownloadData(f);
                    Dictionary<string, object> postParameters = new Dictionary<string, object>();
                    postParameters.Add("replay", new FormUpload.FileParameter(bytes, fileName, Path.GetExtension(f)));
                    string userAgent = "Someone";
                    HttpWebResponse webResponse = FormUpload.MultipartFormPost(ApplicationSettings.ApiEndPoint, userAgent, postParameters, "cache-control", "no-cache");
                    // Process response  
                    StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                    var returnResponseText = responseReader.ReadToEnd();
                    webResponse.Close();

                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "File Watcher Service Service";
                        eventLog.WriteEntry("File Watcher Service has uploaded the file " + f, EventLogEntryType.Information);
                    }
                }
                catch(Exception ex)
                {
                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = "File Watcher Service Service";
                        eventLog.WriteEntry("File Watcher Service failed to upload file " + f, EventLogEntryType.Error);
                    }
                }

                result.Add(f);
            }
            return result;
        }

        //public List<string> UploadFiles1(List<string> files)
        //{
        //    List<string> result = new List<string>();

        //    foreach (string f in files)
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            var array = File.ReadAllBytes(f);
        //            MultipartFormDataContent form = new MultipartFormDataContent();
        //            form.Add(new ByteArrayContent(array), "replay", Path.GetFileName(f));
        //            HttpResponseMessage response = client.PostAsync(ApplicationSettings.ApiEndPoint, form).Result;

        //            response.EnsureSuccessStatusCode();

        //            string sd = response.Content.ReadAsStringAsync().Result;
        //        }
        //    }

        //    return result;
        //}
    }
}
