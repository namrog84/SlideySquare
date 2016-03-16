using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.SceneManagement;
using System;

public class VoteSubmitSceneController : MonoBehaviour {
    private string filename;
    public CustomLevelManager customLevelManagerObject;

    public GameObject VotePanel;
    public GameObject UploadPanel;

    // Use this for initialization
    void Start()
    {
        //disable all
        VotePanel.SetActive(false);
        UploadPanel.SetActive(false);

        if (GameCore.PlayingLevelState == GameCore.PlayingStates.Editor)
        {
            UploadPanel.SetActive(true);
        }
        else if (GameCore.PlayingLevelState == GameCore.PlayingStates.CustomDownloaded)
        {
            VotePanel.SetActive(true);
        }
        else
        {
            //This is neither downloaded nor editor, so just go back to screen
            SceneManager.LoadScene("MakeShareDownloadSelector"); //go there now
        }
    }


    // Update is called once per frame
    void Update () {
	}

    //Map map;
    public void UploadThisLevel()
    {
        string url = CustomLevelManager.coreURL + "/api/uploadlevel";
        Debug.Log("uploading!!");
        Level level = new Level(GameCore.currentBoard);

        var data = Common.Serialize(level);
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
        mySceneManagerGO.GetComponent<MySceneManager>().GoToCustomLevelSelect();
    }
    

    //public void UploadLevel()
    //{


    //    string url = "http://ssapi-v2-2016.azurewebsites.net/api/uploadlevel";

    //    Level level = new Level(map);
    //    level.Solution = PlayerPrefs.GetString("HistoryList", "?");



    //    var data = Common.CompressAndEncodeLevel(level);
    //    Debug.Log("Compressed " + data.Length);
    //    WWW www = new WWW(url, data);

    //    StartCoroutine(WaitForRequest(www));
    //}

    //IEnumerator WaitForRequest(WWW www)
    //{
    //    yield return www;


    //    // check for errors
    //    if (www.error == null)
    //    {
    //        //only save if there were no errors?
    //        map.isSubmitted = true;
    //        var newname = www.text.Trim('"');
    //        Debug.Log(filename + "   " + map.name);
    //        var oldname = map.name;
    //        if(oldname == "")
    //        {
    //            oldname = filename;
    //        }
    //        map.name = newname; // this is now our filename?
    //        Debug.Log("saving " + newname);
    //        FileManager.SaveObjectToFile(newname, map);
    //        Debug.Log("deleting " + oldname);
    //        FileManager.Delete(oldname);

    //        Debug.Log("Done Uploading!: " + www.text);
    //        LoadLevel("MakeShareDownloadSelector");
    //    }
    //    else
    //    {
    //        Debug.Log("WWW Error: " + www.error);
    //    }
    //}


    public void VoteThisUp()
    {
        VoteThisLevel(1);
    }
    public void VoteThisMeh()
    {
        VoteThisLevel(0.2f);
    }
    public void VoteThisDown()
    {
        VoteThisLevel(-1);
    }

    public void VoteThisLevel(float value)
    {
        string url = "http://ssapi-v2-2016.azurewebsites.net/api/vote/";

        Rating r = new Rating();
        r.LevelKey = GameCore.currentBoard.name;
        r.Vote = value;
        Debug.Log("upvoting!");
        WWW www = new WWW(url, Common.Serialize(r));
        StartCoroutine(WaitForVote(www));
    }

//    public MySceneManager scenemanager;
    IEnumerator WaitForVote(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log(www.text);
            Debug.Log("done voting");
            mySceneManagerGO.GetComponent<MySceneManager>().GoToCustomLevelSelect();
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
    public GameObject mySceneManagerGO;

}



    //    private string nextSceneName;
    //    private bool isLevelExiting = false;


    //    private void LoadLevel(string name)
    //    {
    //        PlayerPrefs.SetFloat("TotalTime", AdManager.TimePlayed);
    //        PlayerPrefs.Save();

    //        nextSceneName = name;
    //        Time.timeScale = 1.0f; //just incase something has paused, lets unpause!
    //        if (!isLevelExiting)
    //        {
    //            isLevelExiting = true;
    //            StartCoroutine(LevelSelect());
    //        }
    //    }

    //    private IEnumerator LevelSelect()
    //    {
    //        var fader = GameObject.Find("SceneFader");
    //        fader.GetComponent<SceneFadeInOut>().fadeDir *= -1;
    //        fader.GetComponent<SceneFadeInOut>().startTime = 0;
    //        yield return new WaitForEndOfFrame();

    //        fader.GetComponent<SceneFadeInOut>().FinishedFade += Finished;
    //    }

    //    private void Finished()
    //    {
    //#pragma warning disable 0618
    //        Application.LoadLevel(nextSceneName);
    //#pragma warning restore 0618

    //    }



