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
    private bool notCollide = true;
    private ThrowSimulation m_Throw;
    private bool m_collisionAPriori;
    private float m_ElapseTime;

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
                Vector3 vSize = vObject.GetComponent<Renderer>().bounds.size;
                if (transform.position.x >= 0 && transform.position.x <= vSize.x && transform.position.z >= 0 && transform.position.z <= vSize.z)
                {
                    m_ElapseTime = 0;
                    Debug.Log("collision avec le terrain");
                    Vy = -Vy * 0.1f;

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
                    Vector3 vContactPosition = new Vector3(vLastX, vObject.transform.position.y, vLastZ);
                    m_collisionAPriori = true;
                    return true;
                }

                break;
            }
        }
        m_collisionAPriori = false;
        return false;
    }

    void Update()
    {
        m_ElapseTime += Time.deltaTime;
        DetectCollision();
    }
}
