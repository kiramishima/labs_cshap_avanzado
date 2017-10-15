using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace csharpavanzado.Lab02
{
    class Program
    {
        static void Main(string[] args)
        {
            //RunParallelTasks();
            //ParallelLoopIterate();
            //RunLINQ();
            //RunPLINQ();
            //RunContinuationTasks();
            //RunNestedTasks();
            //RunNestedTasks2();
            HandleTaskExceptions();
            Console.WriteLine("Presione <enter> para finalizar.");
            Console.ReadLine();
        }
        #region Ejercicio 1
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
        static void RunLINQ()
        {
            // Declarar una variable para medir el tiempo de ejecucion.
            var S = new System.Diagnostics.Stopwatch();
            S.Start();
            var DTOProducts = NorthWind.Repository.Products.Select(p =>
            new ProductDTO
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                UnitsInStock = p.UnitsInStock
            }).ToList();
            S.Stop();
            Console.WriteLine($"Tiempo de ejecucion con LINQ: {S.ElapsedTicks} Ticks");
        }

        static void RunPLINQ()
        {
            // Declarar una variable para medir el tiempo de ejecucion.
            var S = new System.Diagnostics.Stopwatch();
            S.Start();
            var DTOProducts = NorthWind.Repository.Products.AsParallel().Select(p =>
            new ProductDTO
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                UnitsInStock = p.UnitsInStock
            }).ToList();
            S.Stop();
            Console.WriteLine($"Tiempo de ejecucion con PLINQ: {S.ElapsedTicks} Ticks");
        }
        #endregion
        #endregion

        #region Ejercicio 2
        #region Tarea 1 Tareas de continuacion (Continuation Task)
        // Las Continuation Task permiten encadenar varias tareas juntas para que se ejecuten una tras otra.
        // La tarea que, al finalizar invoca a otra tarea, es conocida como Antecedente y la tarea que esta invoca,
        // es conocida como Continuacion. Podemos pasar datos desde la tarea Antecedente hacia la tarea de Continuacion
        // y podemos controlar la ejecucion de la cadena de tareas de varias formas.
        /// <summary>
        /// Devuelve una lista con los nombres de los productos del repositorio de datos Northwind
        /// </summary>
        /// <returns></returns>
        static List<string> GetProductNames()
        {
            // Simular un proceso de larga duracion
            Thread.Sleep(3000);
            return NorthWind.Repository.Products.Select(p => p.ProductName).ToList();
        }

        static void RunContinuationTasks()
        {
            var FirstTask = new Task<List<string>>(GetProductNames);

            var SecondTask = FirstTask.ContinueWith(antecedent =>
            {
                return ProcessData(antecedent.Result);
            });
            // Iniciamos la primera tarea, para obtener el resultado de la segunda tarea
            FirstTask.Start();
            Console.WriteLine($"Numero de productos procesados: {SecondTask.Result}");
        }
        /// <summary>
        /// Se ejecuta cuando la primera tarea finaliza
        /// </summary>
        /// <param name="productNames"></param>
        /// <returns></returns>
        static int ProcessData(List<string> productNames)
        {
            // Simular procesamiento de datos
            foreach(var ProductName in productNames)
            {
                Console.WriteLine(ProductName);
            }
            return productNames.Count;
        }
        #endregion
        #region Tarea 2 Crear Tablas Anidadas
        static void RunNestedTasks()
        {
            var OuterTask = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Iniciando la tarea externa...");
                var InnerTask = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Iniciando tarea anidada...");
                    // Simular un proceso de larga duracion
                    Thread.Sleep(3000);
                    Console.WriteLine("Finalizando la tarea anidada...");
                });
            });
            OuterTask.Wait();
            Console.WriteLine("Tarea externa finalizada");
        }
        #endregion
        #region Tarea 3 Crear Tareas Hijas
        static void RunNestedTasks2()
        {
            var OuterTask = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Iniciando la tarea externa...");
                var InnerTask = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Iniciando tarea anidada...");
                    // Simular un proceso de larga duracion
                    Thread.Sleep(3000);
                    Console.WriteLine("Finalizando la tarea anidada...");
                }, TaskCreationOptions.AttachedToParent);
            });
            OuterTask.Wait();
            Console.WriteLine("Tarea externa finalizada");
        }
        #endregion
        #endregion

        #region Ejercicio 3: Manejo de excepciones en Tareas
        #region Tarea 1 Atrapar excepciones de Tareas
        static void RunLongTask(CancellationToken token)
        {
            for (int i =0; i < 5; i++)
            {
                // Simular un proceso de larga duracion
                Thread.Sleep(2000);
                // Lanzar un OperationCanceledException si se solicita una cancelacion
                token.ThrowIfCancellationRequested();
            }
        }
        /// <summary>
        /// Metodo que nos permitira manejar excepciones
        /// </summary>
        static void HandleTaskExceptions()
        {
            // Obtener un Token de cancelacion
            var CTS = new CancellationTokenSource();
            var CT = CTS.Token;

            var LongRunningTask = Task.Run(() => RunLongTask(CT), CT);
            CTS.Cancel();
            try
            {
                LongRunningTask.Wait();
            }
            catch(AggregateException ae)
            {
                foreach(var Inner in ae.InnerExceptions)
                {
                    if(Inner is TaskCanceledException)
                    {
                        Console.WriteLine("La tarea fue cancelada.");
                    }
                    else
                    {
                        // Aqui procesamos las excepciones distintas a la cancelacion
                        Console.WriteLine(Inner.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Excepcion: {ex.Message}");
            }
        }
        #endregion
        #endregion
    }
}
