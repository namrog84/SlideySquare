using UnityEngine;
using System.Collections;

public class RandomMoveRightPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<PlayerMove>().currentDirection = PlayerMove.Direction.right;
        EyeController.playerDirChanged = 10;
	}
	
	// Update is called once per frame
	void Update () {
	    if(gameObject.transform.position.x > 2)
        {
            transform.position = new Vector3(-20, transform.position.y-2, 0);
        }
        if (gameObject.transform.position.y < -12)
        {
            transform.position -= new Vector3(0, 20, 0);
        }
    }
}
