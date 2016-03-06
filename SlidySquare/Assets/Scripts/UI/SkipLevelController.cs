using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkipLevelController : MonoBehaviour {

    private Button MyButton; // assign in the editor

    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(() => { OnSkipLevelClick(); });
    }

    private void OnSkipLevelClick()
    {
        FindObjectOfType<MySceneManager>().SkipToNextCampaignLevel();
    }


}



