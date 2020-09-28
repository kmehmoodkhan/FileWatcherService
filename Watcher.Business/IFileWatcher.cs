using System;
using System.Collections.Generic;
using System.Text;

namespace Watcher.Business
{
   public interface IFileWatcher
    {
        List<string> GetFiles(DateTime startDate);
        List<string> UploadFiles(List<string> files);
    }
}
