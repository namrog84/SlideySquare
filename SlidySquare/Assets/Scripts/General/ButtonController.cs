﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonController : MonoBehaviour {

	public int ButtonToggleID = 0;
	public static List<ToggleSwitch> toggleSwitches = new List<ToggleSwitch>();
	public bool singleUse = true;
	bool canPress = true;

	public Sprite deactivatedSprite;
    public AudioClip buttonSound;
    public bool playSound = true;
    void Awake()
	{
		if (toggleSwitches == null)
		{
			toggleSwitches = new List<ToggleSwitch>();
		}
	}

	// Use this for initialization
	void Start () 
	{
        canPress = true;
		GetComponent<BasicGameObject>().TriggerPool += Triggered;
	}

	void OnLevelWasLoaded(int level) 
	{
		if (toggleSwitches != null)
		{
			toggleSwitches.Clear();
		}
	}

	void Triggered(GameObject other)
	{
		if (!canPress)
		{
			return;
		}
        if (playSound)
        {
            AudioSource.PlayClipAtPoint(buttonSound, Camera.main.transform.position);
        }
		for (int i = 0; i < toggleSwitches.Count; i++)
		{
			if (toggleSwitches[i].ToggleID == ButtonToggleID)
			{
				toggleSwitches[i].Toggle();
			}
		}

		if (singleUse)
		{
			canPress = false;
			GetComponent<SpriteRenderer>().sprite = deactivatedSprite;
		}
	}
	
	void Update () {
	
	}
}
