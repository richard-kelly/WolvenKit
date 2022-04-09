using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ReactiveUI;
using WolvenKit.ViewModels.Documents;

namespace WolvenKit.Views.Documents
{
    /// <summary>
    /// Interaktionslogik für WScriptDocumentView.xaml
    /// </summary>
    public partial class WScriptDocumentView : ReactiveUserControl<WScriptDocumentViewModel>
    {
        private CompletionWindow _completionWindow;

        public WScriptDocumentView()
        {
            InitializeComponent();

            textEditor.TextArea.TextEntering += textEditor_TextArea_TextEntering;
            textEditor.TextArea.TextEntered += textEditor_TextArea_TextEntered;

            this.WhenActivated(disposables =>
            {
                if (DataContext is WScriptDocumentViewModel vm)
                {
                    SetCurrentValue(ViewModelProperty, vm);
                }
            });
        }

        private void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == ".")
            {
                _completionWindow = new CompletionWindow(textEditor.TextArea);
                var data = _completionWindow.CompletionList.CompletionData;
                data.Add(new MyCompletionData("Info"));
                data.Add(new MyCompletionData("Error"));
                data.Add(new MyCompletionData("RandomCatFact"));
                data.Add(new MyCompletionData("OpenFile"));
                data.Add(new MyCompletionData("SaveFile"));
                data.Add(new MyCompletionData("ReplacePath"));
                _completionWindow.Show();
                _completionWindow.Closed += delegate {
                    _completionWindow = null;
                };
            }
        }

        private void textEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel?.RunCommand.Execute().Subscribe();
        }

        private class MyCompletionData : ICompletionData
        {
            public MyCompletionData(string text)
            {
                this.Text = text;
            }

            public System.Windows.Media.ImageSource Image
            {
                get { return null; }
            }

            public string Text { get; private set; }

            // Use this property if you want to show a fancy UIElement in the list.
            public object Content
            {
                get { return this.Text; }
            }

            public object Description
            {
                get { return "Description for " + this.Text; }
            }

            public double Priority { get; set; }

            public void Complete(TextArea textArea, ISegment completionSegment,
                EventArgs insertionRequestEventArgs)
            {
                textArea.Document.Replace(completionSegment, this.Text);
            }
        }
    }
}
