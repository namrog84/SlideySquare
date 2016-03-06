
using System;

namespace Assets.Scripts.Gameplay
{
    [Serializable]
    public class Tile
    {
        public TileType type;
        public int TileID;


        public enum TileType
        {
            None = 0,

            //main elements
            Player = 1, Gold = 2, Goal = 3, Wall = 4, BlueBlock = 5, PurpleBlock = 6,

            // needs TileID
            OrangeWall = 7, OrangeButton = 8, TurnOnWall = 9, TurnOffWall = 10, Teleporter = 11, GreenButton = 12,

            //turning tiles
            TurnLeft = 13, TurnUp = 14, TurnRight = 15, TurnDown = 16,
            TurnLeftUp = 17, TurnRightUp = 18, TurnLeftDown = 19, TurnRightDown = 20
        };

        public Tile()
        {
            type = TileType.None;
            TileID = 0;
        }
        public Tile(TileType t, int id)
        {
            type = t;
            TileID = id;
        }

        
        public bool NeedsID()
        {
            if (type == TileType.Teleporter ||  // teleporter
                type == TileType.OrangeButton || type == TileType.OrangeWall ||  //toggle button/wall
                type == TileType.GreenButton || type == TileType.TurnOnWall || type == TileType.TurnOffWall)  //turn on/off wall
            {
                return true;
            }
            return false;
        }
    }
}
