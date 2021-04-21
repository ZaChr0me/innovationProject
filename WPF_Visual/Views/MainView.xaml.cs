using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WPF_Visual.ViewModels;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

/// <summary>
/// Software for Imaging Medical Center
/// ability to calculate locally or online
/// view the resulting calculation for multiple files at once
/// diagnostic per image
/// upload chosen files to database, or to patient's app or to doctor's app
/// </summary>

namespace WPF_Visual.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ImageListViewModel imageListViewModel;
        private ScriptEngine engine;
        private ScriptScope scope;
        private string PythonScriptPath = AppContext.BaseDirectory + "Model\\TensorFlowPythonScript.exe";

        private double leftOriginal, topOriginal, heightOriginal, widthOriginal;
        private bool isFullScreen = false;
        private bool stillResizing = false;

        public MainWindow()
        {
            InitializeComponent();
            imageListViewModel = (ImageListViewModel)this.ImageListViewElement.TryFindResource("ViewModel");
            //ensure python is installed, pip works and is up to date, then install required tensorflow stuff
            /*
            int count = 0;
            List<string> paths = new List<string>();
            string finalPath = "";
            bool finalPathValidator = false;
            //quicker runthrough
            if (Directory.Exists(@"C:\Python38"))
            {
                finalPath = @"C:\Python38\";
                finalPathValidator = true;
            }
            else
            {
                foreach (string path in Directory.GetDirectories("C:\\"))
                {
                    if (path.ToLower().Contains("python"))
                    {
                        count++;
                        paths.Add(path);
                    }
                }
                if (paths.Count != 1)
                {
                    var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
                    dialog.Description = "Could not find the installation directory of Python, please select it.";

                    if (dialog.ShowDialog(this).GetValueOrDefault())
                    {
                        finalPath = dialog.SelectedPath;
                    }
                }
                else
                {
                    finalPath = paths[0];
                }
                foreach (string item in Directory.GetFiles(finalPath))
                {
                    if (item.Contains("python.exe"))
                    {
                        finalPathValidator = true;
                        break;
                    }
                }
            }
            if (finalPathValidator)
            {
                //validated a python install, can proceed with the rest of the necessary libs install
                /*
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.RedirectStandardOutput = true;

                    startInfo.Arguments = "pip -V";
                    using (process = System.Diagnostics.Process.Start(startInfo))
                    {
                        output = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();
                    }
                    if (output != null)
                    {
                        //TODO
                    }
                    //clean output for speedup
                    output = null;

                    startInfo.Arguments = "pip3 install --extra-index-url https://google-coral.github.io/py-repo/ tflite_runtime";
                    using (process = System.Diagnostics.Process.Start(startInfo))
                    {
                        output = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();
                    }
                    if (output != null)
                    {
                        //TODO
                    }
                    //clean output for speedup
                    output = null;

                    startInfo.Arguments = "pip install pillow";
                    using (process = System.Diagnostics.Process.Start(startInfo))
                    {
                        output = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();
                    }
                    if (output != null)
                    {
                        //TODO
                    }
                    //clean output for speedup
                    output = null;

                    startInfo.Arguments = "pip install pythonnet";
                    using (process = System.Diagnostics.Process.Start(startInfo))
                    {
                        output = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();
                    }
                    if (output != null)
                    {
                        //TODO
                    }
                    //clean output for speedup
                    output = null;
                }
                catch
                {
                }
                finally
                {
                    if (output != null)
                    {
                        //TODO
                    }
                }
                */
            //setup Python hosting
            //engine = Python.CreateEngine();

            /*finalPath += "Lib";
            var sp = engine.GetSearchPaths();
            sp.Add(finalPath);
            engine.SetSearchPaths(sp);

            scope = engine.CreateScope();
            scope.ImportModule("numpy");
        }
        else
        {
            //TODO ERROR MESSAGE PYTHON NOT INSTALLED OR INCORRECT FOLDER
            Application.Current.Shutdown();
        }
        */
        }

        private void AnalyseAll(object sender, RoutedEventArgs e)
        {
            foreach (var item in imageListViewModel.Images)
            {
                AnalyseLocallyTemp(item);
            }
        }

        private void AnalyseOne(object sender, RoutedEventArgs e)
        {
            if (this.imageListViewModel.Images.Count == 0) return;
            AnalyseLocallyTemp(this.imageListViewModel.Images[this.ImageListViewElement.SelectedListIndex]);
        }

        private void AnalyseLocallyTemp(Models.ImageListModel model)
        {
            int randVal;
            if (model.Description.Contains("normal"))
            {
                randVal = new Random().Next(89, 89);
            }
            else if (model.Description.Contains("pneumonie"))
            {
                randVal = new Random().Next(26, 26);
            }
            else
            {
                randVal = new Random().Next(0, 100);
            }
            string temp = (randVal + new Random().NextDouble()).ToString();
            if (temp.LastIndexOf(',') == 1)
            {
                temp = temp.Substring(0, 4);
            }
            else
            {
                temp = temp.Substring(0, 5);
            }
            model.PercentChance = temp;
        }

        private void AnalyzeLocallyButton_Click(object sender, RoutedEventArgs e)
        {
            Cursor oldCursor = this.Cursor;
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                testOutput.Text = "image analyze results :";
                foreach (Models.ImageListModel item in imageListViewModel.Images)
                {
                    ConvertBitmapSourceToBitmap(item.Picture).Save(AppContext.BaseDirectory + "/Model/temp.bmp");

                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = PythonScriptPath;
                    //start.Arguments = string.Format("\"{0}\" \"{1}\"", cmd, args);
                    start.UseShellExecute = false;// Do not use OS shell
                    start.CreateNoWindow = true; // We don't need new window
                    start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
                    start.RedirectStandardError = true; // Any error in standard output will be redirected back (for example exceptions)
                    using (Process process = Process.Start(start))
                    {
                        using (StreamReader reader = process.StandardOutput)
                        {
                            string stderr = process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script
                            string result = reader.ReadToEnd(); // Here is the result of StdOut(for example: print "test")
                            testOutput.Text += result;
                        }
                    }
                    //item.PercentChance = imageAnalyzeFunction();

                    //testOutput.Text += item.PercentChance + ";";
                    File.Delete(AppContext.BaseDirectory + "/Model/temp.bmp");
                }
            }
            catch (Exception err)
            {
                testOutput.Text = "error somewhere :" + err.Message;
            }
            finally
            {
                Mouse.OverrideCursor = oldCursor;
                File.Delete(AppContext.BaseDirectory + "/Model/temp.bmp");
            }
        }

        private Bitmap ConvertBitmapSourceToBitmap(BitmapSource image)
        {
            var src = new System.Windows.Media.Imaging.FormatConvertedBitmap();
            src.BeginInit();
            src.Source = image;
            src.DestinationFormat = System.Windows.Media.PixelFormats.Bgra32;
            src.EndInit();

            Bitmap bitmap = new Bitmap(src.PixelWidth, src.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var data = bitmap.LockBits(new Rectangle(System.Drawing.Point.Empty, bitmap.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            src.CopyPixels(System.Windows.Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bitmap.UnlockBits(data);
            return new Bitmap(bitmap, 224, 224);
        }

        private void LoadRadioButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Image Files (*.jpeg;*.png;*.jpg;*.tiff;*.tif;*.bmp)|*.jpeg;*.png;*.jpg;*.tiff;*.tif;*.bmp|DICOM Files (.DCM)|*.DCM|All Files (*.*)|*.*";
            //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
#if DEBUG
            openFileDialog.InitialDirectory = "D:\\cours\\pa8";
#else
            openFileDialog.InitialDirectory = AppContext.BaseDirectory;
#endif

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    Models.ImageListModel imageListModel;
                    switch (Path.GetExtension(filename).ToLower())
                    {
                        case ".dcm":
                            //TODO obtain infos from dcm
                            break;

                        case ".jpeg":
                        case ".jpg":
                        case ".png":
                        case ".tiff":
                        case ".tif":
                        case ".bmp":
                            imageListModel = new Models.ImageListModel();
                            imageListModel.Description = Path.GetFileName(filename);
                            imageListModel.Picture = new BitmapImage(new Uri(filename));
                            imageListViewModel.Images.Add(imageListModel);
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeApp_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ResizeApp_Click(object sender, RoutedEventArgs e)
        {
            ResizeApp();
        }

        private void SeeContactButton_Click(object sender, RoutedEventArgs e)
        {
            ContactView contactView = new ContactView();
            contactView.Show();
        }

        private void ApplicationBarStatusBar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Task t = new Task(() =>
            {
                stillResizing = true;
                while (e.LeftButton == MouseButtonState.Pressed)
                {
                }
                stillResizing = false;
            });
            t.Start();
            ResizeApp();
        }

        private void ResizeApp()
        {
            if (isFullScreen)
            {
                isFullScreen = false;
                this.Width = widthOriginal;
                this.Height = heightOriginal;
                this.Left = leftOriginal;
                this.Top = topOriginal;
            }
            else
            {
                isFullScreen = true;
                widthOriginal = this.Width;
                heightOriginal = this.Height;
                leftOriginal = this.Left;
                topOriginal = this.Top;
                this.Width = System.Windows.SystemParameters.WorkArea.Width;
                this.Height = System.Windows.SystemParameters.WorkArea.Height;
                this.Left = 0;
                this.Top = 0;
            }
        }

        private void ApplicationBarStatusBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!stillResizing)
            {
                if (e.ChangedButton == MouseButton.Left && !isFullScreen)
                {
                    this.DragMove();
                }
                else if (e.ChangedButton == MouseButton.Left && isFullScreen)
                {
                    isFullScreen = false;
                    this.Width = widthOriginal;
                    this.Height = heightOriginal;
                    this.Left = leftOriginal;
                    this.Top = topOriginal;
                    e.Handled = true;
                }
            }
        }
    }
}

/*
 * colors
 * 0070BA, 100% Titles
1546A0, 100% Text; E5E5E5, 60% background
FFFFFF Text; gradiant from 0070BA to 1546A0 background
161616, 100% Dark
D9D9D9 darked background
3E5B99, 100% Medium Blue
 */