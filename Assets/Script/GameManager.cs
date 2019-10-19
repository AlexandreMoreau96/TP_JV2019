using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] players;

    [SerializeField]
    private GameObject m_GameUI;

    private GameObject player1;
    private GameObject player2;

    private static int m_PointPlayer1;
    private static int m_PointPlayer2;

    private bool player1Playing;

    public Camera m_CinematicCamera;

    private bool m_SwitchingTurn = false;

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
        m_PointPlayer1 = 0;
        m_PointPlayer2 = 0;
        player2.GetComponent<PlayerSetup>().DisableComponents();
        player1.GetComponent<ThrowSimulation>().ThrowProjectile();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameObject.FindGameObjectsWithTag("Ball").Length == 0 && !m_SwitchingTurn)
        {
            if (GameOver())
            {
                EndGame();
            }
            else
            {
                ChangeTurn();
                m_SwitchingTurn = true;
            }
        }

        if (m_SwitchingTurn && !m_CinematicCamera.GetComponent<CinematicCamera>().m_IsMoving)
        {
            if (player1Playing)
            {
                player1.GetComponent<ThrowSimulation>().ThrowProjectile();
            }
            else
            {
                player2.GetComponent<ThrowSimulation>().ThrowProjectile();
            }

            m_SwitchingTurn = false;
        }
    }

    private void EndGame()
    {
        throw new NotImplementedException();
    }

    private bool GameOver()
    {
        return m_PointPlayer1 == 2 || m_PointPlayer2 == 2;
    }

    public void ChangeTurn()
    {
        player1Playing = !player1Playing;

        if (player1Playing)
        {
            player2.GetComponent<PlayerSetup>().DisableComponents();
            player1.GetComponent<PlayerSetup>().EnableComponents();

        }
        else
        {
            player1.GetComponent<PlayerSetup>().DisableComponents();
            player2.GetComponent<PlayerSetup>().EnableComponents();
        }

        m_CinematicCamera.GetComponent<CinematicCamera>().m_IsMoving = true;
    }
}
