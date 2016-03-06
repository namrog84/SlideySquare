using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class ClearMenuButton : MonoBehaviour {

    public RectTransform popupPanel;
    public RectTransform backgroundPanel;

    void Start()
    {
    }

    public void ClearMenu()
    {
        var background = backgroundPanel.GetComponent<Image>();
        if(background != null)
        {
            background.enabled = false;
        }
        Time.timeScale = 1;
        popupPanel.transform.DOMove(popupPanel.position + new Vector3(0, -1500, 0), .3f).SetUpdate(true);
    
    }

}
