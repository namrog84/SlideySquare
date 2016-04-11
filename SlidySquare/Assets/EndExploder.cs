using UnityEngine;
using System.Collections;

public class EndExploder : MonoBehaviour {
    public GameObject derp;

	// Use this for initialization
	void Start () {
	    
	}

    public void kaboom()
    {
        derp.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
