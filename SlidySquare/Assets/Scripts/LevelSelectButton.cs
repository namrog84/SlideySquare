using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class LevelSelectButton : MonoBehaviour {


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
        Application.LoadLevel("Level Select");
    }
}
