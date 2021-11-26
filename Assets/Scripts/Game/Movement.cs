using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody2D))]

public class Movement : MonoBehaviour
{
    //Sets the generic speed that an object can move at
    public float speed = 8.0f;
    //Creates a multipllier in case we need to change the speed of characters
    public float speedMultiplier = 1.0f;
    
    //variable to hold the layer so the charactes will collide with this layer
    public LayerMask obstacleLayer;

    public Rigidbody2D rigidbody { get; private set; }

    //Every varaible for direction and movement
    public Vector2 initialDirection;
    public Vector2 direction {get; private set;}
    public Vector2 nextDirection {get; private set;}
    public Vector3 startingPos {get; private set; }

    PhotonView view;

    //Gets the starting position of the player
    private void Awake()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.startingPos = this.transform.position;
    }

    private void Start()
    {
        view = GetComponent<PhotonView>();
        ResetState();
    }

    //This procedure sets all value back to default values
    public void ResetState()
    {
        this.speedMultiplier = 1.0f;
        this.direction = this.initialDirection;
        this.nextDirection = Vector2.zero;
        this.transform.position = this.startingPos;
        this.rigidbody.isKinematic = false;
        this.enabled = true;
    }

    //Checks which direction the player should be moving in
    private void Update()
    {
        if (view.IsMine)
        {
            if (this.nextDirection != Vector2.zero)
            {
                SetDirection(this.nextDirection);
            }
        }
    }

    //Updates the position of the character
    private void FixedUpdate()
    {
        if (view.IsMine)
        {
            Vector2 position = this.rigidbody.position;
            Vector2 translation = this.direction * this.speed * this.speedMultiplier * Time.fixedDeltaTime;
            this.rigidbody.MovePosition(position + translation);
            if (this.nextDirection != Vector2.zero)
            {
                SetDirection(this.nextDirection);
            }
        }
    }

    //Sets the direction if the player changes the direciton
    public void SetDirection(Vector2 direction, bool forced = false)
    {
        if(view.IsMine)
        {
            if (forced || !Occupied(direction))
            {
                this.direction = direction;
                this.nextDirection = Vector2.zero;
            }
            else
            {
                this.nextDirection = direction;
            }
        }
    }

    //Checks if the next space is able to be moved into using a raycast
    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.75f, 0.0f, direction, 1.5f, this.obstacleLayer);
        return hit.collider != null;
    }
}