using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] players;

    [SerializeField]
    private GameObject m_GameUI;
    [SerializeField]
    private Text m_gameWinnerText;

    private GameObject player1;
    private GameObject player2;

    private static int m_PointPlayer1;
    private static int m_PointPlayer2;

    public bool player1Playing;

    public Camera m_CinematicCamera;

    private bool m_SwitchingTurn = false;
    private bool m_GameOver = false;
    private bool m_UIEnabled = false;

    private int m_MaxPoints = 3;

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
        m_GameUI.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameObject.FindGameObjectsWithTag("Ball").Length == 0 && !m_SwitchingTurn)
        {
            TestWinner();

            if (m_GameOver)
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
        if (!m_UIEnabled)
        {
            m_GameUI.SetActive(true);
            m_UIEnabled = true;
        }
    }

    public void GameOver(string pWinner)
    {
        m_GameOver = true;
        m_gameWinnerText.text += pWinner;
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

    public void AddPoint(int player)
    {
        if (player == 1)
        {
            m_PointPlayer1 += 1;
        }
        else
        {
            m_PointPlayer2 += 1;
        }
    }

    public void TestWinner()
    {
        Debug.Log("Joueur 1: " + m_PointPlayer1 + "\nJoueur 2: " + m_PointPlayer2);
        if ((m_PointPlayer1 == m_MaxPoints || m_PointPlayer2 == m_MaxPoints) && !m_GameOver) {
            if (m_PointPlayer1 > m_PointPlayer2) {
                GameOver("player1");
            }
            else {
                GameOver("player2");
            }
        }
    }
}
