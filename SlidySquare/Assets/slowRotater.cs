using UnityEngine;
using System.Collections;

public class slowRotater : MonoBehaviour {

	// Use this for initialization
	void Start () {
       // GetComponent<SpriteRenderer>().color = HexToColor(Colors[Random.Range(0, Colors.Length)]);

    }
    public void SetColor(int x)
    {
        if(x < 0 || x >= Colors.Length)
            GetComponent<SpriteRenderer>().color = HexToColor(Colors[0]);
        else
        {
            GetComponent<SpriteRenderer>().color = HexToColor(Colors[x]);
        }
    }
    Color HexToColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }


    string[] Colors = {
    "F32224FF", //red
    "22D1FFFF", //blue
    //"16A184FF",  //green low vis
    "E77D21FF", //orange
    "8E44AFFF", //purple
    "000000FF" //black
    };

    // Update is called once per frame
    void Update () {
        transform.Rotate(0,0, Time.deltaTime * 50.0f);
        
	}
}
