using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.IO;
using MsgPack.Serialization;
using Assets.Scripts.Gameplay;
using System.Collections.Generic;

public class CampaignBank {

    public List<GameBoard> _boards = new List<GameBoard>();
    public List<int> _boardsCompleted = new List<int>();
    private static CampaignBank instance;

    public static List<GameBoard> boards
    {
        get
        {
            if(instance == null)
            {
                instance = new CampaignBank();
            }
            return instance._boards;
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void LoadFromFile()
    {
        var path = GameCore.PersistentPath + "/campaign.vault";
        if (!File.Exists(path)) // empty bank!
        {
            return;
        }
        // Creates serializer.
        var serializer = SerializationContext.Default.GetSerializer<CampaignBank>();
        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            // Unpack from stream.
            instance = serializer.Unpack(stream);
        }
    }

    private static void SaveToFile()
    {
        var path = GameCore.PersistentPath + "/campaign.vault";

        // Creates serializer.
        var serializer = SerializationContext.Default.GetSerializer<CampaignBank>();
        using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            // Pack obj to stream.
            serializer.Pack(stream, instance);
        }
    }

    internal static GameBoard FindBoard(string filename)
    {
        LoadFromFile();
        var result = instance._boards.Find(x => x.name == filename);
        if (result != null)
        {
            return result;
        }

        return null;
    }
}
