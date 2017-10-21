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
using System.Net;
using System.Threading;

namespace csharpavanzado.Lab03_Exceptions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Taras no observadas, sin embargo podemos implementar un controlador de excepciones de ultimo recurso mediante la subscripcion al evento TaskScheduler.UnobservedTaskException.
            // En el manejador de excepciones, podemos establecer el estatus de la excepcion a Observed para evitar que sea propagada.
            //TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            // Obtenemos las excepciones de la tarea
            var UnobservedTaskExceptions = e.Exception.InnerExceptions;
            // Mostramos los mensajes de las excepciones
            foreach (var ex in UnobservedTaskExceptions)
            {
                WebContent.Dispatcher.Invoke(() =>
                {
                    WebContent.Content += $"{ex.Message}{Environment.NewLine}";
                });
            }
            // Establecemos la excepcion a Observed para que no se propague.
            e.SetObserved();
        }

        private void GetContent_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                using (WebClient Client = new WebClient())
                {
                    //try
                    //{
                    //WebContent.Content = await Client.DownloadStringTaskAsync("https://ticapacitacion.com2");
                    //}
                    //catch (WebException ex)
                    //{
                    //    WebContent.Content = ex.Message;
                    //}
                    string Content = await Client.DownloadStringTaskAsync("https://ticapacitacion.com2");
                    //string Content = await Client.DownloadStringTaskAsync("https://ticapacitacion.com/webapi/northwind/products");
                    WebContent.Dispatcher.Invoke(() =>
                    {
                        WebContent.Content = Content;
                    });
                }
            });
            // Con fines de prueba:
            // Damos tiempo a que se ejecute la tarea
            Thread.Sleep(3000);
            // Forzamos la recoleccion de basura.
            GC.WaitForPendingFinalizers();
            GC.Collect();
            WebContent.Content += "Tarea ejecutada";
        }
    }
}
