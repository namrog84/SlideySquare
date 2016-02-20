using UnityEngine;
using System.Collections;

public class GenerateBackGroundSquares : MonoBehaviour {

    public bool horizontal = true;
    Transform[] children;
    // Use this for initialization
    float[] speeds = new float[100];
	void Start () {
        children = gameObject.GetComponentsInChildren<Transform>();
        for(int i = 0; i < 100; i++)
        {
            speeds[i] = Random.Range(0.5f, 5f);
        }
	}
	

    private Vector3 tempVector3 = new Vector3(0,0,0);
	// Update is called once per frame
	void Update ()
    {
        for (int i = 1; i < children.Length; i++)
        {
            var pos = children[i].position;

            if (horizontal)
            {
                tempVector3.x = -speeds[i] * Time.deltaTime;
                tempVector3.y = 0;
                tempVector3.z = 0;
                pos += tempVector3;

                if (pos.x < -30)
                {
                    pos += new Vector3(40, 0);
                }
            }
            else
            {
                tempVector3.x = 0;
                tempVector3.y = -speeds[i] * Time.deltaTime;
                tempVector3.z = 0;
                pos += tempVector3;
                if (pos.y < -8)
                {
                    pos += new Vector3(0,30);
                }
            }
            children[i].position = pos;
        }
	}
}
