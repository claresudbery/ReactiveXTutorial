using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Forms;
using Console = System.Console;

namespace ReactiveXTutorial
{
    /// <summary>
    /// Working through the 'Curing the asynchronous blues with the Reactive Extensions for .NET' tutorial (C:\Users\csudbery\Dropbox\IT Training\CSharp Stuff\ReactiveX\ReactiveX Tutorial.pdf)
    /// </summary>
    public static class ImportingDotNetEvents
    {
        /// <summary>
        /// Will show the coordinates of the mouse as it moves
        /// </summary>
        public static void TrackMouseWithoutUsingRx()
        {
            var lbl = new Label();
            var frm = new Form
                {
                    Controls = {lbl}
                };

            frm.MouseMove += (sender, args) =>
                {
                    lbl.Text = args.Location.ToString();
                        // This has become a position-tracking label.     
                };

            Application.Run(frm);

            // We’re sloppy and “forget” to detach the handler... This is harder than you may     
            // expect due to the use of a lambda expression 
        }

        /// <summary>
        /// Will also show the coordinates of the mouse as it moves (this time using Rx)
        /// </summary>
        public static void TrackMouseUsingRx()
        {
            var lbl = new Label();
            var form = new Form
                {
                    Controls = {lbl}
                };

            IObservable<EventPattern<MouseEventArgs>> moves = Observable.FromEventPattern<MouseEventArgs>(form,
                                                                                                          "MouseMove");
            using (moves.Subscribe(evt =>
                {
                    lbl.Text = evt.EventArgs.Location.ToString();
                }))
            {
                Application.Run(form);
                // Proper clean-up just got a lot easier (because of the using clause) 
            }
        }

        /// <summary>
        /// Will show the coordinates of the mouse as it moves, AND show any text inpout by the user 
        /// - this time outputting to the console
        /// </summary>
        public static void TrackMouseAndCaptureTextInputUsingRx()
        {
            var textBox = new TextBox();
            var form = new Form
                {
                    Controls = {textBox}
                };

            IObservable<EventPattern<MouseEventArgs>> moves = Observable.FromEventPattern<MouseEventArgs>(form,
                                                                                                          "MouseMove");
            IObservable<EventPattern<EventArgs>> input = Observable.FromEventPattern<EventArgs>(textBox, "TextChanged");

            // Now we have two separate subscriptions
            IDisposable movesSubscription =
                moves.Subscribe(evt => Console.WriteLine("Mouse at: " + evt.EventArgs.Location));
            IDisposable inputSubscription =
                input.Subscribe(evt => Console.WriteLine("User wrote: " + ((TextBox) evt.Sender).Text));

            // CompositeDisposable just allows us to dispose all disposables in one go
            using (new CompositeDisposable(movesSubscription, inputSubscription))
            {
                Application.Run(form);
            }
        }

        /// <summary>
        /// Will show the coordinates of the mouse as it moves, AND show any text input by the user, outputting to the console
        /// This is an alternative version which uses FromEvent (as in the original tutorial), but it turns out with the latest version of Rx you're better using FromEventPattern (see above)
        /// From this Stack Overflow post: http://stackoverflow.com/questions/11367048/reactive-extensions-missing-method-overloads
        /// </summary>
        public static void AlternativeTrackMouseAndTextInputUsingRx()
        {
            var textBox = new TextBox();
            var form = new Form
                {
                    Controls = {textBox}
                };

            IObservable<MouseEventArgs> mouseMoves = Observable.FromEvent<MouseEventHandler, MouseEventArgs>(
                handlerAction => (_, args) => handlerAction(args),
                handler => form.MouseMove += handler,
                handler => form.MouseMove -= handler);

            IObservable<EventArgs> input = Observable.FromEvent<EventHandler, EventArgs>(
                handlerAction => (_, args) => handlerAction(args),
                handler => textBox.TextChanged += handler,
                handler => textBox.TextChanged -= handler);

            IDisposable mouseMovesSubscription =
                mouseMoves.Select(x => x.Location.ToString()).Subscribe(Console.WriteLine);
            IDisposable inputSubscription = input.Subscribe(x => Console.WriteLine(textBox.Text));

            using (new CompositeDisposable(mouseMovesSubscription, inputSubscription))
            {
                Application.Run(form);
            }
        }
    }
}