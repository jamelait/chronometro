using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;
using ChronoMetro.Business;

namespace ChronoMetro.Data.Chronometer
{
    public class ChronometerStore
    {
        public String Version { get; private set; }

        public List<ChronometerItem> Items { get; set; }

        public ChronometerStore()
        {
            Items = new List<ChronometerItem>();
            Version = Common.GetAppVersion();
        }

        public static ChronometerStore Prepare()
        {
            ChronometerStore chronometerStore = null;
            try
            {
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    //isf.DeleteFile(StoreName);
                    // try get from storage
                    using (var fs = new IsolatedStorageFileStream(Common.ChronometerStore, FileMode.OpenOrCreate, isf))
                    {
                        chronometerStore = DataContractJSONSerializationHelper.Deserialize(fs, typeof(ChronometerStore)) as ChronometerStore;
                    }
                }
            }
            catch (IsolatedStorageException)
            {
            }

            // if not existant create new instance with default values
            return chronometerStore ?? (new ChronometerStore());
        }

        public bool Persist()
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var fs = new IsolatedStorageFileStream(Common.ChronometerStore, FileMode.Create, isf))
                {
                    DataContractJSONSerializationHelper.Serialize(fs, this);
                    return true;
                }
            }
            return false;
        }

        public bool SaveNew(string label, TimeSpan time)
        {
            var item = new ChronometerItem();
            item.CreatedAt = DateTime.Now;
            item.Label = label;
            item.Time = time;

            if (String.IsNullOrEmpty(item.Label))
                item.Label = item.CreatedAt.ToShortDateString() + " " + item.CreatedAt.ToShortTimeString();

            if (Items == null)
                Items = new List<ChronometerItem>();
            Items.Add(item);

            return Persist();
        }

    }


}
