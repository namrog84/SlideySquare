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

    public class Rating
    {
        public int key;
        public string MachineId;   // will be generated once for a given device.
        public int LevelKey;   //what did they vote for
        public DateTime Date;  //when did they vote
        public float Vote;            //rating will be either be a thumbs up, meh, and thumbs down, similiar to Steam  (0, 0.5, 1) or something)


        public Rating()
        {
            MachineId = SystemInfo.deviceUniqueIdentifier;   // will be generated once for a given device.
            //InternalKey = Guid.Empty;   //what did they vote for
            Date = DateTime.UtcNow;  //when did they vote
            Vote = 0.0f;
        }
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
        //public Level(Map map): this()
        //{
        //    Data = "";// Common.ConvertEncode(map);
        //    Width = map.width;
        //    Height = map.height;
        //    Version = 1;
        //}
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
