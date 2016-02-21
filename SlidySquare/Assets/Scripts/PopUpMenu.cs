using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class PopUpMenu : MonoBehaviour {

    public RectTransform popupPanel;
    public RectTransform backgroundPanel;

    private Button MyButton = null; // assign in the editor

    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(() => { MyOnClickFunction(); });
    }

    void Update () {
    }
    
    private void MyOnClickFunction()
    {
        backgroundPanel.GetComponent<Image>().enabled = true;
        popupPanel.transform.DOMove(backgroundPanel.transform.position, .3f).SetUpdate(true);
        Time.timeScale = 0;
    }

}


