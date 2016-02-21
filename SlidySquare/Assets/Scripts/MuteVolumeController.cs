using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MuteVolumeController : MonoBehaviour {

    public Sprite mute;
    public Sprite notmuted;


    private Button MyButton = null; // assign in the editor
    


    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(() => { MyFunction(); });
    }

    private void MyFunction()
    {
        if(GetComponent<Image>().sprite == mute)
        {
            AudioListener.volume = 1;
            //AudioListener.pause = false;
            
            GetComponent<Image>().sprite = notmuted;
        }
        else
        {
            AudioListener.volume = 0;
           // AudioListener.pause = true;
            GetComponent<Image>().sprite = mute;
        }


    }

}


