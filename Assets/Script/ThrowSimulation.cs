using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class ThrowSimulation : MonoBehaviour
{
    public float gravity = 9.8f;
    public Vector3 m_ProjectileVelocity;
    public float m_ElapsedTime = 0;
    public float m_FlightDuration = 0;

    public GameObject BallGameObject;
    private Transform Projectile;
    private Transform myTransform;

    private bool Is_Throwing = false;
    private bool Is_DoneThrowing = false;

    void Awake()
    {
        myTransform = transform;
    }

    void Start()
    {
        
    }

    public void Update()
    {
        if (Is_Throwing)
        {
            Projectile.Translate(Projectile.gameObject.GetComponent<Volleyball>().Vx * Time.deltaTime, (Projectile.gameObject.GetComponent<Volleyball>().Vy - (gravity * m_ElapsedTime)) * Time.deltaTime, Projectile.gameObject.GetComponent<Volleyball>().Vz * Time.deltaTime);

            m_ElapsedTime += Time.deltaTime;

            if (m_ElapsedTime >= m_FlightDuration * 2)
            {
                Is_DoneThrowing = true;
                Is_Throwing = false;
                m_ElapsedTime = 0;
                Destroy(Projectile.gameObject);
            }
        }
    }

    public void SimulateProjectile()
    {
        // To negate the velocity x in case it's the player 2 playing.
        int playerModifier = 1;

        if (transform.name == "Player2")
        {
            playerModifier = -1;
        }

        Vector3 m_ProjectileVelocity = new Vector3(Random.value * 10 * playerModifier, 15f, Random.value * 20 - 10);

        Projectile = Instantiate(BallGameObject).transform;

        // Move projectile to the position of throwing object + add some offset if needed.
        Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);

        // Extract the X  Y  Z componenent of the velocity
        Projectile.gameObject.GetComponent<Volleyball>().Vx = m_ProjectileVelocity.x;
        Projectile.gameObject.GetComponent<Volleyball>().Vy = m_ProjectileVelocity.y;
        Projectile.gameObject.GetComponent<Volleyball>().Vz = m_ProjectileVelocity.z;

        // Calculate the flight duration with the quadratic formula
        m_FlightDuration = (-Projectile.gameObject.GetComponent<Volleyball>().Vy - Mathf.Sqrt(Mathf.Pow(Projectile.gameObject.GetComponent<Volleyball>().Vy, 2) + 2 * gravity * Projectile.position.y)) / -gravity;

        float target_Distance = Mathf.Sqrt(Mathf.Pow(Projectile.gameObject.GetComponent<Volleyball>().Vx, 2) + Mathf.Pow(Projectile.gameObject.GetComponent<Volleyball>().Vz, 2)) * m_FlightDuration;

        if (Projectile.gameObject.GetComponent<Volleyball>().DetectCollisionAPriori(Projectile.position.x, Projectile.position.z, m_FlightDuration)) {
            Debug.Log("Il y aura une collision avec le terrain.");
        }

        Is_Throwing = true;
    }
}