using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unity.IO.Compression;
using UnityEngine;

namespace Assets.Scripts
{
    public static class GameCore
    {
        public static string nextLevel = ""; // ??? 
        public static bool IsLoadingExisting = false;
        public static bool PreppingForSubmit = false; //show 'upload after completion'
        public static bool isDownloaded = false; //yes to show vote

        public static Map currentMap; // saves current/last map;
        internal static int currentLevel; // to keep track of progress or something? 

        public static string LevelName { get; internal set; } // ?? whats this for? 
        public static string tempLevelName { get; internal set; }

        public static string PersistentPath {
            get
            {
                string pathLoc = Application.persistentDataPath + "/levels";
                if (!Directory.Exists(pathLoc))
                {
                    Directory.CreateDirectory(pathLoc);
                }
                return pathLoc;
            }
        }

    }




    public static class Common
    {
        public static Color HexToColor(string hex)
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, 255);
        }

        internal static string ConvertEncode(Map map)
        {
            var mapData = JsonUtility.ToJson(map);
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(mapData));
        }

        public static byte[] Compress(byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                zipStream.Write(data, 0, data.Length);
                zipStream.Close();
                return compressedStream.ToArray();
            }
        }

        public static byte[] Decompress(byte[] gzip)
        {
            // Create a GZIP stream with decompression mode.
            // ... Then create a buffer and write into while reading from the GZIP stream.
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        internal static Level DecompressAndDecodeLevel(byte[] data)
        {
            return JsonUtility.FromJson<Level>(Encoding.ASCII.GetString(Common.Decompress(data)));
        }

        internal static Map DecodeMap(int version, string data)
        {
            if(version == 1)
            {
                return JsonUtility.FromJson<Map>(Encoding.ASCII.GetString((Convert.FromBase64String(data))));
            }
            else
            {
                Debug.Log("Invalid version");
                return null;
            }
        }

        internal static byte [] CompressAndEncodeLevel(Level level)
        {
            var jsoner = JsonUtility.ToJson(level);
            var uncompressed = Encoding.ASCII.GetBytes(jsoner);
            Debug.Log("Uncompressed " + uncompressed.Length);
            return Common.Compress(uncompressed);
        }
        internal static byte[] CompressAndEncodeHistory(List<PlayerMove.Direction> historyMoves)
        {
            var jsoner = JsonUtility.ToJson(historyMoves);
            var uncompressed = Encoding.ASCII.GetBytes(jsoner);
            return Compress(uncompressed);
        }

        public static byte[] ConvertRating(Rating r)
        {
            var rat = JsonUtility.ToJson(r);
            var vanilla = Encoding.ASCII.GetBytes(rat);
            return vanilla;
        }

        

    }

    public enum TileType
    {
        None = 0,
        Player = 1, Gold = 2, Goal = 3, Wall = 4, BlueBlock = 5, PurpleBlock = 6, OrangeWall = 7, OrangeButton = 8, TurnOnWall = 9, TurnOffWall = 10, Teleporter = 11, GreenButton = 12,
        TurnLeft = 13, TurnUp = 14, TurnRight = 15, TurnDown = 16,
        TurnLeftUp = 17, TurnRightUp = 18, TurnLeftDown = 19, TurnRightDown = 20
    };



    [Serializable]
    public class Map
    {
        //Required
        public int width; // map width
        public int height; // map height 
        public List<byte> Board = new List<byte>(); //Full Board
        public List<byte> IDsBoard = new List<byte>(); //Full Board ID  (For teleporters and buttons)

        //optional
        public string name; // ??  saves submitted name?
        public bool isSubmitted; //  has this been submitted already?
        public int onlineKey = 25; // used for voting
    }
}
