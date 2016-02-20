using UnityEngine;
using System.Collections;

public class ExplodeCoin : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(boom());
        
	}
    IEnumerator boom()
    {
        yield return null; // new WaitForSeconds(1);
        
        AddExplosionForce(GetComponent<Rigidbody2D>(), 500, transform.root.transform.position, 10);
    }

    public static void AddExplosionForce(Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }

    float death = 1.5f;
	// Update is called once per frame
	void Update ()
    {
        death -= Time.deltaTime;
        if(death < 0)
        {
            Destroy(gameObject);
        }
        else if (death < 0.1f)
        {
            var scale = transform.localScale; //GetComponent<Transform>().localScale;
            scale *= .97f;  //vanish to smallness!
            transform.localScale = scale;  //GetComponent<Transform>().localScale = scale;
        }
        else if(death < 1)
        {
            var scale = transform.localScale; //GetComponent<Transform>().localScale;
            scale *= .99f;
            transform.localScale = scale;  //GetComponent<Transform>().localScale = scale;

        }



	
	}
}
