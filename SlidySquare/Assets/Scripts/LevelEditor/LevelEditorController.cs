using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Unity.IO.Compression;
using Assets.Scripts;
using Assets.Scripts.Gameplay;
using UnityEngine.SceneManagement;

namespace LevelBuilderNameSpace
{
   
    public class LevelEditorController : MonoBehaviour
    {
        public static Tile.TileType currentSelected;

        public GameObject MapPanel;
        public GameObject theButton;

        //public static GameBoard currentBoard = new GameBoard();

        public static List<GameObject> TheBoardOfButtons;


        public delegate void ToggleGroup();
        public static ToggleGroup ToggleModeOnTileButtons;


        //private int gameBoardCurrentWidth = 8;
        //private int gameBoardCurrentHeight = 6;

        private int gameBoardMaxWidth = 20;
        private int gameBoardMaxHeight = 20;

        private int maxTileSize = 120;

        bool isIdMode = false;

        Text texter;

        //private int x = 0;


        void Start()
        {
            if (GameCore.IsNewMap)
            {
                GameCore.currentBoard = new GameBoard();
            }
            TheBoardOfButtons = new List<GameObject>();
            for (int j = 0; j < gameBoardMaxHeight; j++)
            {
                for (int i = 0; i < gameBoardMaxWidth; i++)
                {
                    var temp = Instantiate(theButton);
                    temp.SetActive(false);
                    temp.transform.SetParent(MapPanel.transform);
                    TheBoardOfButtons.Add(temp);
                }
            }
            RefreshBoard();

            //Are we loading or making new?
            if (!GameCore.IsNewMap)
            {
                LoadLevel();
            }
        }


        void Update()
        {
        }

        public void PlayCurrentLevel()
        {

            GameCore.PlayingLevelFromEditor = true;
            GameCore.CustomLevel = true;

            //unneeded because its already in gamecore current map? 
            //GameCore.LevelNameToLoad = GameCore.currentBoard.name;
            SaveLevel(); //save it!

            //TODO cache or singleton it up?
            FindObjectOfType<MySceneManager>().LoadToDynamicScene();
            
        }


        public void ToggleIDMode()
        {
            isIdMode = !isIdMode;
            if(ToggleModeOnTileButtons == null)
            {
                return;
            }

            if (isIdMode)
            {
                //turn on ID mode
                currentSelected = Tile.TileType.None;
            }
            ToggleModeOnTileButtons();

        }

        public void IncreaseWidth()
        {
            GameCore.currentBoard.width++;
            RefreshBoard();
        }
        public void DecreaseWidth()
        {
            GameCore.currentBoard.width--;
            RefreshBoard();
        }
        public void IncreaseHeight()
        {
            GameCore.currentBoard.height++;
            RefreshBoard();
        }
        public void DecreaseHeight()
        {
            GameCore.currentBoard.height--;
            RefreshBoard();
        }


        public void SaveLevel()
        {
            //no longer needed becaue we always keep it up to date now?
            //for (int j = 0; j < TheBoardOfButtons.Count; j++)
            //{
            //var tileButton = TheBoardOfButtons[j].GetComponent<LevelBuilderTileButton>();

            //int tempTileX = j % GameCore.currentBoard.width;
            //int tempTileY = j / GameCore.currentBoard.height;
            //GameCore.currentBoard.SetTile(tempTileX, tempTileY, tileButton.tile);
            //}
            if(GameCore.currentBoard.name == null || GameCore.currentBoard.name == "")
            {
                GameCore.currentBoard.name = "BeepBoop" + UnityEngine.Random.Range(1, 100); ;
            }
            //FileManager.SaveBoardToFile(GameCore.currentBoard);
        }

        public void LoadLevel()
        {
            //Debug.Log("Loading: " + GameCore.LevelNameToLoad);
            //GameCore.currentBoard = (GameBoard)FileManager.LoadBoardFromFile("custom", GameCore.LevelNameToLoad);

            //if (GameCore.currentBoard == null)
            //{
            //    Debug.Log("ERROR loading level");
            //    return;
            //}

            //for (int j = 0; j < TheBoardOfButtons.Count; j++)
            //{
            //    TheBoardOfButtons[j].SetActive(true);

            //    int tempTileX = j % GameCore.currentBoard.width;
            //    int tempTileY = j / GameCore.currentBoard.height;

            //    Tile tile = GameCore.currentBoard.GetTile(tempTileX, tempTileY);
            //    TheBoardOfButtons[j].GetComponent<LevelBuilderTileButton>().SetTile(tile);
            //}

            //RefreshBoard(); //refresh level
        }


        public void RefreshBoard()
        {
            //move to inside gameboard?
            GameCore.currentBoard.width = Mathf.Clamp(GameCore.currentBoard.width, 4, gameBoardMaxWidth);
            GameCore.currentBoard.height = Mathf.Clamp(GameCore.currentBoard.height, 4, gameBoardMaxHeight);

            //DisableAllButtonsFirst
            for (int i = 0; i < TheBoardOfButtons.Count; i++) //deactivate ALL!
            {
                TheBoardOfButtons[i].SetActive(false);
            }
          
            //Resolve size constraints
            if (GameCore.currentBoard.width > GameCore.currentBoard.height)
            {
                MapPanel.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                MapPanel.GetComponent<GridLayoutGroup>().constraintCount = GameCore.currentBoard.width;
            }
            else
            {
                MapPanel.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedRowCount;
                MapPanel.GetComponent<GridLayoutGroup>().constraintCount = GameCore.currentBoard.height;
            }

            var dw = (int)MapPanel.GetComponent<RectTransform>().rect.width / GameCore.currentBoard.width;
            var dh = (int)MapPanel.GetComponent<RectTransform>().rect.height / GameCore.currentBoard.height;

            //Go with side that is smaller
            var size = Mathf.Min(dw, dh, maxTileSize);
            
            //set size of buttons for editor
            MapPanel.GetComponent<GridLayoutGroup>().cellSize = new Vector2(size, size);
            dw = size;
            dh = size;

            for (int y = 0; y < GameCore.currentBoard.height; y++)
            {
                for (int x = 0; x < GameCore.currentBoard.width; x++)
                {
                    int ID = x + y * gameBoardMaxWidth;

                    // turn on this button
                    TheBoardOfButtons[ID].SetActive(true); 

                    GameObject temp = TheBoardOfButtons[ID];

                    //set size and scale
                    temp.GetComponent<RectTransform>().sizeDelta = new Vector2(dw, dh);
                    temp.GetComponent<RectTransform>().localScale = Vector3.one;
                    var btncontroller = temp.GetComponent<LevelBuilderTileButton>();

                    //set button location
                    btncontroller.setLocation(x, y);
                }
            }
        }
    }


}



