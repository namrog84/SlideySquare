using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class WebCommon
    {
    }

    public class Level
    {
        public int key; //SQL Key
        
        public string PublicName; //will generated server side for name like Gfycat has. (e.g. "PurpleElphantChair" )
        public string MachineId; // Owner Machine
        public string Passcode;   // TBD
        public DateTime Date;  // upload date
        public int Width;  //  map width
        public int Height;  // map height
        public int Downloads;  //how many downloads
        public int Plays;      //number of plays? 
        public float Score;   //cached calculated score, calculated elsewhere
        public int Version;   //for map data
        public string Data;    //map data
        public string Solution; // to verify solvable later
        public string SpecialFlag; // TBD

        public Level()
        {
            //InternalKey = new Guid();  // used to map to Rating
            PublicName = "none";  //will generate a public name like Gfycat has. (e.g. "PurpleElphantChair" )
            MachineId = SystemInfo.deviceUniqueIdentifier; // Owner
            Passcode = "";   // TBD
            Date = DateTime.UtcNow;// upload date time
            Width = 0;  //  map width
            Height = 0;  // map height
            Downloads = 0;  //how many downloads
            Plays = 0;           //cached data, calculated elsewhere via Analytics
            Score = 0.0f;   //cached calculated score, calculated elsewhere
            Version = 1;
            Data = "";    //map data
            Solution = ""; // to verify solvable later
            SpecialFlag = ""; // TBD
        }
        public Level(Map map): this()
        {
            Data = Common.ConvertEncode(map);
            Width = map.width;
            Height = map.height;
            Version = 1;
        }
    }
    //public class Level2
    //{
    //    public int key; //SQL Key

    //    public string PublicName; //will generated server side for name like Gfycat has. (e.g. "PurpleElphantChair" )
    //    public string MachineId; // Owner Machine
    //    public string Passcode;   // TBD
    //    public DateTime Date;  // upload date
    //    public int Width;  //  map width
    //    public int Height;  // map height
    //    public int Downloads;  //how many downloads
    //    public int Plays;      //number of plays? 
    //    public float Score;   //cached calculated score, calculated elsewhere
    //    public int Version;   //for map data
    //    public string Data;    //map data
    //    public string Solution; // to verify solvable later
    //    public string SpecialFlag; // TBD

    //    public Level2()
    //    {
    //        //InternalKey = new Guid();  // used to map to Rating
    //        PublicName = "none";  //will generate a public name like Gfycat has. (e.g. "PurpleElphantChair" )
    //        MachineId = SystemInfo.deviceUniqueIdentifier; // Owner
    //        Passcode = "";   // TBD
    //        Date = DateTime.UtcNow;// upload date time
    //        Width = 0;  //  map width
    //        Height = 0;  // map height
    //        Downloads = 0;  //how many downloads
    //        Plays = 0;           //cached data, calculated elsewhere via Analytics
    //        Score = 0.0f;   //cached calculated score, calculated elsewhere
    //        Version = 1;
    //        Data = "";    //map data
    //        Solution = null; // to verify solvable later
    //        SpecialFlag = ""; // TBD
    //    }
    //}
}
