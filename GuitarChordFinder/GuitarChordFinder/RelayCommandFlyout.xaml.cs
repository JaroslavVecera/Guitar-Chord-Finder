using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GuitarChordFinder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RelayCommandFlyout : ContentPage
    {
        public ListView ListView;

        public RelayCommandFlyout()
        {
            InitializeComponent();

            BindingContext = new RelayCommandFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class RelayCommandFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<RelayCommandFlyoutMenuItem> MenuItems { get; set; }

            public RelayCommandFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<RelayCommandFlyoutMenuItem>(new[]
                {
                    new RelayCommandFlyoutMenuItem { Id = 0, Title = "Page 1" },
                    new RelayCommandFlyoutMenuItem { Id = 1, Title = "Page 2" },
                    new RelayCommandFlyoutMenuItem { Id = 2, Title = "Page 3" },
                    new RelayCommandFlyoutMenuItem { Id = 3, Title = "Page 4" },
                    new RelayCommandFlyoutMenuItem { Id = 4, Title = "Page 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}