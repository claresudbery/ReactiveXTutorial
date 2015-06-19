using System;

namespace ReactiveXTutorial
{
    public static class ReactiveXGeneralExamples
    {
        /// <summary>
        /// source and handler not initialised, so not actually functional code
        /// </summary>
        public static void TrivialInitialisation()
        {
            IObservable<int> source;
            IObserver<int> handler;

            // !! Won't build, as source and handler not initialised !!
            //IDisposable subscription = source.Subscribe(handler);

            Console.WriteLine("Press ENTER to unsubscribe...");
            Console.ReadLine();

            // !! Won't build, as source and handler not initialised !!
            //subscription.Dispose(); 
        }

        /// <summary>
        /// source and handler not initialised, so not actually functional code
        /// </summary>
        public static void ImplementingHandlerMethodsUsingDelegates()
        {
            IObservable<int> source;

            // !! Won't build, as source and handler not initialised !!
            //IDisposable subscription = source.Subscribe
            //    (
            //        (int x) => Console.WriteLine("Received {0} from source.", x),
            //        (Exception ex) => Console.WriteLine("Source signaled an error: {0}.", ex.Message),
            //        () => Console.WriteLine("Source said there are no messages to follow anymore.")
            //    );

            Console.WriteLine("Press ENTER to unsubscribe...");
            Console.ReadLine();

            // !! Won't build, as source and handler not initialised !!
            //subscription.Dispose(); 
        }
    }
}