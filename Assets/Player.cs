using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int distanceMaximum = 2;
    private int health = 100;
    public bool IsPlaying { get; set; }
    private PlayerSetup player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerSetup>();
        IsPlaying = false;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetHealth()
    {
        return health;
    }

    public void LoseHealth(int damage)
    {
        health -= damage;
    }

    public int GetDistanceMax()
    {
        return distanceMaximum;
    }
}
