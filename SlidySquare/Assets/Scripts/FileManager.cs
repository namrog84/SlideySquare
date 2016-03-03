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
            string saveLoc = GameCore.PersistentPath + "/" + filename;
            Debug.Log("Saving " + saveLoc);
            IFormatter formatter = new BinaryFormatter();
            using (var stream = new FileStream(saveLoc, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var zipper = new GZipStream(stream, CompressionMode.Compress, false))
                {
                    formatter.Serialize(zipper, theObject);
                }
            }
        }

        internal static void SaveDownloadedToFile(string filename, Map map)
        {
            var downloadLoc = GameCore.PersistentPath + "/downloaded";
            if (!Directory.Exists(downloadLoc))
            {
                Directory.CreateDirectory(downloadLoc);
            }
            SaveObjectToFile("downloaded/" + filename, map);
        }

      

        internal static void Delete(string filename)
        {
            File.Delete(GameCore.PersistentPath + "/" + filename);
        }

        public static object LoadFromFile(string filename)
        {
            object result;
            IFormatter formatter = new BinaryFormatter();
            Debug.Log("Load " + GameCore.PersistentPath + "/" + filename);
            using (Stream stream = new FileStream(GameCore.PersistentPath + "/" + filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var zipper = new GZipStream(stream, CompressionMode.Decompress))
                {
                    result = formatter.Deserialize(zipper);
                }
            }
            return result;
        }
        internal static List<string> GetSavedLevelNames()
        {
            return Directory.GetFiles(GameCore.PersistentPath).Select(fullname => Path.GetFileName(fullname)).ToList();
        }
        internal static List<string> GetDownloadedLevelNames()
        {
            var downloadLoc = GameCore.PersistentPath + "/downloaded";
            if (!Directory.Exists(downloadLoc))
            {
                Directory.CreateDirectory(downloadLoc);
            }
            return Directory.GetFiles(downloadLoc).Select(fullname => Path.GetFileName(fullname)).ToList();
        }


    }
}
