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

    #region Messages

    // Start is called before the first frame update
    private void Start()
    {
        m_rigidBody = gameObject.GetComponent<Rigidbody>();
        m_transform = gameObject.GetComponent<Transform>();
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        Thrust();
        Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == Tags.Friendly)
        {
            print(Tags.Friendly);
        }
        else
        {
            print(collision.gameObject.name + " hostile");
        }
    }

    #endregion


    #region Private methods

    private void Thrust()
    {
        if (m_audioSource.isPlaying)
        {
            m_audioSource.mute = true;
        }

        if (Input.GetKey(KeyCode.Space))
        {
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
            m_transform.Rotate(Vector3.forward * GetRotationFrame());
        }
        else if (Input.GetKey(KeyCode.D))
        {
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

    #endregion

    private static class Tags
    {
        public const string Friendly = "Friendly";
    }
}
