using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChronoMetro.Data.Chronometer;

namespace ChronoMetro.ViewModels
{
    class StoredChronometerViewModel : ViewModelBase
    {
        public ChronometerStore ChronometerStore { get; set; }

        private bool _isSelectionEnabled;
        private ObservableCollection<ChronometerItem> _chronoCollection;

        public bool IsSelectionEnabled
        {
            get { return _isSelectionEnabled; }
            set { this.SetProperty(ref _isSelectionEnabled, value); }
        }

        public ObservableCollection<ChronometerItem> ChronoCollection
        {
            get { return _chronoCollection; }
            //set { _chronoCollection = value; }
            set { this.SetProperty(ref _chronoCollection, value); }
        }

        public StoredChronometerViewModel()
        {
            // Load data from storage
            ChronometerStore = ChronometerStore.Prepare();
            // Initialize the list
            ChronoCollection = new ObservableCollection<ChronometerItem>(ChronometerStore.Items);
        }

        public void RemoveItems(IEnumerable selectedItems)
        {
            var removeItems = selectedItems.Cast<ChronometerItem>();
            foreach (var item in removeItems)
            {
                ChronometerStore.Items.Remove(item);
            }
            ChronoCollection = new ObservableCollection<ChronometerItem>(ChronometerStore.Items);

            ChronometerStore.Persist();
        }
    }
}
