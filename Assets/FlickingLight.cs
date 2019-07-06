using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
[DisallowMultipleComponent]
public class FlickingLight : MonoBehaviour
{
    private const float c_defaultPeriod = 5f;

    [SerializeField] private float m_period = c_defaultPeriod;
    [SerializeField] private float m_maxIntendisy = 100;
    [SerializeField] private float m_minPeriod = 0.1f;
    [SerializeField] private float m_maxPeriod = 100f;
    [SerializeField]
    [Range(2, 10)]
    private int m_enableRange;
    [SerializeField] private bool m_enableByZero = false;
    [SerializeField] private bool m_flickEnable = false;

    private Light m_light;

    // Start is called before the first frame update
    void Start()
    {
        m_light = GetComponent<Light>();
        m_period = m_period <= Mathf.Epsilon ? c_defaultPeriod : m_period;
    }

    // Update is called once per frame
    void Update()
    {
        m_period = Random.Range(m_minPeriod, m_maxPeriod);
        float cycles = Time.time / m_period;

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau);
        float factor = rawSinWave / 2f + 0.5f;
        m_light.intensity = m_maxIntendisy * factor;

        if (m_flickEnable)
        {
            int x = Random.Range(0, m_enableRange);
            bool enable = m_enableByZero ? x == 0 : x != 0;
            m_light.enabled = enable;
        }
        else
        {
            m_light.enabled = true;
        }
    }
}
