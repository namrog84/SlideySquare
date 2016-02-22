using UnityEngine;
using System.Collections;
using Assets.Scripts;

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



    public void UploadLevel(string data)
    {
        string url = "http://localhost:61156/api/create";
        WWWForm form = new WWWForm();
        form.AddField("levelData", data);
        WWW www = new WWW(url, form);
        StartCoroutine(WaitForRequest(www));
    }
}
