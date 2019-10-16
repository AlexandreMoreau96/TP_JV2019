using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject ball;

    private GameObject ballPosition;

    private bool m_IsThrown;

    // Start is called before the first frame update
    void Start()
    {
        ballPosition = GetComponent<GameObject>();
        m_IsThrown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsThrown)
        {
            // Créer la balle
            Instantiate(ball, ballPosition.transform);

            // Lancer la balle

        }


    }
}
