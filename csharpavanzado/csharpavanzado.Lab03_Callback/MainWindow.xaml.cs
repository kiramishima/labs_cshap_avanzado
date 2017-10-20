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

namespace csharpavanzado.Lab03_Callback
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

        private async void GetProducts_Click(object sender, RoutedEventArgs e)
        {
            await GetAllProducts(RemoveDuplicates);
        }

        async Task GetAllProducts(Action<List<string>> callBack)
        {
            var Products = await Task.Run(() =>
            {
                Thread.Sleep(5000);
                return new List<string>
                {
                    "Azucar", "Café", "Leche", "Frijol", "Queso", "Azucar", "Frijol"
                };
            });

            await Task.Run(() => callBack(Products));
        }

        void RemoveDuplicates(List<string> products)
        {
            var UniqueProducts = products.Distinct();
            Products.Dispatcher.Invoke(() =>
            {
                Products.ItemsSource = UniqueProducts;
            });
        }
    }
}
