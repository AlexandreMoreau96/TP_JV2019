using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gm;
    public GameObject ball;

    // Update is called once per frame
    void Update()
    {
        //collision detection instead of a mouse press
        if (Input.GetButtonUp("Fire1"))
        {
            //Change the players turn
            gm.ChangeTurn();
        }

        if(DetectCollision())
        {

        }
    }

    private bool DetectCollision()
    {
        return true;
    }
}
