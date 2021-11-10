using UnityEngine;

public class Passage : MonoBehaviour
{
    public Transform connection;
    //Gives a location of where the end of the
    //passage way is so you can teleport there

    //When an object collides with the passageway
    private void OnTriggerEnter2D(Collider2D other)
    {
        //get position of the end of the passageway
        Vector3 position = other.transform.position;
        position.x = this.connection.position.x;
        position.y = this.connection.position.y;

        //send object to the end of the passageway
        other.transform.position = position;
    }
}
