using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Gameplay;

namespace LevelBuilderNameSpace
{
    public class LevelBuilderID : MonoBehaviour
    {
        public Tile.TileType ThisTile;
        public static LevelBuilderID currentSelected;

        //public static LevelBuilderID currentSelected;
        public static Tile.TileType selectedTile;

        public void ClickMe()
        {
            //if (LevelBuilder.currentSelected == TileType.Wall)
            {
                if (currentSelected != this && currentSelected != null)
                {
                    currentSelected.TurnOffBorder();
                }
                TurnOnBorder();
                selectedTile = ThisTile;
                currentSelected = this;
            }
            UpdateDetails();
        }
        public void UpdateDetails()
        {
            //None = 0,
            ////main elements
            //Player = 1, Gold = 2, Goal = 3, Wall = 4, BlueBlock = 5, PurpleBlock = 6,
            //// needs TileID
            //OrangeWall = 7, OrangeButton = 8, TurnOnWall = 9, TurnOffWall = 10, Teleporter = 11, GreenButton = 12,
            ////turning tiles
            //TurnLeft = 13, TurnUp = 14, TurnRight = 15, TurnDown = 16,
            //TurnLeftUp = 17, TurnRightUp = 18, TurnLeftDown = 19, TurnRightDown = 20

            string[] Details = new string[] {
                "Delete existing block",
                "Where the player starts",
                "Gold that can be picked up",
                "The final level goal",
                "A standard wall",
                "Countdowns to explode after hit",
                "Can be hit 3 times before explode",
                "Wall that can be turned on/off with button",
                "A button that can be toggled on/off",
                "A wall that can be enabled via button",
                "A wall that can be disabled via button",
                "Moves Slidey Square to other connected teleporters",
                "Single use button for green walls",
                "Makes Slidey Square go left",
                "Makes Slidey Square go up",
                "Makes Slidey Square go right",
                "Makes Slidey Square go down",
                "Makes Slidey Square turn in shown directions",
                "Makes Slidey Square turn in shown directions",
                "Makes Slidey Square turn in shown directions",
                "Makes Slidey Square turn in shown directions"
            };
            if(DescriptionText == null)
            {
                DescriptionText = GameObject.Find("DescriptionText");
            }
            var dIndex = (int)selectedTile;
            if (dIndex >= 0 || dIndex <= 20)
            {
                DescriptionText.GetComponent<Text>().text = Details[dIndex];
            }
            else
            {
                DescriptionText.GetComponent<Text>().text = "Description error";
            }

        }
        GameObject DescriptionText;

        public void TurnOnBorder()
        {
            GetComponent<Outline>().enabled = true;
        }
        public void TurnOffBorder()
        {
            GetComponent<Outline>().enabled = false;
        }

        public void OnGUI()
        {
           // GUI.skin.button.border;
        }

    }


}