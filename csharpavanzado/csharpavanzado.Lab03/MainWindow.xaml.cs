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

        private void btnGetResult_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                string Result = "Resultado obtenido";
                //lblResult.Dispatcher.BeginInvoke(new Action(() => ShowMessage(Result)));
                lblResult.Dispatcher.BeginInvoke(new ShowDelegate(ShowMessage), Result);
            });
        }
        #region Ejercicio 1 - Utilizando el objecto Dispatcher
        delegate void ShowDelegate(string message);
        private void ShowMessage(string message)
        {
            lblResult.Content = message;
        }
        #endregion
    }
}
