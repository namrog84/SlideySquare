using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class teleportController : MonoBehaviour {

	public static List<teleportController> TeleportsList;
    public AudioClip teleportSound;
    public int ID = 0;
    public GameObject TeleportExplosion;
    public GameObject TeleportImplosion;

    protected float cooldownTime = 0.0f;

    bool teleport = false;
    static float DefaultCooldownTime = 0.2f;

    //static float teleportDelay = 0.05f;
	//static float lastTeleport = 0;

    //public int x = -1, y = -1;
   // public void SetStart(int x, int y)
   // {
   //     this.x = x;
   //     this.y = y;
   // }



	void Start () {
		if (TeleportsList == null)
		{
			TeleportsList = new List<teleportController>();
		}
		TeleportsList.Add(this);
        teleport = false;
        GetComponent<BasicGameObject>().TriggerPool += Triggered;
        cooldownTime = 0.0f;
       // ID = BasicGameObject.ToggleID++;

    }

	void OnLevelWasLoaded(int level)
	{
		if (TeleportsList != null)
		{
			TeleportsList.Clear();
		}
	}

	
	// Update is called once per frame
	void Update () 
	{
	}

	void Triggered(GameObject other)
	{
        if (CanTeleport())
        {
            cooldownTime = DefaultCooldownTime;
            StartCoroutine(BeginTeleporting(other));
        }
	}

    private bool CanTeleport()
    {
        return cooldownTime == 0.0f;
    }

    IEnumerator BeginTeleporting(GameObject other)
	{
        teleport = false;
        while (!teleport)
		{
			if (other.GetComponent<PlayerMove>().currentDirection == PlayerMove.Direction.right)
			{
				if (transform.position.x - other.transform.position.x <= other.GetComponent<CharacterController2D>().skinWidth)
				{
					teleport = true;
				}
			}
			else if (other.GetComponent<PlayerMove>().currentDirection == PlayerMove.Direction.left)
			{
				if (other.transform.position.x - transform.position.x <= other.GetComponent<CharacterController2D>().skinWidth)
				{
					teleport = true;
				}
			}
			else if (other.GetComponent<PlayerMove>().currentDirection == PlayerMove.Direction.up)
			{
				if (transform.position.y - other.transform.position.y <= other.GetComponent<CharacterController2D>().skinWidth)
				{
					teleport = true;
				}
			}
			else if (other.GetComponent<PlayerMove>().currentDirection == PlayerMove.Direction.down)
			{
				if (other.transform.position.y - gameObject.transform.position.y <= other.GetComponent<CharacterController2D>().skinWidth)
				{
					teleport = true;
				}
			}
			yield return null;
		}

        // teleport = false;
        var derp = (GameObject)Instantiate(TeleportExplosion, transform.position, transform.rotation);
        Destroy(derp, 1.0f);
        //derp.GetComponent<ParticleSystem>.sim
        gameObject.GetComponent<teleportController>().Teleport(other.gameObject);
        AudioSource.PlayClipAtPoint(teleportSound, transform.position);
        other.GetComponent<PlayerMove>().isMoving = true;
		//delay for another teleport	
		yield return null;
	}


    public void Teleport(GameObject teleportingObject)
	{
        //lastTeleport = Time.time;
        var TeleportMatches = new List<teleportController>();
        for (int i = 0; i < TeleportsList.Count; i++)
		{
			if(TeleportsList[i].ID == this.ID && this != TeleportsList[i])
			{
                TeleportMatches.Add(TeleportsList[i]);
                //break;
			}

		}

        if(TeleportMatches.Count > 0)
        {
            //select 1 of the matches randomly. 
            var other = TeleportsList[UnityEngine.Random.Range(0, TeleportMatches.Count)];

            teleportingObject.transform.position = other.gameObject.transform.position;
            var derp2 = (GameObject)Instantiate(TeleportImplosion, other.transform.position, other.transform.rotation);
            Destroy(derp2, 1.0f);
            //these 2 are the teleporters,  they just activated, so lets set a cool down
            this.BeginTeleportCoolDown();
            other.BeginTeleportCoolDown();
        }

    }

    private void BeginTeleportCoolDown()
    {
        cooldownTime = DefaultCooldownTime;
        // if (teleportController.cooldownTime == 0.0f)
        {
            StartCoroutine(TeleportCoolDown());
        }
    }

    public IEnumerator TeleportCoolDown()
    {
        while (cooldownTime > 0)
        {
            cooldownTime -= UnityEngine.Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        cooldownTime = 0.0f;
    }


}


