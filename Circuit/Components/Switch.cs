﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComputerAlgebra;
using System.ComponentModel;

namespace Circuit
{ 
    /// <summary>
    /// Resistor is a linear component with V = R*i.
    /// </summary>
    [Category("Standard")]
    [DisplayName("Switch")]
    [DefaultProperty("Closed")]
    [Description("Switch.")]
    public class Switch : TwoTerminal
    {
        protected bool closed = false;
        [Serialize, Description("Switch position.")]
        public bool Closed { get { return closed; } set { closed = value; NotifyChanged("Closed"); } }

        public Switch() { Name = "S1"; }
        public Switch(bool Closed) : this() { closed = Closed; }

        public override void Analyze(Analysis Mna)
        {
            if (closed)
                Conductor.Analyze(Mna, Name, Anode, Cathode);
        }

        public override void LayoutSymbol(SymbolLayout Sym)
        {
            base.LayoutSymbol(Sym);

            Sym.AddWire(Anode, new Coord(0, 12));
            Sym.AddWire(Cathode, new Coord(0, -12));
            Sym.AddCircle(EdgeType.Black, new Coord(0, 12), 2);
            Sym.AddCircle(EdgeType.Black, new Coord(0, -12), 2);
            Sym.InBounds(new Coord(-10, 0), new Coord(10, 0));

            if (closed)
                Sym.AddWire(new Coord(0, -12), new Coord(0, 12));
            else
                Sym.AddWire(new Coord(0, -12), new Coord(-8, 10));

            Sym.DrawText(() => Name, new Coord(2, 0), Alignment.Near, Alignment.Center);
        }
    }
}
