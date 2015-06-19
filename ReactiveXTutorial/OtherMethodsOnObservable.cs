using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Windows.Forms;

namespace ReactiveXTutorial
{
    /// <summary>
    /// Working through the 'Curing the asynchronous blues with the Reactive Extensions for .NET' tutorial (C:\Users\csudbery\Dropbox\IT Training\CSharp Stuff\ReactiveX\ReactiveX Tutorial.pdf)
    /// </summary>
    public static class OtherMethodsOnObservable
    {
        /// <summary>
        /// This blocks the calling thread until execution is completed, effectively allowing synschronous usage (as opposed to the default which is asynchronous)
        /// !! Note that although this is in the tutorial, it doesn't seem to exist any more
        ///     According to this article, Run has now been renamed to ForEach: https://social.msdn.microsoft.com/Forums/en-US/f987b3ef-c4ec-4b74-8ca5-a32bff364dde/where-has-the-run-extension-method-gone?forum=rx
        ///     But apparently ForEach is no longer supported - see here: http://go.microsoft.com/fwlink/?LinkId=260866
        /// The message you get when you try to use it looks like this: "Instead, use the async version in combination with C# async/await support. 
        ///     If you need a blocking operation, use Wait or convert the resulting observable sequence to a Task and block."
        /// </summary>
        public static void ForEach()
        {
            IObservable<int> source = Observable.Range(0, 10);

            //source.Run
            //    (
            //        (int x) => Console.WriteLine("OnNext:  {0}", x),
            //        (Exception ex) => Console.WriteLine("OnError: {0}", ex.Message),
            //        () => Console.WriteLine("OnCompleted")
            //    );
            source.ForEach
                (
                    (int x) => Console.WriteLine("OnNext:  {0}", x)
                );
        }

        /// <summary>
        /// Project Data Using Select:
        /// Will show the coordinates of the mouse as it moves, AND show any text input by the user, outputting to the console
        ///  - this time using Select to do some useful mapping / projecting
        /// </summary>
        public static void Select()
        {
            var textBox = new TextBox();
            var form = new Form
            {
                Controls = { textBox }
            };

            // Use Select to map evt.EventArgs.Location directly so that every element in the sequence is a position element (which is all we're interested in)
            var moves = Observable.FromEventPattern<MouseEventArgs>(form, "MouseMove")
                                  .Select(evt => evt.EventArgs.Location);

            var input = Observable.FromEventPattern<EventArgs>(textBox, "TextChanged")
                                  .Select(evt => ((TextBox)evt.Sender).Text);

            // Now we have two separate subscriptions
            IDisposable movesSubscription = moves.Subscribe(pos => Console.WriteLine("Mouse at: " + pos));
            IDisposable inputSubscription = input.Subscribe(text => Console.WriteLine("User wrote: " + text));

            // CompositeDisposable just allows us to dispose all disposables in one go
            using (new CompositeDisposable(movesSubscription, inputSubscription))
            {
                Application.Run(form);
            }
        }

        /// <summary>
        /// Track Specific Mouse Move Using Where:
        /// Will show the coordinates of the mouse as it moves, AND show any text input by the user, outputting to the console
        ///  - this time using Where to do some useful filtering
        /// </summary>
        public static void Where()
        {
            var lbl = new Label();
            var form = new Form
            {
                Controls = { lbl }
            };

            // Use Where to specify only a subset of mouse movements (when the mouse hits the diagonal bisecting line in the middle of the form)
            var moves = Observable.FromEventPattern<MouseEventArgs>(form, "MouseMove")
                                  .Select(evt => evt.EventArgs.Location)
                                  .Where(pos => pos.X == pos.Y);

            using (moves.Subscribe(pos => Console.WriteLine("Mouse at: " + pos)))
            {
                Application.Run(form);
            }
        }

        /// <summary>
        /// Detect When Sequence Output Has Changed
        /// Shows text input, but if user overwrites input with the same input, will not report a change
        /// (using DistinctUntilChanged method)
        /// Note that DistinctUntilChanged can also be passed an IEqualityComparer to specify how equality of elements is defined (not demonstrated here)
        /// </summary>
        public static void DistinctUntilChanged()
        {
            var textBox = new TextBox();
            var form = new Form
            {
                Controls = { textBox }
            };

            var input = Observable.FromEventPattern<EventArgs>(textBox, "TextChanged")
                                  .Select(evt => ((TextBox)evt.Sender).Text)
                                  .DistinctUntilChanged();

            using (input.Subscribe(inp => Console.WriteLine("User wrote: " + inp)))
            {
                Application.Run(form);
            }
        }

        /// <summary>
        /// Use Do To Observe The Flow Of Events:
        /// Each call on your sequence represents another step in the flow of events from source to your observer
        /// You can insert a call to Do at any point, which allows you to log what is actually happening, and therefore debug
        /// </summary>
        public static void Do()
        {
            var textBox = new TextBox();
            var form = new Form
            {
                Controls = { textBox }
            };

            var input = Observable.FromEventPattern<EventArgs>(textBox, "TextChanged")
                                  .Do(evt => Console.WriteLine("Before Select: " + ((TextBox)evt.Sender).Text))
                                  .Select(evt => ((TextBox)evt.Sender).Text)
                                  .Do(inp => Console.WriteLine("Before DistinctUntilChanged: " + inp))
                                  .DistinctUntilChanged();

            using (input.Subscribe(inp => Console.WriteLine("User wrote: " + inp)))
            {
                Application.Run(form);
            }
        }

        /// <summary>
        /// Use Throttle To Slow Things Down:
        /// If we are using the text input to look something up in as dictionary service, and the user is a fast typist, we don't want to look up for every character typed.
        /// Instead we can wait a second before performing the lookup.
        /// </summary>
        public static void Throttle()
        {
            var textBox = new TextBox();
            var form = new Form
            {
                Controls = { textBox }
            };

            var input = Observable.FromEventPattern<EventArgs>(textBox, "TextChanged")
                                  .Select(evt => ((TextBox)evt.Sender).Text)
                                  .Throttle(TimeSpan.FromSeconds(1))
                                  .DistinctUntilChanged();

            using (input.Subscribe(inp => Console.WriteLine("User wrote: " + inp)))
            {
                Application.Run(form);
            }
        }

        /// <summary>
        /// Use Timestamp And Do To Observe When Things Happen:
        /// We can add a timestamp into the sequence of actions, then use Do to see when things are happening
        /// !! Important! After calling Timestamp you have to project your collection back into a simple string again to remove the timestamp element
        /// </summary>
        public static void Timestamp()
        {
            var textBox = new TextBox();
            var form = new Form
            {
                Controls = { textBox }
            };

            var input = Observable.FromEventPattern<EventArgs>(textBox, "TextChanged")
                                  .Select(evt => ((TextBox)evt.Sender).Text)

                                  .Timestamp()
                                  .Do(inp => Console.WriteLine("Before throttle: " + inp.Timestamp.Second + ":" + inp.Timestamp.Millisecond + " - " + inp.Value))
                                  .Select(t => t.Value) // project your collection back into a simple string again to remove the timestamp element

                                  .Throttle(TimeSpan.FromSeconds(1))

                                  .Timestamp()
                                  .Do(inp => Console.WriteLine("After throttle: " + inp.Timestamp.Second + ":" + inp.Timestamp.Millisecond + " - " + inp.Value))
                                  .Select(t => t.Value) // project your collection back into a simple string again to remove the timestamp element

                                  .DistinctUntilChanged();

            using (input.Subscribe(inp => Console.WriteLine("User wrote: " + inp)))
            {
                Application.Run(form);
            }
        }

        /// <summary>
        /// We can create an extension method which contains all the code needed to do timestamped logging
        /// Note that I have also created a more generic extension method, ActOnTimestampedValue, 
        /// which allows the user to specify an action other than writing the timestamp to the console
        /// (not demonstrated here)
        /// </summary>
        public static void UseExtensionMethodForTimestampedLogging()
        {
            var textBox = new TextBox();
            var form = new Form
            {
                Controls = { textBox }
            };

            var input = Observable.FromEventPattern<EventArgs>(textBox, "TextChanged")
                .Select(evt => ((TextBox)evt.Sender).Text)
                // Use new extension method, which collapses three lines into one
                .LogTimestampedValue("Before throttle")
                .Throttle(TimeSpan.FromSeconds(1))
                // Use new extension method, which collapses three lines into one
                .LogTimestampedValue("After throttle")
                .DistinctUntilChanged();

            using (input.Subscribe(inp => Console.WriteLine("User wrote: " + inp)))
            {
                Application.Run(form);
            }
        }
    }
}