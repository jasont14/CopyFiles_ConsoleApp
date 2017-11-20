using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace SimpleFileCopy
{
    public class SettingsConfiguration
    {
        string _originDirectory;
        string _remoteDirectory;
        int _maxFileSize;

        public SettingsConfiguration()
        {
         
        }

        public void ChangeOriginDirectory(string NewOriginDirectory)
        {
            _originDirectory = NewOriginDirectory;
            
        }

        public void ChangeRemoteDirectory(string NewRemoteDirectory)
        {
            _remoteDirectory = NewRemoteDirectory;
        }

        public void ChangeMaxFileSize(int NewMaxFileSize_MB)
        {
            _maxFileSize = NewMaxFileSize_MB * 1000000;
        }
    }

}
