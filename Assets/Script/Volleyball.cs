using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volleyball : MonoBehaviour
{
    [SerializeField]
    private GameObject[] possibleCollisionGameObjects;

    public float Vx = 0;
    public float Vy = 0;
    public float Vz = 0;
    private Vector3 m_ContactPoint;
    private bool m_JustBounced = false;
    private float m_ElapsedTime = 0.0f;
    public bool m_IsThrowing = false;
    public float m_Gravity = 9.81f;
    private Physics m_Physics;

    private void Start()
    {
        m_Physics = GetComponent<Physics>();
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
                    Vx = 0;
                    Vz = 0;
                    Vy = -m_Physics.GRAVITY / 2;
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

                    if (Vy < 2.0f && Vy > -2.0f)
                    {
                        Vy = 0;
                    }
                    else
                    {
                        Vy = -Vy * vObject.GetComponent<Physics>().m_Bounciness;
                        m_JustBounced = true;
                        m_ElapsedTime = 0.0f;
                    }

                    Vx = Vx * vObject.GetComponent<Physics>().m_Friction;
                    Vz = Vz * vObject.GetComponent<Physics>().m_Friction;
                }
            }
        }
    }

    public bool DetectCollisionAPriori(float initialX, float initialZ, float pFlightDuration)
    {
        float vLastX = initialX - Vx * pFlightDuration;
        float vLastZ = initialZ + Vz * pFlightDuration;

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
            transform.Translate(Vx * Time.deltaTime, Vy * Time.deltaTime, Vz * Time.deltaTime);
            Vy = Vy - m_Physics.GRAVITY * Time.deltaTime;
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
