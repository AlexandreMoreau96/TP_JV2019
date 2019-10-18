using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager m_Gm;
    public GameObject m_Ball;

    private float m_Radius = 1.0f;

    // Update is called once per frame
    void Update()
    {

    }

    private bool DetectCollision()
    {
        return true;
    }

    public float sqrRadius()
    {
        return m_Radius * m_Radius;
    }
}
