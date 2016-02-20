﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestAd : MonoBehaviour {

    private Button MyButton = null; // assign in the editor

    void Start()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(() => { MyFunction(); });
    }

    private void MyFunction()
    {
        //pplication.LoadLevel("Level Select");
        AdManager.PlayAd();
    }
}