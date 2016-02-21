using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{

	private Button MyButton = null; // assign in the editor

	void Start()
	{
		MyButton = GetComponent<Button>();
		MyButton.onClick.AddListener(() => { MyFunction(); });
	}

	private void MyFunction()
	{
        StartCoroutine(LevelSelect());    
	}

    private IEnumerator LevelSelect()
    {
        var fader = GameObject.Find("SceneFader");
        fader.GetComponent<SceneFadeInOut>().fadeDir *= -1;
        fader.GetComponent<SceneFadeInOut>().startTime = 0;
        yield return new WaitForEndOfFrame();

        fader.GetComponent<SceneFadeInOut>().FinishedFade += Finished;
    }
    private void Finished()
    {
#pragma warning disable 0618
        //SceneManager.LoadScene("Level Select"); // ??
        Application.LoadLevel("Level Select");
#pragma warning restore 0618

    }

}


