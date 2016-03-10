using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.IO;
using UnityEngine;
using Unity.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Gameplay;

namespace Assets.Scripts
{
    public static class FileManager
    {

        public static void Delete(string folder, string filename)
        {
            //remove from gamevault instead!
            //File.Delete(GameCore.PersistentPath + "/" + folder + "/" + filename);
            BoardBank.boards.RemoveAll(x => x.name == filename);
            BoardBank.SaveToFile();
        }

        public static List<string> GetSavedLevelNames()
        {
            return Directory.GetFiles(GameCore.PersistentPath).Select(fullname => Path.GetFileName(fullname)).ToList();
        }

        public static List<string> GetDownloadedLevelNames()
        {
            var downloadLoc = GameCore.PersistentPath + "/downloads";
            if (!Directory.Exists(downloadLoc))
            {
                Directory.CreateDirectory(downloadLoc);
            }
            return Directory.GetFiles(downloadLoc).Select(fullname => Path.GetFileName(fullname)).ToList();
        }


        public static void SaveBoardToFile(GameBoard currentBoard)
        {
            SaveBoardToFile("custom", currentBoard);
            
        }

        public static void SaveBoardToFile(string folderName, GameBoard currentBoard)
        {
            var FolderPath = GameCore.PersistentPath + "/" + folderName;
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            string saveLoc = FolderPath + "/" + currentBoard.name;
            Debug.Log("Saving " + saveLoc);

            IFormatter formatter = new BinaryFormatter();
            using (var stream = new FileStream(saveLoc, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var zipper = new GZipStream(stream, CompressionMode.Compress, false))
                {
                    formatter.Serialize(zipper, currentBoard);
                }
            }
            //TODO: GENERATE THUMBNAIL
        }


        public static GameBoard LoadBoardFromFile(string foldername, string filename)
        {
            string path = GameCore.PersistentPath + "/" + foldername + "/" + filename;
            Debug.Log("Loading " + path);

            GameBoard result;
            IFormatter formatter = new BinaryFormatter();

            if (!File.Exists(path))
            {
                Debug.Log("File Not Found!");
                return null;
            }

            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var zipper = new GZipStream(stream, CompressionMode.Decompress))
                {
                    result = (GameBoard)formatter.Deserialize(zipper);
                }
            }
            return result;
        }
    }
}
