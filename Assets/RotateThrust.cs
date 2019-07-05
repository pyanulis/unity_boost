using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class RotateThrust : MonoBehaviour
{
    private const float c_maxVel = 1500f;

    [SerializeField]
    private Axis m_axis;

    [SerializeField]
    private Spin m_spin;

    private Vector3 m_startAngle;

    private bool m_spaceDown;
    private float m_actionTime;
    private float m_delta;

    [SerializeField]
    private float m_accel = 1000;

    [SerializeField]
    private float m_deccel = -500;

    [SerializeField]
    private float m_startVel;

    [SerializeField]
    private float m_vel = 1;

    [SerializeField]
    private float m_curAccel = 0;

    [SerializeField]
    private bool m_spinning;


    [SerializeField]
    private float m_setStartVel = 100;
    private float m_spinAngle;

    private float Angle360;

    // Start is called before the first frame update
    void Start()
    {
        m_startAngle = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !m_spaceDown)
        {
            m_startVel = m_spinning ? m_vel : m_setStartVel;
            m_spaceDown = true;
            m_spinning = true;
            m_actionTime = Time.time;
            m_startAngle = transform.eulerAngles;

            m_curAccel = m_accel;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && m_spaceDown)
        {
            m_spaceDown = false;
            m_spinning = true;
            m_actionTime = Time.time;
            m_startVel = m_vel;
            m_startAngle = transform.eulerAngles;

            m_curAccel = m_deccel;
        }

        if (!m_spinning) return;

        if (m_spaceDown && Time.time > m_actionTime)
        {
            m_delta = Time.time - m_actionTime;
        }
        else if (m_actionTime > Mathf.Epsilon)
        {
            m_delta = Time.time - m_actionTime;
        }

        float degrees = GetGegrees(m_axis, m_spin, m_delta);
        Angle360 = 360 * degrees % 360;
        transform.eulerAngles = GetRotation(m_axis, m_spin, degrees);
    }

    private void Thrust()
    {
    }

    private float GetGegrees(Axis axis, Spin spin, float timeDelta)
    {
        if (!m_spinning) return 0f;
        m_spinAngle = m_startVel * timeDelta + (m_curAccel * Mathf.Pow(timeDelta, 2f) / 2f);
        m_vel = m_startVel + m_curAccel * timeDelta;
        if (m_vel <= 0)
        {
            m_spinning = false;
            m_curAccel = 0;
            //m_startVel = 0f;
            //m_actionTime = Time.time;
            //m_startAngle = transform.eulerAngles;
        }
        else if (m_vel > c_maxVel)
        {
            m_curAccel = 0;
            m_startVel = c_maxVel;
            m_actionTime = Time.time;
            m_startAngle = transform.eulerAngles;
        }
        m_spinAngle = spin == Spin.Counterclockwise ? m_spinAngle : -m_spinAngle;

        return m_spinAngle;
    }

    private Vector3 GetRotation(Axis axis, Spin spin, float angle)
    {
        //angle = spin == Spin.Counterclockwise ? angle : -angle;

        switch (axis)
        {
            case Axis.X:
                angle += m_startAngle.x;
                return new Vector3(angle, m_startAngle.y, m_startAngle.z);
            case Axis.Y:
                angle += m_startAngle.y;
                return new Vector3(m_startAngle.x, angle, m_startAngle.z);
            case Axis.Z:
                angle += m_startAngle.z;
                return new Vector3(m_startAngle.x, m_startAngle.y, angle);
        }

        return new Vector3(m_startAngle.x, m_startAngle.y, m_startAngle.z);
    }

    public enum Axis
    {
        X,
        Y,
        Z
    }

    public enum Spin
    {
        Clockwise,
        Counterclockwise,
    }
}
