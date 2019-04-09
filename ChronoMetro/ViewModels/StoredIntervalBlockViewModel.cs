using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChronoMetro.Data.Interval;

namespace ChronoMetro.ViewModels
{
    class StoredIntervalBlockViewModel : ViewModelBase
    {
        public IntervalBlockStore IntervalBlockStore { get; set; }
        
        private bool _isSelectionEnabled;
        private ObservableCollection<IntervalBlockItem> _blockCollection;

        public bool IsSelectionEnabled
        {
            get { return _isSelectionEnabled; }
            set { this.SetProperty(ref _isSelectionEnabled, value); }
        }

        public ObservableCollection<IntervalBlockItem> BlockCollection
        {
            get { return _blockCollection; }
            set { this.SetProperty(ref _blockCollection, value); }
        }

        public StoredIntervalBlockViewModel()
        {
            IntervalBlockStore = IntervalBlockStore.Prepare();
            BlockCollection = new ObservableCollection<IntervalBlockItem>(IntervalBlockStore.Items);
        }

        public void RemoveItems(IList selectedItems)
        {
            var removeItems = selectedItems.Cast<IntervalBlockItem>();
            foreach (var item in removeItems)
            {
                IntervalBlockStore.Items.Remove(item);
            }
            BlockCollection = new ObservableCollection<IntervalBlockItem>(IntervalBlockStore.Items);

            IntervalBlockStore.Persist();
        }
    }
}
