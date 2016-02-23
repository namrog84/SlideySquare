using UnityEngine;
using System.Collections;

public class moveBackgroundController : MonoBehaviour {


    public float scale = 0.01f;

    Vector3 playerPosLastFrame;
    Transform player;
    bool firsttime = true;

    private Bounds cachedBounds;
    private Vector2 delta;


    void Start()
    {
        float x = 0;
        float y = 0;
        if (Random.value > 0.5f)
        {
            x = Random.Range(0.01f, 0.1f);
        }
        else
        {
            y = Random.Range(0.01f, 0.1f);
        }
        delta = new Vector2(x, y);
    }

    void Update ()
    {
        if(firsttime)
        {
            firsttime = false;
            cachedBounds = OrthographicBounds(Camera.main);
            cachedBounds.Expand(5);
            cachedBounds.center = new Vector3(cachedBounds.center.x, cachedBounds.center.y, 0);
        }
        if (!cachedBounds.Contains(transform.position))
        {
            delta.x = -delta.x;
            delta.y = -delta.y;
        }
     
        transform.Translate(1000 * scale * delta * Time.deltaTime);

    }
    public static Bounds OrthographicBounds(Camera camera)
    {
        var campcontroller = camera.GetComponent<CameraController>();
        if (campcontroller != null)
        {
            var cameracomp = camera.GetComponent<CameraController>().s_baseOrthographicSize;
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float cameraHeight = cameracomp * 2.5f;
            Bounds bounds = new Bounds(
                camera.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

            return bounds;
        }
        else
        {
            return new Bounds();
        }

    }


}


