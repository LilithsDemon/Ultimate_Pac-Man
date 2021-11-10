using UnityEngine;

public class Pellet : MonoBehaviour
{
    //How many points you want pellet to give
    //This value can be changed in the editor
    public int points = 10;

    //Fucntion to eat pellet that can be changed by
    //another class that inherits it
    protected virtual void Eat()
    {
        FindObjectOfType<GameManager>().PelletEaten(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //If pacman collides with it then eat the pellet
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            Eat();
        }
    }
}
