using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class LevelGUISelector : MonoBehaviour {

    public GameObject editbutton;
	// Use this for initialization
	void Start () {
        GameCore.IsNewMap = false;
        transform.localScale = Vector3.one;

    }
	
    public void DisableEditButton()
    {
        editbutton.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
	
	}

    GameObject levelmanager;
    public string filename;

    public CustomLevelManager customLevelManagerObject;
    public static int MY_CUSTOM_LEVEL = -3;
    public static int DOWNLOADED_LEVEL = -4;


    public void DeleteCustomCurrentLevel()
    {
        FileManager.Delete("custom", filename);
        Destroy(gameObject);
    }
    public void DeleteDownloadedCurrentLevel()
    {
        FileManager.Delete("downloads", filename);
        Destroy(gameObject);
    }

    //public void DownloadLevel()
    //{
    //    if(customLevelManagerObject == null)
    //    {
    //        customLevelManagerObject = FindObjectOfType<CustomLevelManager>();
    //    }
    //    customLevelManagerObject.DownloadLevel(filename);
    //}


    public void PlayCurrentLevel()
    {
        if (filename.Contains("downloaded"))
        {
            GameCore.isDownloaded = true;
        }

        PlayerPrefs.SetInt("CurrentLevel", -3); // -3 is for custom level I guess? 
        

        PlayerPrefs.SetString("LevelName", filename);
        PlayerPrefs.Save();
        GameCore.currentBoard = BoardBank.boards.Find(x => x.name == filename);

        Debug.Log(GameCore.currentBoard.Board[0].type);
        Debug.Log(GameCore.currentBoard.Board[1].type);
        FindObjectOfType<MySceneManager>().LoadToDynamicScene();
    }


    public void EditCurrentLevel()
    {
        
        GameCore.currentLevel = -3;

        GameCore.IsNewMap = true;
        GameCore.tempLevelName = filename;
        Debug.Log(filename);
        
        PlayerPrefs.SetInt("CurrentLevel", -3); // -3 is for custom level I guess? 
        PlayerPrefs.SetString("LevelName", filename);
        PlayerPrefs.Save();
        FindObjectOfType<MySceneManager>().GoToLevelEditor();
    }






}
