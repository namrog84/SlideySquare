using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class LevelGUISelector : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    GameObject levelmanager;
    public string filename;

    public void UploadThisLevel()
    {
        var map = (Map)FileManager.LoadFromFile(filename);
        //Debug.Log(map);

        //var data = JsonUtility.ToJson(map);
        //var data = Convert.ToBase64String(ObjectToByteArray(map));
        //Debug.Log(data);
        
        GetComponent<CustomLevelManager>().UploadLevel(map);
    }

    public void DeleteCurrentLevel()
    {
        FileManager.Delete(filename);
        Destroy(gameObject);
    }



    public void PlayCurrentLevel()
    {
        PlayerPrefs.SetInt("CurrentLevel", -3); // -3 is for custom level I guess? 
        PlayerPrefs.SetString("LevelName", filename);
        PlayerPrefs.Save();
        StartCoroutine(LoadOut());
    }

    private IEnumerator LoadOut()
    {
        var fader = GameObject.Find("SceneFader");
        fader.GetComponent<SceneFadeInOut>().fadeDir *= -1;
        fader.GetComponent<SceneFadeInOut>().startTime = 0;
        yield return new WaitForEndOfFrame();
        fader.GetComponent<SceneFadeInOut>().FinishedFade += LoadLevelOnFinished;
    }
    void LoadLevelOnFinished()
    {
        Time.timeScale = 1;
#pragma warning disable 0618
        Application.LoadLevel(4);
#pragma warning restore 0618
        //Application.LoadLevel(Application.loadedLevel);
    }


}
