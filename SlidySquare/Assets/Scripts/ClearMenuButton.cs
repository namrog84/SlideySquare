using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class ClearMenuButton : MonoBehaviour {

    public RectTransform popupPanel;
    public RectTransform backgroundPanel;


    //[SerializeField]
    private Button MyButton = null; // assign in the editor

    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(() => { MyFunction(); });
        //DOTween.defaultTimeScaleIndependent = true;
    }

    private void MyFunction()
    {
        backgroundPanel.GetComponent<Image>().enabled = false;
        Time.timeScale = 1;
        popupPanel.transform.DOMove(popupPanel.position + new Vector3(0, -1500, 0), .3f).SetUpdate(true);
    
    }

}
