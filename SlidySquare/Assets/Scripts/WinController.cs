using UnityEngine;
using System.Collections;

public class WinController : MonoBehaviour {

    public float delta = 4f;
    public float rate = 3f;

    public float delta2 = 1f;
    public float rate2 = 1f;

	void Start () {
	}

    void Update () {
        //var temp = GetComponent<RectTransform>().position;
        //temp.y = temp.y + delta*Mathf.Sin(rate*Time.time);
        //GetComponent<RectTransform>().position = temp;

        //var temp2 = GetComponent<RectTransform>().localScale;
        //temp2.x = 1 + delta2 * Mathf.Cos(rate2 * Time.time);
        //temp2.y = 1 + delta2 * Mathf.Cos(rate2 * Time.time);
        //GetComponent<RectTransform>().localScale = temp2;
    }

}


