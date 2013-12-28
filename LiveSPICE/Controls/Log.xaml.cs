﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Util;

namespace LiveSPICE
{
    /// <summary>
    /// Interaction logic for Output.xaml
    /// </summary>
    public partial class Log : UserControl, ILog, INotifyPropertyChanged
    {
        public Log()
        {
            InitializeComponent();
        }

        public void Clear() { text.Text = ""; }
        public void Clear_Click(object sender, EventArgs e) { Clear(); }

        private MessageType verbosity = App.Current.Settings.LogVerbosity;
        public MessageType Verbosity 
        {
            get { return verbosity; } 
            set { App.Current.Settings.LogVerbosity = verbosity = value; NotifyChanged("Verbosity"); } 
        }

        public void WriteLine(MessageType Type, string Message, params object[] Format)
        {
            if (Type > verbosity)
                return;
            Dispatcher.InvokeAsync(() =>
            {
                bool atEnd = text.VerticalOffset + text.ViewportHeight >= text.ExtentHeight - 1.0;
                text.AppendText(String.Format(Message, Format) + "\r\n");
                if (atEnd)
                    text.ScrollToEnd();
            });
        }
        
        public void WriteLines(MessageType Type, IEnumerable<string> Lines)
        {
            if (Type > verbosity)
                return;
            Dispatcher.InvokeAsync(() =>
            {
                bool atEnd = text.VerticalOffset + text.ViewportHeight >= text.ExtentHeight - 1.0;
                text.AppendText(String.Join("\r\n", Lines) + "\r\n");
                if (atEnd)
                    text.ScrollToEnd();
            });
        }

        public void WriteException(Exception Ex)
        {
#if DEBUG
            WriteLine(MessageType.Error, "Exception: " + Ex.ToString());
#else
            WriteLine(MessageType.Error, "Exception: " + Ex.Message);
#endif
        }

        void ILog.WriteLine(MessageType Type, string Message, params object[] Format)
        {
            WriteLine(Type, Message, Format);
        }
        void ILog.WriteLines(MessageType Type, IEnumerable<string> Lines)
        {
            WriteLines(Type, Lines);
        }

        // INotifyPropertyChanged.
        private void NotifyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
