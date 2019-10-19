using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class ThrowSimulation : MonoBehaviour
{
    public Vector3 m_ProjectileVelocity;
    public float m_ElapsedTime = 0;
    public float m_FlightDuration = 0;

    public GameObject BallGameObject;
    private Transform Projectile;

    public void ThrowProjectile()
    {
        // To negate the velocity x in case it's the player 2 playing.
        int playerModifier = 1;

        if (transform.name == "Player2")
        {
            playerModifier = -1;
        }

        // Random values for the shots
        Vector3 m_ProjectileVelocity = new Vector3((Random.value * 10 + 2 )* playerModifier, Random.value * 20 + 5 , Random.value * 10 - 5);
        

        // Create the new ball
        Projectile = Instantiate(BallGameObject).transform;
        Projectile.gameObject.SetActive(true);

        // Get the gravity value from the physics of the ball
        float gravity = Projectile.gameObject.GetComponent<Physics>().GRAVITY;

        // Move projectile to the position of throwing object + add some offset if needed.
        Projectile.position = transform.position + new Vector3(0, 0.0f, 0);

        // Set the velocity of the ball object
        Projectile.gameObject.GetComponent<Volleyball>().m_Vx = m_ProjectileVelocity.x;
        Projectile.gameObject.GetComponent<Volleyball>().m_Vy = m_ProjectileVelocity.y;
        Projectile.gameObject.GetComponent<Volleyball>().m_Vz = m_ProjectileVelocity.z;

        // Calculate the flight duration with the quadratic formula
        m_FlightDuration = (-Projectile.gameObject.GetComponent<Volleyball>().m_Vy - Mathf.Sqrt(Mathf.Pow(Projectile.gameObject.GetComponent<Volleyball>().m_Vy, 2) + 2 * gravity * Projectile.position.y)) / -gravity;

        // Calculate the distance of the flight
        float target_Distance = Mathf.Sqrt(Mathf.Pow(Projectile.gameObject.GetComponent<Volleyball>().m_Vx, 2) + Mathf.Pow(Projectile.gameObject.GetComponent<Volleyball>().m_Vz, 2)) * m_FlightDuration;

        // Test for a priori collision with the terrain
        if (Projectile.gameObject.GetComponent<Volleyball>().DetectCollisionAPriori(Projectile.position.x, Projectile.position.z, m_FlightDuration, transform.name)) {
            Debug.Log("La detection a priori detecte un collision avec le terrain!!");
        }

        // Set the ball state to Is_Throwing
        Projectile.gameObject.GetComponent<Volleyball>().m_IsThrowing = true;
    }
}