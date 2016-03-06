using UnityEngine;
using System.Collections;

public class IntroGUI : MonoBehaviour {
    //now exists in MySceneManager?


//    private string nextSceneName;
//    private bool isLevelExiting = false;

//    public void RateUsClick()
//    {
//#if UNITY_ANDROID
//            Application.OpenURL("market://details?id=YOUR_ID");
//#elif UNITY_IPHONE
//            Application.OpenURL("itms-apps://itunes.apple.com/app/idYOUR_ID");
//#endif
//    }


//    public void PlayAd()
//    {
//        AdManager.PlayAd();
//    }

//    public void GoToMainLevelSelect()
//    {
//        LoadLevel("Level Select");
//    }
//    public void GoToCustomLevelSelect()
//    {
//        LoadLevel("MakeShareDownloadSelector");
//    }

//    public void GoToAboutScene()
//    {
//        LoadLevel("About");
//    }


//    private void LoadLevel(string name)
//    {
//        nextSceneName = name;

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



}
