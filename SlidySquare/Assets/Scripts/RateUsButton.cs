using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RateUsButton : MonoBehaviour {
  
	private Button MyButton = null; // assign in the editor

    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(() => { MyFunction(); });
       // myDelegate += merpy;
    }

    void merp()
    {
       
    }
    private void MyFunction()
    {
        #if UNITY_ANDROID
            Application.OpenURL("market://details?id=YOUR_ID");
        #elif UNITY_IPHONE
            Application.OpenURL("itms-apps://itunes.apple.com/app/idYOUR_ID");
        #endif
        /*var moveme = GameObject.Find("MainMenuUI").transform.position;
        moveme += new Vector3(0, 1000, 0);
        GameObject.Find("MainMenuUI").transform.position = moveme;
        */

    }

}
