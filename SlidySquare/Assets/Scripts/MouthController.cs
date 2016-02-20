using UnityEngine;
using System.Collections;
using System;

public class MouthController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		player = transform.parent.GetComponent<PlayerMove>();
        cc2d = transform.parent.GetComponent<CharacterController2D>();
    }
    CharacterController2D cc2d;

	
	PlayerMove player;
	public float happiness = 10f;

	float maxHappiness = 10f;
	public float changeRate = 4f;

    
	// Update is called once per frame
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
    float isWin = 0;
    internal void NomNom()
    {
        isWin = 1.0f;
    }


    private Vector3 temphappiness = new Vector3(0,0,0);
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
