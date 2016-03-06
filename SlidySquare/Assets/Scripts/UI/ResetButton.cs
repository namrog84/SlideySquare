using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class ResetButton : MonoBehaviour {

    private bool resetting = false;
    private Button MyButton;

	void Start()
	{
		MyButton = GetComponent<Button>();
		MyButton.onClick.AddListener(() => { OnResetButtonClick(); });
	}

	void Update () {
	}

	private void OnResetButtonClick()
	{
        if (!resetting)
        {
            resetting = true;

            //TODO Improve with singleton or better design
            FindObjectOfType<MySceneManager>().ReloadCurrentLevel();
        }
	}

   


}




