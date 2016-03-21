using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public float s_baseOrthographicSize = 10;
    // Use this for initialization
    public float UnitsHigh = 10;


    public void AdjustViewHeight(float width, float height)
    {
        var HeightMaybe = Mathf.Round(height / 2)+1;
        if (HeightMaybe > width)
        {
            UnitsHigh = HeightMaybe;
        }
        else
        {
            UnitsHigh = Mathf.Min(height, width);
        }
        
        //UnitsHigh = height;
        AdjustView();
    }
    
    public void AdjustView()
    {
        s_baseOrthographicSize = UnitsHigh; ////Mathf.Round(UnitsHigh / 2) + 1.5f; //// (32.0f * UnitsHigh) / Screen.height;
        Camera.main.orthographicSize = s_baseOrthographicSize;
    }

    void Start()
    {
        //Camera.main.orthographicSize = 1f;
        //AdjustView();
    }

    // Update is called once per frame
    void Update () {

    }

}
