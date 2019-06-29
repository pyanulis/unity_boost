using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private AudioSource m_audioSource;

    #region Messages

    // Start is called before the first frame update
    private void Start()
    {
        m_audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        print($"Collision: {collision.relativeVelocity}, {collision.impulse}");

        if (m_audioSource.isPlaying)
        {
            m_audioSource.Stop();
        }
        else
        {
            m_audioSource.Play();
        }
    }

    #endregion
}
