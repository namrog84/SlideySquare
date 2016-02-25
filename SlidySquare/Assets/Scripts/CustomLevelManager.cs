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
    public class LevelList
    {
        public List<string> levelNames = new List<string>();
        public List<int> levelIds = new List<int>();
    }


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
    IEnumerator WaitForOnlineLevel(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            var MyList = (LevelList)JsonUtility.FromJson(www.text, typeof(LevelList));
            for (int i = 0; i < MyList.levelNames.Count; i++)
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


    public void UploadLevel(Map map)
    {
        string url = "http://localhost:61156/api/uploadlevel";

        Level level = new Level(map);
        var data = Common.CompressAndEncodeLevel(level);
        Debug.Log("Compressed " + data.Length);
        WWW www = new WWW(url, data);
        
        StartCoroutine(WaitForRequest(www));
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
            Level level = Common.DecompressAndDecodeLevel(www.bytes);
            Map map = Common.DecodeMap(level.Version, level.Data);

            FileManager.SaveObjectToFile(level.PublicName, map);
            Debug.Log("done saving");
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }

    

}
