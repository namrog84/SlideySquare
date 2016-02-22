using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class CustomLevelManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LocalLevels()
    {
        string url = "http://localhost:61156/api/levels/1";
        WWW www = new WWW(url); 
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
    

    public void OnlineLevels()
    {
        string url = "http://localhost:61156/api/levels/2";
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
    }


    public void UploadLevel(object data)
    {
        string url = "http://localhost:61156/api/uploadlevel";
        WWWForm form = new WWWForm();
        //form.AddField("levelData", data);
        form.AddBinaryData("levelData", ObjectToByteArray(data));
        WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequest(www));
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


}
