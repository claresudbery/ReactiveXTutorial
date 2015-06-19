using System;
using System.Reactive.Linq;

namespace ReactiveXTutorial
{
    /// <summary>
    /// Working through the 'Curing the asynchronous blues with the Reactive Extensions for .NET' tutorial (C:\Users\csudbery\Dropbox\IT Training\CSharp Stuff\ReactiveX\ReactiveX Tutorial.pdf)
    /// </summary>
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

        /// <summary>
        /// Put a breakpoint in the places indicated
        /// Use Debug | Windows | Threads and Call Stack (right-click | Show External Code) to observe the different threads in use
        /// Note that when one of the breakpoints is hit, the ReadLine() statement will be lightly highlighted in grey - this is because the main thread will still be at that point of execution.
        /// See ObservableFactoryMethods.Generate_WithTime for more description of this method
        /// </summary>
        public static void ObservingTheBehaviourOfAnObservableSequence()
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

            // Uses a using block for immediate disposal - wouldn't normally do this
            using (source.Subscribe(
                x => Console.WriteLine("OnNext:  {0}", x), // Put breakpoint here
                ex => Console.WriteLine("OnError: {0}", ex.Message),
                () => Console.WriteLine("OnCompleted") // Put breakpoint here
                ))
            {
                Console.WriteLine("Press ENTER to unsubscribe..."); 
                Console.ReadLine();
            }
        }
    }
}