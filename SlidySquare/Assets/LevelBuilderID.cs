﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace LevelBuilderNameSpace
{
    public class LevelBuilderID : MonoBehaviour
    {
        public TileType ThisTile;
        public static LevelBuilderID currentSelected;
        public static TileType selectedTile;

        public void ClickMe()
        {
            //if (LevelBuilder.currentSelected == TileType.Wall)
            {
                if (currentSelected != this && currentSelected != null)
                {
                    currentSelected.TurnOffBorder();
                }
                TurnOnBorder();
                Debug.Log(ThisTile);
                selectedTile = ThisTile;
                currentSelected = this;
            }
        }
        
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