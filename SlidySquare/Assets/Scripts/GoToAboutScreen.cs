using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class GoToAboutScreen : MonoBehaviour {


    public RectTransform popupPanel;


    //[SerializeField]
    private Button MyButton = null; // assign in the editor

    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(() => { MyFunction(); });
        DOTween.defaultTimeScaleIndependent = true;
    }

    private void MyFunction()
    {
        Application.LoadLevel("About");
        //GetComponent<Image>().enabled = false;
        //popupPanel.transform.DOMove(popupPanel.GetComponent<ClearAboutScreenController>().start, .4f);
    }
}
