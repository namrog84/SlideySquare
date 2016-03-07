using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        BoardBank.LoadFromFile();
        Debug.Log(BoardBank.boards.Count);
        var board = BoardBank.boards[0];
        
        Debug.Log(board.width + " " + board.height);
        
        GetComponent<SpriteRenderer>().sprite = board.GetSprite();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
