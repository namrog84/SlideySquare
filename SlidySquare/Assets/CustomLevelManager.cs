using UnityEngine;
using System.Collections;

public class CustomLevelManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LocalLevels()
    {
        Debug.Log("local");
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
            Debug.Log("WWW Ok!: " + www.data);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
    

    public void OnlineLevels()
    {
        Debug.Log("online");

        string url = "http://localhost:61156/api/levels/2";
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www));


    }
}
