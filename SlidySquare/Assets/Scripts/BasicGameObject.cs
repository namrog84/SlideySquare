using UnityEngine;
using System.Collections;

public class BasicGameObject : MonoBehaviour {

	public delegate void CollideListener(GameObject other);
	public event CollideListener CollidePool;

	public delegate void TriggerListener(GameObject other);
	public event TriggerListener TriggerPool;

    public static int ToggleID = 0;


	public void Collide(GameObject other)
	{
		CollidePool(other);
	}

	public void Trigger(GameObject other)
	{
		if(TriggerPool != null)
			TriggerPool(other);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
