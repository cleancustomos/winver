using Microsoft.UI.Xaml;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Management;
using System.Text;
using WinUIEx;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace winver
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
  
            SetTitleBar(titleBar);
            this.AppWindow.MoveAndResize(new Windows.Graphics.RectInt32(620, 800, 620, 800));
            WindowManager.Get(this).IsMinimizable = false;
            WindowManager.Get(this).IsMaximizable = false;
            //WindowManager.Get(this).IsResizable = false;
            //WindowManager.Get(this).MaxHeight = 600;
            //WindowManager.Get(this).MaxWidth = 800;

            this.Edition = GetEdition();
            this.Version = GetVersion();
            this.InstalledOn = GetInstalledOn();
            this.OSBuild = GetOSBuild();
            this.ReleaseId = GetReleaseId();

            this.Processor = GetProcessor();
            this.Memory = GetMemory();
            this.Graphics = GetGraphics();
            this.OSArchitecture = GetOSArchitecture();
            this.DiskSpace = GetDiskSpace();
        }
        public string Edition { get; set; }
        public string Version { get; set; }
        public string InstalledOn { get; set; }
        public string OSBuild { get; set; }
        public string ReleaseId { get; set; }


        public string Processor { get; set; }
        public string Memory { get; set; }
        public string Graphics { get; set; }
        public string OSArchitecture { get; set; }
        public string DiskSpace { get; set; }


        private string GetEdition()
        {
            var edition = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "EditionID", null);
            if (edition != null)
            {
                if (edition.ToString() == "Core")
                {
                    return "Home";
                }
                else if (edition.ToString() == "CoreSingleLanguage")
                {
                    return "Home Single Language";
                }
                else
                {
                    return edition.ToString();
                }
            }
            else
            {
                return "-";
            }
        }


        private string GetVersion()
        {
            var version = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "DisplayVersion", null);
            return version != null ? version.ToString() : "-";
        }

        private string GetInstalledOn()
        {
            var installedOn = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "InstallDate", null);
            return installedOn != null ? new DateTime(1970, 1, 1).AddSeconds((int)installedOn).ToString("dd.MM.yyyy") : "Unknown";
        }

        private string GetOSBuild()
        {
            var build = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentBuild", null);
            var ubr = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "UBR", null);
            if (build != null && ubr != null)
            {
                return $"{build.ToString()}.{ubr.ToString()}";
            }
            else
            {
                return "-";
            }
        }

        private string GetReleaseId()
        {
            var version = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", null);
            return version != null ? version.ToString() : "-";
        }




        private string GetProcessor()
        {
            var processor = new ManagementObjectSearcher("select * from Win32_Processor").Get().Cast<ManagementObject>().First();
            return processor["Name"].ToString();
        }

        private string GetMemory()
        {
            var memory = new ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get().Cast<ManagementObject>().First();
            return Math.Round(Convert.ToDouble(memory["TotalPhysicalMemory"]) / (1024 * 1024 * 1024)).ToString() + " GB";
        }

        private string GetGraphics()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            StringBuilder sb = new StringBuilder();
            foreach (ManagementObject wmi_VideoController in searcher.Get())
            {
                string graphicsCardName = wmi_VideoController["Name"].ToString();  // Graphics Card Name
                if (sb.Length > 0)
                {
                    sb.AppendLine();
                }
                sb.Append(graphicsCardName);
            }
            return sb.ToString();
        }

        private string GetOSArchitecture()
        {
            var osArchitecture = new ManagementObjectSearcher("select * from Win32_OperatingSystem").Get().Cast<ManagementObject>().First();
            return osArchitecture["OSArchitecture"].ToString();
        }

        private string GetDiskSpace()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            StringBuilder sb = new StringBuilder();
            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                string diskModel = wmi_HD["Model"].ToString();  // Model Number
                string diskSize = Math.Round((Convert.ToDouble(wmi_HD["Size"]) / (1024 * 1024 * 1024))).ToString() + " GB";  // size in GB
                if (sb.Length > 0)
                {
                    sb.AppendLine();
                }
                sb.Append(diskModel + ", Size : " + diskSize);
            }
            return sb.ToString();
        }
        private void ClsBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
