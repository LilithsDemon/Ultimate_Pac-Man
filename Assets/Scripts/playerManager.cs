using UnityEngine;
using System;

public class playerManager : MonoBehaviour
{
    public byte val; /*Value of the character 0-3
    0 = pacman, 1-3 are different ghosts
    Using byte instead of int as byte contains 
    less data for movement over a network */

    public void set_char(byte n)
    {
        //Sets val to a specific value that it is 
        if (n < 4) {this.val = n;}
    }

    public void increment_char()
    {
        /*
        Should increment character by 1
        if the character goes to 4 then
        they go to 0 as character 4 does
        not exist
        */
        if ((this.val + 1) == 4)
        {
            this.val = 0;
        }
        else
        {
            this.val = Convert.ToByte(Convert.ToInt32(this.val) + 1);
        }
    }
}
