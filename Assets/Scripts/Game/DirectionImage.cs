using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionImage : MonoBehaviour
{
    public GameObject directionImage;

    public void ChangeDirection(Quaternion angle)
    {
        directionImage.GetComponent<Renderer>().enabled = true;
        directionImage.transform.rotation = angle;
    }

    public void TurnOffDirection()
    {
        directionImage.GetComponent<Renderer>().enabled = false;
    }

}
