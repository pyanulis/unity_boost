using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class OscillatorRotate : MonoBehaviour
{
    private const float c_defaultPeriod = 5f;
    [SerializeField] private float m_period = c_defaultPeriod;

    [SerializeField]
    private Axis m_axis;

    [SerializeField]
    private Spin m_spin;

    private Vector3 m_startAngle;

    // Start is called before the first frame update
    void Start()
    {
        m_startAngle = transform.eulerAngles;
        m_period = m_period <= Mathf.Epsilon ? c_defaultPeriod : m_period;
    }

    // Update is called once per frame
    void Update()
    {
        float cycles = Time.time / m_period;

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);
        float degrees = cycles * 360;

        transform.eulerAngles = GetRotation(m_axis, m_spin, degrees);
    }

    private Vector3 GetRotation(Axis axis, Spin spin, float angle)
    {
        angle = spin == Spin.Counterclockwise ? angle : -angle;

        switch (axis)
        {
            case Axis.X:
                return new Vector3(angle, m_startAngle.y, m_startAngle.z);
            case Axis.Y:
                return new Vector3(m_startAngle.x, angle, m_startAngle.z);
            case Axis.Z:
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
