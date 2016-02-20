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
#pragma warning disable 0618
        Application.LoadLevel("About");
#pragma warning restore 0618
    }
}
