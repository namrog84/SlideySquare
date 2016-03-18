using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Gameplay;
using MsgPack.Serialization;
using System.IO;
using Assets.Scripts;
using System;

[Serializable]
public class BoardBank
{
    public List<GameBoard> _boards = new List<GameBoard>();

    public List<GameBoard> _Downloaded = new List<GameBoard>();

    public static BoardBank instance = new BoardBank();


    public static List<GameBoard> boards
    {
        get
        {
            return instance._boards;
        }
    }
    public static List<GameBoard> downloadedBoards
    {
        get
        {
            return instance._Downloaded;
        }
    }

    internal static void RemoveAll(string folder, string filename)
    {
        //if (folder == "downloads")
        {
            int count  = instance._Downloaded.RemoveAll(x => x.name == filename);
            Debug.Log("Success downloaded remove " + count);
        }
        //if (folder == "custom")
        {
            var count = instance._boards.RemoveAll(x => x.name == filename);
            Debug.Log("Success custom remove " + count);
        }
        SaveToFile();
    }

    public static void AddStandard(GameBoard b)
    {
        var index = instance._boards.FindIndex(x => x.name == b.name);
        if(index == -1) // not found!
        {
            instance._boards.Add(b);
        }
        else
        {
            instance._boards[index] = b;
        }
        SaveToFile();
    }

    public static void AddDownloaded(GameBoard b)
    {
        instance._Downloaded.Add(b);
        SaveToFile();
    }


    private static void SaveToFile()
    {
        var path = GameCore.PersistentPath + "/board.vault";

        // Creates serializer.
        var serializer = SerializationContext.Default.GetSerializer<BoardBank>();
        using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            // Pack obj to stream.
            serializer.Pack(stream, instance);
        }
    }

    public static void LoadFromFile()
    {
        var path = GameCore.PersistentPath + "/board.vault";
        if (!File.Exists(path)) // empty bank!
        {
            return;
        }
        // Creates serializer.
        var serializer = SerializationContext.Default.GetSerializer<BoardBank>();
        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            // Unpack from stream.
            instance = serializer.Unpack(stream);
        }
    }

    internal static GameBoard FindBoard(string filename)
    {
        LoadFromFile();
        var result = instance._boards.Find(x => x.name == filename);
        if(result != null)
        {
            return result;
        }
        result = instance._Downloaded.Find(x => x.name == filename);
        if (result != null)
        {
            return result;
        }

        return null;
    }
}
