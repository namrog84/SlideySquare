using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class Common
    {
    }

    public enum TileType
    {
        None = 0,
        Player = 1, Gold = 2, Goal = 3, Wall = 4, BlueBlock = 5, PurpleBlock = 6, OrangeWall = 7, OrangeButton = 8, TurnOnWall = 9, TurnOffWall = 10, Teleporter = 11, GreenButton = 12,
        TurnLeft = 13, TurnUp = 14, TurnRight = 15, TurnDown = 16,
        TurnLeftUp = 17, TurnRightUp = 18, TurnLeftDown = 19, TurnRightDown = 20
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
}
