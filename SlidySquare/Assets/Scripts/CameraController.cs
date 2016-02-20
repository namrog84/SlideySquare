using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    // Use this for initialization
    public float UnitsHigh = 10;
	void Start () {

        //Camera.main.orthographicSize = 1f;
        //AdjustView();
	}


    public void AdjustViewHeight(float height)
    {
        UnitsHigh = height;
        AdjustView();
    }
    public float s_baseOrthographicSize = 10;
    public void AdjustView()
    {
        //Debug.Log(UnitsHigh);
        //float s_baseOrthographicSize = Screen.height / 64.0f / (10f / UnitsHigh);
        
        s_baseOrthographicSize = UnitsHigh / 2 + 0.5f; //// (32.0f * UnitsHigh) / Screen.height;
        Camera.main.orthographicSize = s_baseOrthographicSize;
    }
	
	// Update is called once per frame
	void Update () {

    }

}
