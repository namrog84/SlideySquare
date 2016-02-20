using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour {


	// Update is called once per frame
	void Update () {
	
	}

	//[SerializeField]
	private Button MyButton = null; // assign in the editor

	void Start()
	{
		MyButton = GetComponent<Button>();
		MyButton.onClick.AddListener(() => { MyFunction(); });
	}

    bool resetting = false;
	private void MyFunction()
	{
        if (!resetting)
        {
            resetting = true;
            StartCoroutine(LevelSelect());
        }
        //Time.timeScale = 1;
        //Application.LoadLevel(Application.loadedLevel);

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
        Time.timeScale = 1;
        resetting = false;
#pragma warning disable 0618
        Application.LoadLevel(Application.loadedLevel);
#pragma warning restore 0618


    }



}
