using UnityEngine;
using System.Collections;

public class SceneFadeInOut : MonoBehaviour {

    public float fadeSpeed = 0.5f;

    private bool sceneStarting = true;

    public float [,] gridStartTime;
    //public float[,] gridFadeRate;
    public float[,] alphas;
    // Use this for initialization
    float size = 1;
    int wid, hei;
    void Start () {
        size = Mathf.Min(Screen.width, Screen.height)/10.0f;
        wid = Mathf.RoundToInt(Screen.width / size) + 1;
        hei = Mathf.RoundToInt(Screen.height / size) + 1;
        gridStartTime = new float[wid, hei];
        //gridFadeRate= new float[wid, hei];
        alphas = new float[wid, hei];
        for (int i = 0; i < wid; i++)
        {
            for (int j = 0; j < hei; j++)
            {
                gridStartTime[i, j] = Random.Range(0.5f, 1.1f); //till start of fade
                alphas[i, j] = 1.0f;
                //gridFadeRate[i,j] =  // unused fade rate
            }
        }
        totalBlocks = hei * wid;
        //guiTexture.pixelInset = new Rect(0, 0, Screen.width, Screen.height);
    }
    public Texture2D textur;
    int drawDepth = 5;
    float alpha = 1.0f;
    public int fadeDir = -1;
    private int totalBlocks = 0;
    public float startTime = 0;
    int fullFadeCount = 0;
	void OnGUI()
    {
        if (startTime < 5) // no need for later? 
        {
            startTime += Time.deltaTime;
            fullFadeCount = 0;
            for (int i = 0; i < wid; i++)
            {
                for (int j = 0; j < hei; j++)
                {
                    if (startTime > gridStartTime[i, j]) // start fading?
                    {
                        alphas[i, j] += fadeDir * fadeSpeed * Time.deltaTime;
                        alphas[i, j] = Mathf.Clamp01(alphas[i, j]);
                        if(alphas[i,j] == 1)
                        {
                            fullFadeCount++;
                        }
                        else if(alphas[i,j] == 0)
                        {
                            fullFadeCount--;
                        }
                    }
                    GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alphas[i, j]);
                    GUI.DrawTexture(new Rect(i * size, j * size, size, size), textur);
                }
            }
        }
        
        if(fullFadeCount == totalBlocks)
        {
            if (FinishedFade != null)
            {
                FinishedFade();
            }
        }
        else if(fullFadeCount == -totalBlocks)
        {
            if(FinishedUnFade != null)
            {
                FinishedUnFade();
            }
        }

    }
    public delegate void Derp();
    public Derp FinishedFade;
    public Derp FinishedUnFade;

}
