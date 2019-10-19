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
    private bool m_HitZone;
    public float m_Vx = 0;
    public float m_Vy = 0;
    public float m_Vz = 0;
    private Vector3 m_ContactPoint;
    private bool m_JustBounced = false;
    private float m_ElapsedTime = 0.0f;
    public bool m_IsThrowing = false;
    private Physics m_Physics;
    private bool m_PointAdded = false;

    private void Start()
    {
        m_Physics = GetComponent<Physics>();
    }

    public void DetectCollision()
    {
        foreach (GameObject vObject in possibleCollisionGameObjects)
        {
            if (vObject.tag == "ServeZone") {
                Vector3 vSize = vObject.GetComponent<Renderer>().bounds.size;

                Vector3 vClosestPoint = vObject.GetComponent<Renderer>().bounds.ClosestPoint(transform.position);

                if ((transform.position - vClosestPoint).sqrMagnitude <=
                    GetComponent<Renderer>().bounds.extents.sqrMagnitude) {
                    m_HitZone = true;
                }
            }

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

                    if (!m_PointAdded) {
                        if (m_GameManager.player1Playing) {
                            m_GameManager.AddPoint(2);
                        }
                        else {
                            m_GameManager.AddPoint(1);
                        }
                        m_PointAdded = true;
                    }
                }
            }

            if (vObject.tag == "terrain") {

                if (m_ElapsedTime >= 0.25f && m_JustBounced) {
                    m_JustBounced = false;
                }

                Vector3 vSize = vObject.GetComponent<Renderer>().bounds.size;

                Vector3 vClosestPoint = vObject.GetComponent<Renderer>().bounds.ClosestPoint(transform.position);

                float vMagnitude = (transform.position - vClosestPoint).magnitude;
                float vRayon = GetComponent<Renderer>().bounds.extents.x; // It's a sphere, so any vertice is equidistant to the origin

                Debug.Log(vMagnitude + " " + vRayon);

                if (vMagnitude <= vRayon && m_JustBounced == false) {

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

                    if (!m_PointAdded) {
                        if (transform.position.x <= vSize.x / 2) {

                            if (m_GameManager.player1Playing && m_HitZone) {
                                m_GameManager.AddPoint(1);
                            }
                            else {
                                m_GameManager.AddPoint(2);
                            }
                        }
                        else {

                            if (m_GameManager.player1Playing && m_HitZone) {
                                m_GameManager.AddPoint(2);
                            }
                            else {
                                m_GameManager.AddPoint(1);
                            }
                        }

                        m_PointAdded = true;
                    }

                }
            }
        }
    }

    public bool DetectCollisionAPriori(float initialX, float initialZ, float pFlightDuration)
    {
        float vLastX = initialX - m_Vx * pFlightDuration;
        float vLastZ = initialZ + m_Vz * pFlightDuration;

        foreach (GameObject vObject in possibleCollisionGameObjects)
        {

            if (vObject.tag == "terrain")
            {
                Vector3 vSize = vObject.GetComponent<Renderer>().bounds.size;

                if (vLastX >= 0 && vLastX <= vSize.x && vLastZ >= 0 && vLastZ <= vSize.z)
                {
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
