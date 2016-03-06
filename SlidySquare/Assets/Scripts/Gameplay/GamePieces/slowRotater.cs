using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class slowRotater : MonoBehaviour {

    string[] Colors = {
        "F32224FF", //red
        "22D1FFFF", //blue
        //"16A184FF",  //green low vis
        "E77D21FF", //orange
        "8E44AFFF", //purple
        "000000FF" //black
    };

    void Start () {
       // GetComponent<SpriteRenderer>().color = HexToColor(Colors[Random.Range(0, Colors.Length)]);
    }

    public void SetColor(int x)
    {
        if(x < 0 || x >= Colors.Length)
        {
            GetComponent<SpriteRenderer>().color = Common.HexToColor(Colors[0]);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Common.HexToColor(Colors[x]);
        }
    }


    void Update ()
    {
        transform.Rotate(0,0, Time.deltaTime * 50.0f);
	}

}


