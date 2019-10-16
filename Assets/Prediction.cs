using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Prediction : MonoBehaviour
{
    public float vitInit = 20;
    public float speed = 1;
    public float pas = 1.0f;
    public float chute_temps = 10.0f;
    public LayerMask A_masquer;
    public Vector3 vitesse_lancer;
    public Volleyball ball;

    private Vector3 xz_forward = Vector3.up;
    [SerializeField]
    private LineRenderer parabole;
    private Transform Camera;
    private float distance_max, angle;

    private void Start()
    {
        Camera = GameObject.Find("Camera Player1").GetComponent<Transform>();
        //parabole = GetComponent<LineRenderer>();
        parabole.enabled = false;
        distance_max = -vitInit * vitInit / Physics.gravity.y;
    }

    private void FixedUpdate()
    {
        if (parabole.enabled)
        {
            vitesse_lancer = Predire_ligne(false);
        }
    }

    public void Show()
    {
        parabole.enabled = true;
    }

    public void Hide()
    {
        parabole.enabled = false;
    }

    public Vector3 Predire_ligne(bool atteignable)
    {
        float xz, A, a, b, c, t, X_coord, Z_coord, vit_xz, vit_y, saut_temps, temps, mi_temps;
        List<Vector3> points = new List<Vector3>();
        Vector3 direction_visée, repositionnement, point_avant, prochain_point;

        RaycastHit hit;
        if (Physics.Raycast(Camera.position, Camera.forward, out hit, distance_max, ~A_masquer))
        {
            direction_visée = hit.point - transform.position;
            xz_forward = new Vector3(direction_visée.x, 0.0f, direction_visée.z).normalized;
            xz = new Vector3(direction_visée.x, 0.0f, direction_visée.z).magnitude;
            A = -xz * Physics.gravity.y / (2 * vitInit * vitInit);

            if (A * A + A * direction_visée.y / xz - 0.25f <= 0)
            {
                atteignable = true;
                if (direction_visée.y < 0)
                {
                    angle = Mathf.Atan((-1 + Mathf.Sqrt(1 - 4 * (A * A + A * direction_visée.y / xz))) / (-2 * A));
                }
                else
                {
                    angle = Mathf.Atan((-1 - Mathf.Sqrt(1 - 4 * (A * A + A * direction_visée.y / xz))) / (-2 * A));
                }
            }
        }

        if (!atteignable)
        {
            repositionnement = Camera.position - transform.position;
            a = Mathf.Pow(Camera.forward.x, 2) + Mathf.Pow(Camera.forward.z, 2);
            b = 2 * (Camera.forward.x * repositionnement.x + Camera.forward.z * repositionnement.z + vitInit * vitInit * Camera.forward.y / -Physics.gravity.y);
            c = Mathf.Pow(repositionnement.z, 2) + Mathf.Pow(repositionnement.x, 2) - Mathf.Pow(vitInit * vitInit / -Physics.gravity.y, 2);
            t = (Mathf.Sqrt(b * b - 4 * a * c) - b) / (2 * a);
            X_coord = Camera.forward.x * t + repositionnement.x;
            Z_coord = Camera.forward.z * t + repositionnement.z;
            angle = Mathf.Atan(vitInit * vitInit / (-Physics.gravity.y * Mathf.Sqrt(X_coord * X_coord + Z_coord * Z_coord)));
            xz_forward = new Vector3(X_coord, 0.0f, Z_coord).normalized;

        }
        vit_xz = Mathf.Cos(angle) * vitInit;
        vit_y = Mathf.Sin(angle) * vitInit;



        saut_temps = pas / vit_xz;
        temps = saut_temps;
        mi_temps = vit_y <= 0 ? 0 : -vit_y / Physics.gravity.y;
        bool collision = false;

        point_avant = transform.position;
        points.Add(point_avant);

        while (!collision && temps < mi_temps)
        {

            collision = CalculTrajectoire(temps, vit_y, vit_xz, xz_forward, point_avant, out prochain_point);
            points.Add(prochain_point);
            point_avant = prochain_point;
            temps += saut_temps;
        }
        while (!collision && mi_temps < chute_temps)
        {

            collision = CalculTrajectoire(mi_temps, vit_y, vit_xz, xz_forward, point_avant, out prochain_point);
            points.Add(prochain_point);
            point_avant = prochain_point;
            mi_temps += saut_temps;

        }
        ball.Throw(mi_temps, vit_y, vit_xz, xz_forward, point_avant);
        parabole.positionCount = points.Count;
        parabole.SetPositions(points.ToArray());
        return xz_forward * vit_xz + new Vector3(0.0f, vit_y, 0.0f);
    }


    private bool CalculTrajectoire(float temps, float vit_y, float vit_xz, Vector3 xz_forward, Vector3 point_avant, out Vector3 prochain_point)
    {
        float y = vit_y * temps + Physics.gravity.y * temps * temps / 2;
        float xz = vit_xz * temps;

        prochain_point = transform.position + new Vector3(xz_forward.x * xz, y, xz_forward.z * xz);
        RaycastHit hit;
        Vector3 direction = prochain_point - point_avant;
        if (Physics.Raycast(point_avant, direction, out hit, direction.magnitude, ~A_masquer))
        {
            prochain_point = point_avant + direction.normalized * hit.distance;
            return true;
        }
        return false;
    }
}
