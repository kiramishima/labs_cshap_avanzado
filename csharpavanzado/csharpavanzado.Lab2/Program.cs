using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace csharpavanzado.Lab023
{
    class Program
    {
        static void Main(string[] args)
        {
            //RunParallelTasks();
            ParallelLoopIterate();
            Console.WriteLine("Presione <enter> para finalizar.");
            Console.ReadLine();
        }

        static void RunParallelTasks()
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}. Ejecutar tareas en paralelo");
            // Para ejecutar un conjunto de tareas en paralelo, podemos utilizar el metodo Invoke de la clase Parallel
            // Tiene 2 sobrecargas una recibe objetos Action
            // La segunfa recibe 2 parametros, el primer parametro permite configurar el comportamiento de la operacion
            // y el otro parametro permite proporcionar la serie de objetos Action que deseamos ejecutar
            // Al invocar esots metodos, podemos utilizar exp lambda  para especificar las tareas que queremos ejecutar de forma simultanea
            Parallel.Invoke(
                () => { WriteToConsole("Tarea 1"); },
                () => { WriteToConsole("Tarea 2"); },
                () => { WriteToConsole("Tarea 3"); }
                );
        }

        static void WriteToConsole(string message)
        {
            Console.WriteLine($"{message}. {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(5000); // Simular un proceso de larga duracion
            Console.WriteLine($"Fin de la tarea {Thread.CurrentThread.ManagedThreadId}");
        }

        #region Tarea 3
        static void ParallelLoopIterate()
        {
            // Para ejecutar las iteraciones de un ciclo for en paralelo, podemos utilizar el metodo Parallel.For
            // Posee muchas sobrecargas, en su metodo mas simple, Parallel.For toma 3 parametros.
            // El primero indica el inicio de la operacion, el segundo representa el indice final de la operacion,
            // el tercer parametro es un delegado que es ejecutado por cada iteracion
            int[] SquareNumbers = new int[5];
            Parallel.For(0, 5, i =>
            {
                SquareNumbers[i] = i * i;
                Console.WriteLine($"Calculando el cuadrado de {i}");
            });

            // Para ejecutar las iteraciones del ciclo foreach en paralelo, utilizamos el metodo Parallel.Foreach
            // Al igual que Parallel.For incluye varias sobrecargas, en su forma mas simple toma 2 parametros
            // El primero es una coleccion IEnumerable<TSource> sobre la que vamos a iterar, y el segundo es un delegado
            // Action<TSource> que es ejecutado una vez por iteracion
            Parallel.ForEach(SquareNumbers, n =>
            {
                Console.WriteLine($"Cuadrado de {Array.IndexOf(SquareNumbers, n)}: {n}");
            });
            
        }
        #endregion

        #region Tarea 4 PLINQ
        #endregion
    }
}
