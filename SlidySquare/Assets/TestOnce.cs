using UnityEngine;
using System.Collections;
using Assets.Scripts.Gameplay;

public class TestOnce : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var board = new GameBoard();
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(board.thumbnail, new Rect(0,0,16,16), Vector2.zero);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
