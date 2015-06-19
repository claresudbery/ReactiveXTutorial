using System;
using System.Reactive;
using System.Reactive.Linq;

namespace ReactiveXTutorial
{
    public static class IObservableExtensions
    {
        /// <summary>
        /// This version allows the user to specify an action
        /// </summary>
        public static IObservable<T> ActOnTimestampedValue<T>(this IObservable<T> source, Action<Timestamped<T>> onNext)
        {
            return source
                .Timestamp()
                .Do(onNext)
                // project your collection back into a simple string again, to remove the timestamp element
                .Select(x => x.Value);
        }

        /// <summary>
        ///  This version allows the user to specify a string, which will be printed to the console, followed by the timestamp
        /// </summary>
        public static IObservable<T> LogTimestampedValue<T>(this IObservable<T> source, string message)
        {
            return source
                .Timestamp()
                .Do(inp => Console.WriteLine(message + ": " + inp.Timestamp.Second + ":" + inp.Timestamp.Millisecond + " - " + inp.Value))
                // project your collection back into a simple string again, to remove the timestamp element
                .Select(x => x.Value);
        }  
    }
}