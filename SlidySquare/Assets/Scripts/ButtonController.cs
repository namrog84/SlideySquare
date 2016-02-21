using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonController : MonoBehaviour {

	public int ToggleID = 0;
	public static List<ToggleSwitch> toggleSwitches;
	public bool singleUse = true;
	bool canPress = true;

	public Sprite deactivatedSprite;
    public AudioClip buttonSound;

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
        ToggleID = BasicGameObject.ToggleID++;
        canPress = true;
        //Debug.Log(gameObject.name);
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

        AudioSource.PlayClipAtPoint(buttonSound, transform.position);
		for (int i = 0; i < toggleSwitches.Count; i++)
		{
			if (toggleSwitches[i].ToggleID == ToggleID)
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
