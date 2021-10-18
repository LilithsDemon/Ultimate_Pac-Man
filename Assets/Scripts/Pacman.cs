using UnityEngine;

[RequireComponent(typeof(Movement))]

public class Pacman : MonoBehaviour
{

    public Movement movement {get; private set; }

    public GameObject directionImage;

    private void Awake()
    {
        this.movement = GetComponent<Movement>();
    }

    private void Update()
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

        float angle = Mathf.Atan2(this.movement.direction.y, this.movement.direction.x);
        this.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
        if (this.movement.nextDirection != Vector2.zero)
        {
            directionImage.GetComponent<Renderer>().enabled = true;
            float nextAngle = Mathf.Atan2(this.movement.nextDirection.y, this.movement.nextDirection.x);
            directionImage.transform.rotation = Quaternion.AngleAxis(nextAngle * Mathf.Rad2Deg, Vector3.forward);
        }
        else 
        {
            directionImage.GetComponent<Renderer>().enabled = false;
        }
    }
}
