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
using System.Threading;
using System.Diagnostics;

namespace csharpavanzado.Lab03
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Ejercicio 1 - Utilizando el objecto Dispatcher
        //private void btnGetResult_Click(object sender, RoutedEventArgs e)
        //{
        //    Task.Run(() =>
        //    {
        //        string Result = "Resultado obtenido";
        //        //lblResult.Dispatcher.BeginInvoke(new Action(() => ShowMessage(Result)));
        //        lblResult.Dispatcher.BeginInvoke(new ShowDelegate(ShowMessage), Result);
        //    });
        //}
        
        delegate void ShowDelegate(string message);
        private void ShowMessage(string message)
        {
            lblResult.Content = message;
        }
        #endregion

        #region Ejercicio 2 - Utilizando Async / Await
        // Agregamos el modificador async al metodo click del boton
        private async void btnGetResult_Click(object sender, RoutedEventArgs e)
        {
            // Esto congelara la interfaz del usuario , impidiendonos hacer otra accion hasta que se acabe el proceso de la Task.
            // Para permitir que la interfaz de usuario siga respondiendo, podemos convertir el mandejador de evento en un metodo asincrono.
            // Utilizamos el modificador async para indicar que un metodo puede ejecutarse de forma asincrona.
            lblResult.Content = "Calculando un numero aleatorio";
            Debug.WriteLine($"Hilo que lanza la tarea: {Thread.CurrentThread.ManagedThreadId}");
            Task<int> T = Task.Run<int>(
                () =>
                {
                    Debug.WriteLine($"Hilo que ejecuta la tarea: {Thread.CurrentThread.ManagedThreadId}");
                    System.Threading.Thread.Sleep(10000); // Simulamos proceso
                    return new Random().Next(5000);
                });
            Debug.WriteLine($"Hilo antes del await: {Thread.CurrentThread.ManagedThreadId}");
            // agregamos el modificador await para indicar la ejecucion del metodo puede ser suspendida mientras la operacion es completada
            lblResult.Content += $"Numero obtenido: {await T}";
            Debug.WriteLine($"Hilo despues del await: {Thread.CurrentThread.ManagedThreadId}");
        }
        #endregion
    }
}
