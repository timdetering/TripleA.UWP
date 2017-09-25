﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripleA.ViewModel
{
    public class MenuItemViewModel : ViewModelBase
    {
        public string Label { get; set; }

        private char symbolAsChar;
        public char SymbolAsChar
        {
            get
            {
                return symbolAsChar;
            }
            set
            {
                symbolAsChar = value;
                RaisePropertyChanged();
            }
        }
        public Type PageType { get; set; }
    }
}
