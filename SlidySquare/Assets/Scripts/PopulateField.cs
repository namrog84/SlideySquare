using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using Assets.Scripts;

public class PopulateField : MonoBehaviour {

    public GameObject LevelItem;

	// Use this for initialization
	void Start() {

        var filenames = FileManager.GetSavedLevelNames(); 
        foreach (var file in filenames)
        {
            //Debug.Log(file);
            var item = Instantiate(LevelItem);
            item.transform.SetParent(gameObject.transform);
            item.GetComponentInChildren<Text>().text = file.TrimEnd('.');
            item.GetComponent<LevelGUISelector>().filename = file;

        }


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
