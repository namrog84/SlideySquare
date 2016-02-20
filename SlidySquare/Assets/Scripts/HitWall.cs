using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class HitWall : MonoBehaviour {
	private Rect labelPos;
	private GUIStyle style;
	public int hitsLeftCount = 3;

    private int startingHits;
    public static int HitWallCount = 0;
    public AudioClip explodeSound;
	// Use this for initialization
	void Start () {
        if(Walls == null)
        {
            Walls = new HashSet<GameObject>();
        }
        Walls.Add(gameObject);
        startingHits = hitsLeftCount;
        

		style = new GUIStyle();
		style.normal.textColor = Color.white;
		style.fontSize = 20;
		labelPos = new Rect(0, 0, 50, 50);
		GetComponent<BasicGameObject>().CollidePool += CollisionEnter2D;
        InstantiatedBits = Instantiate(TheBits);
        InstantiatedBits.gameObject.SetActive(false);
    }
    private GameObject InstantiatedBits;


    // Update is called once per frame
    void Update () 
	{
        TimeImpact += Time.deltaTime;
    }
    public GameObject TheBits;
    
	void CollisionEnter2D(GameObject other)
	{
        if(TimeImpact < 0.25)
        {
            return;
        }
		//Debug.Log("ouch");
		hitsLeftCount--;
        TimeImpact = 0;
        var pm = other.GetComponent<PlayerMove>();
        if (pm != null)
        {
            pm.tryTime = 0;
            pm.preventContinuedDirection();
        }
        SwipeDetector.recentPurple = true;

        if (hitsLeftCount == 0)
		{
           
            HitWallCount--;
            AudioSource.PlayClipAtPoint(explodeSound, transform.position);
            InstantiatedBits.gameObject.SetActive(true);
            var temp = InstantiatedBits;
            temp.transform.position = gameObject.transform.position;
            Walls.Remove(gameObject);
            Handheld.Vibrate();
            Destroy(gameObject);
		}
        
	}
    private float TimeImpact = 0;

	public Vector2 offset = new Vector2(-6, -12);
	void OnGUI()
	{
		labelPos.x = Camera.main.WorldToScreenPoint(gameObject.transform.position).x + offset.x;
		labelPos.y = Screen.height - Camera.main.WorldToScreenPoint(gameObject.transform.position).y + offset.y;

        if (hitsLeftCount != startingHits)
        {
            GUI.Label(labelPos, "" + hitsLeftCount, style);
        }
		

	}


    private static HashSet<GameObject> Walls;
    private static bool wiggling = false;

   
    internal static void Wiggle()
    {
        //Debug.Log("WIGGLE");
        if (!wiggling)
        {
            wiggling = true;

            foreach(var wall in Walls)
            {
                wall.GetComponent<HitWall>().wigglewiggle();
            }
        }
        
    }
    private void wigglewiggle()
    {
        //Debug.Log("woggle");
        StartCoroutine(Derp());
    }
    
    
    private IEnumerator Derp()
    {
        var child = gameObject.GetComponentsInChildren<Transform>()[1];
        //Debug.Log(GetComponentsInChildren<Transform>().Length);
        float elapsedTime = 0.0f;
        Quaternion start = child.localRotation;
        Quaternion target = Quaternion.Euler(new Vector3(0, 0, 180));
        while(elapsedTime <= 1.0f)
        {
            //Debug.Log("twisty");
            child.localRotation= Quaternion.Slerp(start, target, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //reset back
        child.localRotation = Quaternion.identity;

        wiggling = false;
    }
}
