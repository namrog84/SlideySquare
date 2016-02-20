﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerMove))]
public class EyeController : MonoBehaviour {



	SpriteRenderer sr;
	void Start () {
		sr = GetComponent<SpriteRenderer>();
		eyeStart = sr.sprite;
		StartCoroutine(blink());
        playermove = transform.parent.GetComponent<PlayerMove>();
    }
    PlayerMove playermove;
	public Sprite eyeClosed;
	private Sprite eyeStart;
	IEnumerator blink()
	{
		while(true)
		{
			yield return new WaitForSeconds(3.0f);
			sr.sprite = eyeClosed;
			yield return new WaitForSeconds(0.2f);
			sr.sprite = eyeStart;
		}

		//yield return null;

	}
    public void updateEyes()
    {
        if (playermove.currentDirection == PlayerMove.Direction.left)
        {
            transform.localRotation = left;
        }
        else if (playermove.currentDirection == PlayerMove.Direction.down)
        {
            transform.localRotation = down;
        }
        else if (playermove.currentDirection == PlayerMove.Direction.right)
        {
            transform.localRotation = right;
        }
        else if (playermove.currentDirection == PlayerMove.Direction.up)
        {
            transform.localRotation = up;
        }
    }

    private static Quaternion left = Quaternion.Euler(0, 0, 90);
    private static Quaternion down = Quaternion.Euler(0, 0, 180);
    private static Quaternion right = Quaternion.Euler(0, 0, 270);
    private static Quaternion up = Quaternion.Euler(0, 0, 0);

    public static int playerDirChanged = 0;
    void Update () {

        if (playerDirChanged != 0)
        {
            playerDirChanged--;
            updateEyes();
        }
	}
}
