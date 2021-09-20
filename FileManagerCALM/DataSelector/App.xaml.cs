using System;
using System.Diagnostics;
using System.Windows;

namespace DataSelector
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
        }

        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Debug.WriteLine("MyHandler caught : " + e.Message);
            Debug.WriteLine("Runtime terminating: {0}", args.IsTerminating);
            Debug.WriteLine("Stack Trace: {0}", e.StackTrace);
        }
    }
}
