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

namespace LevelBuilderNameSpace
{
   
    public class LevelEditorController : MonoBehaviour
    {
        public static Tile.TileType currentSelected;

        public GameObject MapPanel;
        public GameObject theButton;

        public static GameBoard currentBoard = new GameBoard();

        public List<GameObject> TheBoardOfButtons;


        public delegate void ToggleGroup();
        public static ToggleGroup ToggleModeOnTileButtons;


        private int gameBoardCurrentWidth = 8;
        private int gameBoardCurrentHeight = 6;

        private int gameBoardMaxWidth = 20;
        private int gameBoardMaxHeight = 20;

        private int maxTileSize = 120;

        bool isIdMode = false;

        Text texter;

        //private int x = 0;


        void Start()
        {
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
            SaveLevel(); // save it!

            GameCore.CustomLevel = true;
            GameCore.LevelNameToLoad = currentBoard.name;


            StartCoroutine(SceneFadeInOut.LoadToDynamicScene());
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
            gameBoardCurrentWidth++;
            RefreshBoard();
        }
        public void DecreaseWidth()
        {
            gameBoardCurrentWidth--;
            RefreshBoard();
        }
        public void IncreaseHeight()
        {
            gameBoardCurrentHeight++;
            RefreshBoard();
        }
        public void DecreaseHeight()
        {
            gameBoardCurrentHeight--;
            RefreshBoard();
        }


        public void SaveLevel()
        {
            for (int j = 0; j < TheBoardOfButtons.Count; j++)
            {
                var tileButton = TheBoardOfButtons[j].GetComponent<LevelBuilderTileButton>();

                int tempTileX = j % gameBoardCurrentWidth;
                int tempTileY = j / gameBoardCurrentHeight;
                currentBoard.SetTile(tempTileX, tempTileY, tileButton.tile);
            }

            FileManager.SaveBoardToFile(currentBoard);
        }

        public void LoadLevel()
        {
            Debug.Log("Loading: " + GameCore.LevelNameToLoad);
            currentBoard = (GameBoard)FileManager.LoadBoardFromFile("custom", GameCore.LevelNameToLoad);

            if (currentBoard == null)
            {
                Debug.Log("ERROR loading level");
                return;
            }

            gameBoardCurrentHeight = currentBoard.height;
            gameBoardCurrentWidth = currentBoard.width;
            for (int j = 0; j < TheBoardOfButtons.Count; j++)
            {
                TheBoardOfButtons[j].SetActive(true);

                int tempTileX = j % gameBoardCurrentWidth;
                int tempTileY = j / gameBoardCurrentHeight;

                Tile tile = currentBoard.GetTile(tempTileX, tempTileY);
                TheBoardOfButtons[j].GetComponent<LevelBuilderTileButton>().SetTile(tile);
            }

            RefreshBoard(); //refresh level
        }


        public void RefreshBoard()
        {
            gameBoardCurrentWidth = Mathf.Clamp(gameBoardCurrentWidth, 4, gameBoardMaxWidth);
            gameBoardCurrentHeight = Mathf.Clamp(gameBoardCurrentHeight, 4, gameBoardMaxHeight);

            //DisableAllButtonsFirst
            for (int i = 0; i < TheBoardOfButtons.Count; i++) //deactivate ALL!
            {
                TheBoardOfButtons[i].SetActive(false);
            }
          
            //Resolve size constraints
            if (gameBoardCurrentWidth > gameBoardCurrentHeight)
            {
                MapPanel.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                MapPanel.GetComponent<GridLayoutGroup>().constraintCount = gameBoardCurrentWidth;
            }
            else
            {
                MapPanel.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedRowCount;
                MapPanel.GetComponent<GridLayoutGroup>().constraintCount = gameBoardCurrentHeight;
            }

            var dw = (int)MapPanel.GetComponent<RectTransform>().rect.width / gameBoardCurrentWidth;
            var dh = (int)MapPanel.GetComponent<RectTransform>().rect.height / gameBoardCurrentHeight;

            //Go with side that is smaller
            var size = Mathf.Min(dw, dh, maxTileSize);
            
            //set size of buttons for editor
            MapPanel.GetComponent<GridLayoutGroup>().cellSize = new Vector2(size, size);
            dw = size;
            dh = size;

            for (int y = 0; y < gameBoardCurrentHeight; y++)
            {
                for (int x = 0; x < gameBoardCurrentWidth; x++)
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



