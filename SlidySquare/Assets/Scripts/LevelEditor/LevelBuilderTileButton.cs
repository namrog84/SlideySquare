using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LevelBuilderNameSpace;
using Assets.Scripts;
using Assets.Scripts.Gameplay;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
public class LevelBuilderTileButton : MonoBehaviour {

    public Vector2 location = new Vector2(0, 0);
    public Sprite[] sprites;

    private Text TileText;
    private Image image;
    private Button button;

    public static Vector2 LastPlayerPosition = Vector2.zero;

    void Start () {
        LevelEditorController.ToggleModeOnTileButtons += Tiggler;
        TileText = GetComponentInChildren<Text>();
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    private bool isIdMode = false;
    void Tiggler()
    {
        isIdMode = !isIdMode;

        if (isIdMode && !GameCore.currentBoard.GetTile(location).NeedsID())
        {
            setAlpha(Tile.TileType.None, 0.2f);
            button.interactable = false;
        }
        else
        {
            setAlpha(GameCore.currentBoard.GetTile(location).type);
            button.interactable = true;
        }
    }

    private void setAlpha(Tile.TileType tt, float newAlpha = 0.5f)
    {
        //THIS SHIT RIGHT HERE UNITY, SCREW THIS BULLSHIT
        var c = button.colors;
        var norm = c.normalColor;
        norm.a = 1.0f;
        if (tt == Tile.TileType.None)
        {
            norm.a = newAlpha;
        }
        c.normalColor = norm;
        button.colors = c;
    }


    void Update () {
	}


    public void IncreaseID()
    {
        if (!isIdMode)
        {
            SetTileGraphic(GameCore.currentBoard.GetTile(location).type);
            setAlpha(GameCore.currentBoard.GetTile(location).type);
        }
        else if(GameCore.currentBoard.GetTile(location).NeedsID())
        {
            var ListTiles = GameCore.currentBoard.GetSortedIDs();

            //if its currently the highest, and the 2nd highest isn't at the same, we need to wrap around
            if(ListTiles[0] == GameCore.currentBoard.GetTile(location).TileID && ListTiles[1] != GameCore.currentBoard.GetTile(location).TileID)
            {
                GameCore.currentBoard.GetTile(location).TileID = 0;
            }
            else
            {
                GameCore.currentBoard.GetTile(location).TileID++;
            }

            TileText.text = "" + GameCore.currentBoard.GetTile(location).TileID;
        }
    }

    public void SetTile(Tile t)
    {
        GameCore.currentBoard.SetTile(location, t);

        SetTileGraphic(GameCore.currentBoard.GetTile(location).type);
        setAlpha(GameCore.currentBoard.GetTile(location).type);
    }

    private void SetTileGraphic(Tile.TileType type)
    {
        var newsprite = sprites[(int)type];
        if (type==Tile.TileType.Player)
        {
            for(int i = 0; i < LevelEditorController.TheBoardOfButtons.Count; i++)
            {
                var temp = LevelEditorController.TheBoardOfButtons[i];
                if (temp.activeSelf)
                {
                    if (temp.GetComponent<LevelBuilderTileButton>().image.sprite == newsprite)
                    {
                        LevelEditorController.TheBoardOfButtons[i].GetComponent<LevelBuilderTileButton>().SetTileGraphic(Tile.TileType.None);
                    }

                }
            }
        }
        var col = image.color;
        if (type == Tile.TileType.None)
        {
            col.a = 0.5f;
        }
        else
        {
            col.a = 1.0f;
        }

        image.sprite = newsprite;
        image.color = col;

    }


    public void OnTileClickedSetTileType()
    {
        if (isIdMode) // if in ID mode, don't change tilegraphic.
        {
            return;
        }
        if (Application.isMobilePlatform || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            var TileType = LevelBuilderID.selectedTile;
            SetTileGraphic(TileType);
            setAlpha(TileType);
        }
    }

    public void setLocation(int i, int j)
    {
        location.x = i;
        location.y = j;
    }
}
