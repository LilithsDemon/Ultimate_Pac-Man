using UnityEngine;

public class Ghost : MonoBehaviour
{
    public int points = 200;

    private void OnCollisionEnter2D(Collision2D other)
    {
        //If pacman collides with it then eat the pellet
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            FindObjectOfType<GameManager>().AddToScore(1000);
            FindObjectOfType<GameManager>().PacmanDeath();
        }
    }

}
