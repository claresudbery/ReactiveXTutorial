using System;
using System.Reactive.Linq;

namespace ReactiveXTutorial
{
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
            // This is a bit like a for loop
            IObservable<int> source = Observable.Generate
                (
                    // Starting value is 0
                    0, 
                    // Keep going as long as i < 5
                    i => i < 5, 
                    // On each iteration, execute BOTH of the following statements: First add 1, then multiply by itself
                    i => i + 1, 
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