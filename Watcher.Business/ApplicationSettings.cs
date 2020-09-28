using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Watcher.Business
{
    public static class ApplicationSettings
    {
        public static string PollFilePath
        {
            get
            {
                var appPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                appPath += "\\lastpoll.txt";
                return appPath;
            }
        }

        public static string WatchFolderPath
        {
            get
            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string path = appDataPath+ @"\Local\FortniteGame\Saved\Demos"; 

                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public static string ApiEndPoint
        {
            get
            {
                return "https://frenzy.farm/replay";
            }
        }
    }
}
