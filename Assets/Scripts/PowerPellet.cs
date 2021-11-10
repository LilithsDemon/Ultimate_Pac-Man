using UnityEngine;

public class PowerPellet : Pellet
{
    //Time of powerPellet effect
    public float duration = 8.0f;

    //Overrides Pellet Eat() to use a different function
    protected override void Eat()
    {
        FindObjectOfType<GameManager>().PowerPelletEaten(this);
    }
}
