using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sucker : MonoBehaviour
{
    [SerializeField] private GameObject m_rocket;
    [SerializeField] private bool m_isUnder;
    [SerializeField] private float m_speedThrust = 100f;

    private BoxCollider m_boxCollider;
    private Renderer m_renderer;

    // Start is called before the first frame update
    void Start()
    {
        m_rocket = GameObject.Find("Rocket Ship");
        m_boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = m_rocket.transform.position.x;
        m_isUnder = x < transform.position.x + m_boxCollider.size.x / 2f &&
            x > transform.position.x - m_boxCollider.size.x / 2f;

        if (m_isUnder)
        {
            Suck();
        }
    }

    private float GetSpeedFrame()
    {
        return m_speedThrust * Time.deltaTime;
    }

    private void Suck()
    {
        Rigidbody rigidBody = m_rocket.GetComponent<Rigidbody>();
        rigidBody.AddRelativeForce(new Vector3(0, 1, 0) * GetSpeedFrame(), ForceMode.Impulse);
    }
}
