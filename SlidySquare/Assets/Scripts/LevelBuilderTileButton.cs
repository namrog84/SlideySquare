using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LevelBuilderNameSpace;
using Assets.Scripts;

public class LevelBuilderTileButton : MonoBehaviour {

    private int Id = 0;
    public Vector2 location = new Vector2(0, 0);
    public Sprite[] sprites;
    public int index = 0;


    void Start () {
        LevelBuilder.Toggler += Tiggler;
    }

    private bool isIdMode = false;
    void Tiggler()
    {
        isIdMode = !isIdMode;

        if (isIdMode && !IsIdTile(index))
        {
            updateAlpha(0, 0.2f);
            GetComponent<Button>().interactable = false;
        }
        else
        {
            updateAlpha(index);
            GetComponent<Button>().interactable = true;
        }
    }



    //public enum TileType
    //{
    //    None = 0,
    //    Player = 1, Gold = 2, Goal = 3, Wall = 4, BlueBlock = 5, PurpleBlock = 6, OrangeWall = 7, OrangeButton = 8, TurnOnWall = 9, TurnOffWall = 10, Teleporter = 11, GreenButton = 12,
    //    TurnLeft = 13, TurnUp = 14, TurnRight = 15, TurnDown = 16,
    //    TurnLeftUp = 17, TurnRightUp = 18, TurnLeftDown = 19, TurnRightDown = 20
    //};

    private bool IsIdTile(int x)
    {
        if (x == (int)TileType.Teleporter ||  //teleporter
            x == (int)TileType.OrangeButton || x == (int)TileType.OrangeWall ||  //toggle button/wall
            x == (int)TileType.GreenButton || x == (int)TileType.TurnOnWall || x == (int)TileType.TurnOffWall)  //turn on/off wall
        {
            return true;
        }
        return false;

    }

    void Update () {
	}



    public void Derp()
    {
        if (!isIdMode)
        {
            index = (int)LevelBuilderNameSpace.LevelBuilderID.selectedTile;
            GetComponent<Image>().sprite = sprites[index];
            updateAlpha(index);
        }
        else if(IsIdTile(index))
        {
            Id++;
            GetComponentInChildren<Text>().text = "" + Id;
        }
    }

    public void SetTile(int ind)
    {
        index = ind;
        //index = (int)LevelBuilderNameSpace.LevelBuilderID.selectedTile;
        GetComponent<Image>().sprite = sprites[index];
        updateAlpha(index);
    }


    private void updateAlpha(int x, float newAlpha = 0.5f)
    {
        //THIS SHIT RIGHT HERE UNITY, SCREW THIS BULLSHIT
        var c = GetComponent<Button>().colors;
        var norm = c.normalColor;
        norm.a = 1.0f;
        if (x == 0)
        {
            norm.a = newAlpha;
        }
        c.normalColor = norm;
        GetComponent<Button>().colors = c;
    }
    public void Derp2()
    {
        if (!isIdMode)
        {
            if (Application.isMobilePlatform || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                index = (int)LevelBuilderNameSpace.LevelBuilderID.selectedTile;
                GetComponent<Image>().sprite = sprites[index];
                updateAlpha(index);
            }
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
