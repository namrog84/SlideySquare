using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class MuteVolumeController : MonoBehaviour {

    private Sprite mute;
    private Sprite notmuted;

    private Image image;
    private Button myButton; // assign in the editor
    

    void Start()
    {
        image = GetComponent<Image>();
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(() => { OnMuteButtonClick(); });
        if(AudioListener.volume == 1)
        {
            image.sprite = notmuted;
        }
        else
        {
            image.sprite = mute;
        }
    }

    private void OnMuteButtonClick()
    {
        if(image.sprite == mute)
        {
            AudioListener.volume = 1;
            image.sprite = notmuted;
        }
        else
        {
            AudioListener.volume = 0;
            image.sprite = mute;
        }

    }

}


