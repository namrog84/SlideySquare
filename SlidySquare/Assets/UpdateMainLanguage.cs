using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using I2.Loc;

public class UpdateMainLanguage : MonoBehaviour {

    public Dropdown DropDownObject;
	// Use this for initialization
	void Start () {

        //DropDownObject.AddOptions(LocalizationManager.GetAllLanguages());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnValueChanged(int index)
    {

        //LocalizationManager.CurrentLanguage = LocalizationManager.GetAllLanguages()[index];
    }
}
