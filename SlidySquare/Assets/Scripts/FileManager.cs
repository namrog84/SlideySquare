using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.IO;
using UnityEngine;
using Unity.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace Assets.Scripts
{
    public static class FileManager
    {
        public static void SaveObjectToFile(string filename, object theObject)
        {
            IFormatter formatter = new BinaryFormatter();
            using (var stream = new FileStream(Application.dataPath + "/" + filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var zipper = new GZipStream(stream, CompressionMode.Compress, false))
                {
                    formatter.Serialize(zipper, theObject);
                }
            }
        }
       
        public static object LoadFromFile(string filename)
        {
            object result;
            IFormatter formatter = new BinaryFormatter();

            using (Stream stream = new FileStream(Application.dataPath + "/" + filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var zipper = new GZipStream(stream, CompressionMode.Decompress))
                {
                    result = formatter.Deserialize(zipper);
                }
            }
            return result;
        }

    }
}
