using System.Collections;
using UnityEngine;
//using System.Collections;


[ExecuteInEditMode]
public class ArrowPiece : MonoBehaviour
{

    public enum Dir { NORTH, EAST, SOUTH, WEST };

    public Dir direction = Dir.EAST;
    // Use this for initialization

    //[ExecuteInEditMode]
    void Start()
    {
        GetComponent<BasicGameObject>().TriggerPool += Triggered;

    }

    void Triggered(GameObject other)
    {
        StartCoroutine(PrepareForTurn(other));
    }

    bool activatedTurn = false;

    public AudioClip sound1;

    IEnumerator PrepareForTurn(GameObject other)
    {
        AudioSource.PlayClipAtPoint(sound1, transform.position);
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
    }

    static float teleportDelay = 0.10f;
    static float lastTeleport = 0;

    public void NewDirection(GameObject turningObject)
    {
        //too soon to teleport?
        if (Time.time - lastTeleport < teleportDelay)
        {
            return;
        }
        lastTeleport = Time.time;


        turningObject.transform.GetComponent<PlayerMove>().CenterOnTile();
        if (direction == Dir.EAST)
        {
            turningObject.transform.GetComponent<PlayerMove>().currentDirection = PlayerMove.Direction.right;
        }
        else if (direction == Dir.WEST)
        {
            turningObject.transform.GetComponent<PlayerMove>().currentDirection = PlayerMove.Direction.left;
        }
        else if (direction == Dir.NORTH)
        {
            turningObject.transform.GetComponent<PlayerMove>().currentDirection = PlayerMove.Direction.up;
        }
        else if (direction == Dir.SOUTH)
        {
            turningObject.transform.GetComponent<PlayerMove>().currentDirection = PlayerMove.Direction.down;
        }




    }


}
