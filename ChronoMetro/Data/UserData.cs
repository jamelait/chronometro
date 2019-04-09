using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChronoMetro.Business;

namespace ChronoMetro.Data
{
    public class UserData
    {
        public int AppLaunchCount { get; set; }

        public DateTime AppFirstLaunchDate { get; set; }

        public UserData()
        {
            // Default values
            AppLaunchCount = 0;
            AppFirstLaunchDate = DateTime.MinValue;
        }

        public static UserData GetUserData()
        {
            UserData userData = null;
            try
            {
                using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    //isf.DeleteFile(StoreName);
                    // try get from storage
                    using (var fs = new IsolatedStorageFileStream(Common.UserDataStore, FileMode.OpenOrCreate, isf))
                    {
                        userData = DataContractJSONSerializationHelper.Deserialize(fs, typeof(UserData)) as UserData;
                    }
                }
            }
            catch (IsolatedStorageException)
            {
            }

            // if not existant create new instance with default values
            return userData ?? (new UserData());
        }

        public bool Persist()
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var fs = new IsolatedStorageFileStream(Common.UserDataStore, FileMode.Create, isf))
                {
                    DataContractJSONSerializationHelper.Serialize(fs, this);
                    return true;
                }
            }
            return false;
        }

        public void AppLaunched()
        {
            // Increment the launch count
            AppLaunchCount = AppLaunchCount + 1;
            //AppLaunchCount = 1; // todo remove
            // Set the date of the first launch
            if (AppFirstLaunchDate.CompareTo(DateTime.MinValue) == 0)
                AppFirstLaunchDate = DateTime.Now;
        }
    }
}
