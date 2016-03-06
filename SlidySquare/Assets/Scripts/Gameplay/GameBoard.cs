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
            Debug.Log(x + " " + y + " " + Board[key].type);
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
