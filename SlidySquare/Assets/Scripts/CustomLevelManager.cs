using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using System;

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
        WWWForm form = new WWWForm();
        //form.AddField("levelData", data);
        var dataToSend = Convert.ToBase64String(ObjectToByteArray(data));

        form.AddField("levelData", dataToSend);
        Debug.Log(dataToSend);
        WWW www = new WWW(url, form);
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
            var derpdata = www.text.Trim(new char[] {'[', ']'});
            Debug.Log(derpdata);

            Level level = JsonUtility.FromJson<Level>(derpdata);
            //Debug.Log(level.Data);
            Debug.Log(level.PublicName.ToString());
            var newdata = level.Data.Substring(10);
            

            newdata = newdata.Replace("%3d", "=");
            newdata = newdata.Replace("%2f", "/"); 
            Debug.Log(newdata);
            var leveldata = Convert.FromBase64String(newdata);
            var map = (Map)ByteArrayToObject(leveldata);
            FileManager.SaveObjectToFile(level.PublicName, map);



            //Debug.Log(level.Date.ToString());
            //Debug.Log(level.key);




            //Level level2 = (Level)JsonUtility.FromJson(www.text, typeof(Level));
            //Debug.Log(level.Data);
           // Debug.Log(level2.PublicName);

            //Map map = (Map)ByteArrayToObject(Encoding.ASCII.GetBytes(level.Data));
            //Debug.Log(map.name);

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
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }
    object ByteArrayToObject(byte [] obj)
    {
        if (obj == null)
            return null;
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream(obj))
        {
            return bf.Deserialize(ms);
            //return ms.;
        }
    }


}
