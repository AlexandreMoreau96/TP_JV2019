using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] players;

    private GameObject player1;
    private GameObject player2;

    private bool player1Playing;

    private void Awake()
    {
        player1 = players[0];
        player2 = players[1];
    }

    // Start is called before the first frame update
    void Start()
    {
        //Le joueur 1 joue en premier
        player1Playing = true;
        player2.GetComponent<PlayerSetup>().DisableComponents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTurn()
    {
        if (player1Playing)
        {
            player1Playing = false;
            player2.GetComponent<PlayerSetup>().EnableComponents();
            player1.GetComponent<PlayerSetup>().DisableComponents();
        }
        else
        {
            player1Playing = true;
            player2.GetComponent<PlayerSetup>().DisableComponents();
            player1.GetComponent<PlayerSetup>().EnableComponents();
        }
    }
}
