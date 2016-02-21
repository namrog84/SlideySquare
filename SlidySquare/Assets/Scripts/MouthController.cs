using UnityEngine;
using System.Collections;
using System;

public class MouthController : MonoBehaviour {

	
	PlayerMove player;
	public float happiness = 10f;
	public float changeRate = 4f;

    CharacterController2D cc2d;
    float maxHappiness = 10f;
    float isWin = 0;

    private Vector3 temphappiness = new Vector3(0, 0, 0);


	void Start () {
		player = transform.parent.GetComponent<PlayerMove>();
        cc2d = transform.parent.GetComponent<CharacterController2D>();
    }

    void Update () {
        if (isWin > 0)
        {
            happiness += 3 * isWin * changeRate * Time.deltaTime;

            isWin *= 1.05f;
            updateHappiness();
            return;
        }
		if (cc2d.collisionState.hasCollision())
		{
			if(happiness > 0f)
				happiness += - 3 * changeRate * Time.deltaTime;
			happiness -= changeRate * Time.deltaTime;
		}
		if(player.isMoving)
		{
			happiness += 3*changeRate * Time.deltaTime;
		}
		else
		{

		}
		happiness = Mathf.Clamp(happiness, -10f, 10f);

		updateHappiness();
	}

    internal void NomNom()
    {
        isWin = 1.0f;
    }

    void updateHappiness()
	{

		if (happiness > 0.05f)
		{
            temphappiness.x = happiness / maxHappiness;
            temphappiness.y = happiness / maxHappiness;
            temphappiness.z = happiness / maxHappiness;
			transform.localScale = temphappiness;
		}
		else if (happiness < 0.05f)
		{
            temphappiness.x = -happiness / maxHappiness;
            temphappiness.y = happiness / maxHappiness;
            temphappiness.z = -happiness / maxHappiness;
            transform.localScale = temphappiness;// new Vector3(-happiness / maxHappiness, happiness / maxHappiness, -happiness / maxHappiness);
		}
		else
		{
            temphappiness.x = 0.5f;
            temphappiness.y = 0.5f;
            temphappiness.z = 0.5f;
            transform.localScale = temphappiness;
		}
	}

}


