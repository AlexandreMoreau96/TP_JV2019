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
    private ThrowSimulation m_Throw;
    private Vector3 m_ContactPoint;
    private bool m_JustBounced = false;
    private float m_ElapsedTime = 0.0f;
    public bool m_IsThrowing = false;
    public float m_Gravity = 9.81f;
    public float m_Friction = 1f;
    public float m_Bounciness = 0.75f;
    private Vector3 m_LastVelocity = new Vector3(0,0,0);

    private void Start()
    {
        m_Throw = GameObject.Find("Player1").GetComponent<ThrowSimulation>();
    }

    public void DetectCollision()
    {
        foreach (GameObject vObject in possibleCollisionGameObjects)
        {
            //transform de la balle devrait etre changer par le transform de la balle a sa fin?
            Vector3 vBuffer = vObject.transform.position - transform.position;

            if (vObject.tag == "Player") {
                if (vBuffer.sqrMagnitude <= vObject.GetComponent<Player>().sqrRadius()) {
                    //Debug.Log("La balle sera attrapee par le joueur adverse");
                }
            }

            if (vObject.tag == "terrain") {

                if (m_ElapsedTime >= 0.25f && m_JustBounced) {
                    m_JustBounced = false;
                }

                Vector3 vSize = vObject.GetComponent<Renderer>().bounds.size;
                //float vMagnitude = (transform.position - m_ContactPoint).sqrMagnitude;

                if (transform.position.x >= 0 && transform.position.x <= vSize.x && transform.position.z >= 0 && transform.position.z <= vSize.z
                    && transform.position.y - GetComponent<Renderer>().bounds.extents.y <= vObject.transform.position.y && m_JustBounced == false)
                {
                    Debug.Log("collision avec le terrain");

                    Debug.Log("Before: " + Vy);

                    if (Vy < 2.0f && Vy > -2.0f)
                    {
                        Vy = m_LastVelocity.y;
                        Vx = Vx * m_Friction;
                        Vz = Vz * m_Friction;
                    }
                    else
                    {
                        Vy = -Vy * m_Bounciness;
                        Vx = Vx * m_Friction;
                        Vz = Vz * m_Friction;
                        m_JustBounced = true;
                        m_ElapsedTime = 0.0f;
                    }
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
    }

    public void FixedUpdate() {
        DetectCollision();

        if (m_IsThrowing) {
            transform.Translate(Vx * Time.deltaTime, Vy * Time.deltaTime, Vz * Time.deltaTime);
            Vy = Vy - m_Gravity * Time.deltaTime;
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
