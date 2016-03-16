using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class DeciderOfAltButton : MonoBehaviour {


    public GameObject SkipLevelButton;
    public GameObject BackToLevelSelect;
    public GameObject BackToLevelEditor;

    // Use this for initialization
    void Start () {
        SkipLevelButton.SetActive(false);
        BackToLevelSelect.SetActive(false);
        BackToLevelEditor.SetActive(false);

        switch(GameCore.PlayingLevelState)
        {
            case GameCore.PlayingStates.Editor:
                BackToLevelEditor.SetActive(true);
                break;
            case GameCore.PlayingStates.Custom:
                BackToLevelSelect.SetActive(true);
                break;

            case GameCore.PlayingStates.Standard:
            default:
                SkipLevelButton.SetActive(true);
                break;
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
