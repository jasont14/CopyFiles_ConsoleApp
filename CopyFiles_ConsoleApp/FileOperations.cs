using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace CopyFiles_ConsoleApp
{
    class FileOperations
    {  
        public FileOperations()
        {
            OriginRoot = "D://FC//";
            RemoteRoot = "D://FD//";
            _originFiles = GetFileNames(OriginRoot);
            _remoteFiles = GetFileNames(RemoteRoot);
            _filesToCopy = new List<string>();            
            _filesNotToCopy = new List<string>();
            SetFilesToCopy();
            
        }

        public void CopyFiles(bool overWrite)
        {
            int NumberOfFilesCopied = CopyFileTo(OriginRoot, RemoteRoot, FilesToCopy);
            Console.WriteLine("Copied " + NumberOfFilesCopied + " Files.");
        } 
 
        private void SetFilesToCopy()
        {
            List<string> FilesThatDontMatch = new List<string>();
            FilesThatDontMatch = _originFiles.Except(_remoteFiles).ToList();

            foreach (var file in FilesThatDontMatch)
            {
                FileInfo myFileInfo = new FileInfo(_originRoot + file);
                if(myFileInfo.Length < MaxFileSizeLimit)
                {
                    _filesToCopy.Add(file);
                }
                else
                {
                    _filesNotToCopy.Add(file);
                }
            }
        }

        public List<string> GetFileNames(string origin)
        {
            List<string> result = new List<string>();
            Queue<string> ListOfDirectories = new Queue<string>();
            ListOfDirectories.Enqueue(origin);

            LogOperations ErrorLog = new LogOperations();
            

            string[] Files;
            string[] SubDirs;

            while (ListOfDirectories.Count > 0)
            {
                string currentDirectory = ListOfDirectories.Dequeue();

                try
                {
                    Files = Directory.GetFiles(currentDirectory);

                    foreach (string file in Files)
                    {
                        result.Add(file.Replace(origin, ""));
                    }

                    SubDirs = Directory.GetDirectories(currentDirectory);

                    foreach (string subDirectory in SubDirs)
                    {
                        ListOfDirectories.Enqueue(subDirectory);
                    }
                }
                catch (DirectoryNotFoundException ex)
                {
                    ErrorLog.WriteToErrorLog("Get File Names: " + ex.Message);
                    result.Add("Error");
                }
                catch (UnauthorizedAccessException ex)
                {
                    ErrorLog.WriteToErrorLog("Get File Names: " + ex.Message);
                    result.Add("Error");
                }                
            }

            if(ErrorLog.ErrorMessageCount > 1)
            {
                ErrorLog.WriteLogs();
            }
            else
            {
                
            }
            return result;
        }

        private static int CopyFileTo(string CopyFromRoot, string CopyToRoot, List<string> FilesToCopy)
        {
            int result = 0; //Return number of files copied.

            LogOperations myLog = new LogOperations();

            int kk = 10001;
            foreach (string file in FilesToCopy)
            {

                kk += 9;

                string DirectoryToCheckExists = Directory.GetParent(CopyToRoot + file).ToString() + "\\";

                if (Directory.Exists(DirectoryToCheckExists))
                {
                    try
                    {
                        File.Copy(CopyFromRoot + file, CopyToRoot + file);
                        myLog.WriteToLogFile("Copied " + CopyFromRoot + file + " To " + CopyToRoot + file);
                        result++;
                    }
                    catch (FileNotFoundException ex)
                    {
                        myLog.WriteToErrorLog(ex.Message + " " + ex.FileName);
                    }
                    catch(UnauthorizedAccessException ex)
                    {
                        myLog.WriteToErrorLog(ex.Message + " " + ex.Source);
                    }
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(Directory.GetParent(CopyToRoot + file).ToString());
                        File.Copy(CopyFromRoot + file, CopyToRoot + file);
                        myLog.WriteToLogFile("Copied " + CopyFromRoot + file + " To " + CopyToRoot + file);
                        result++;
                    }
                    catch (FileNotFoundException ex)
                    {
                        myLog.WriteToErrorLog(ex.Message + " " + ex.FileName);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        myLog.WriteToErrorLog(ex.Message + " " + ex.Source);
                    }
                    
                }
            }

            myLog.WriteLogs();
           
            return result;
        }


        private int MaxFileSizeLimit = 2500000;
        private string _originRoot;
        private string _remoteRoot;
        private List<string> _originFiles;
        private List<string> _remoteFiles;
        private List<string> _filesToCopy;
        private List<string> _filesNotToCopy;

        public List<string> FilesNotToCopy
        {
            get
            {
                return _filesNotToCopy;
            }
        }

        public List<string> FilesToCopy
        {
            get
            {
                return _filesToCopy;
            }
        }
        public List<string> OriginFiles
        {
            get
            {
                return _originFiles;
            }
        }

        public List<string> RemoteFiles
        {
            get
            {
                return _remoteFiles;
            }
        }

        public string OriginRoot
        {
            get
            {
                return _originRoot;
            }
            set
            {
                if (Directory.Exists(value))
                {
                    _originRoot = value;
                }
            }
        }

        public string RemoteRoot
        {
            get
            {
                return _remoteRoot;
            }
            set
            {
                if (Directory.Exists(value))
                {
                    _remoteRoot = value;
                }
                else
                {
                    
                }
            }
        }
    }
}
