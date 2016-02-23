using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    class WebCommon
    {
    }

    public class Level
    {
        public int key;
        // used to map to Rating
        public string PublicName; //will generate a public name like Gfycat has. (e.g. "PurpleElphantChair" )
        public Guid MachineId; // Owner
        public Guid Passcode;   // TBD
        public DateTime Date;  // upload date
        public int Width;  //  map width
        public int Height;  // map height
        public int Downloads;  //how many downloads
        public int Plays;           //cached data, calculated elsewhere via Analytics
        public float Score;   //cached calculated score, calculated elsewhere
        public string Data;    //map data
        public string Solution; // to verify solvable later
        public string SpecialFlag; // TBD

        public Level()
        {

            //InternalKey = new Guid();  // used to map to Rating
            PublicName = "none";  //will generate a public name like Gfycat has. (e.g. "PurpleElphantChair" )
            MachineId = Guid.Empty; // Owner
            Passcode = Guid.Empty;   // TBD
            Date = DateTime.UtcNow;// upload date time
            Width = 0;  //  map width
            Height = 0;  // map height
            Downloads = 0;  //how many downloads
            Plays = 0;           //cached data, calculated elsewhere via Analytics
            Score = 0.0f;   //cached calculated score, calculated elsewhere
            Data = "";    //map data
            Solution = ""; // to verify solvable later
            SpecialFlag = ""; // TBD
        }
    }
}
