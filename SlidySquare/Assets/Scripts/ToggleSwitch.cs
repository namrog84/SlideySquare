using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToggleSwitch : MonoBehaviour {

    public enum Option { TurnOn, TurnOff, Both }

    public GameObject TheBits;
	public int ToggleID = 0;
	public Option mode;

    GameObject InstantiatedBits;

    void Start()
	{
        ToggleID = BasicGameObject.ToggleID++;
        ButtonController.toggleSwitches.Add(this);
        InstantiatedBits = Instantiate(TheBits);
        InstantiatedBits.gameObject.SetActive(false);
    }

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


