using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using GoogleMobileAds.Api;
using UnityEngine.Analytics;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.IO.Compression;
using LevelBuilderNameSpace;
using Assets.Scripts;

public class LevelLoader : MonoBehaviour {

    public string levelname = "leveltest.tmx";
    public Vector3 offset;
    public Vector3 mapOffset;

    // Use this for initialization
    void Start () {
        Analytics.CustomEvent("LevelStart", new Dictionary<string, object>{ { "Level", PlayerPrefs.GetInt("CurrentLevel", -1) } });

        HitWall.HitWallCount = GameObject.FindGameObjectsWithTag("HitWall").Length;
       
        //AdManager.PlayAd();
        //Debug.Log("Level Starting is " + PlayerPrefs.GetInt("CurrentLevel"));
        levelname = PlayerPrefs.GetInt("CurrentLevel", 1) + ".tmx";
        offset = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
        Debug.Log("What? " + levelname);
        if(PlayerPrefs.GetInt("CurrentLevel", 1) == -3)
        {
            //custom new level design
            StartCoroutine(LoadLevelNew(PlayerPrefs.GetString("LevelName")));
        }
        else
        {
            //standard tmx level
            StartCoroutine(LoadLevel(levelname));
        }
        

        if (AdManager.PlayCount == 4 || AdManager.Skipped)
        {
            AdManager.PlayAd();
        }
        else
        {
            AdManager.RequestInterstitial();
        }
        Time.timeScale = 1;
    }
    private int teleCount = 0;
    //width and height of level
    private int _height = 0;
    private int _width = 0;

    public GameObject[] tiles;

    GameObject[,] worldTiles;
    private IEnumerator LoadLevelNew(string filename)
    {
        //string fpath = Path.Combine(Application.streamingAssetsPath, filename);
        //string leveldata;

        Map m = (Map)FileManager.LoadDeserialize(filename);
        if (m == null)
        {
            Debug.Log("ERROR");
            return null;
        }

        _width = m.width;
        _height = m.height;
        mapOffset = new Vector3(_width / 2.0f, _height / 2, 0);
        worldTiles = new GameObject[_width, _height];
        //Debug.Log(_width + " " + _height);
        Camera.main.GetComponent<CameraController>().UnitsHigh = _width;//(_width / 2)+1;
        Camera.main.GetComponent<CameraController>().AdjustView();
        Camera.main.transform.position += new Vector3(-0.5f, 0.5f);


        //scan tile data layer

        for (int j = 0; j < _height; j++)
        {
            for (int i = 0; i < _width; i++)
            {
                int ID = i + j * 20;
                CreateTile(i, _height - j, j, ConvertNewToOldIDs(m.Board[ID]), "");
            }
        }

        // Teleporter Layer?

        ////xmlReader.ReadToFollowing("objectgroup");
        //if (xmlReader.IsStartElement("objectgroup"))
        //{
        //    xmlReader.ReadToDescendant("object");
        //    //while ()
        //    do
        //    {
        //        // Debug.Log("huh");
        //        if (xmlReader.IsStartElement("object"))
        //        {
        //            //Debug.Log("object");
        //            float x = float.Parse(xmlReader.GetAttribute("x"));
        //            float y = float.Parse(xmlReader.GetAttribute("y"));
        //            // Debug.Log("xy");
        //            xmlReader.ReadToDescendant("polyline");
        //            //int gid = int.Parse(xmlReader.GetAttribute("gid"));
        //            string points = xmlReader.GetAttribute("points");
        //            points = points.Split()[1];
        //            float x2 = float.Parse(points.Split(',')[0]) + x;
        //            float y2 = float.Parse(points.Split(',')[1]) + y;
        //            int xf = Mathf.FloorToInt(x / 32);
        //            int yf = Mathf.FloorToInt(y / 32);
        //            int x2f = Mathf.FloorToInt(x2 / 32);
        //            int y2f = Mathf.FloorToInt(y2 / 32);

        //            //Debug.Log(x + "," + y + "   " + x2 + "," + y2);
        //            ConnectTiles(xf, yf, x2f, y2f);

        //            //CreateTile(x, y, gid, name);
        //        }
        //    } while (xmlReader.ReadToFollowing("object"));
        //}

        HitWall.HitWallCount = GameObject.FindGameObjectsWithTag("HitWall").Length;
        return null;
    }


    //public enum TileType {
    //None=0,
    //    Player=1, Gold=2, Goal=3, Wall=4,  BlueBlock=5, PurpleBlock=6, OrangeWall=7, OrangeButton=8,  TurnOnWall=9, TurnOffWall=10, Teleporter=11, GreenButton=12,
    //    TurnLeft=13, TurnUp=14, TurnRight=15, TurnDown=16,
    //    TurnLeftUp=17, TurnRightUp=18, TurnLeftDown=19, TurnRightDown=20
    //};
    int ConvertNewToOldIDs(int newID)
    {
        int[] old = { 0, 15, 8, 1, 6, 3, 4, 12, 5, 2, 9, 10, 17, 18, 19, 16, 25, 27, 24, 26 };
        if (newID < 0 || newID >= old.Length)
        {
            return 0;
        }
        return old[newID];
    }

    private IEnumerator LoadLevel(string filename)
    {
        //ReadFile(filename)
        //var fpath = GetFilePath(filename);
        string fpath = Path.Combine(Application.streamingAssetsPath, filename);
        //if (Application.platform == RuntimePlatform.Android)
        {
        //    fpath = "jar:file://" + Application.dataPath + "!/assets/StreamingAssets/levels/" + filename;
        }

        string leveldata;

        //if(fpath)
        {
            //Debug.Log(File.Exists(GetFilePath(filename)));
          //  Application.LoadLevel(0);
            //yield break;
        }

        if (fpath.Contains("://"))
        {
            WWW www = new WWW(fpath);// GetFilePath(filename));
            //var fileInfo = new FileInfo(GetFilePath(filename));
            yield return www;
            //while (!www.isDone) { }

            leveldata = www.text;
        }
        else
        {
            //yield break;
            //FileTools
            //leveldata = File.ReadAllText(fpath);
            leveldata = string.Join("\n", File.ReadAllLines(fpath));
        }
       
       // Debug.Log(leveldata);
        //if(level)
        XmlReader xmlReader = XmlReader.Create(new StringReader(leveldata));

        //keep reading until end-of-file
        while (xmlReader.Read())
        {
            //Debug.Log("READING ");
            //scan map size
            if (xmlReader.IsStartElement("map"))
            {
                _width = int.Parse(xmlReader.GetAttribute("width"));
                _height = int.Parse(xmlReader.GetAttribute("height"));
                mapOffset = new Vector3(_width/2.0f, _height/2, 0);
                worldTiles = new GameObject[_width, _height];
                //Debug.Log(_width + " " + _height);
                Camera.main.GetComponent<CameraController>().UnitsHigh = _width;//(_width / 2)+1;
                Camera.main.GetComponent<CameraController>().AdjustView();
                Camera.main.transform.position += new Vector3(-0.5f, 0.5f);
            }

            //scan tile data layer

            if (xmlReader.IsStartElement("layer"))
            {
                //Debug.Log(xmlReader.GetAttribute("name"));

                if (xmlReader.GetAttribute("name") == "mainlevel")
                {
                    //Debug.Log("starting core!");
                    //if (xmlReader.IsStartElement("data"))
                    {
                        
                        string data = xmlReader.ReadInnerXml();
                       // Debug.Log(data);
                        string[] lines = data.Split('\n');
                        
                        int height = _height;// lines.Length - 1; //removes additional empty line
                        for (int j = 0; j <= height; j++)
                        {
                            string line = lines[j+2];

                            string[] cols = line.Split(',');
                            int width = cols.Length;
                            for (int i = 0; i < width; i++)
                            {
                                int tile = 0;
                                if (int.TryParse(cols[i], out tile))
                                {
                                    CreateTile(i, _height - j, j, tile, "");
                                }
                            }
                        }
                    }
                }
            }
            yield return null;

            //xmlReader.ReadToFollowing("objectgroup");
            if (xmlReader.IsStartElement("objectgroup"))
            {
                xmlReader.ReadToDescendant("object");
                //while ()
                do
                {
                   // Debug.Log("huh");
                    if (xmlReader.IsStartElement("object"))
                    {
                        //Debug.Log("object");
                        float x = float.Parse(xmlReader.GetAttribute("x"));
                        float y = float.Parse(xmlReader.GetAttribute("y"));
                        // Debug.Log("xy");
                        xmlReader.ReadToDescendant("polyline");
                        //int gid = int.Parse(xmlReader.GetAttribute("gid"));
                        string points = xmlReader.GetAttribute("points");
                        points = points.Split()[1];
                        float x2 = float.Parse(points.Split(',')[0]) + x;
                        float y2 = float.Parse(points.Split(',')[1]) + y;
                        int xf = Mathf.FloorToInt(x / 32);
                        int yf = Mathf.FloorToInt(y / 32);
                        int x2f = Mathf.FloorToInt(x2 / 32);
                        int y2f = Mathf.FloorToInt(y2 / 32);

                        //Debug.Log(x + "," + y + "   " + x2 + "," + y2);
                        ConnectTiles(xf, yf, x2f, y2f);

                        //CreateTile(x, y, gid, name);
                    }
                } while (xmlReader.ReadToFollowing("object"));
            }

            //else if (xmlReader.GetAttribute("name") == "TeleporterLayer")
            //{

            //    yield return null;
            //    //if (xmlReader.IsStartElement("data"))
            //    {
            //        Debug.Log("starting tele!");
            //        string data = xmlReader.ReadInnerXml();
            //        Debug.Log(data);
            //        string[] lines = data.Split('\n');
            //        int height = lines.Length - 2; //removes additional empty line
            //        for (int j = 1; j < height + 1; j++)
            //        {
            //            string line = lines[j];
            //            string[] cols = line.Split(',');
            //            int width = cols.Length - 1;
            //            for (int i = 0; i < width + 1; i++)
            //            {
            //                int tile = 0;
            //                if (int.TryParse(cols[i], out tile))
            //                {
            //                    SetTeleporterTile(i, _height - j, tile);
            //                    //CreateTile(i, _height - j, tile, "");
            //                }
            //            }
            //        }
            //    }
            //}
            // }

            //teleporter layer

        }
        //Debug.Log(_width);
        HitWall.HitWallCount = GameObject.FindGameObjectsWithTag("HitWall").Length;
    }

    private void ConnectTiles(int x, int y, int x2, int y2)
    {
        if (x < 0 || y < 0 || x >= _width || x >= _height)
        {
            //Debug.Log("?");
            return;
        }
        if (x2 < 0 || y2 < 0 || x2 >= _width || x2 >= _height)
        {
            //Debug.Log("2");
            return;
        }

        //Debug.Log(x+","+y +"   " + x2+","+ y2);
        if (worldTiles[x, y] != null && worldTiles[x2, y2] != null)
        {
            var tele1 = worldTiles[x, y].GetComponent<teleportController>();
            var tele2 = worldTiles[x2, y2].GetComponent<teleportController>();
            if (tele1 != null && tele2 != null)
            {
               // Debug.Log("Setting Tele");
                tele2.ID = tele1.ID;
                tele1.GetComponentInChildren<slowRotater>().SetColor(teleCount);
                tele2.GetComponentInChildren<slowRotater>().SetColor(teleCount);
                teleCount++;
            }

            var togglebutton = worldTiles[x, y].GetComponent<ButtonController>();
            var togglewall = worldTiles[x2, y2].GetComponent<ToggleSwitch>();
            if (togglebutton != null && togglewall != null)
            {
                togglewall.ToggleID = togglebutton.ToggleID;
            }
            
            

        }
        
        //for (int i = 0; i < teleportController.TeleportsList.Count; i++)
        //{
        //    var tempTele = teleportController.TeleportsList[i];
        //    if (tempTele.x == x && tempTele.y == y)
        //    {
        //        tempTele.ID = id;
        //        break;
        //    }
        //}

        
        //GameObject newTile = (GameObject)Instantiate(tiles[tile - 1]); //create tile
        //if (name != "") newTile.name = "Tile" + tile; //set name if needed
        //newTile.transform.position = new Vector3(x, y, 0) + offset - mapOffset; //set position
        //newTile.transform.parent = gameObject.transform; //make child of this object
    }
    
    private void CreateTile(int x, int y, int trueY, int tile, string name)
    {
        if (tile == 0)
            return;
        if (tiles[tile - 1] == null)
        {
            return;
        }
        //Debug.Log(tile);
        GameObject newTile = (GameObject)Instantiate(tiles[tile - 1]); //create tile
        //Debug.Log(x + " " + trueY); //3 7   7 11
        worldTiles[x, trueY] = newTile;
        if (name != "") newTile.name = "Tile" + tile; //set name if needed
        newTile.transform.position = new Vector3(x, y, 0) + offset - mapOffset; //set position
        newTile.transform.parent = gameObject.transform; //make child of this object
        //if(tile == 10)
        {
           //Debug.Log(x + " " + trueY);
            
           // var teleporter = newTile.GetComponent<teleportController>();
           // if(teleporter != null)
            {
           //     teleporter.SetStart(x, y);
            }
            //else
            {
                //Debug.Log("?HWAT? " + newTile);
            }
        }
    }
    


    // Update is called once per frame
    void Update()
    {

    }


    private string GetFilePath(string filename)
    {
        return GetStreamingAssetsPath() + "/levels/" + filename;
        //return Path.Combine(Path.Combine(GetStreamingAssetsPath(), "levels"), filename);

        //Path.Combine()
        //return GetStreamingAssetsPath() + "/levels/" + filename;

        //if (Application.platform == RuntimePlatform.Android)
        //    return Application.streamingAssetsPath + "/levels/" + filename;//"jar:file://" + Application.dataPath + "!/assets/levels/" + filename;
        //if (Application.platform == RuntimePlatform.OSXWebPlayer)
        //    return "StreamingAssets/levels/" + filename;
        //if (Application.platform == RuntimePlatform.WindowsWebPlayer)
        //    return "StreamingAssets/levels/" + filename;
        //if (Application.platform == RuntimePlatform.OSXPlayer)
        //    return "file://" + Application.dataPath + "/Data/StreamingAssets/levels/" + filename;
        //return "file://" + Application.dataPath + "/StreamingAssets/levels/" + filename;
    }
    string GetStreamingAssetsPath()
    {
        string path;
        #if UNITY_EDITOR
            path = "file:" + Application.dataPath + "/StreamingAssets";
        #elif UNITY_ANDROID
            path = "jar:file://"+ Application.dataPath + "!/assets/";
        #elif UNITY_IOS
            path = "file:" + Application.dataPath + "/Raw";
        #else
            //Desktop (Mac OS or Windows)
            path = "file:"+ Application.dataPath + "/StreamingAssets";
        #endif

        return path;
    }

}
