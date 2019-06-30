using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    private const float c_defaultPeriod = 5f;
    [SerializeField] private Vector3 m_vectorMovement;
    [SerializeField] private float m_period = c_defaultPeriod;

    [SerializeField]
    [Range(0, 1)]
    private float m_movementFactor;

    private Vector3 m_startPosition;

    // Start is called before the first frame update
    void Start()
    {
        m_startPosition = transform.position;
        m_period = m_period <= Mathf.Epsilon ? c_defaultPeriod : m_period;
    }

    // Update is called once per frame
    void Update()
    {
        float cycles = Time.time / m_period;

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);
        m_movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = m_vectorMovement * m_movementFactor;
        transform.position = m_startPosition + offset;
    }
}
