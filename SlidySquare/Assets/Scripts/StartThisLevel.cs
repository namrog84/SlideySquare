using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartThisLevel : MonoBehaviour {


	public int levelNumber = 0;
	//[SerializeField]
	private Button MyButton = null; // assign in the editor

    private static int LevelCompleted = 7;
	void Start()
	{
        LevelCompleted = PlayerPrefs.GetInt("BaseGameLevelsCompleted", 0);


        MyButton = GetComponent<Button>();
		MyButton.onClick.AddListener(() => { MyFunction(); });
		

        TryModifyButtonColor();
	}

	private void MyFunction()
	{
		if (levelNumber != 0)
		{
            PlayerPrefs.SetInt("CurrentLevel", levelNumber);
            PlayerPrefs.Save();
#pragma warning disable 0618
            Application.LoadLevel(4);// "level "+ levelNumber);
#pragma warning restore 0618
		}

	}


    public Color [] buttonColors;
    public static GameObject parentPanel;
    private StartThisLevel [] children;

    private void TryModifyButtonColor()
    {
        if(parentPanel == null)
        {
            parentPanel = GameObject.FindGameObjectWithTag("ButtonGroup");
            children = parentPanel.GetComponentsInChildren<StartThisLevel>();

            for (int i = 0; i < children.Length; i++)
            {
                children[i].levelNumber = i + 1;
                children[i].GetComponentInChildren<Text>().text = "" + (i+1);
                var tempColor = children[i].GetMyButton().colors;
                tempColor.normalColor = buttonColors[i % buttonColors.Length];
                tempColor.disabledColor = buttonColors[i % buttonColors.Length] * .3f;
                children[i].GetMyButton().colors = tempColor;
                if (i >= LevelCompleted + 1)
                {
                    children[i].GetMyButton().interactable = false;
                }
            }
        }
        

       // var panel
        

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
