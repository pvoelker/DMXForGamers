using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DMXForGamers.Models
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public NotifyPropertyChangedBase()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void AnnouncePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanging != null)
                this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        protected void AnnouncePropertyChanging([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanging(propertyName);
        }
    }
}
