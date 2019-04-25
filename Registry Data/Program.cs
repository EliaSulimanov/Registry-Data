using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Registry_Data
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                PrintLogo();
                Console.Write(
                    "1. Machine Info\n" +
                    "2. User Info\n" +
                    "3. Programs\n" +
                    "4. GPU\n" +
                    "5. Low Rights\n" +
                    "6. PC Info\n" +
                    "Your Choise: ");
                PrintOptions(int.Parse(Console.ReadLine()));
                Console.Clear();
            }
        }

        private static void PrintLogo()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            string[] lines = {
                "Registry Data",
                "Elia Sulimanov - 2019"
            };
            foreach (string line in lines)
            {
                Console.SetCursorPosition((Console.WindowWidth - line.Length) / 2, Console.CursorTop);
                Console.WriteLine(line);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void PrintOptions(in int option)
        {
            switch(option)
            {
                case 1:
                    MachineInfo();
                    break;
                case 2:
                    UserInfo();
                    break;
                case 3:
                    Programs();
                    break;
                case 4:
                    GPU();
                    break;
                case 5:
                    LowRights();
                    break;
                case 6:
                    PCInfo();
                    break;
                default:
                    break;
            }
        }

        private static void MachineInfo()
        {
            Console.Clear();
            PrintLogo();

            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\International", false);
                string language = (string)registryKey.GetValue("sLanguage");
                Console.WriteLine("Machine Language: " + language);
            }
            catch { }
            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop\MuiCached", false);
                string mui = (string)registryKey.GetValue("MachinePreferredUILanguages");
                Console.WriteLine("Machine Preferred UI Languages: " + mui);
            }
            catch { }
            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\International", false);
                string country = (string)registryKey.GetValue("sCountry");
                Console.WriteLine("Machine Country: " + country);
            }
            catch { }
            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\International", false);
                string shortDate = (string)registryKey.GetValue("sShortDate");
                Console.WriteLine("Machine Short Date Format: " + shortDate);
            }
            catch { }
            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", false);
                string wallPaper = (string)registryKey.GetValue("WallPaper");
                Console.WriteLine("Wallpaper Source: " + wallPaper);
            }
            catch { }
            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey();
        }

        private static void UserInfo()
        {
            Console.Clear();
            PrintLogo();

            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Office\Common\UserInfo", false);
                string displayName = (string)registryKey.GetValue("UserName");
                Console.WriteLine("User Name: " + displayName);
            }
            catch { }

            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\IdentityCRL\UserExtendedProperties", false);
                string email = "";
                String[] values = registryKey.GetSubKeyNames(); //email
                foreach (string value in values)
                    if (value.Contains("@"))
                        email = value;
                Console.WriteLine("Email: " + email);
            }
            catch { }

            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey();
        }

        private static void Programs()
        {
            Console.Clear();
            PrintLogo();

            try
            {
                Console.WriteLine("Anvi-Virus: " + GetAvName());
            }
            catch { }

            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey();
        }

        private static void GPU()
        {
            Console.Clear();
            PrintLogo();

            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\GPU", false);
                string gpu = (string)registryKey.GetValue("AdapterInfo");
                gpu = gpu.Replace(",", "\n");
                Console.WriteLine("GPU Data: \n" + gpu);
            }
            catch { }

            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey();
        }

        private static void LowRights()
        {
            Console.Clear();
            PrintLogo();

            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\Low Rights", false);
                string[] rights = registryKey.GetSubKeyNames();

                foreach(string right in rights)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Right: " + right);
                    Console.ForegroundColor = ConsoleColor.White;

                    string[] subKeysCodes = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\Low Rights\" + right, false).GetSubKeyNames();
                    foreach(string program in subKeysCodes)
                    {
                        RegistryKey programName = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\Low Rights\" + right + @"\" + program, false);
                        string appName = (string)programName.GetValue("AppName");
                        string appPath = (string)programName.GetValue("AppPath");

                        Console.WriteLine("Name: " + appName);
                        Console.WriteLine("Path: " + appPath);
                    }
                    Console.WriteLine("");
                }
            }
            catch { }

            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey();
        }

        private static string GetAvName()
        {
            string av = "";
            bool defenderFlag = false;
            try
            {
                var searcher = new ManagementObjectSearcher(@"\\" + Environment.MachineName + @"\root\SecurityCenter2", "SELECT * FROM AntivirusProduct");
                var programs = searcher.Get();
                foreach (var program in programs)
                {
                    if (program.GetPropertyValue("displayName").ToString() == "Windows Defender")
                        defenderFlag = true;
                    else
                        av = program.GetPropertyValue("displayName").ToString();
                }

                if (av == string.Empty && defenderFlag)
                    av = "Windows Defender";
                if (av == "" && !defenderFlag)
                    av = "Non";
            }

            catch
            {
                return "Error";
            }
            return av;
        }

        private static void PCInfo()
        {
            Console.Clear();
            PrintLogo();

            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\TaskFlow\DeviceCache", false);
                String[] values = registryKey.GetSubKeyNames();
                foreach (string value in values)
                {
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\TaskFlow\DeviceCache\" + value, false);
                    string deviceMake = (string)key.GetValue("DeviceMake");
                    string deviceModel = (string)key.GetValue("DeviceModel");
                    string deviceName = (string)key.GetValue("DeviceName");
                    Console.WriteLine("Device Make: " + deviceMake);
                    Console.WriteLine("Device Model: " + deviceModel);
                    Console.WriteLine("Device Name: " + deviceName);
                }
            }
            catch { }

            Console.WriteLine("Press Any Key To Return");
            Console.ReadKey();
        }
    }
}