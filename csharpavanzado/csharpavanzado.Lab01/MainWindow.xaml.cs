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

namespace csharpavanzado.Lab01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateTask();
        }

        void CreateTask()
        {
            Task T;
            var Code = new Action(ShowMessage);

            T = new Task(Code);

            Task T2 = new Task(delegate
            {
                MessageBox.Show("Ejecutando una tarea en un delegado anonimo");
            });
            // Expresion Lambda que representa a un delegado que invoca a un metodo con nombre
            Task T3 = new Task(() => ShowMessage());
            // Expresion Lambda que representa a un delegado que invoca a un metodo anonimo.
            Task T4 = new Task(() => MessageBox.Show("Ejecutanto la Tarea 4"));
            // Lambda que representa un bloque de codigo
            Task T5 = new Task(() =>
            {
                DateTime CurrentDate = DateTime.Today;
                DateTime StartDate = CurrentDate.AddDays(30);
                MessageBox.Show($"Tarea 5. Fecha calculada: {StartDate}");
            });
            // Task que acepta un parametro
            Task T6 = new Task((message) =>
            MessageBox.Show(message.ToString()), "Expression lambda con parametros");

            Task T7 = new Task(() => AddMessage("Ejecutandose la tarea"));
            T7.Start(); // Iniciando una tarea

            AddMessage("En el hilo principal");

            // Usando la clase TaskFactory, para crear y pner en cola una tarea
            var T8 =
                Task.Factory.StartNew(() => AddMessage("Tarea iniciada con TaskFactory"));

            // Otra alternativa es usar el metodo estatico Task.Run como atajo al metodo Task.Factory.StartNew
            var T9 = Task.Run(() => AddMessage("Tarea ejecutada con Task.Run"));
        }

        void ShowMessage()
        {
            MessageBox.Show("Ejecutando el metodo ShowMessage");
        }

        void AddMessage(string message)
        {
            // Obtener el identificador del thread que invoca al metodo AddMessage
            int CurrentThreadID = Thread.CurrentThread.ManagedThreadId;
            // Con dispatcher podemos hacer que un hilo distinto al hilo principal pueda modificar los elementos de la interfaz de usuario
            this.Dispatcher.Invoke(() =>
            {
                Messages.Content +=
                    $"Mensaje: {message}, " +
                    $"Hilo actual: {CurrentThreadID}\n";
            });
        }
        /// <summary>
        /// Envia una cadena de texto a la ventana Output de Visual Studio
        /// </summary>
        /// <param name="message"></param>
        void WriteToOutput(string message)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Mensaje: {message}, " +
                $"Hilo actual: {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
