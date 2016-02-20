using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkipLevelController : MonoBehaviour {


    //[SerializeField]
    private Button MyButton = null; // assign in the editor

    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(() => { MyFunction(); });
    }

    private void MyFunction()
    {
#pragma warning disable 0618
        if (!Application.isLoadingLevel)
        {
            AdManager.Skipped = true;
            PlayerPrefs.SetInt("CurrentLevel", 1 + PlayerPrefs.GetInt("CurrentLevel"));
            PlayerPrefs.Save();
            Application.LoadLevel(4);
        }
#pragma warning restore 0618
    }
}
