using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class LanguageController : MonoBehaviour {

    public GameObject backgroundPanel;
    public GameObject WindowOfChoices;

	// Use this for initialization
	void Start () {
        var lang = PlayerPrefs.GetString("Language", "English");
        I2.Loc.LocalizationManager.CurrentLanguage = lang;
        PlayerPrefs.SetString("Language", lang);
        PlayerPrefs.Save();
    }

    public void CancelLanguageSelect()
    {
        if (backgroundPanel == null)
        {
            backgroundPanel = GameObject.Find("backgroundPanel");
        }
        if (WindowOfChoices == null)
        {
            WindowOfChoices = GameObject.Find("LanguageSelect");
        }

        backgroundPanel.GetComponent<Image>().enabled = false;
        WindowOfChoices.transform.DOMove(WindowOfChoices.transform.position + new Vector3(0, -1500, 0), .3f).SetUpdate(true);
    }

    public void PopUpLanguageSelector()
    {
        if (backgroundPanel == null)
        {
            backgroundPanel = GameObject.Find("backgroundPanel");
        }
        if (WindowOfChoices == null)
        {
            WindowOfChoices = GameObject.Find("LanguageSelect");
        }

        backgroundPanel.GetComponent<Image>().enabled = true;
        WindowOfChoices.transform.DOMove(backgroundPanel.transform.position, .3f).SetUpdate(true);
    }

    public enum MyLanguages { Chinese, Dutch, English, French, German, Italian, Japanese, Korean, Portugese, Spanish };
    
    public void SetLanguage(MyLanguages lang)
    {
        I2.Loc.LocalizationManager.CurrentLanguage = lang.ToString();
        PlayerPrefs.SetString("Language", I2.Loc.LocalizationManager.CurrentLanguage);
        PlayerPrefs.Save();
        CancelLanguageSelect();
    }

    public void SetChinese()
    {
        SetLanguage(MyLanguages.Chinese);
    }
    public void SetDutch()
    {
        SetLanguage(MyLanguages.Dutch);
    }
    public void SetEnglish()
    {
        SetLanguage(MyLanguages.English);
    }
    public void SetFrench()
    {
        SetLanguage(MyLanguages.French);
    }
    public void SetGerman()
    {
        SetLanguage(MyLanguages.German);
    }
    public void SetItalian()
    {
        SetLanguage(MyLanguages.Italian);
    }
    public void SetJapanese()
    {
        SetLanguage(MyLanguages.Japanese);
    }
    public void SetKorean()
    {
        SetLanguage(MyLanguages.Korean);
    }
    public void SetPortugese()
    {
        SetLanguage(MyLanguages.Portugese);
    }
    public void SetSpanish()
    {
        SetLanguage(MyLanguages.Spanish);
    }


    // Update is called once per frame
    void Update () {
	
	}
}
