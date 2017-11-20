/* 
 * J.Thatcher
 * Copy Files - Console App from A to B
 * Classes, Fields, Methods, Queues, Arrays, List<T>, Iterations, Limited Error Handling, IO, Text File, App Settings * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CopyFiles_ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationRun();
        }

        static void ApplicationRun()
        {
            bool NoDirectoryIssue = CheckDefaultDirectoriesExist();

            if (NoDirectoryIssue)
            {
                int z = MainMenu();

                switch (z)
                {
                    case 1:
                        RunFileCopyNow();
                        break;
                    case 2:
                        UserSettingsMenu(false);
                        break;
                    case 3:
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Console.WriteLine("***THERE IS AN ISSUE WITH YOUR DEFAULT DIRECTORY SETTINGS***");
                Console.WriteLine("***PLEASE UPDATE THE REMOTE(COPY TO) AND ORIGIN(COPY FROM) DIRECTORY LOCATIONS***\n");
                System.Threading.Thread.Sleep(2000);
                UserSettingsMenu(true);
            }



        }

        //Check to see if origin and remote directories exist...
        private static bool CheckDefaultDirectoriesExist()
        {
            bool result, CheckOriginDirectory, CheckRemoteDirectory;            
            
            CheckOriginDirectory = Directory.Exists(UserSettings.Default.OriginDirectory);
            CheckRemoteDirectory = Directory.Exists(UserSettings.Default.RemoteDirectory);
            
            if (CheckOriginDirectory && CheckRemoteDirectory)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        private static int RunFileCopyNow()
        {
            int result = 0;

            FileOperations myFileOps = new FileOperations();

            if (myFileOps.FilesNotToCopy.Count > 0)
            {
                Console.WriteLine("\n\nThe Following Files will NOT be Copied\n******************** ");
                foreach (string file in myFileOps.FilesNotToCopy)
                {
                    Console.WriteLine(file);
                }
            }

            Console.WriteLine("\n\nThe Following Files will be Copied\n******************** ");
            foreach (string file in myFileOps.FilesToCopy)
            {
                Console.WriteLine(file);
            }

            Console.Write("********************\nDo You Want to Proceed? (Yes/No)");
            string Proceed = Console.ReadLine();

            if (Proceed.ToUpper() == "YES")
            {
                myFileOps.CopyFiles(true);
            }
            else
            {
                Console.WriteLine("\nYou did not select \"Yes\".");
            }

            return result;
        }

        public static string SetDirectory(string ToOrFrom)
        {
            string result = "";
            string _errorMessage = "";
            string CopyDirectory;
            bool validated = false;

            Console.WriteLine("Enter the full directory you wish to copy " + ToOrFrom.ToLower() + " (i.e. C:\\A\\Folder\\)\n");

            do
            {
                Console.Write(_errorMessage + "Directory to Copy " + ToOrFrom + ": ");
                CopyDirectory = Console.ReadLine();

                if (Directory.Exists(CopyDirectory))
                {
                    _errorMessage = "";
                    validated = true;
                }
                else
                {
                    _errorMessage = "\n**ERROR**\n\nThe Directory does not exist or you do not have permission.\n ";
                }

            } while (!validated);

            result = CopyDirectory;
            return result;
        }

        static int MainMenu()
        {
            string Header = "CONSOLE FILE COPY UTILITY";
            string Message1 = "Max File Size Set To (Bytes)" + UserSettings.Default.MaxFileSize.ToString("N0");
            string Message2 = "Origin Directory: " + UserSettings.Default.OriginDirectory;
            string Message3 = "Destination Directory: "+ UserSettings.Default.RemoteDirectory;
            string Message4 = "Option 1 => Run File Copy Now";
            string Message5 = "Option 2 => Change File Copy Settings";
            string Message6 = "Option 9 => Exit Now";

            Console.WriteLine(Header);
            Console.WriteLine("***************************");
            Console.WriteLine(Message1);
            Console.WriteLine(Message2);
            Console.WriteLine(Message3);
            Console.WriteLine("****************************\n");
            Console.WriteLine(Message4);
            Console.WriteLine(Message5);
            Console.WriteLine(Message6);
            Console.Write("\nEnter Option: ");

            string response = Console.ReadLine();

            int MenuResponse;

            while (!Int32.TryParse(response, out MenuResponse))
            {
                Console.WriteLine("Not a valid number, try again.");

                response = Console.ReadLine();
                Console.Write("\nEnter Option: ");
            }

            return MenuResponse;


        }//END


        static void UserSettingsMenu(bool exitNow)
        {
            string Header = "CHANGE SETTINGS (RESTART REQUIRED)";
            string Message1 = "Option 1 => Change Origin Directory";
            string Message2 = "Option 2 => Change Remote Directory";
            string Message3 = "Option 3 => Change Max File Size";
            string Message5 = "Option 8 => Return To Main Menu";
            string Message4 = "Option 9 => Exit Now";

            Console.Clear();
            Console.WriteLine(Header);
            Console.WriteLine("***************************");
            Console.WriteLine(Message1);
            Console.WriteLine(Message2);
            Console.WriteLine(Message3);

            if (!exitNow)
            {
                Console.WriteLine(Message5);
            }

            Console.WriteLine(Message4);

            Console.Write("\nEnter Option: ");

            string response = Console.ReadLine();

            int MenuResponse;

            while (!Int32.TryParse(response, out MenuResponse))
            {
                Console.WriteLine("Not a valid number, try again.");

                response = Console.ReadLine();
                Console.Write("\nEnter Option: ");
            }

            SettingsConfiguration mySettings = new SettingsConfiguration();

            switch (MenuResponse)
            {
                case 1:
                    string NewOriginDirectory;
                    Console.Write("\nEnter New Directory to Copy From: ");
                    NewOriginDirectory = Console.ReadLine();
                    if (Directory.Exists(NewOriginDirectory))
                    {
                        mySettings.ChangeOriginDirectory(NewOriginDirectory);
                        Console.WriteLine("\nOrigin Directory Updated");
                        System.Threading.Thread.Sleep(2000);
                        UserSettingsMenu(true);
                    }
                    else
                    {
                        Console.WriteLine("Directory Does Not Exist or You Do Not Have Permissions");
                        System.Threading.Thread.Sleep(2000);
                        UserSettingsMenu(exitNow);
                    }
                    break;
                case 2:
                    string NewRemoteDirectory;
                    Console.Write("Enter New Directory to Copy To: ");
                    NewRemoteDirectory = Console.ReadLine();
                    if (Directory.Exists(NewRemoteDirectory))
                    {
                        mySettings.ChangeRemoteDirectory(NewRemoteDirectory);
                        Console.WriteLine("\nRemote Directory Updated");
                        System.Threading.Thread.Sleep(2000);
                        UserSettingsMenu(true);
                    }
                    else
                    {
                        Console.WriteLine("Directory Does Not Exist or You Do Not Have Permissions");
                        System.Threading.Thread.Sleep(2000);
                        UserSettingsMenu(exitNow);
                    }
                    break;
                case 3:
                    int NewMaxFileSize;
                    Console.Write("Enter New Max File Size (MB): ");
                    NewMaxFileSize = int.Parse(Console.ReadLine());
                    mySettings.ChangeMaxFileSize(NewMaxFileSize);

                    Console.WriteLine("\nMax File Size Updated");
                    System.Threading.Thread.Sleep(2000);
                    UserSettingsMenu(true);
                    break;
                case 8:
                    Console.Clear();
                    ApplicationRun();
                    break;
                default:
                    break;
            }

        }
    }
}
