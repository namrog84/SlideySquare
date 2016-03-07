using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    
    [System.Serializable]
    public class GameBoard
    {
        public byte[] pngImage;
        public GameBoard()
        {
            CreateRandomThumbnail();
        }

        public Sprite GetSprite()
        {
            //UpdateThumbnail();
            var tex = new Texture2D(4*width, 4 * height);
            tex.filterMode = FilterMode.Point;
            tex.LoadImage(pngImage);
            return Sprite.Create(tex, new Rect(0, 0, width, height), Vector2.zero);
        }

        public void UpdateThumbnail()
        {

            Texture2D thumbnail = new Texture2D(width, height, TextureFormat.ARGB32, false);
            thumbnail.filterMode = FilterMode.Bilinear;

            for (int i = 0; i < width; i++)
            {
                for (int k = 0; k < height; k++)
                {
                    var type = GetTile(i, k).type;
                    int j = height - k - 1;
                    if (type == Tile.TileType.Player)
                    {
                        thumbnail.SetPixel(i, j, Common.HexToColor("E60C0BFF"));
                    }
                    else if (type == Tile.TileType.Wall)
                    {
                        thumbnail.SetPixel(i, j, Common.HexToColor("94A5A5FF"));
                    }
                    else if (type == Tile.TileType.OrangeWall)
                            thumbnail.SetPixel(i, j, Common.HexToColor("E77D21FF"));
                    else if (type == Tile.TileType.PurpleBlock)
                            thumbnail.SetPixel(i, j, Common.HexToColor("8F44ADFF"));
                    else if (type == Tile.TileType.TurnOnWall)
                            thumbnail.SetPixel(i, j, Common.HexToColor("16A184FF"));
                    else if (type == Tile.TileType.Teleporter)
                            thumbnail.SetPixel(i, j, Common.HexToColor("3499DCFF"));
                    else if (type == Tile.TileType.BlueBlock)
                            thumbnail.SetPixel(i, j, Common.HexToColor("2980BBFF"));
                    else if (type == Tile.TileType.Goal)
                            thumbnail.SetPixel(i, j, Common.HexToColor("26AE60FF"));
                    else if (type == Tile.TileType.TurnDown || type == Tile.TileType.TurnLeft || type == Tile.TileType.TurnRight || type == Tile.TileType.TurnUp
                    || type == Tile.TileType.TurnLeftDown || type == Tile.TileType.TurnLeftUp
                    || type == Tile.TileType.TurnRightUp || type == Tile.TileType.TurnRightDown)
                    {
                        thumbnail.SetPixel(i, j, Common.HexToColor("E74C3CFF"));
                    }
                    else if (type == Tile.TileType.Gold)
                            thumbnail.SetPixel(i, j, Common.HexToColor("F2C410FF"));
                    else if (type == Tile.TileType.None)
                    {

                        thumbnail.SetPixel(i, j, new Color(0, 0, 0, 0));
                    }
                    else
                    {
                        thumbnail.SetPixel(i, j, new Color(0,0,0,0));
                    }
                }
            }

            thumbnail.Apply();
            pngImage = thumbnail.EncodeToPNG();

        }


        private void CreateRandomThumbnail()
        {
            //setup default thumbnail
            Texture2D thumbnail = new Texture2D(16, 16, TextureFormat.ARGB32, false);
            thumbnail.filterMode = FilterMode.Point;
            generateRandomTexture(thumbnail);
            thumbnail.Apply();

            pngImage = thumbnail.EncodeToPNG();
        }

        private void generateRandomTexture(Texture2D texture)
        {
            int tiles = 16;

            var textureData = new Color[tiles, tiles];

            for (int i = 0; i < tiles; i++)
            {
                for (int j = 0; j < tiles; j++)
                {
                    texture.SetPixel(i, j, UnityEngine.Random.ColorHSV());
                }
            }
            
        }
        

        //Required
        public int width; // map width
        public int height; // map height 
        public string name; // ??  saves submitted name?
        public bool isSubmitted; //  has this been submitted already?

        //public int onlineKey = 25; // used for voting

        private Dictionary<int, Tile> Board = new Dictionary<int, Tile>();

        //Set the Tile
        internal void SetTile(int x, int y, Tile tile)
        {

            //don't add nulls
            if (tile == null)
            {
                return;
            }

            int key = (x << 16) | y;
            if (Board.ContainsKey(key))
            {
                Board[key] = tile;
            }
            else
            {
                Board.Add(key, tile);
            }

            //oops, didn't want to add these!
            if(tile.type == Tile.TileType.None)
            {
                Board.Remove(key);
            }
        }
        internal void SetTile(Vector2 location, Tile t)
        {
           SetTile((int)location.x, (int)location.y, t);
        }

        public List<int> GetSortedIDs()
        {
            return Board.Select(x => x.Value.TileID).OrderByDescending(y => y).ToList();
        }
        public Tile GetTile(int x, int y)
        {
            int key = (x << 16) | y;
            if (!Board.ContainsKey(key))
            {
                return new Tile(); //empty tile
            }
            return Board[key];
        }

      

        public Tile GetTile(Vector2 location)
        {
            return GetTile((int)location.x, (int)location.y);
        }

    }




    //[Serializable]
    //public class Map
    //{
    //    //Required
    //    public int width; // map width
    //    public int height; // map height 
    //    public List<byte> Board = new List<byte>(); //Full Board
    //    public List<byte> IDsBoard = new List<byte>(); //Full Board ID  (For teleporters and buttons)

    //    //optional
    //    public string name; // ??  saves submitted name?
    //    public bool isSubmitted; //  has this been submitted already?
    //    public int onlineKey = 25; // used for voting
    //}


}
