using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class MainCampaignLevelButtonController : MonoBehaviour
{
    public static GameObject parentPanel;

	public int levelNumber = 0;
    public Color[] buttonColors;

    private static int LevelCompleted = 7;
    private MainCampaignLevelButtonController[] children;
    private Button MyButton = null; // assign in the editor
    
    void Start()
	{
        LevelCompleted = PlayerPrefs.GetInt("BaseGameLevelsCompleted", 1);
        MyButton = GetComponent<Button>();
		MyButton.onClick.AddListener(() => { MyOnClickFunction(); });
        TryModifyButtonColor();
	}

	private void MyOnClickFunction()
	{
        FindObjectOfType<MySceneManager>().LoadCampaignLevel(levelNumber);
		
	}

    public GameObject star;
    private bool doOnce = false;
    private void TryModifyButtonColor()
    {
        if (doOnce)
            return;
        doOnce = true;

        if(parentPanel == null)
        {
            parentPanel = GameObject.FindGameObjectWithTag("ButtonGroup");
            children = parentPanel.GetComponentsInChildren<MainCampaignLevelButtonController>();

            for (int i = 0; i < children.Length; i++)
            {
                children[i].levelNumber = i + 1;
                children[i].GetComponentInChildren<Text>().text = "" + (i+1);
                var tempColor = children[i].GetMyButton().colors;
                tempColor.normalColor = buttonColors[i % buttonColors.Length];
                tempColor.disabledColor = buttonColors[i % buttonColors.Length] * .3f;
                children[i].GetMyButton().colors = tempColor;
                if (i > LevelCompleted-1)
                {
                    children[i].GetMyButton().interactable = false;
                }

                //Debug.Log("ah " + PlayerPrefs.GetInt("" + children[i].levelNumber, 0));
                if (PlayerPrefs.GetInt("" + children[i].levelNumber, 0) == 1)
                {
                    children[i].GetComponent<MainCampaignLevelButtonController>().star.SetActive(true);
                }

            }
        }
    }

    private Button GetMyButton()
    {
        if(MyButton == null)
        {
            MyButton = GetComponent<Button>();
        }
        return MyButton;            
    }

}


