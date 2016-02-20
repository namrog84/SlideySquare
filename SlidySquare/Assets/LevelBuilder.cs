using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Unity.IO.Compression;

namespace LevelBuilderNameSpace
{
    public enum TileType {
        None=0,
        Player=1, Gold=2, Goal=3, Wall=4,  BlueBlock=5, PurpleBlock=6, OrangeWall=7, OrangeButton=8,  TurnOnWall=9, TurnOffWall=10, Teleporter=11, GreenButton=12,
        TurnLeft=13, TurnUp=14, TurnRight=15, TurnDown=16,
        TurnLeftUp=17, TurnRightUp=18, TurnLeftDown=19, TurnRightDown=20
    };

    [Serializable]
    public class Map
    {
        //Required
        public int width; // map width
        public int height; // map height 
        public List<byte> Board = new List<byte>(); //Full Board
        public List<byte> IDsBoard = new List<byte>(); //Full Board ID  (For teleporters and buttons)

        //optional
        public string name; // ??  saves submitted name?
        public bool isSubmitted; //  has this been submitted already?

    }

    public class LevelBuilder : MonoBehaviour
    {
        public static TileType currentSelected;

        // Use this for initialization
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
                    temp.GetComponentInChildren<Text>().text = j + "," + i;

                    TheBoard.Add(temp);
                }
            }
            Clickity();
        }
        private int maxWidth = 20;
        private int maxHeight = 20;
        private int maxSize = 120;

        // Update is called once per frame
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
            fader.GetComponent<SceneFadeInOut>().FinishedFade += finished;
        }

        void finished()
        {
            Time.timeScale = 1;
            Application.LoadLevel(4);
            //Application.LoadLevel(Application.loadedLevel);
        }


        public GameObject theButton;
        private int x = 0;
        Text texter;
        public List<GameObject> TheBoard;
        //public List<GameObject> TheBoard = new List<GameObject>();

        public int w = 3;
        public int h = 3;
        public void IncreaseWidth()
        {
            w++;
            Clickity();
        }
        public void DecreaseWidth()
        {
            w--;
            Clickity();
        }
        public void IncreaseHeight()
        {
            h++;
            Clickity();
        }
        public void DecreaseHeight()
        {
            h--;
            Clickity();
        }


        public void SaveLevel()
        {
            Map m = new Map();
            m.height = h;
            m.width = w;
            for (int j = 0; j < TheBoard.Count; j++)
            {
                var tileType = (byte)TheBoard[j].GetComponent<LevelBuilderTileButton>().index;
                m.Board.Add(tileType);
            }
            SaveSerialize("level.txt", m);
        }

        public void SaveSerialize(string filename, object m)
        {
            IFormatter formatter = new BinaryFormatter();
            using (var stream = new FileStream(Application.dataPath + "/" + filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var zipper = new GZipStream(stream, CompressionMode.Compress, false))
                {
                    formatter.Serialize(zipper, m);
                }
            }
        }
        public object LoadDeserialize(string filename)
        {
            object result;
            IFormatter formatter = new BinaryFormatter();

            using (Stream stream = new FileStream(Application.dataPath + "/level.txt", FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var zipper = new GZipStream(stream, CompressionMode.Decompress))
                {
                    result = formatter.Deserialize(zipper);
                }
            }
            return result;
        }


        public void LoadLevel()
        {
            Map m = (Map)LoadDeserialize("level.txt");
            if(m == null)
            {
                Debug.Log("ERROR");
                return;
            }

            h = m.height;
            w = m.width;
            for (int j = 0; j < TheBoard.Count; j++)
            {
                TheBoard[j].SetActive(true);
                TheBoard[j].GetComponent<LevelBuilderTileButton>().SetTile((int)m.Board[j]);
            }
            Clickity(); //refresh level
        }
        public GameObject MapPanel;

        public void Clickity()
        {
            w = Mathf.Clamp(w, 4, maxWidth);
            h = Mathf.Clamp(h, 4, maxHeight);
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
          
            if (w > h)
            {
                MapPanel.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                MapPanel.GetComponent<GridLayoutGroup>().constraintCount = w;
            }
            else
            {
                MapPanel.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedRowCount;
                MapPanel.GetComponent<GridLayoutGroup>().constraintCount = h;
            }



            var dw = (int)MapPanel.GetComponent<RectTransform>().rect.width / w;
            var dh = (int)MapPanel.GetComponent<RectTransform>().rect.height / h;

            var size = Mathf.Min(dw, dh, maxSize);
            
            MapPanel.GetComponent<GridLayoutGroup>().cellSize = new Vector2(size, size);
           // Debug.Log(size);
            dw = size;
            dh = size;


            //int lastID = 0;
            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    int ID = i + j * maxWidth;// ((2 * w + 1));
                    //lastID = Mathf.Max(ID, lastID);
                    GameObject temp;
                    TheBoard[ID].SetActive(true);
                    temp = TheBoard[ID];
                    

                    temp.GetComponent<RectTransform>().sizeDelta = new Vector2(dw, dh);
                    

                    //temp.transform.position = placement;// new Vector3(dw * i + 200, dh * j - 600);
                    //placement.x += dw;





                    var btncontroller = temp.GetComponent<LevelBuilderTileButton>();
                    btncontroller.id = ID;// i + j * w;
                    btncontroller.setLocation(i, j);

                }
            }

        }
    }
}