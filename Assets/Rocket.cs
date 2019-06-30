using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    private const float c_sceneDelay = 2f;
    private Rigidbody m_rigidBody;
    private Transform m_transform;
    private AudioSource m_audioSource;
    private Light m_headLight;

    private State m_state;

    [SerializeField] private float m_rcsThrust = 100f;
    [SerializeField] private float m_speedThrust = 700f;
    [SerializeField] private float m_levelLoadDelay = c_sceneDelay;
    [SerializeField] private AudioClip m_audioEngine;
    [SerializeField] private AudioClip m_audioLevel;
    [SerializeField] private AudioClip m_audioCollision;

    [SerializeField] private ParticleSystem m_particlesEngine;
    [SerializeField] private ParticleSystem m_particlesSuccess;
    [SerializeField] private ParticleSystem m_particlesDeath;

    #region Messages

    // Start is called before the first frame update
    private void Start()
    {
        m_state = State.Alive;
        m_rigidBody = gameObject.GetComponent<Rigidbody>();
        m_transform = gameObject.GetComponent<Transform>();
        m_audioSource = gameObject.GetComponent<AudioSource>();

        m_headLight = transform.Find("Head Light").GetComponent<Light>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_state != State.Alive) return;

        RespondToThrustInput();
        RespondToRotateInput();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (m_state != State.Alive) return;

        if (collision.gameObject.tag == Tags.Finish)
        {
            StartSuccessSequence();
        }
        else if (collision.gameObject.tag == Tags.Obstacle)
        {
            StartDeathSequence();
        }
    }

    private void StartDeathSequence()
    {
        m_state = State.Dying;

        m_headLight.enabled = false;

        m_audioSource.Stop();
        m_audioSource.PlayOneShot(m_audioCollision);

        m_particlesEngine.Stop();
        m_particlesDeath.Play();

        Invoke(nameof(LoadPreviousScene), m_levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        m_state = State.Transcending;

        m_audioSource.Stop();
        m_audioSource.PlayOneShot(m_audioLevel);

        m_particlesEngine.Stop();
        m_particlesSuccess.Play();

        Invoke(nameof(LoadNextScene), m_levelLoadDelay);
    }

    #endregion


    #region Private methods

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else 
        {
            m_audioSource.Stop();
            m_particlesEngine.Stop();
        }
    }

    private void RespondToRotateInput()
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

    private void ApplyThrust()
    {
        m_rigidBody.AddRelativeForce(new Vector3(0, 1, 0) * GetSpeedFrame(), ForceMode.Force);
        m_particlesEngine.Play();
        if (!m_audioSource.isPlaying)
        {
            m_audioSource.PlayOneShot(m_audioEngine);
        }
    }

    private float GetRotationFrame()
    {
        return m_rcsThrust * Time.deltaTime;
    }

    private float GetSpeedFrame()
    {
        return m_speedThrust * Time.deltaTime;
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void LoadPreviousScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        index = index == 0 ? 0 : index - 1;
        SceneManager.LoadScene(index);
    }

    #endregion

    private static class Tags
    {
        public const string Friendly = "Friendly";
        public const string Finish = "Finish";
        public const string Obstacle = "Obstacle";
    }

    private enum State
    {
        Alive,
        Dying,
        Transcending
    }
}
