using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

/*
 * VISUAL CHART
 * https://www.figma.com/file/9z8YfGAj990qHK3uM9rZDh/Untitled?node-id=0%3A1
 */

namespace WPF_Visual
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Create the startup window
            Views.MainWindow wnd = new Views.MainWindow();
            // Do stuff here, e.g. to the window
            wnd.Title = "XRai Medical Center Application";
            // Show the window
            wnd.Show();
        }
    }
}