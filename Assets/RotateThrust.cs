using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

[DisallowMultipleComponent]
public class RotateThrust : MonoBehaviour
{
    [SerializeField] private float m_maxVel = 1500f;
    [SerializeField] private Axis m_axis;
    [SerializeField] private Spin m_spin;
    [SerializeField] private float m_accel = 1000;
    [SerializeField] private float m_deccel = -500;
    [SerializeField] private float m_setStartVel = 100;
    [SerializeField] private KeyCode m_thrustKey = KeyCode.Space;

    private float m_startVel;
    private float m_vel = 0f;
    private float m_curAccel = 0;
    private bool m_spinning;

    private Vector3 m_startAngle;

    private float m_actionTime;
    private float m_delta;

    // Start is called before the first frame update
    void Start()
    {
        m_startAngle = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        HandleThrustKey();

        if (!m_spinning) return;

        float angle = GetFrameSpinAngle();
        transform.localEulerAngles = GetRotation(m_axis, m_spin, angle, m_startAngle);
    }

    private void HandleThrustKey()
    {
        if (IsThrustDown())
        {
            m_startVel = m_spinning ? m_vel : m_setStartVel;
            m_curAccel = m_accel;

            InitSpinning(startVel: m_spinning ? m_vel : m_setStartVel, accel: m_accel);
        }
        else if (IsThrustUp())
        {
            m_startVel = m_vel;
            m_curAccel = m_deccel;

            InitSpinning(startVel: m_vel, accel: m_deccel);
        }
    }

    private void InitSpinning(float startVel, float accel)
    {
        m_spinning = true;
        m_actionTime = Time.time;
        m_startAngle = transform.localEulerAngles;
        m_startVel = startVel;
        m_curAccel = accel;
    }

    private float GetFrameSpinAngle()
    {
        float timeDelta = Time.time - m_actionTime;
        float spinAngle = m_startVel * timeDelta + (m_curAccel * Mathf.Pow(timeDelta, 2f) / 2f);
        m_vel = m_startVel + m_curAccel * timeDelta;
        if (m_vel <= 0)
        {
            m_spinning = false;
        }
        else if (m_vel > m_maxVel)
        {
            InitSpinning(startVel: m_maxVel, accel: 0);
        }

        return spinAngle;
    }

    private static Vector3 GetRotation(Axis axis, Spin spin, float angle, Vector3 startAngle)
    {
        angle = spin == Spin.Counterclockwise ? angle : -angle;
        switch (axis)
        {
            case Axis.X:
                angle += startAngle.x;
                return new Vector3(angle, startAngle.y, startAngle.z);
            case Axis.Y:
                angle += startAngle.y;
                return new Vector3(startAngle.x, angle, startAngle.z);
            case Axis.Z:
                angle += startAngle.z;
                return new Vector3(startAngle.x, startAngle.y, angle);
        }

        return new Vector3(startAngle.x, startAngle.y, startAngle.z);
    }

    private bool IsThrustDown()
    {
        return Input.GetKeyDown(m_thrustKey) ||
            Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.W);
    }
    private bool IsThrustUp()
    {
        return Input.GetKeyUp(m_thrustKey) ||
            Input.GetKeyUp(KeyCode.UpArrow) ||
            Input.GetKeyUp(KeyCode.W);
    }
}
