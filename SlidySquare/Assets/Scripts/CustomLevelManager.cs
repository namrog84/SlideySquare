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

    public GameObject MyLevelGUIItem;
    public GameObject OnlineGUIItem;

    public GameObject ContentList;
    public List<GameObject> TheList = new List<GameObject>();
    private string coreURL = "http://ssapi-v2-2016.azurewebsites.net";
    public class LevelList
    {
        public List<string> levelNames = new List<string>();
        public List<int> levelIds = new List<int>();
    }


    // Use this for initialization
    void Start () {
        GameCore.isDownloaded = false;
        GameCore.PreppingForSubmit = false;

        LocalLevels();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LocalLevels()
    {
        TurnOnLocalChoices(true);
        foreach (var item in TheList)
        {
            Destroy(item);
        }
        TheList.Clear();

        var filenames = FileManager.GetSavedLevelNames();
        foreach (var file in filenames)
        {
            //Debug.Log(file);
            var item = Instantiate(MyLevelGUIItem);
            item.transform.SetParent(ContentList.transform);
            item.GetComponentInChildren<Text>().text = file.TrimEnd('.');
            item.GetComponent<LevelGUISelector>().filename = file;
            TheList.Add(item);
        }
    }
    public void DownloadedLevels()
    {
        TurnOnLocalChoices(true);
        foreach (var item in TheList)
        {
            Destroy(item);
        }
        TheList.Clear();

        var filenames = FileManager.GetDownloadedLevelNames();
        foreach (var file in filenames)
        {
            //Debug.Log(file);
            var item = Instantiate(MyLevelGUIItem);
            item.transform.SetParent(ContentList.transform);
            item.GetComponentInChildren<Text>().text = file.TrimEnd('.');
            item.GetComponent<LevelGUISelector>().DisableEditButton();
            item.GetComponent<LevelGUISelector>().filename = "downloaded/"+file;
            TheList.Add(item);
        }
    }



    public void MostDownloadedLevels()
    {
        TurnOnLocalChoices(false);
        foreach (var item in TheList)
        {
            Destroy(item);
        }
        TheList.Clear();

        string url = coreURL + "/api/mostdownloads/1";
        WWW www = new WWW(url);
        StartCoroutine(WaitForOnlineLevel(www));
    }

    public void NewestLevels()
    {
        TurnOnLocalChoices(false);
        foreach (var item in TheList)
        {
            Destroy(item);
        }
        TheList.Clear();

        string url = coreURL + "/api/newest/1";
        WWW www = new WWW(url);
        StartCoroutine(WaitForOnlineLevel(www));
    }
    public void HighestRatedLevels()
    {
        TurnOnLocalChoices(false);
        foreach (var item in TheList)
        {
            Destroy(item);
        }
        TheList.Clear();

        string url = coreURL + "/api/highestrating/1";
        WWW www = new WWW(url);
        StartCoroutine(WaitForOnlineLevel(www));
    }

    public void OnlineLevels()
    {
        TurnOnLocalChoices(false);
        foreach (var item in TheList)
        {
            Destroy(item);
        }
        TheList.Clear();

        string url = coreURL + "/api/levels/1";
        WWW www = new WWW(url);
        StartCoroutine(WaitForOnlineLevel(www));
    }
    IEnumerator WaitForOnlineLevel(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log(www.text);
            var MyList = (LevelList)JsonUtility.FromJson(www.text, typeof(LevelList));
            for (int i = 0; i < MyList.levelNames.Count; i++)
            {
                //Debug.Log(file);
                var item = Instantiate(OnlineGUIItem);
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
        string url = coreURL + "/api/uploadlevel";

        Level level = new Level(map);
        level.Solution = PlayerPrefs.GetString("HistoryList", "?");


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
        string url = coreURL + "/api/download/" + key;
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
            map.onlineKey = level.key;
            FileManager.SaveDownloadedToFile(level.PublicName, map);
            Debug.Log("done saving");
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }





    public GameObject offlinePanel;
    public GameObject onlinePanel;
    public void TurnOnLocalChoices(bool val)
    {
        if (onlinePanel == null || offlinePanel == null)
        {
            return;
        }
        if (val)
        {
            
            onlinePanel.SetActive(false);
            offlinePanel.SetActive(true);
        }
        else
        {
            onlinePanel.SetActive(true);
            offlinePanel.SetActive(false);
        }

    }



    










}
