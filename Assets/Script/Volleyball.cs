using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volleyball : MonoBehaviour
{
    [SerializeField]
    private GameObject[] possibleCollisionGameObjects;

    private static int m_PointPlayer1;
    private static int m_PointPlayer2;

    [SerializeField]
    private GameManager m_GameManager;
    private bool m_HitNet;
    public float m_Vx = 0;
    public float m_Vy = 0;
    public float m_Vz = 0;
    private Vector3 m_ContactPoint;
    private bool m_JustBounced = false;
    private float m_ElapsedTime = 0.0f;
    public bool m_IsThrowing = false;
    private Physics m_Physics;
    private bool m_AddPointPlayer1;
    private bool m_AddPointPlayer2;

    private void Start()
    {
        m_Physics = GetComponent<Physics>();
        m_AddPointPlayer2 = false;
        m_AddPointPlayer1 = false;
    }

    public void DetectCollision()
    {
        foreach (GameObject vObject in possibleCollisionGameObjects)
        {
            if (vObject.tag == "Net") {
                Vector3 vSize = vObject.GetComponent<Renderer>().bounds.size;

                Vector3 vClosestPoint = vObject.GetComponent<Renderer>().bounds.ClosestPoint(transform.position);

                if ((transform.position - vClosestPoint).sqrMagnitude <=
                    GetComponent<Renderer>().bounds.extents.sqrMagnitude)
                {
                    m_HitNet = true;
                    m_Vx = 0;
                    m_Vz = 0;
                    m_Vy = -m_Physics.GRAVITY / 2;
                }
            }

            if (vObject.tag == "terrain") {

                if (m_ElapsedTime >= 0.25f && m_JustBounced) {
                    m_JustBounced = false;
                }

                Vector3 vSize = vObject.GetComponent<Renderer>().bounds.size;

                Vector3 vClosestPoint = vObject.GetComponent<Renderer>().bounds.ClosestPoint(transform.position);

                if (transform.position.x >= 0 && transform.position.x <= vSize.x && transform.position.z >= 0 && transform.position.z <= vSize.z
                    && transform.position.y - GetComponent<Renderer>().bounds.extents.y <= vObject.transform.position.y && m_JustBounced == false) {

                    if (m_Vy < 2.0f && m_Vy > -2.0f)
                    {
                        m_Vy = 0;
                    }
                    else
                    {
                        m_Vy = -m_Vy * vObject.GetComponent<Physics>().m_Bounciness;
                        m_JustBounced = true;
                        m_ElapsedTime = 0.0f;
                    }

                    m_Vx = m_Vx * vObject.GetComponent<Physics>().m_Friction;
                    m_Vz = m_Vz * vObject.GetComponent<Physics>().m_Friction;
                }

                
            }
        }
    }

    public bool DetectCollisionAPriori(float initialX, float initialZ, float pFlightDuration, string player)
    {
        float vLastX = initialX - m_Vx * pFlightDuration;
        float vLastZ = initialZ + m_Vz * pFlightDuration;
        m_AddPointPlayer2 = false;
        m_AddPointPlayer1 = false;

        foreach (GameObject vObject in possibleCollisionGameObjects)
        {

            if (vObject.tag == "terrain")
            {
                Debug.Log("Player shooting : " + player);
                Vector3 vSize = vObject.GetComponent<Renderer>().bounds.size;

                if (vLastX >= 0 && vLastX <= vSize.x && vLastZ >= 0 && vLastZ <= vSize.z)
                {
                    if (vLastX <= vSize.x / 2 && vLastX <= vSize.x && vLastZ <= vSize.z && vLastZ <= vSize.z)
                    {
                        if (player == "Player1")
                        {
                            m_PointPlayer1 += 1;
                            Debug.Log("La balle tombera du cote du joueur2");
                        }
                    } else
                    {
                        if (player == "Player2")
                        {
                            m_PointPlayer2 += 1;
                            Debug.Log("La balle tombera du cote du joueur1");
                        }
                    }
                    
                    m_ContactPoint = new Vector3(vLastX, vObject.transform.position.y, vLastZ);
                    return true;
                }

                break;
            }
        }
        return false;
    }

    void Update()
    {
        m_ElapsedTime += Time.deltaTime;

        if (m_ElapsedTime >= 4 && m_ContactPoint != Vector3.zero)
        {
            Destroy(gameObject);
        }

        if((m_PointPlayer1 == 1 || m_PointPlayer2  == 1) && m_JustBounced && !m_HitNet)
        {
            Debug.Log("Parti Terminer");
        }

    }

    public void FixedUpdate() {
        DetectCollision();

        if (m_IsThrowing) {
            transform.Translate(m_Vx * Time.deltaTime, m_Vy * Time.deltaTime, m_Vz * Time.deltaTime);
            m_Vy = m_Vy - m_Physics.GRAVITY * Time.deltaTime;
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
