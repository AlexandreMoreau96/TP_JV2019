using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volleyball : MonoBehaviour
{
    Vector3 velocity;
    Vector3 gravity = new Vector3(0f, -9.81f, 0f);
    public bool throwe = false;
    Vector3 acceleration;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ThrowAction(Vector3 _acceleration)
    {
        acceleration = _acceleration;
        throwe = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (throwe)
        {
            Throw();
        }
    }

    public void Throw()
    {
        velocity += acceleration * Time.deltaTime;
        transform.position +=  velocity + gravity.y/2 * acceleration * Time.deltaTime * Time.deltaTime;
    }

    public void Throw(float temps, float vit_y, float vit_xz, Vector3 xz_forward, Vector3 point_avant)
    {
        float y = vit_y * temps + Physics.gravity.y * temps * temps / 2;
        float xz = vit_xz * temps;

        transform.position += new Vector3(xz, y, xz) * Time.deltaTime;

    }
}
