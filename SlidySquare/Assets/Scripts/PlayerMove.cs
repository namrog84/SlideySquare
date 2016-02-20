using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//using System.Collections;

public class PlayerMove : MonoBehaviour
{

    GameObject startToken;

    public enum Direction { none, up, left, right, down };
    public Direction currentDirection = Direction.none;

    public float maxSpeed = 5;
    public float speed = 5;
    public bool isMoving = false;
    public float thresholdActivate = 0.02f;

    internal bool canMove = true;


    internal int goldAmount = 0;

    public delegate void CollidePool();
    public event CollidePool collidePool;

    public GameObject MyMouth;
    public AudioClip [] impactSound;


    internal void AddMoney(int amount)
    {
        goldAmount += amount;
        GameObject.FindGameObjectWithTag("GoldUI").GetComponent<Text>().text = ""+goldAmount;
        
    }


    void Start()
    {
        MyMouth = GetComponentInChildren<MouthController>().gameObject;
        GetComponent<CharacterController2D>().onTriggerEnterEvent += MyTriggerFunction;
        startToken = new GameObject();
        startToken.transform.position = transform.position;

        goldAmount = PlayerPrefs.GetInt("Gold");
        GameObject.FindGameObjectWithTag("GoldUI").GetComponent<Text>().text = "" + goldAmount;
        cc2d = GetComponent<CharacterController2D>();

        bounds = OrthographicBounds(Camera.main);
        bounds.Expand(2);
        bounds.center = new Vector3(bounds.center.x, bounds.center.y, 0);
    }

    private float RecentPurpleImpact = 0;
    internal void preventContinuedDirection()
    {
        RecentPurpleImpact = 0.15f;
    }

    private Bounds bounds;

    public static Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    void Update()
    {
       
        //CheatMode();
        CheckDeathBounds();
        CheckKeyboardControls();

        Move();

        if (isMoving && cc2d.collisionState.hasCollision())
        {
            if (isMoving)
            {
                if (Math.Abs((LastStoppedLocation - transform.position).magnitude) > 1)
                {
                    AudioSource.PlayClipAtPoint(impactSound[UnityEngine.Random.Range(0, 5)], transform.position);
                }
            }
            LastStoppedLocation = transform.position;
            isMoving = false;
        }
    }

    private Vector3 deltamove = new Vector3(0,0,0);
    private void Move()
    {
        tryTime -= Time.deltaTime;
        RecentPurpleImpact -= Time.deltaTime;
        
        if (!isMoving)
        {
            if (tryTime > 0)
            {
                isMoving = true;
                currentDirection = desiredMove;
                tryTime = 0;
            }
            else
            {
                speed = 0;
                return;
            }
        }
        if(speed == 0)
        {
            speed = maxSpeed*0.35f;
        }
        speed += 10.0f*Time.deltaTime;
        speed = Mathf.Min(speed, maxSpeed);
        deltamove.z = 0;
        deltamove.y = 0;
        deltamove.x = 0;
        if (currentDirection == Direction.up)
        {
            deltamove.y = speed * Time.deltaTime;   
            cc2d.move(deltamove);
        }
        else if (currentDirection == Direction.down)
        {
            deltamove.y = -speed * Time.deltaTime;
            cc2d.move(deltamove);
        }
        else if (currentDirection == Direction.left)
        {
            deltamove.x = -speed * Time.deltaTime;
            cc2d.move(deltamove);
        }
        else if (currentDirection == Direction.right)
        {
            deltamove.x = speed * Time.deltaTime;
            cc2d.move(deltamove);
        }
        
    }
    CharacterController2D cc2d;

    private void CheatMode()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (speed == 5)
            {
                speed = 10;
            }
            else
            {
                speed = 5;
            }
        }
    }

    private Vector3 LastStoppedLocation = new Vector3(0, 0, 0);
    private void CheckKeyboardControls()
    {
        if (!canMove)
        {
            return;
        }
        //yes you can move
        if (isMoving)
        {

            return;
        }
        CenterOnTile();
        //you are not currently moving;

        bool left = Input.GetKeyDown(KeyCode.A);
        bool up = Input.GetKeyDown(KeyCode.W);
        bool right = Input.GetKeyDown(KeyCode.D);
        bool down = Input.GetKeyDown(KeyCode.S);

        if (up)
        {
            GoUp();
        }
        else if (down)
        {
            GoDown();
        }
        if (left)
        {
            GoLeft();
        }
        else if (right)
        {
            GoRight();
        }
        EyeController.playerDirChanged = 2;

    }

    private void CheckDeathBounds()
    {

        if (!bounds.Contains(transform.position))
        {
            if (Dying)
                return;

            Dying = true;
            StartCoroutine(PlayDeathAndReset());
        }
    }
    public AudioClip[] deathSound;
    private IEnumerator PlayDeathAndReset()
    {
        AudioSource.PlayClipAtPoint(deathSound[UnityEngine.Random.Range(0, 2)], transform.position);
        yield return new WaitForSeconds(0.75f);
        StartCoroutine(LoadOut());
        //Application.LoadLevel(Application.loadedLevel);
    }

    private IEnumerator LoadOut()
    {
        var fader = GameObject.Find("SceneFader");
        fader.GetComponent<SceneFadeInOut>().fadeDir *= -1;
        fader.GetComponent<SceneFadeInOut>().startTime = 0;
        yield return new WaitForEndOfFrame();
        fader.GetComponent<SceneFadeInOut>().FinishedFade += finished;
    }

    void finished()
    {
        Time.timeScale = 1;
        Application.LoadLevel(Application.loadedLevel);
    }


    bool Dying = false;

    public float tryTime = 0;
    private float attemptTryTime = 0.5f;
    private Direction desiredMove = Direction.none;
    public void GoUp()
    {
        if (RecentPurpleImpact > 0)// && currentDirection == desiredMove)
        {
            return;
        }
        if (!canMove)
        {
            return;
        }
        if (isMoving)
        {
            if (currentDirection != Direction.up)
            {
                desiredMove = Direction.up;
                tryTime = attemptTryTime;
            }
            return;
        }

        isMoving = true;
        currentDirection = Direction.up;
    }
    public void GoDown()
    {
        if (RecentPurpleImpact > 0)// && currentDirection == desiredMove)
        {
            return;
        }
        if (!canMove)
        {
            return;
        }
        if (isMoving)
        {
            if (currentDirection != Direction.down)
            {
                desiredMove = Direction.down;
                tryTime = attemptTryTime;
            }
            return;
        }
        isMoving = true;
        currentDirection = Direction.down;

    }
    public void GoRight()
    {
        if (RecentPurpleImpact > 0)// && currentDirection == desiredMove)
        {
            return;
        }
        if (!canMove)
        {
            return;
        }
        if (isMoving)
        {
            if (currentDirection != Direction.right)
            {
                desiredMove = Direction.right;
                tryTime = attemptTryTime;
            }
            return;
        }
        isMoving = true;
        currentDirection = Direction.right;
    }
    public void GoLeft()
    {
        if (RecentPurpleImpact > 0)// && currentDirection == desiredMove)
        {
            return;
        }
        if (!canMove)
        {
            return;
        }
        if (isMoving)
        {
            if (currentDirection != Direction.left)
            {
                desiredMove = Direction.left;
                tryTime = attemptTryTime;
            }
            return;
        }
        isMoving = true;
        currentDirection = Direction.left;
    }

    public void CenterOnTile()
    {
        Vector3 current = transform.position;
        current.x = Mathf.Round(current.x);
        current.y = Mathf.Round(current.y);
        transform.position = current;
    }



    void SnapToX()
    {
        Vector3 current = transform.position;
        current.x = Mathf.Round(current.x);
        transform.position = current;
    }
    void SnapToY()
    {
        Vector3 current = transform.position;
        current.y = Mathf.Round(current.y);
        transform.position = current;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            
    
            isMoving = false;
        }
        collidePool();
    }

    private void MyTriggerFunction(Collider2D other)
    {

    }


    public void IAmWinning()
    {
        canMove = false;
        GetComponentInChildren<MouthController>().NomNom();
    }
   




    //private void LaunchNewDirection(CornerPiece cornerPiece)
    //{
    //	if (cornerPiece == null)
    //		return;

    //	isMoving = true;

    //	if(cornerPiece.direction == CornerPiece.Dir.NORTH)
    //	{
    //		SnapToX();
    //		GetComponent<CharacterController>().Move(new Vector3(0, speed * Time.deltaTime));
    //	}
    //	else if (cornerPiece.direction == CornerPiece.Dir.SOUTH)
    //	{
    //		SnapToX();
    //		myRigidbody.AddForce(new Vector2(0, -speed), ForceMode2D.Impulse);
    //	}
    //	else if(cornerPiece.direction == CornerPiece.Dir.EAST)
    //	{
    //		SnapToY();
    //		myRigidbody.AddForce(new Vector2(speed, 0), ForceMode2D.Impulse);
    //	}
    //	else if (cornerPiece.direction == CornerPiece.Dir.WEST)
    //	{
    //		SnapToY();
    //		myRigidbody.AddForce(new Vector2(-speed, 0), ForceMode2D.Impulse);
    //	}
    //}






}

