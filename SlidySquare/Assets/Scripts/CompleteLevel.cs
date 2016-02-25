using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using Assets.Scripts;
using System;

public class CompleteLevel : MonoBehaviour {
    public AudioClip nomnomnom;

    public Sprite LockSprite;
    public Sprite GreenDotSprite;
    public GameObject TheBits;

    GUIStyle style;
    PlayerMove ThePlayer;
    bool activatedWin = false;

    private SpriteRenderer sr;
    private int currentCount = 0;
    //private bool wasLocked = false;


    void Start () {
        
		style = new GUIStyle();
		style.fontSize = 100;
		style.normal.textColor = Color.green;
		gameObject.GetComponent<BasicGameObject>().TriggerPool += OnTriggered;
        sr = GetComponent<SpriteRenderer>();
        //wasLocked = false;
        currentCount = 0;
    }

    public void Update()
    {
        //Debug.Log(HitWall.HitWallCount);
        if (HitWall.HitWallCount > 0)
        {
            if (HitWall.HitWallCount != currentCount)
            {
                //wasLocked = true;
                currentCount = HitWall.HitWallCount;
                sr.sprite = LockSprite;
            }
        }
        else
        {
            if (HitWall.HitWallCount != currentCount)
            {
                var temp = Instantiate(TheBits);
                temp.transform.position = gameObject.transform.position;

                currentCount = HitWall.HitWallCount;
                sr.sprite = GreenDotSprite;
            }
        }
    }

	private void OnTriggered(GameObject other)
	{
        if(HitWall.HitWallCount != 0)
        {
            HitWall.Wiggle();
            return;
        }
        ThePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        PlayerPrefs.SetInt("Gold", ThePlayer.goldAmount);
        //ThePlayer.isMoving = false; // LastStoppedLocation = transform.position;
        //ThePlayer.canMove = false;
        ThePlayer.IAmWinning();
        StartCoroutine(PrepareForWin(ThePlayer.gameObject));
	}


    IEnumerator PrepareForWin(GameObject other)
    {
        activatedWin = false;
        var otherPlayer = other.GetComponent<PlayerMove>();
        var otherCC2D = other.GetComponent<CharacterController2D>();

        while (!activatedWin)
        {
            if (otherPlayer.currentDirection == PlayerMove.Direction.right)
            {

                if (transform.position.x - other.transform.position.x <= otherCC2D.skinWidth)
                {
                    activatedWin = true;
                }
            }
            else if (otherPlayer.currentDirection == PlayerMove.Direction.left)
            {
                if (other.transform.position.x - transform.position.x <= otherCC2D.skinWidth)
                {
                    activatedWin = true;
                }
            }
            else if (otherPlayer.currentDirection == PlayerMove.Direction.up)
            {
                if (transform.position.y - other.transform.position.y <= otherCC2D.skinWidth)
                {
                    activatedWin = true;
                }
            }
            else if (otherPlayer.currentDirection == PlayerMove.Direction.down)
            {
                if (other.transform.position.y - gameObject.transform.position.y <= otherCC2D.skinWidth)
                {
                    activatedWin = true;
                }
            }
            yield return null;
        }

        otherPlayer.CenterOnTile();
        otherPlayer.isMoving = false;

        StartCoroutine(YouWin());
        //delay for another teleport	
        yield return null;
    }


    IEnumerator YouWin()
	{
        if(PlayerPrefs.GetInt("CurrentLevel") >= PlayerPrefs.GetInt("BaseGameLevelsCompleted", -1))
        {
            PlayerPrefs.SetInt("BaseGameLevelsCompleted", PlayerPrefs.GetInt("CurrentLevel"));
        }
        PlayerPrefs.SetFloat("TotalTime", AdManager.TimePlayed);

        int totalCompleted = PlayerPrefs.GetInt("TotalLevels", 0);
        totalCompleted++;
        PlayerPrefs.SetInt("TotalLevels", totalCompleted);

        string history = Convert.ToBase64String(Common.CompressAndEncodeHistory(PlayerMove.HistoryMoves));
        PlayerPrefs.SetString("HistoryList", history);
        Debug.Log(history);
        PlayerPrefs.Save();
        AudioSource.PlayClipAtPoint(nomnomnom, transform.position);
        GameObject.FindGameObjectWithTag("Win").GetComponent<Text>().enabled = true;
        Handheld.Vibrate();

        
        

        yield return new WaitForSeconds(1.1f);

        //Application.LoadLevel("level " + (1 + 1 + PlayerPrefs.GetInt("CurrentLevel")));// int.Parse(Application.loadedLevelName.Split(new char[] { ' ' })[1]))); // Split()[1])))
        //var temp = Application.LoadLevelAsync("level " + (1 + int.Parse(Application.loadedLevelName.Split(new char[] { ' ' })[1]))); // Split()[1])))
        //yield return new WaitForEndOfFrame();
        //if(temp.progress == 0) // it hasn't started, likely because its not found

#pragma warning disable 0618
        if (!Application.isLoadingLevel)
		{
#pragma warning restore 0618
            PlayerPrefs.SetInt("CurrentLevel", 1 + PlayerPrefs.GetInt("CurrentLevel"));
            PlayerPrefs.Save();

            StartCoroutine(LoadOut());
		}
	
		yield return null;

	}
    private IEnumerator LoadOut()
    {
        var fader = GameObject.Find("SceneFader");
        fader.GetComponent<SceneFadeInOut>().fadeDir *= -1;
        fader.GetComponent<SceneFadeInOut>().startTime = 0;
        yield return new WaitForEndOfFrame();
        fader.GetComponent<SceneFadeInOut>().FinishedFade += LoadNextLevelOnFinished;
    }

    void LoadNextLevelOnFinished()
    {
        Time.timeScale = 1;
#pragma warning disable 0618
        Application.LoadLevel(4);
#pragma warning restore 0618
    }


}
