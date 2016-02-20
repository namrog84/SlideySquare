using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HomeButtonController : MonoBehaviour {

    //[SerializeField]
    private Button MyButton = null; // assign in the editor
    public string sceneToLoad;
    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(() => { MyFunction(); });
    }

    private void MyFunction()
    {
        Time.timeScale = 1;
        StartCoroutine(LevelSelect());

    }

    private IEnumerator LevelSelect()
    {
        var fader = GameObject.Find("SceneFader");
        fader.GetComponent<SceneFadeInOut>().fadeDir *= -1;
        fader.GetComponent<SceneFadeInOut>().startTime = 0;
        yield return new WaitForEndOfFrame();
        fader.GetComponent<SceneFadeInOut>().FinishedFade += finished;
    }

    void finished()
    {
        Application.LoadLevel(sceneToLoad);

        if (!Application.isLoadingLevel)
        {
            Application.LoadLevel(0);
        }
    }
}
