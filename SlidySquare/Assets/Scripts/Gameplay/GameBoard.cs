using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Gameplay
{
    
    public class GameBoard
    {
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
