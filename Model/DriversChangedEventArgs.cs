using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace Model
{
    public class DriversChangedEventArgs : EventArgs
    {
        public Track Track { get; set; }

    }
}
