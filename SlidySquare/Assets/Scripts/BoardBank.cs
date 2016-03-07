//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using Assets.Scripts.Gameplay;
//using MsgPack.Serialization;
//using System.IO;

//public class BoardBank {


//    public static List<GameBoard> boards = new List<GameBoard>();


//    public static void LoadFromFile()
//    {
//        var path = Application.persistentDataPath + "/board.vault";

//        // Creates serializer.
//        var serializer = SerializationContext.Default.GetSerializer<List<GameBoard>>();
//        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
//        {
//            // Unpack from stream.
//            boards = serializer.Unpack(stream);
//        }
//    }

//    public static void SaveToFile()
//    {
//        var path = Application.persistentDataPath + "/board.vault";

//        // Creates serializer.
//        var serializer = SerializationContext.Default.GetSerializer<List<GameBoard>>();
//        using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
//        {
//            // Pack obj to stream.
//            serializer.Pack(stream, boards);
//        }

//    }


//}
