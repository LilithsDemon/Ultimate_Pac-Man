using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    public byte charVal {get; private set;} /*Value of the character 0-3
    0 = pacman, 1-3 are different ghosts
    Using byte instead of int as byte contains 
    less data for movement over a network */

    public void SetChar(byte n)
    {
        //Sets charVal to a specific value that it is 
        if (n < 4) {this.charVal = n;}
    }

    public void IncrementChar()
    {
        /*
        Should increment character by 1
        if the character goes to 4 then
        they go to 0 as character 4 does
        not exist
        */
        if ((this.charVal + 1) == 4)
        {
            this.charVal = 0;
        }
        else
        {
            this.charVal = Convert.ToByte(Convert.ToInt32(this.charVal) + 1);
        }
    }
}