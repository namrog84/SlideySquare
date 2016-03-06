using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    
    public class GameBoard
    {
        public Texture2D thumbnail;
        public GameBoard()
        {
            //setup default thumbnail
            thumbnail = new Texture2D(16, 16, TextureFormat.ARGB32, false);
            thumbnail.filterMode = FilterMode.Point;
            generateRandomTexture(thumbnail);
            thumbnail.Apply();
        }

        private void generateRandomTexture(Texture2D texture)
        {
            int tiles = 16;

            var textureData = new Color[tiles, tiles];

            for (int i = 0; i < tiles; i++)
            {
                for (int j = 0; j < tiles; j++)
                {
                    thumbnail.SetPixel(i, j, Random.ColorHSV());
                    
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
            int key = (x << 16) & y;
            if (Board.ContainsKey(key))
            {
                Board[key] = tile;
            }
            else
            {
                Board.Add(key, tile);
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
            int key = (x << 16) & y;
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
