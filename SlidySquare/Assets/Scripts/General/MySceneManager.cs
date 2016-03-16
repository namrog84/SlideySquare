using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.SceneManagement;
using System;
using I2.Loc;

[RequireComponent(typeof(SceneFadeInOut))]
public class MySceneManager : MonoBehaviour {

    public void Start()
    {
        LocalizationManager.CurrentLanguage = PlayerPrefs.GetString("Lang", "English");

        sceneFader = GetComponent<SceneFadeInOut>();
        if(sceneFader == null)
        {
            Debug.Log("why am i null?");

        }
        sceneFader.FinishedFade += OnFinishedLoadScene;
    }


    private string nextSceneName;
    private bool isLevelExiting = false;
    private SceneFadeInOut sceneFader;

    public void RateUsClick()
    {
#if UNITY_ANDROID
            Application.OpenURL("market://details?id=YOUR_ID");
#elif UNITY_IPHONE
            Application.OpenURL("itms-apps://itunes.apple.com/app/idYOUR_ID");
#endif
    }

    public void PlayAd()
    {
        AdManager.PlayAd();
    }

    internal void LoadCampaignLevel(int levelNumber)
    {
        if (levelNumber != 0)
        {
            GameCore.PlayingLevelState = GameCore.PlayingStates.Campaign;
            GameCore.campaignLevelNumber = levelNumber;
            LoadToDynamicScene();
        }
    }

    public void GoToIntroScene()
    {
        LoadLevel("introScene");
    }
    public void GoToMainLevelSelect()
    {
        LoadLevel("Level Select");
    }
    public void GoToLevelEditor()
    {
        //creating a new level? Lets save it here
        var levelCounter = PlayerPrefs.GetInt("CustomLevels", 0);
        levelCounter++;
        PlayerPrefs.SetInt("CustomLevels", levelCounter);
        PlayerPrefs.Save();
        //GameCore.tempLevelName = "MyLevel " + levelCounter;

        LoadLevel("LevelEditor");
    }
    public void GoToCustomLevelSelect()
    {
        LoadLevel("CustomLevelSelect");
    }

    public void GoToAboutScene()
    {
        LoadLevel("About");
    }

    public void LoadToDynamicScene()
    {
        LoadLevel("DynamicLevel");
    }
    public void LoadToVoteScene()
    {
        LoadLevel("VoteScene");
    }


    public void ReloadCurrentLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().name);
    }

    public void SkipToNextCampaignLevel()
    {
        AdManager.Skipped = true;
        PlayerPrefs.SetInt("CurrentLevel", 1 + PlayerPrefs.GetInt("CurrentLevel"));
        PlayerPrefs.Save();
        LoadLevel("DynamicLevel");
    }


    private void LoadLevel(string name)
    {
        PlayerPrefs.SetFloat("TotalTime", AdManager.TimePlayed);
        PlayerPrefs.Save();

        nextSceneName = name;
        Time.timeScale = 1.0f; //just in case something has paused, lets unpause!
        if (!isLevelExiting)
        {
            isLevelExiting = true;
            sceneFader.StartSceneFadeOut();
        }
    }

    public void OnFinishedLoadScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(nextSceneName);

    }


}
