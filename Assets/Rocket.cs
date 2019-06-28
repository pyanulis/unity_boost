using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Rigidbody m_rigidBody;
    private Transform m_transform;
    private AudioSource m_audioSource;

    [SerializeField]
    private float m_rcsThrust = 100f;
    [SerializeField]
    private float m_speedThrust = 700f;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = gameObject.GetComponent<Rigidbody>();
        m_transform = gameObject.GetComponent<Transform>();
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        if (m_audioSource.isPlaying)
        {
            m_audioSource.mute = true;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            print("Thrust");
            if (!m_audioSource.isPlaying)
            {
                m_audioSource.Play();
            }
            else
            {
                m_audioSource.mute = false;
            }
            m_rigidBody.AddRelativeForce(new Vector3(0, 1, 0) * GetSpeedFrame(), ForceMode.Force);
        }
    }

    private void Rotate()
    {
        m_rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A))
        {
            print("Left");
            m_transform.Rotate(Vector3.forward * GetRotationFrame());
        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("Right");
            m_transform.Rotate(Vector3.back * GetRotationFrame());
        }

        m_rigidBody.freezeRotation = false;
    }

    private float GetRotationFrame()
    {
        return m_rcsThrust * Time.deltaTime;
    }

    private float GetSpeedFrame()
    {
        return m_speedThrust * Time.deltaTime;
    }
}
