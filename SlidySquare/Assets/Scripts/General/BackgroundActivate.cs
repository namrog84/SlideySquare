using UnityEngine;
using System.Collections;

public class BackgroundActivate : MonoBehaviour {


	// Use this for initialization
	void Start () {
        Debug.Log("IM ALIVE!");
		GetComponent<SpriteRenderer>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
