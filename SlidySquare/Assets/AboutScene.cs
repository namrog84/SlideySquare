using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AboutScene : MonoBehaviour {

    public GameObject TotalTime;
    public GameObject TimePlayed;
    public GameObject goldCoinText;
	void Start () {
        if(TotalTime != null)
        {
            TotalTime.GetComponent<Text>().text += Mathf.Round(PlayerPrefs.GetFloat("TotalTime", 0));
        }
        if (TimePlayed != null)
        {
            TimePlayed.GetComponent<Text>().text += PlayerPrefs.GetInt("TotalLevels", 0);
        }
        if (goldCoinText != null)
        {
            goldCoinText.GetComponent<Text>().text += PlayerPrefs.GetInt("Gold", 0);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
