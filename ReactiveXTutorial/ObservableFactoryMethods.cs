using System;
using System.Reactive.Linq;

namespace ReactiveXTutorial
{
    /// <summary>
    /// Working through the 'Curing the asynchronous blues with the Reactive Extensions for .NET' tutorial (C:\Users\csudbery\Dropbox\IT Training\CSharp Stuff\ReactiveX\ReactiveX Tutorial.pdf)
    /// </summary>
    public static class ObservableFactoryMethods
    {
        /// <summary>
        /// The output looks like this:
        ///   OnCompleted 
        /// </summary>
        public static void Empty()
        {
            IObservable<int> source = Observable.Empty<int>();

            IDisposable subscription = source.Subscribe
                (
                    (int x) => Console.WriteLine("OnNext:  {0}", x),
                    (Exception ex) => Console.WriteLine("OnError: {0}", ex.Message), 
                    () => Console.WriteLine("OnCompleted")
                );

            Console.WriteLine("Press ENTER to unsubscribe...");
            Console.ReadLine();

            subscription.Dispose();
        }

        /// <summary>
        /// The output looks like this:
        ///   OnError: Oops 
        /// </summary>
        public static void Throw()
        {
            IObservable<int> source = Observable.Throw<int>(new Exception("Oops"));

            IDisposable subscription = source.Subscribe
                (
                    (int x) => Console.WriteLine("OnNext:  {0}", x),
                    (Exception ex) => Console.WriteLine("OnError: {0}", ex.Message),
                    () => Console.WriteLine("OnCompleted")
                );

            Console.WriteLine("Press ENTER to unsubscribe...");
            Console.ReadLine();

            subscription.Dispose();
        }

        /// <summary>
        /// The output looks like this:
        ///   OnNext: 42     
        ///   OnCompleted 
        /// </summary>
        public static void Return()
        {
            IObservable<int> source = Observable.Return(42);

            IDisposable subscription = source.Subscribe
                (
                    (int x) => Console.WriteLine("OnNext:  {0}", x),
                    (Exception ex) => Console.WriteLine("OnError: {0}", ex.Message),
                    () => Console.WriteLine("OnCompleted")
                );

            Console.WriteLine("Press ENTER to unsubscribe...");
            Console.ReadLine();

            subscription.Dispose();
        }

        /// <summary>
        /// The output looks like this:
        ///   OnNext: 5     
        ///   OnNext: 6     
        ///   OnNext: 7     
        ///   OnCompleted
        /// </summary>
        public static void Range()
        {
            IObservable<int> source = Observable.Range(5, 3);

            IDisposable subscription = source.Subscribe
                (
                    (int x) => Console.WriteLine("OnNext:  {0}", x),
                    (Exception ex) => Console.WriteLine("OnError: {0}", ex.Message),
                    () => Console.WriteLine("OnCompleted")
                );

            Console.WriteLine("Press ENTER to unsubscribe...");
            Console.ReadLine();

            subscription.Dispose();
        }

        /// <summary>
        /// The output looks like this:
        ///   OnNext: 0     
        ///   OnNext: 1     
        ///   OnNext: 4     
        ///   OnNext: 9     
        ///   OnNext: 16     
        ///   OnCompleted 
        /// </summary>
        public static void Generate()
        {
            // This is basically a for loop
            IObservable<int> source = Observable.Generate
            (
                // Starting value is 0 (first part of the for statement)
                0,
                // Keep going as long as i < 5 (second part of the for statement)
                i => i < 5,
                // How to get to the next iterator (third part of the for statement)
                i => i + 1,
                // On each iteration, execute the following statement (body of the for statement)
                i => i * i
            ); 

            IDisposable subscription = source.Subscribe
                (
                    (int x) => Console.WriteLine("OnNext:  {0}", x),
                    (Exception ex) => Console.WriteLine("OnError: {0}", ex.Message),
                    () => Console.WriteLine("OnCompleted")
                );

            Console.WriteLine("Press ENTER to unsubscribe...");
            Console.ReadLine();

            subscription.Dispose();
        }

        /// <summary>
        /// Same as Generate (see above) (in fact uses an overload of Generate), but specifying time between iterations
        /// The output looks like this:
        ///   OnNext: 0     
        ///   OnNext: 1     
        ///   OnNext: 4     
        ///   OnNext: 9     
        ///   OnNext: 16     
        ///   OnCompleted 
        /// </summary>
        public static void Generate_WithTime()
        {
            // This is basically a for loop with an added time element
            IObservable<int> source = Observable.Generate
            (
                // Starting value is 0 (first part of the for statement)
                0,
                // Keep going as long as i < 5 (second part of the for statement)
                i => i < 5,
                // How to get to the next iterator (third part of the for statement)
                i => i + 1,
                // On each iteration, execute the following statement (body of the for statement)
                i => i * i,
                // Specify the iteration time between each result
                i => TimeSpan.FromSeconds(i)
            );

            IDisposable subscription = source.Subscribe
                (
                    (int x) => Console.WriteLine("OnNext:  {0}", x),
                    (Exception ex) => Console.WriteLine("OnError: {0}", ex.Message),
                    () => Console.WriteLine("OnCompleted")
                );

            Console.WriteLine("Press ENTER to unsubscribe...");
            Console.ReadLine();

            subscription.Dispose();
        }

        /// <summary>
        /// Produces no output at all!
        /// </summary>
        public static void Never()
        {
            IObservable<int> source = Observable.Never<int>();

            IDisposable subscription = source.Subscribe
                (
                    (int x) => Console.WriteLine("OnNext:  {0}", x),
                    (Exception ex) => Console.WriteLine("OnError: {0}", ex.Message),
                    () => Console.WriteLine("OnCompleted")
                );

            Console.WriteLine("Press ENTER to unsubscribe...");
            Console.ReadLine();

            subscription.Dispose();
        }
    }
}