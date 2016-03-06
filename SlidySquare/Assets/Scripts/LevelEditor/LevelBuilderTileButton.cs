using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LevelBuilderNameSpace;
using Assets.Scripts;
using Assets.Scripts.Gameplay;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Sprite))]
[RequireComponent(typeof(Button))]
public class LevelBuilderTileButton : MonoBehaviour {

    public Vector2 location = new Vector2(0, 0);
    public Sprite[] sprites;

    public Tile tile;

    private Text TileText;
    private Sprite sprite;
    private Button button; 

    void Start () {
        LevelEditorController.ToggleModeOnTileButtons += Tiggler;
        TileText = GetComponentInChildren<Text>();
        sprite = GetComponent<Sprite>();
        button = GetComponent<Button>();
    }

    private bool isIdMode = false;
    void Tiggler()
    {
        isIdMode = !isIdMode;

        if (isIdMode && !tile.NeedsID())
        {
            setAlpha(Tile.TileType.None, 0.2f);
            button.interactable = false;
        }
        else
        {
            setAlpha(tile.type);
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
            SetTileGraphic(tile.type);
            setAlpha(tile.type);
        }
        else if(tile.NeedsID())
        {
            var ListTiles = LevelEditorController.currentBoard.GetSortedIDs();

            //if its currently the highest, and the 2nd highest isn't at the same, we need to wrap around
            if(ListTiles[0] == tile.TileID && ListTiles[1] != tile.TileID)
            {
                tile.TileID = 0;
            }
            else
            {
                tile.TileID++;
            }

            TileText.text = "" + tile.TileID;
        }
    }

    public void SetTile(Tile t)
    {
        tile = t;

        SetTileGraphic(tile.type);
        setAlpha(tile.type);
    }

    private void SetTileGraphic(Tile.TileType type)
    {
        sprite = sprites[(int)type];
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
