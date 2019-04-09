using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace ChronoMetro.Data
{
    public class DataContractJSONSerializationHelper
    {
        public static void Serialize(Stream streamObject, object objForSerialization)
        {
            if (objForSerialization == null || streamObject == null)
                return;

            DataContractJsonSerializer ser = new DataContractJsonSerializer(objForSerialization.GetType());
            ser.WriteObject(streamObject, objForSerialization);
        }

        public static object Deserialize(Stream streamObject, Type serializedObjectType)
        {
            if (serializedObjectType == null || streamObject == null)
                return null;

            DataContractJsonSerializer ser = new DataContractJsonSerializer(serializedObjectType);
            return ser.ReadObject(streamObject);
        }
    }
}
