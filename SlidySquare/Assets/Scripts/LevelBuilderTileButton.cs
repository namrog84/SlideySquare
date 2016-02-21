using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelBuilderTileButton : MonoBehaviour {

	void Start () {
	}
	
	void Update () {
	}

    public int id = 0;
    public Vector2 location = new Vector2(0, 0);
    public Sprite [] sprites;
    public int index = 0;

    public void Derp()
    {
        index = (int)LevelBuilderNameSpace.LevelBuilderID.selectedTile;
        GetComponent<Image>().sprite = sprites[index];
        updateAlpha();
    }

    public void SetTile(int ind)
    {
        index = ind;
        //index = (int)LevelBuilderNameSpace.LevelBuilderID.selectedTile;
        GetComponent<Image>().sprite = sprites[index];
        updateAlpha();
    }

    private void updateAlpha()
    {
        //THIS SHIT RIGHT HERE UNITY, SCREW THIS BULLSHIT
        var c = GetComponent<Button>().colors;
        var norm = c.normalColor;
        norm.a = 1.0f;
        if (index == 0)
        {
            norm.a = 0.5f;
        }
        c.normalColor = norm;
        GetComponent<Button>().colors = c;
    }
    public void Derp2()
    {
        if (Application.isMobilePlatform || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            index = (int)LevelBuilderNameSpace.LevelBuilderID.selectedTile;
            GetComponent<Image>().sprite = sprites[index];
            updateAlpha();
        }
    }
    //  None,
   // Player, Gold, Goal, Wall,  BlueBlock, PurpleBlock, OrangeWall, OrangeButton,  TurnOnWall, TurnOffWall, Teleporter, GreenButton,
   //     TurnLeft, TurnUp, TurnRight, TurnDown,
   //     TurnLeftUp, TurnRightUp, TurnLeftDown, TurnRightDown

internal void setLocation(int i, int j)
    {
        location = new Vector2(i, j);
        //if (i % 2 == 0 || j % 2 == 0)
        {
            //var temp = GetComponent<Button>().colors;
            //temp.normalColor = Color.white;
            //GetComponent<Button>().colors = temp;
        }
        //else
        //{
        //    var temp = GetComponent<Button>().colors;
        //    temp.normalColor = Color.white;
        //    GetComponent<Button>().colors = temp;
        //}
    }
}
