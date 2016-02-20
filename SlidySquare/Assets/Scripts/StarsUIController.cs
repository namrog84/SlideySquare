using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StarsUIController : MonoBehaviour {

    int currentStars = 0;
	void Start () {
        if(PlayerPrefs.HasKey("TotalStars"))
        {
            currentStars = PlayerPrefs.GetInt("TotalStars");
        }
        else
        {
            currentStars = 0;
            PlayerPrefs.SetInt("TotalStars", currentStars);
            PlayerPrefs.Save();
        }
        GetComponent<Text>().text = "STARS : " + currentStars;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
