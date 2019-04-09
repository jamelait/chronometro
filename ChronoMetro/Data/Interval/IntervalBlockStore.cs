using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChronoMetro.Business;
using ChronoMetro.Resources;
using ChronoMetro.UserControls;

namespace ChronoMetro.Data.Interval
{
    public class IntervalBlockStore
    {
        public String Version { get; private set; }

        public List<IntervalBlockItem> Items { get; set; }

        public IntervalBlockStore()
        {
            Items = new List<IntervalBlockItem>();
            Version = Common.GetAppVersion();
        }

        public static IntervalBlockStore Prepare()
        {
            IntervalBlockStore intervalBlockStore = null;
            try
            {
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    //isf.DeleteFile(StoreName);
                    // try get from storage
                    using (var fs = new IsolatedStorageFileStream(Common.IntervalBlockStore, FileMode.OpenOrCreate, isf))
                    {
                        intervalBlockStore = DataContractJSONSerializationHelper.Deserialize(fs, typeof(IntervalBlockStore)) as IntervalBlockStore;
                    }
                }
            }
            catch (IsolatedStorageException)
            {
            }

            // if not existant create new instance with default values
            return intervalBlockStore ?? (new IntervalBlockStore());
        }

        public bool Persist()
        {
            try
            {
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var fs = new IsolatedStorageFileStream(Common.IntervalBlockStore, FileMode.Create, isf))
                    {
                        DataContractJSONSerializationHelper.Serialize(fs, this);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SaveNew(string title, IEnumerable<IntervalBlock> blocks)
        {
            if (String.IsNullOrEmpty(title))
                title = AppResources.NoName;

            var item = new IntervalBlockItem();
            item.Title = title;
            
            foreach (var intervalBlock in blocks.OrderBy(b => b.Order))
            {
                item.TotalTime = item.TotalTime.Add(intervalBlock.GetOriginalTime());
                item.Blocks.Add(intervalBlock.CenterText, intervalBlock.GetOriginalTime());
            }

            Items.Add(item);

            return Persist();
        }

        public IntervalBlockItem Get(Guid id)
        {
            return Items.Find(b => b.Guid == id);
        }
    }
}
