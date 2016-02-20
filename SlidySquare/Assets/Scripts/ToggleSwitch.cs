using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToggleSwitch : MonoBehaviour {

	public int ToggleID = 0;

	public enum Option { TurnOn, TurnOff, Both }
	public Option mode;

	void Start()
	{
        ToggleID = BasicGameObject.ToggleID++;
        ButtonController.toggleSwitches.Add(this);
        InstantiatedBits = Instantiate(TheBits);
        InstantiatedBits.gameObject.SetActive(false);
    }
    GameObject InstantiatedBits;
    



    public GameObject TheBits;

    public void Toggle()
	{

		if (mode == Option.TurnOn)
		{
            InstantiatedBits.gameObject.SetActive(true);
            var temp = InstantiatedBits;
            temp.transform.position = gameObject.transform.position;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
			gameObject.GetComponent<BoxCollider2D>().enabled = true;
		}
		else if (mode == Option.TurnOff)
		{
            InstantiatedBits.gameObject.SetActive(true);
            var temp = InstantiatedBits;
            temp.transform.position = gameObject.transform.position;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
			gameObject.GetComponent<BoxCollider2D>().enabled = false;

		}
		else if (mode == Option.Both)
		{
			if(gameObject.GetComponent<SpriteRenderer>().enabled)
			{
                InstantiatedBits.gameObject.SetActive(true);
                var temp = InstantiatedBits;
                temp.transform.position = gameObject.transform.position;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
				gameObject.GetComponent<BoxCollider2D>().enabled = false;
			}
			else
			{
                InstantiatedBits.gameObject.SetActive(true);
                var temp = InstantiatedBits;
                temp.transform.position = gameObject.transform.position;
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
				gameObject.GetComponent<BoxCollider2D>().enabled = true;

			}

		}
	}

}
