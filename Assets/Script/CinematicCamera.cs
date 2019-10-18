using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCamera : MonoBehaviour
{
    public Camera[] m_ReferenceCameras;

    public float m_Speed = 100f;

    public bool m_IsMoving = false;

    private int m_CurrentSide = 0;

    private float m_rotation = 0;

    private Vector3 m_CenterPoint = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = m_ReferenceCameras[m_CurrentSide].transform.position;
        m_CenterPoint = (m_ReferenceCameras[m_CurrentSide].transform.position + m_ReferenceCameras[1].transform.position) / 2;
        m_CenterPoint = new Vector3(m_CenterPoint.x, 0, m_CenterPoint.z);
        transform.LookAt(m_CenterPoint);
        ChangeCurrentSide();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsMoving)
        {
            m_rotation += m_Speed * Time.deltaTime;
            transform.RotateAround(m_CenterPoint, Vector3.down, m_Speed * Time.deltaTime);

            if (m_rotation >= 180)
            {
                m_IsMoving = false;
                transform.position = m_ReferenceCameras[m_CurrentSide].transform.position;
                transform.LookAt(m_CenterPoint);
                m_rotation = 0;
                ChangeCurrentSide();
                
            }
        }
    }

    void ChangeCurrentSide()
    {
        if (m_CurrentSide == 0) {
            m_CurrentSide = 1;
        }
        else {
            m_CurrentSide = 0;
        }
    }
}
