using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class CompleteLevel : MonoBehaviour {
	GUIStyle style;
    PlayerMove ThePlayer;
	// Use this for initialization

      //  Inters
    

    void Start () {

        
		style = new GUIStyle();
		style.fontSize = 100;
		style.normal.textColor = Color.green;
		gameObject.GetComponent<BasicGameObject>().TriggerPool += OnTriggered;
        sr = GetComponent<SpriteRenderer>();
        wasLocked = false;
        currentCount = 0;
    }

    private SpriteRenderer sr;
    private int currentCount = 0;
    private bool wasLocked = false;
    public void Update()
    {
        //Debug.Log(HitWall.HitWallCount);
        if (HitWall.HitWallCount > 0)
        {
            if (HitWall.HitWallCount != currentCount)
            {
                wasLocked = true;
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
        //Debug.Log(HitWall.HitWallCount);
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

    bool activatedWin = false;
    IEnumerator PrepareForWin(GameObject other)
    {
        activatedWin = false;
        while (!activatedWin)
        {
            if (other.GetComponent<PlayerMove>().currentDirection == PlayerMove.Direction.right)
            {

                if (transform.position.x - other.transform.position.x <= other.GetComponent<CharacterController2D>().skinWidth)
                {
                    activatedWin = true;
                }
            }
            else if (other.GetComponent<PlayerMove>().currentDirection == PlayerMove.Direction.left)
            {
                if (other.transform.position.x - transform.position.x <= other.GetComponent<CharacterController2D>().skinWidth)
                {
                    activatedWin = true;
                }
            }
            else if (other.GetComponent<PlayerMove>().currentDirection == PlayerMove.Direction.up)
            {
                if (transform.position.y - other.transform.position.y <= other.GetComponent<CharacterController2D>().skinWidth)
                {
                    activatedWin = true;
                }
            }
            else if (other.GetComponent<PlayerMove>().currentDirection == PlayerMove.Direction.down)
            {
                if (other.transform.position.y - gameObject.transform.position.y <= other.GetComponent<CharacterController2D>().skinWidth)
                {
                    activatedWin = true;
                }
            }
            yield return null;
        }

        other.GetComponent<PlayerMove>().CenterOnTile();
        other.GetComponent<PlayerMove>().isMoving = false;
        //other.GetComponent<PlayerMove>().canMove = false;

        //Debug.Log("hi " );

        StartCoroutine(YouWin());
        //delay for another teleport	
        yield return null;
    }

    public AudioClip nomnomnom;

    public Sprite LockSprite;
    public Sprite GreenDotSprite;
    public GameObject TheBits;

    IEnumerator YouWin()
	{
        if(PlayerPrefs.GetInt("CurrentLevel") >= PlayerPrefs.GetInt("BaseGameLevelsCompleted", -1))
        {
            PlayerPrefs.SetInt("BaseGameLevelsCompleted", PlayerPrefs.GetInt("CurrentLevel"));
        }
        
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
            //Application.LoadLevelAsync(0).
            PlayerPrefs.SetInt("CurrentLevel", 1 + PlayerPrefs.GetInt("CurrentLevel"));
            PlayerPrefs.Save();

            StartCoroutine(LoadOut());
            //Application.LoadLevel(4);
		}
	
		yield return null;

	}
    private IEnumerator LoadOut()
    {
        var fader = GameObject.Find("SceneFader");
        fader.GetComponent<SceneFadeInOut>().fadeDir *= -1;
        fader.GetComponent<SceneFadeInOut>().startTime = 0;
        yield return new WaitForEndOfFrame();
        fader.GetComponent<SceneFadeInOut>().FinishedFade += finished;
    }

    void finished()
    {
        Time.timeScale = 1;
#pragma warning disable 0618
        Application.LoadLevel(4);
#pragma warning restore 0618
    }


}
