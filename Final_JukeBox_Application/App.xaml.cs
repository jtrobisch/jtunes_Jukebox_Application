using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WMPLib;

namespace Final_JukeBox_Application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Final_JukeBox_Application.Pages.JukeBoxApp GlobalJukeBox = new Final_JukeBox_Application.Pages.JukeBoxApp();
        public int page_id { get; set; }

    }
}
