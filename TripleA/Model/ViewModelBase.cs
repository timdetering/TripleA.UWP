using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace TripleA.Model
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        //public ResourceLoader ResourceLoader { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fires an event when called. Used to update the UI in the MVVM world.
        /// [CallerMemberName] Ensures only the peoperty that calls it gets the event
        /// and not every property
        /// </summary>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ViewModelBase()
        {
            //ResourceLoader = ResourceLoader.GetForCurrentView();
        }

        public virtual void Refresh()
        {
        }
    }
}