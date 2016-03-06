using UnityEngine;
using System.Collections;

public class SwipeDetector : MonoBehaviour
{

    public static bool recentPurple = false;


    private const int mMessageWidth = 200;
    private const int mMessageHeight = 64;

    private readonly Vector2 mXAxis = new Vector2(1, 0);
    private readonly Vector2 mYAxis = new Vector2(0, 1);

    // The angle range for detecting swipe
    private const float mAngleRange = 30;

    // To recognize as swipe user should at lease swipe for this many pixels
    private const float mMinSwipeDist = 50.0f;

    // To recognize as a swipe the velocity of the swipe
    // should be at least mMinVelocity
    // Reduce or increase to control the swipe speed
    private const float mMinVelocity = 100.0f;

    private Vector2 mStartPosition;
    //private float mSwipeStartTime;
    private Vector2 swipeTemp = new Vector2(0, 0);

    void Start()
    {
        pm = GetComponent<PlayerMove>();
    }

    void Update()
    {
        // Mouse button down, possible chance for a swipe
        if (Input.GetMouseButtonDown(0))
        {
            swipeTemp.x = Input.mousePosition.x;
            swipeTemp.y = Input.mousePosition.y;
            // Record start time and position
            mStartPosition = swipeTemp;
            //mSwipeStartTime = Time.time;
            recentPurple = false;
        }
        
        // Mouse button up, possible chance for a swipe
        if (Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            if (recentPurple)
            {
                return;
            }
            //float deltaTime = Time.time - mSwipeStartTime;
            swipeTemp.x = Input.mousePosition.x;
            swipeTemp.y = Input.mousePosition.y;
            Vector2 endPosition = swipeTemp;
            Vector2 swipeVector = endPosition - mStartPosition;

            float velocity = swipeVector.magnitude;// / deltaTime;

            if (velocity > mMinVelocity &&
                swipeVector.magnitude > mMinSwipeDist)
            {
                // if the swipe has enough velocity and enough distance

                swipeVector.Normalize();

                float angleOfSwipe = Vector2.Dot(swipeVector, mXAxis);
                angleOfSwipe = Mathf.Acos(angleOfSwipe) * Mathf.Rad2Deg;

                // Detect left and right swipe
                if (angleOfSwipe < mAngleRange)
                {
                    OnSwipeRight();
                }
                else if ((180.0f - angleOfSwipe) < mAngleRange)
                {
                    OnSwipeLeft();
                }
                else
                {
                    // Detect top and bottom swipe
                    angleOfSwipe = Vector2.Dot(swipeVector, mYAxis);
                    angleOfSwipe = Mathf.Acos(angleOfSwipe) * Mathf.Rad2Deg;
                    if (angleOfSwipe < mAngleRange)
                    {
                        OnSwipeTop();
                    }
                    else if ((180.0f - angleOfSwipe) < mAngleRange)
                    {
                        OnSwipeBottom();
                    }
                    else
                    {
                     //   mMessageIndex = 0;
                    }
                }
            }
        }
    }

    // void OnGUI()
    //  {
    // Display the appropriate message
    //  GUI.Label(new Rect((Screen.width - mMessageWidth) / 2,
    //                     (Screen.height - mMessageHeight) / 2,
    //                     mMessageWidth, mMessageHeight),
    //           mMessage[mMessageIndex]);
    // }

    private PlayerMove pm;
    private void OnSwipeLeft()
    {
        pm.GoLeft();
       // mMessageIndex = 1;
    }

    private void OnSwipeRight()
    {
        pm.GoRight();
      // mMessageIndex = 2;
    }

    private void OnSwipeTop()
    {
        pm.GoUp();
       //mMessageIndex = 3;
    }

    private void OnSwipeBottom()
    {
        pm.GoDown();
      // mMessageIndex = 4;
    }

}


