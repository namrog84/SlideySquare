using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class ClearAboutScreenController : MonoBehaviour {

    public RectTransform popupPanel;


    //[SerializeField]
    private Button MyButton = null; // assign in the editor

    public Vector3 start;
    void Start()
    {
        if (name == "AboutPanel")
        {
            start = transform.position;
            transform.position += new Vector3(2000, 0, 0);
        }
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(() => { MyFunction(); });
    }

    private void MyFunction()
    {
        //GetComponent<Image>().enabled = false;
        popupPanel.transform.DOMove(popupPanel.position + new Vector3(2000f, 0, 0), .4f);
    }

}
