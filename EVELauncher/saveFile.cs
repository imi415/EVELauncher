using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Net;
using System.Web;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Xml;
using System.Threading;

namespace EVELauncher
{
    class saveFile
    {
        public string path { get; set; }
        public string userName { get; set; }
        public string userPass { get; set; }
        public bool isCloseAfterLaunch { get; set; }
        public bool isDX9Choosed { get; set; }
        public void Write(string Path,string ContentJson)
        {
            File.WriteAllText(Path, ContentJson);
        }
        public string Read(string Path, Encoding Encoding)
        {
            string Result = File.ReadAllText(Path, Encoding);
            return Result;
        }
    }
}
