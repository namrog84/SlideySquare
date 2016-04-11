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
        LoadLevel("MainCampaignLevelSelect");
    }
    public void GoToLevelEditor()
    {
        LoadLevel("LevelEditor");
    }

    public void GoBackToOpenedLevelEditor()
    {
        //GameCore.campaignLevelNumber = -3;
        GameCore.IsNewMap = false;
        //GameCore.currentBoard = BoardBank.boards.Find(x => x.name == filename);
        GoToLevelEditor();

    }


    public void GoToCustomLevelSelect()
    {
        LoadLevel("CustomLevelSelect");
    }

    public void GoToAboutScene()
    {
        Debug.Log("Going Aboot");
        LoadLevel("About");
    }

    public void LoadToDynamicScene()
    {
        if (GameCore.PlayingLevelState == GameCore.PlayingStates.Campaign)
        {
            
            var index = GameCore.campaignLevelNumber;
            Debug.Log(index + " " + CampaignBank.boards.Count);
            if (index >= CampaignBank.boards.Count || index < 0)
            {
                GoToAboutScene();
            }
        }
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
        GameCore.campaignLevelNumber++;

        if (GameCore.campaignLevelNumber > PlayerPrefs.GetInt("BaseGameLevelsCompleted", -1))
        {
            PlayerPrefs.SetInt("BaseGameLevelsCompleted", GameCore.campaignLevelNumber);
            PlayerPrefs.Save();
            Debug.Log("Unlocked new level " + GameCore.campaignLevelNumber);
        }

        //PlayerPrefs.SetInt("CurrentLevel", 1 + PlayerPrefs.GetInt("CurrentLevel"));
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
            if (sceneFader != null)
            {
                sceneFader.StartSceneFadeOut();

            }
            else
            {
                OnFinishedLoadScene();
            }
        }
    }

    public void OnFinishedLoadScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(nextSceneName);

    }


}
