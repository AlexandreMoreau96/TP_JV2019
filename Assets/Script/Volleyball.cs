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
                if (Vector3.Dot(vObject.GetComponent<Plane>().normal, transform.position) < GetComponent<Renderer>().bounds.extents.magnitude) {
                    Vy = -Vy * 0.8f;
                    Debug.Log("La balle frappera le sol");
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
                    return true;
                }

                break;
            }
        }

        return false;
    }

    void Update()
    {
        DetectCollision();
    }
}
