using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

    public static bool oneExists = false;
	// Use this for initialization
	void Start ()
    {
        // no duplicates!
        if (oneExists)
        {
            Destroy(gameObject);
            return;

        }

        oneExists = true;
        DontDestroyOnLoad(gameObject);
	}
    //public AudioClip song1;
    //public AudioSource derp;

	
	// Update is called once per frame
	void Update () {
	
	}
}
