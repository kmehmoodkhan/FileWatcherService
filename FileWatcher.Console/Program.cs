using System;
using System.Collections.Generic;
using System.Text;

namespace FileWatcher.Console
{
    class Program
    {
       static void Main(string[] args)
        {

            try
            {
                RunAsync();
            }
            catch(Exception ex)
            {
                ;
            }
            finally
            {
                int a = 0;
            }
        }

        static void RunAsync()
        {
            Watcher.Business.FileWatcher watcher = new Watcher.Business.FileWatcher();
            var files = watcher.GetFiles(DateTime.Now);
            var result = watcher.UploadFiles(files);
        }
    }
}
