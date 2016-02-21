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

namespace LevelBuilderNameSpace
{
   
    public class LevelBuilder : MonoBehaviour
    {
        public static TileType currentSelected;
        public GameObject MapPanel;
        public GameObject theButton;

        Map currentMap = new Map();
        public List<GameObject> TheBoard;

        public delegate void ToggleGroup();
        public static ToggleGroup Toggler;

        private int width = 8;
        private int height = 6;

        private int maxWidth = 20;
        private int maxHeight = 20;
        private int maxSize = 120;
        bool isIdMode = false;

        Text texter;

        //private int x = 0;

        void Start()
        {
            TheBoard = new List<GameObject>();
            for (int j = 0; j < maxHeight; j++)
            {
                for (int i = 0; i < maxWidth; i++)
                {
                    //Debug.Log(gameObject.name);
                    var temp = Instantiate(theButton);
                    temp.SetActive(false);
                    temp.transform.SetParent(MapPanel.transform);
                    //temp.GetComponentInChildren<Text>().text = j + "," + i;

                    TheBoard.Add(temp);
                }
            }
            Clickity();
        }


        void Update()
        {
        }

        public void PlayCurrentLevel()
        {
            PlayerPrefs.SetInt("CurrentLevel", -3); // -3 is for custom level I guess? 
            PlayerPrefs.SetString("LevelName", "level.txt");
            PlayerPrefs.Save();
            StartCoroutine(LoadOut());
        }

        private IEnumerator LoadOut()
        {
            var fader = GameObject.Find("SceneFader");
            fader.GetComponent<SceneFadeInOut>().fadeDir *= -1;
            fader.GetComponent<SceneFadeInOut>().startTime = 0;
            yield return new WaitForEndOfFrame();
            fader.GetComponent<SceneFadeInOut>().FinishedFade += LoadLevelOnFinished;
        }

        public void ToggleIDMode()
        {
            isIdMode = !isIdMode;
            if (isIdMode)
            {
                //turn on ID mode
                if(Toggler != null)
                {
                    currentSelected = TileType.None;
                    Toggler();
                }
                
            }
            else
            {
                //turn off ID mode
                if (Toggler != null)
                {
                    Toggler();
                }
            }

        }

        void LoadLevelOnFinished()
        {
            Time.timeScale = 1;
#pragma warning disable 0618
            Application.LoadLevel(4);
#pragma warning restore 0618
            //Application.LoadLevel(Application.loadedLevel);
        }

        public void IncreaseWidth()
        {
            width++;
            Clickity();
        }
        public void DecreaseWidth()
        {
            width--;
            Clickity();
        }
        public void IncreaseHeight()
        {
            height++;
            Clickity();
        }
        public void DecreaseHeight()
        {
            height--;
            Clickity();
        }


        public void SaveLevel()
        {
            Map m = new Map();
            m.height = height;
            m.width = width;
            for (int j = 0; j < TheBoard.Count; j++)
            {
                var tileType = (byte)TheBoard[j].GetComponent<LevelBuilderTileButton>().index;
                m.Board.Add(tileType);
            }
            FileManager.SaveObjectToFile("level.txt", m);
        }

        public void LoadLevel()
        {
            Map m = (Map)FileManager.LoadFromFile("level.txt");
            if(m == null)
            {
                Debug.Log("ERROR");
                return;
            }

            height = m.height;
            width = m.width;
            for (int j = 0; j < TheBoard.Count; j++)
            {
                TheBoard[j].SetActive(true);
                TheBoard[j].GetComponent<LevelBuilderTileButton>().SetTile((int)m.Board[j]);
            }
            Clickity(); //refresh level
        }

        public void Clickity()
        {
            width = Mathf.Clamp(width, 4, maxWidth);
            height = Mathf.Clamp(height, 4, maxHeight);
            //if (x == 1)
            {
                for (int i = 0; i < TheBoard.Count; i++) //deactivate ALL!
                {
                    TheBoard[i].SetActive(false);
                    //Destroy(TheBoard[i]);
                }
                //TheBoard.Clear();
                //return;
            }
          
            if (width > height)
            {
                MapPanel.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                MapPanel.GetComponent<GridLayoutGroup>().constraintCount = width;
            }
            else
            {
                MapPanel.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedRowCount;
                MapPanel.GetComponent<GridLayoutGroup>().constraintCount = height;
            }

            var dw = (int)MapPanel.GetComponent<RectTransform>().rect.width / width;
            var dh = (int)MapPanel.GetComponent<RectTransform>().rect.height / height;

            var size = Mathf.Min(dw, dh, maxSize);
            
            MapPanel.GetComponent<GridLayoutGroup>().cellSize = new Vector2(size, size);
            dw = size;
            dh = size;

            //int lastID = 0;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    int ID = i + j * maxWidth;// ((2 * w + 1));
                    //lastID = Mathf.Max(ID, lastID);
                    GameObject temp;
                    TheBoard[ID].SetActive(true);
                    temp = TheBoard[ID];

                    temp.GetComponent<RectTransform>().sizeDelta = new Vector2(dw, dh);
                    
                    var btncontroller = temp.GetComponent<LevelBuilderTileButton>();
                    //btncontroller.Id = ID;// i + j * w;
                    btncontroller.setLocation(i, j);

                }
            }

        }
    }


}



