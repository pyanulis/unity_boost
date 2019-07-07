using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sucker : MonoBehaviour
{
    [SerializeField] private float m_force = 100f;

    private GameObject m_rocketObject;

    private BoxCollider m_boxCollider;
    private Renderer m_renderer;
    private bool m_rocketAlive = true;
    private Rocket m_rocket;

    // Start is called before the first frame update
    void Start()
    {
        m_rocketObject = GameObject.Find("Rocket Ship");
        m_rocket = m_rocketObject?.GetComponent<Rocket>();
        m_boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_rocket == null || m_rocket.State == Assets.RocketState.Collided)
        {
            return;
        }

        float x = m_rocketObject.transform.position.x;
        bool isUnder = x < transform.position.x + m_boxCollider.size.x / 2f &&
            x > transform.position.x - m_boxCollider.size.x / 2f;

        if (isUnder)
        {
            Suck();
        }
    }

    private float GetSpeedFrame()
    {
        return m_force * Time.deltaTime;
    }

    private void Suck()
    {
        Rigidbody rigidBody = m_rocketObject.GetComponent<Rigidbody>();
        rigidBody.AddRelativeForce(new Vector3(0, 1, 0) * GetSpeedFrame(), ForceMode.Impulse);
    }
}
