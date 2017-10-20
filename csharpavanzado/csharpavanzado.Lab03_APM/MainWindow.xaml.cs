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

// Modelo de Programación Asincrona o simplemente APM por sus siglas en ingles (asynchronous Programming Mdel)
// El Patron APM es implementado tipicamente como 2 metodos: un metodo
// BeginNombreDeLaOperacion que inicia la operacion asincrona y un metodo
// EndNombreDeLaOperacion que proporciona el resultado de la operacion asincrona.
// Normalmente, invocamos al metodo EndNombreDeLaOperacion
namespace csharpavanzado.Lab03_APM
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

        private async void ValidateURL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(URLToValidate.Text);
                // IAsyncResult Result = Request.BeginGetResponse(GetResponse, Request);

                // La biblioteca Task Parallel facilita trabajar con las clases que implementan el patron APM. En lugar
                // de implementar un metodo Callback para invocar al metodo EndNombreDeLaOperacion como en el 
                // caso del metodo GetResponse que implementamos anteriormente, podemos utilizar el metodo
                // TaskFactory.FromAsync para invocar la operacion asincronicamente y devolver el resultado en una
                // sola instruccion.
                HttpWebResponse Response = await Task<WebResponse>.Factory.FromAsync(
                    Request.BeginGetResponse,
                    Request.EndGetResponse, Request) as HttpWebResponse;

                Result.Content = $"Estatus devuelto: {Response.StatusCode}";
            }
            catch (Exception ex)
            {
                Result.Content = ex.Message;
            }
        }

        private void GetResponse(IAsyncResult result)
        {
            // Recuperamos el objeto Request proporcionado como argumento en la
            // lamada al metdo Request.BeginResponse.
            HttpWebRequest Request = (HttpWebRequest)result.AsyncState;
            // Con el objeto Request podemos obtener el resultado de la peticion
            HttpWebResponse Response = (HttpWebResponse)Request.EndGetResponse(result);
            Result.Dispatcher.Invoke(() =>
            {
                Result.Content = $"Estatus devuelto: {Response.StatusCode}";
            });
        }
    }
}
