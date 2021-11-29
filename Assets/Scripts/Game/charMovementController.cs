using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Movement))]

public class charMovementController : MonoBehaviour
{

    public Movement movement {get; private set; }
    private Touch theTouch;
    private Vector2 touchStartPosition;
    private Vector2 touchEndPosition;
    private Vector2 direction;


    public PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        this.movement = GetComponent<Movement>();
    }

    private void Update()
    {

        //Logic to get the right direction using keyboard
        if (view.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                this.movement.SetDirection(Vector2.up);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                this.movement.SetDirection(Vector2.down);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                this.movement.SetDirection(Vector2.left);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                this.movement.SetDirection(Vector2.right);
            }

            //Logic tog et direction using touch
            if (Input.touchCount > 0)
            {
                theTouch = Input.GetTouch(0);

                if (theTouch.phase == TouchPhase.Began)
                {
                    touchStartPosition = theTouch.position;
                }

                else if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended)
                {
                    touchEndPosition = theTouch.position;

                    float x = touchEndPosition.x - touchStartPosition.x;
                    float y = touchEndPosition.y - touchStartPosition.y;

                    if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
                    {
                        direction = direction;
                    }

                    else if (Mathf.Abs(x) > Mathf.Abs(y))
                    {
                        direction = x > 0 ? Vector2.right : Vector2.left;
                    }

                    else
                    {
                        direction = y > 0 ? Vector2.up : Vector2.down;
                    }

                    this.movement.SetDirection(direction);
                }
            }

            float angle = Mathf.Atan2(this.movement.direction.y, this.movement.direction.x);
            if(FindObjectOfType<PlayerManager>().charVal == 0)
            {
                this.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
            }
            if (this.movement.nextDirection != Vector2.zero)
            {
                float nextAngle = Mathf.Atan2(this.movement.nextDirection.y, this.movement.nextDirection.x);
                Quaternion rotation = Quaternion.AngleAxis(nextAngle * Mathf.Rad2Deg, Vector3.forward);
                FindObjectOfType<DirectionImage>().ChangeDirection(rotation);
            }
            else 
            {
                FindObjectOfType<DirectionImage>().TurnOffDirection();
            }
        }
    }
}