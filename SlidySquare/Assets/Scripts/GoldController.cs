using UnityEngine;
using System.Collections;

public class GoldController : MonoBehaviour {

	public int Amount = 10;
    public GameObject TheBits;

    public AudioClip coinSound;
    private GameObject InstantiatedBits;


    void Start () {
        InstantiatedBits = Instantiate(TheBits);
        InstantiatedBits.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
        AudioSource.PlayClipAtPoint(coinSound, transform.position);
		GetComponent<SpriteRenderer>().enabled = false;
		if (Amount == 0)
        {
			return;
        }

        other.gameObject.GetComponent<PlayerMove>().AddMoney(Amount);
		StartCoroutine(DestroyMe());
	}

    IEnumerator DestroyMe()
	{
		Amount = 0;
        InstantiatedBits.gameObject.SetActive(true);
        var temp = InstantiatedBits;
        temp.transform.position = gameObject.transform.position;
		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);

	}
}

