using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using System;
using Unity.IO.Compression;

public class CustomLevelManager : MonoBehaviour {

    public GameObject LevelItem; //this is where the level list lives

    public GameObject ContentList;
    public List<GameObject> TheList = new List<GameObject>();

    // Use this for initialization
    void Start () {
        LocalLevels();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LocalLevels()
    {
        foreach (var item in TheList)
        {
            Destroy(item);
        }
        TheList.Clear();

        //string url = "http://localhost:61156/api/levels/1";
        //WWW www = new WWW(url); 
        //StartCoroutine(WaitForRequest(www));

        var filenames = FileManager.GetSavedLevelNames();
        foreach (var file in filenames)
        {
            //Debug.Log(file);
            var item = Instantiate(LevelItem);
            item.transform.SetParent(ContentList.transform);
            item.GetComponentInChildren<Text>().text = file.TrimEnd('.');
            item.GetComponent<LevelGUISelector>().filename = file;
            TheList.Add(item);
        }

    }


    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        
        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
    IEnumerator WaitForOnlineLevel(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            var MyList = (LevelList)JsonUtility.FromJson(www.text, typeof(LevelList));
            for(int i= 0; i < MyList.levelNames.Count; i++)
            {
                //Debug.Log(file);
                var item = Instantiate(LevelItem);
                item.transform.SetParent(ContentList.transform);
                item.GetComponentInChildren<Text>().text = MyList.levelNames[i].TrimEnd('.');
                
                item.GetComponent<LevelGUISelector>().filename = MyList.levelIds[i].ToString();
                TheList.Add(item);
            }
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }


    public void OnlineLevels()
    {

        foreach (var item in TheList)
        {
            Destroy(item);
        }
        TheList.Clear();

        string url = "http://localhost:61156/api/levels/2";
        WWW www = new WWW(url);
        StartCoroutine(WaitForOnlineLevel(www));

        

    }
    public class LevelList
    {
        public List<string> levelNames = new List<string>();
        public List<int> levelIds = new List<int>();
    }

    public void DownloadLevel()
    {

    }

    public void UploadLevel(object data)
    {
        string url = "http://localhost:61156/api/uploadlevel";
        Level level = new Level();
        
        var mapData = JsonUtility.ToJson((Map)data);
        level.Data = Convert.ToBase64String(Encoding.ASCII.GetBytes(mapData)); 
        //level.MachineId already assigned
        level.Width = ((Map)data).width;
        level.Height= ((Map)data).height;
        level.Version = 1;
        Debug.Log("Attempting to Upload");
        Debug.Log(level.Width);
        Debug.Log(mapData);
        var jsoner = JsonUtility.ToJson(level);
        Debug.Log(jsoner);
        var uncompressed = Encoding.ASCII.GetBytes(jsoner);
        Debug.Log("Uncompressed " + uncompressed.Length);
        var ddata = Compress(uncompressed);
        Debug.Log("Compressed " + ddata.Length);
        WWW www = new WWW(url, ddata);
        
        StartCoroutine(WaitForRequest(www));
    }

    public void DownloadLevel(string key)
    {
        Debug.Log("Downloading");
        string url = "http://localhost:61156/api/download/"+key;
        WWW www = new WWW(url);
        StartCoroutine(WaitForDownload(www));
        

    }
    IEnumerator WaitForDownload(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log(www.text);
            Debug.Log(www.bytes.Length);

            Level level = JsonUtility.FromJson<Level>(Encoding.ASCII.GetString(Decompress(www.bytes))); // Encoding.ASCII.GetBytes(JsonUtility.ToJson(mapData)))
            Level2 level2 = JsonUtility.FromJson<Level2>(Encoding.ASCII.GetString(Decompress(www.bytes))); // Encoding.ASCII.GetBytes(JsonUtility.ToJson(mapData)))
            Debug.Log(Encoding.ASCII.GetString(Decompress(www.bytes)));
            Debug.Log(level.PublicName);
            
            //Level level = (Level)ByteArrayToObject(Decompress(www.bytes));
            Debug.Log("Something " + level.PublicName.ToString());
            Debug.Log(level.Data);
            Debug.Log(level.Data.Length);
            Debug.Log(level2.Data);
            Debug.Log(level2.Data.Length);
            Map map = JsonUtility.FromJson<Map>(Encoding.ASCII.GetString((Convert.FromBase64String(level2.Data))));//(Map)ByteArrayToObject(level.Data);
            Debug.Log(map.Board);
            Debug.Log(map.height);
            Debug.Log(map.IDsBoard);
            Debug.Log("done convert now saving");
            FileManager.SaveObjectToFile(level.PublicName, map);

            Debug.Log("done saving");
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
            return null;
        var data = JsonUtility.ToJson(obj);
        return System.Text.Encoding.ASCII.GetBytes(data);
        //BinaryFormatter bf = new BinaryFormatter();
        //using (MemoryStream ms = new MemoryStream())
        //{
        //    bf.Serialize(ms, obj);
        //    return ms.ToArray();
        //}
    }

    // DO NOT USE BINARYFORMATTER?

    //object ByteArrayToObject(byte [] obj)
    //{
    //    if (obj == null)
    //        return null;
    //    var data = JsonUtility.FromJson(obj);
    //    return System.Text.Encoding.ASCII.GetBytes(data);
    //    BinaryFormatter bf = new BinaryFormatter();
    //    using (MemoryStream ms = new MemoryStream(obj))
    //    {
    //        return bf.Deserialize(ms);
    //    }
    //}
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

    //public static byte[] Decompress(byte[] data)
    //{
    //    using (var compressedStream = new MemoryStream(data))
    //    using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
    //    using (var resultStream = new MemoryStream()
    //    {
            
    //        //zipStream.CopyTo(resultStream);
    //        return resultStream.ToArray(); // resultStream.ToArray();
    //    }
    //}

}
