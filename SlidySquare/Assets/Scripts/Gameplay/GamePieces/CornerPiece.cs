﻿using System.Collections;
using UnityEngine;
//using System.Collections;


[ExecuteInEditMode]
public class CornerPiece : MonoBehaviour {

	public enum Dir {NORTH, EAST, SOUTH, WEST};

	public Dir vertDirection = Dir.NORTH;
    public Dir horDirection = Dir.WEST;
    public AudioClip sound1;
    
    bool iscoroutuinedStarted = false;
    bool activatedTurn = false;

    static float teleportDelay = 0.10f;
    static float lastTeleport = 0;

    //[ExecuteInEditMode]
    void Start () 
	{
        GetComponent<BasicGameObject>().TriggerPool += Triggered;

    }

    void Triggered(GameObject other)
    {
        if (!iscoroutuinedStarted)
        {
            iscoroutuinedStarted = true;
            StartCoroutine(PrepareForTurn(other));
        }
    }

    IEnumerator PrepareForTurn(GameObject other)
    {
        AudioSource.PlayClipAtPoint(sound1, Camera.main.transform.position);
        activatedTurn = false;
        while (!activatedTurn)
        {
            if (other.GetComponent<PlayerMove>().currentDirection == PlayerMove.Direction.right)
            {

                if (transform.position.x - other.transform.position.x <= other.GetComponent<CharacterController2D>().skinWidth)
                {
                    activatedTurn = true;
                }
            }
            else if (other.GetComponent<PlayerMove>().currentDirection == PlayerMove.Direction.left)
            {
                if (other.transform.position.x - transform.position.x <= other.GetComponent<CharacterController2D>().skinWidth)
                {
                    activatedTurn = true;
                }
            }
            else if (other.GetComponent<PlayerMove>().currentDirection == PlayerMove.Direction.up)
            {
                if (transform.position.y - other.transform.position.y <= other.GetComponent<CharacterController2D>().skinWidth)
                {
                    activatedTurn = true;
                }
            }
            else if (other.GetComponent<PlayerMove>().currentDirection == PlayerMove.Direction.down)
            {
                if (other.transform.position.y - gameObject.transform.position.y <= other.GetComponent<CharacterController2D>().skinWidth)
                {
                    activatedTurn = true;
                }
            }
            yield return null;
        }
        NewDirection(other.gameObject);
        other.GetComponent<PlayerMove>().isMoving = true;

        //delay for another teleport	
        yield return null;
        iscoroutuinedStarted = false;
    }

    public void NewDirection(GameObject turningObject)
    {
        //too soon to teleport?
        if (Time.time - lastTeleport < teleportDelay)
        {
            return;
        }
        lastTeleport = Time.time;

        var playerDir = turningObject.transform.GetComponent<PlayerMove>();
        if (playerDir.currentDirection == PlayerMove.Direction.left)
        {
            if (horDirection == Dir.EAST)
            {
                if (vertDirection == Dir.NORTH)
                {
                    playerDir.currentDirection = PlayerMove.Direction.up;
                }
                else
                {
                    playerDir.currentDirection = PlayerMove.Direction.down;
                }
            }
        }
        else if (playerDir.currentDirection == PlayerMove.Direction.right)
        {
            if (horDirection == Dir.WEST)
            {
                if (vertDirection == Dir.NORTH)
                {
                    playerDir.currentDirection = PlayerMove.Direction.up;
                }
                else
                {
                    playerDir.currentDirection = PlayerMove.Direction.down;
                }
            }
        }

        else if (playerDir.currentDirection == PlayerMove.Direction.up)
        {
            if (vertDirection == Dir.SOUTH)
            {
                if (horDirection == Dir.WEST)
                {
                    playerDir.currentDirection = PlayerMove.Direction.left;
                }
                else
                {
                    playerDir.currentDirection = PlayerMove.Direction.right;
                }
            }
        }
        else if (playerDir.currentDirection == PlayerMove.Direction.down)
        {
            if (vertDirection == Dir.NORTH)
            {
                if (horDirection == Dir.WEST)
                {
                    playerDir.currentDirection = PlayerMove.Direction.left;
                }
                else
                {
                    playerDir.currentDirection = PlayerMove.Direction.right;
                }
            }
        }

        turningObject.transform.GetComponent<PlayerMove>().CenterOnTile();
    }
}
