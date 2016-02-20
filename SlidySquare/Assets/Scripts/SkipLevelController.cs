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

        if (!Application.isLoadingLevel)
        {
            AdManager.Skipped = true;
            PlayerPrefs.SetInt("CurrentLevel", 1 + PlayerPrefs.GetInt("CurrentLevel"));
            PlayerPrefs.Save();
            Application.LoadLevel(4);
        }


        //Application.LoadLevel("level " + (1 + int.Parse(Application.loadedLevelName.Split(new char[] { ' ' })[1])));
        //if (!Application.isLoadingLevel)
        {
          //  Application.LoadLevel(0);
        }


    }
}
