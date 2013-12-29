﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace LiveSPICE
{
    public class SignalDisplay : Control, INotifyPropertyChanged
    {
        static SignalDisplay() { DefaultStyleKeyProperty.OverrideMetadata(typeof(SignalDisplay), new FrameworkPropertyMetadata(typeof(SignalDisplay))); }

        protected SignalCollection signals = new SignalCollection();
        public SignalCollection Signals
        {
            get { return signals; }
            set
            {
                if (signals != null)
                    signals.ClockTicked -= Invalidate;
                signals = value;
                if (signals != null)
                    signals.ClockTicked += Invalidate;
                InvalidateVisual();
                NotifyChanged("Signals");
            }
        }

        void Invalidate(object sender, EventArgs e)
        {
            Dispatcher.InvokeAsync(() => InvalidateVisual(), System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        }
                
        private Signal selected;
        public Signal SelectedSignal { get { return selected; } set { selected = value; NotifyChanged("SelectedSignal"); } }

        public void Clear()
        {
            signals.Clear();
            InvalidateVisual();
        }

        // INotifyPropertyChanged interface.
        protected void NotifyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}