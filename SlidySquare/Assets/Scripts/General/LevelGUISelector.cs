﻿using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine.UI;
using DG.Tweening;
using System.Text.RegularExpressions;

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
    public string key;

    public CustomLevelManager customLevelManagerObject;
    public static int MY_CUSTOM_LEVEL = -3;
    public static int DOWNLOADED_LEVEL = -4;


    private GameObject popupDeletePanel;
    private GameObject backgroundPanel;
    private GameObject DeleteButton;

    public GameObject PlayButton;
    public GameObject DownloadButton;



    public void CancelDeleteCustomCurrentLevel()
    {
        backgroundPanel = GameObject.Find("backgroundPanel");
        DeleteButton = GameObject.Find("TrueDeleteButton");
        popupDeletePanel = GameObject.Find("DeleteConfirmPopUpPanel");
        if (backgroundPanel != null)
        {
            backgroundPanel.GetComponent<Image>().enabled = false;
        }
        popupDeletePanel.transform.DOMove(popupDeletePanel.transform.position + new Vector3(0, -1500, 0), .3f).SetUpdate(true);
        DeleteButton.GetComponent<Button>().onClick.RemoveAllListeners();
        FileToDelete = "";
    }
    

    public void DeleteCustomCurrentLevel()
    {

        backgroundPanel = GameObject.Find("backgroundPanel");
        DeleteButton = GameObject.Find("TrueDeleteButton");
        popupDeletePanel = GameObject.Find("DeleteConfirmPopUpPanel");
        
            
        GameObject.Find("LevelNameToBeDeleted").GetComponent<Text>().text = filename;

        if (backgroundPanel != null)
        {
            backgroundPanel.GetComponent<Image>().enabled = true;
        }
        if(popupDeletePanel == null)
        {
            Debug.Log("No Delete Panel!");
        }
        if (DeleteButton == null)
        {
            Debug.Log("No DeleteButton!");
        }
        if (backgroundPanel == null)
        {
            Debug.Log("No backgroundPanel!");
        }
        popupDeletePanel.transform.DOMove(backgroundPanel.transform.position, .3f).SetUpdate(true);
        FileToDelete = filename;
        DeleteButton.GetComponent<Button>().onClick.AddListener(() => { ActuallyDeleteLevel(); });
    }
    public static string FileToDelete = "";

    private void ActuallyDeleteLevel()
    {
        if (FileToDelete == "")
        {
        }
        else
        {
            //FileManager.Delete("custom", FileToDelete);
            BoardBank.RemoveAll("custom", FileToDelete);
            DeleteButton.GetComponent<Button>().onClick.RemoveAllListeners();
            Destroy(gameObject);
        }
        CancelDeleteCustomCurrentLevel();
        
    }


    public void DeleteDownloadedCurrentLevel()
    {
        BoardBank.RemoveAll("downloads", filename);
        Destroy(gameObject);
    }

    public void DownloadLevel()
    {
        int downloadedLevels = PlayerPrefs.GetInt("DownloadedLevels", 0);
        downloadedLevels++;
        PlayerPrefs.SetInt("DownloadedLevels", downloadedLevels);


        if (customLevelManagerObject == null)
        {
            customLevelManagerObject = FindObjectOfType<CustomLevelManager>();
        }
        Debug.Log(key);
        customLevelManagerObject.DownloadLevel(this, key);
    }


    public void PlayCurrentLevel()
    {
        int playedcustom = PlayerPrefs.GetInt("PlayedCustomLevel", 0);
        playedcustom++;
        PlayerPrefs.SetInt("PlayedCustomLevel", playedcustom);

        //if (filename.Contains("downloaded"))
        //{
            //GameCore.isDownloaded = true;
        //}

        PlayerPrefs.SetInt("CurrentLevel", -3); // -3 is for custom level I guess? 
        PlayerPrefs.Save();
        GameCore.currentBoard = BoardBank.FindBoard(filename);
        
        FindObjectOfType<MySceneManager>().LoadToDynamicScene();
    }


    public void EditCurrentLevel()
    {
        
        GameCore.campaignLevelNumber = -3;
        GameCore.IsNewMap = false;
        //GameCore.tempLevelName = filename;
        Debug.Log(filename);

        GameCore.currentBoard = BoardBank.boards.Find(x => x.name == filename);
        FindObjectOfType<MySceneManager>().GoToLevelEditor();
    }

    public void CreateNewLevel()
    {
        GameCore.campaignLevelNumber = -3;
        GameCore.IsNewMap = true;
        //GameCore.currentBoard = BoardBank.boards.Find(x => x.name == filename);
        FindObjectOfType<MySceneManager>().GoToLevelEditor();
    }

    public void SaveToCampaign()
    {
        CampaignBank.LoadFromFile();
        var tempboard = BoardBank.boards.Find(x => x.name == filename);
        //Debug.Log(tempboard.name);
        tempboard.name = Regex.Replace(tempboard.name, @"[^\d]", "");
        //Debug.Log(tempboard.name);
        CampaignBank.boards.Add(tempboard);
        Debug.Log(CampaignBank.boards.Count);
        CampaignBank.SaveToFile();
        
    }

    internal void SetThumbnail(string v)
    {
        var tex = new Texture2D(10, 10);
        tex.filterMode = FilterMode.Point;
        tex.LoadImage(Convert.FromBase64String(v));
        SetThumbnail(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero));
    }
    public GameObject previewThumbnailGO;
    internal void SetThumbnail(Sprite s)
    {
        //if(previewThumbnailGO == null)
        //{
        //    return;
        //}
        if (s != null)
        {
            previewThumbnailGO.GetComponent<Image>().sprite = s;
        }
    }
}
