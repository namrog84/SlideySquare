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
using System;
using Assets.Scripts.Gameplay;

public class LevelLoader : MonoBehaviour {

    public string levelname = "leveltest.tmx";
    public Vector3 offset;
    public Vector3 mapOffset;
    public GameObject[] tiles;

    GameObject[,] worldTiles;

    private int teleCount = 0;
    
    //width and height of level
    private int _height = 0;
    private int _width = 0;


    // Use this for initialization
    void Start () {
        Analytics.CustomEvent("LevelStart", new Dictionary<string, object>{ { "Level", PlayerPrefs.GetInt("CurrentLevel", -1) } });

        //HitWall.HitWallCount = GameObject.FindGameObjectsWithTag("HitWall").Length;
        offset = new Vector3(Camera.main.transform.position.x+0.5f, Camera.main.transform.position.y-0.5f, 0);
        
        if(GameCore.PlayingLevelState == GameCore.PlayingStates.Campaign)
        {
            var index = GameCore.campaignLevelNumber - 1;
            Debug.Log(index + " " + CampaignBank.boards.Count);
            if (index < CampaignBank.boards.Count && index >= 0)
            {
                GameCore.currentBoard = CampaignBank.boards[index];
            }
            if(GameCore.currentBoard == null)
            {
                BoardBank.LoadFromFile();
                GameCore.currentBoard = BoardBank.boards[0];
            }
        }
        if (GameCore.currentBoard == null)
        {
            BoardBank.LoadFromFile();
            // tall level, 6 wide, 16 high 
            GameCore.currentBoard = BoardBank.FindBoard("Grateful Trex");

            //8x8
            //GameCore.currentBoard = BoardBank.FindBoard("7 maybe");
        }


        // if(GameCore.PlayingLevelFromEditor)
        {
            StartCoroutine(LoadCurrentLevel());

        }

        //if(PlayerPrefs.GetInt("CurrentLevel", 1) == -3)
        //{
        //    //custom new level design
        //    var filename = PlayerPrefs.GetString("LevelName");
        //    Debug.Log(filename);
        //    Debug.Log(filename);
        //    StartCoroutine(LoadLevelNew(filename));
        //}
        //else
        //{
        //    Debug.Log("Old Format removed");
        //    FindObjectOfType<MySceneManager>().GoToIntroScene();
        //}

        if (AdManager.PlayCount == 4 || AdManager.Skipped)
        {
            AdManager.PlayAd();
        }
        else
        {
            AdManager.RequestInterstitial();
        }
        Time.timeScale = 1;

        StartCoroutine(PhoneHome());
    }
    public IEnumerator PhoneHome(){
        string url = CustomLevelManager.coreURL + "/api/event/"+SystemInfo.deviceUniqueIdentifier+"/startedLevel/" + GameCore.currentBoard.name+"/";
        url = url.Replace(" ", "_"); // stupid thing can't handle spaces correctly?
        Debug.Log(url);
        //what level and being played by what machine?
        WWW www = new WWW(url);
        yield return www;
        Debug.Log(www.text);
    }



    public IEnumerator LoadCurrentLevel()
    {
        yield return new WaitForEndOfFrame();

        GameBoard board = GameCore.currentBoard;

        if (teleportController.TeleportsList != null) //it has old teleporters in it?
        {
            teleportController.TeleportsList.Clear();
        }

        _width = board.width;
        _height = board.height;
        mapOffset = new Vector3(_width / 2, _height / 2, 0);
        worldTiles = new GameObject[_width, _height];
        Debug.Log(_width + " " + _height);
        Camera.main.GetComponent<CameraController>().AdjustViewHeight(_width, _height);//.UnitsHigh = _height;//(_width / 2)+1;
        if (_width % 2 == 1)
        {
            Camera.main.transform.position += new Vector3(0.5f, 0.0f);
        }
        //Debug.Log("Creating");
        //Debug.Log(_width + " " + _height);
        //scan tile data layer
        for (int j = 0; j < _height; j++)
        {
            //Debug.Log("Height!");
            for (int i = 0; i < _width; i++)
            {
                //Debug.Log(board.GetTile(i,j).type);

                CreateTile(i, _height - j, j, board.GetTile(i, j), "");
                //SetTileId(i, j, board.GetTile(i, j).TileID);
            }
        }

        HitWall.HitWallCount = GameObject.FindGameObjectsWithTag("HitWall").Length;


    }


    private IEnumerator LoadLevelNew(string filename)
    {
        //string fpath = Path.Combine(Application.streamingAssetsPath, filename);
        //string leveldata;
        yield return new WaitForEndOfFrame();

        GameBoard board = BoardBank.FindBoard(filename);

        if (board == null)
        {
            Debug.Log("ERROR");
            yield break;
        }

        GameCore.currentBoard = board;
        PlayerPrefs.SetInt("isSubmitted", Convert.ToInt32(board.isSubmitted));
        PlayerPrefs.Save();

        _width = board.width;
        _height = board.height;
        mapOffset = new Vector3(_width / 2, _height / 2, 0);
        worldTiles = new GameObject[_width, _height];
        //Debug.Log(_width + " " + _height);
        Camera.main.GetComponent<CameraController>().AdjustViewHeight(_width, _height);//.UnitsHigh = _height;//(_width / 2)+1;
        if(_width % 2 == 1)
        {
            Camera.main.transform.position += new Vector3(-0.5f, 0.0f);
        }
        

        //scan tile data layer
        for (int j = 0; j < _height; j++)
        {
            for (int i = 0; i < _width; i++)
            {
                //CreateTile(i, _height - j, j, board.GetTile(i,j).type, "");
                //SetTileId(i, j, board.GetTile(i,j).TileID); 
            }
        }

        HitWall.HitWallCount = GameObject.FindGameObjectsWithTag("HitWall").Length;
    }


    private void SetTileId(int x, int y, int id)
    {
        if (x < 0 || y < 0 || x >= _width || x >= _height)
        {
            return;
        }
        if (worldTiles[x, y] != null)
        {
            var tele1 = worldTiles[x, y].GetComponent<teleportController>();
            if (tele1 != null)
            {
                tele1.ID = id;
                tele1.GetComponentInChildren<slowRotater>().SetColor(teleCount);
                teleCount++;
            }

            var togglebutton = worldTiles[x, y].GetComponent<ButtonController>();
            if (togglebutton != null)
            {
                togglebutton.ButtonToggleID = id;
            }

            var togglewall = worldTiles[x, y].GetComponent<ToggleSwitch>();
            if (togglewall != null)
            {
                togglewall.ToggleID = id;
            }
        }
    }

  
    public List<int> TeleIDCount = new List<int>();
    private void ConnectTiles(int x, int y, int x2, int y2)
    {
        if (x < 0 || y < 0 || x >= _width || x >= _height)
        {
            //Debug.Log("?");
            return;
        }
        if (x2 < 0 || y2 < 0 || x2 >= _width || x2 >= _height)
        {
            return;
        }

        if (worldTiles[x, y] != null && worldTiles[x2, y2] != null)
        {
            var tele1 = worldTiles[x, y].GetComponent<teleportController>();
            var tele2 = worldTiles[x2, y2].GetComponent<teleportController>();
            if (tele1 != null && tele2 != null)
            {
                //add if it doesn't exist in list
                if(!TeleIDCount.Contains(tele1.ID))
                {
                    TeleIDCount.Add(tele1.ID);
                }
                //make sure they are synced
                tele2.ID = tele1.ID;

                //what color did we set it to last time? lets choose that again
                var teleColor = TeleIDCount.IndexOf(tele1.ID);
                tele1.GetComponentInChildren<slowRotater>().SetColor(teleColor);
                tele2.GetComponentInChildren<slowRotater>().SetColor(teleColor);
                
            }

            var togglebutton = worldTiles[x, y].GetComponent<ButtonController>();
            var togglewall = worldTiles[x2, y2].GetComponent<ToggleSwitch>();
            if (togglebutton != null && togglewall != null)
            {
                togglewall.ToggleID = togglebutton.ButtonToggleID;
            }
            
            

        }
        
    }
    
    private void CreateTile(int x, int y, int trueY, Tile tile, string name)
    {

        int tileIndex = (int)tile.type;
        if (tile.type == Tile.TileType.None || tileIndex == 0)
            return;

        if (tiles[tileIndex] == null)
        {
            return;
        }
    
        GameObject newTile = Instantiate(tiles[tileIndex]); //create tile
        worldTiles[x, trueY] = newTile;

        newTile.transform.position = new Vector3(x, y, 0) + offset - mapOffset; //set position
        newTile.transform.parent = gameObject.transform; //make child of this object

        var tele = newTile.GetComponent<teleportController>();
        if(tele != null)
        {
            tele.ID = tile.TileID;
        }
        var toggle1 = newTile.GetComponent<ToggleSwitch>();
        if (toggle1 != null)
        {
            toggle1.ToggleID = tile.TileID;
        }
        var button1 = newTile.GetComponent<ButtonController>();
        if (button1 != null)
        {
            button1.ButtonToggleID = tile.TileID;
        }


    }
    

    // Update is called once per frame
    void Update()
    {
    }


}


