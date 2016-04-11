using UnityEngine;
using System.Collections;

public class SelfDestructController : MonoBehaviour {

    public AudioClip countdownSound;
    public AudioClip explodeSound;
    public GameObject TheBits;
	public Vector2 offset = new Vector2(-20, -12);
	public float timeLeft = 3.0f;

	private Rect labelPos;
	private GUIStyle style;
    private GameObject InstantiatedBits;
    private bool isCounting = false;
    private float startTime;


    void Start()
	{
        startTime = timeLeft;
        style = new GUIStyle();
		style.normal.textColor = Color.white;
		style.fontSize = 20;
		labelPos = new Rect(0, 0, 50, 50);
        GetComponent<BasicGameObject>().CollidePool += Ouch;
        InstantiatedBits = Instantiate(TheBits);
        InstantiatedBits.gameObject.SetActive(false);
    }
    
    void Update()
	{
	}

	void OnCollisionEnter2D(Collision2D other)
	{
        if (other.gameObject.tag == "Player")
        {
        }
	}

    private void Ouch(GameObject other)
    {
        if (isCounting)
        {
            return;
        }
        isCounting = true;
        StartCoroutine(CountdownTimer());
    }


	IEnumerator CountdownTimer()
	{
        AudioSource.PlayClipAtPoint(countdownSound, Camera.main.transform.position);

		while(timeLeft > 0)
		{
			yield return new WaitForSeconds(0.1f);
			timeLeft-=.1f;
		}
		timeLeft = 0.0f;
        AudioSource.PlayClipAtPoint(explodeSound, Camera.main.transform.position);
        InstantiatedBits.gameObject.SetActive(true);
        var temp = InstantiatedBits;
        temp.transform.position = gameObject.transform.position;
        Handheld.Vibrate();
        Destroy(gameObject);
	}

	void OnGUI()
	{
		labelPos.x = Camera.main.WorldToScreenPoint(gameObject.transform.position).x + offset.x;
		labelPos.y = Screen.height - Camera.main.WorldToScreenPoint(gameObject.transform.position).y + offset.y;

        if (startTime != timeLeft)
        {
            GUI.Label(labelPos, timeLeft.ToString("#0.0"), style);
        }
	}
}
