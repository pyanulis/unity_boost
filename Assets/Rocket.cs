using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets;

public class Rocket : MonoBehaviour
{
    private const float c_sceneDelay = 2f;
    private Rigidbody m_rigidBody;
    private Transform m_transform;
    private AudioSource m_audioSource;
    private Light m_headLight;

    private RocketState m_state;
    private bool m_disableCollision;

    [SerializeField] private float m_rcsThrust = 200f;
    [SerializeField] private float m_speedThrust = 800f;
    [SerializeField] private float m_levelLoadDelay = c_sceneDelay;
    [SerializeField] private AudioClip m_audioEngine;
    [SerializeField] private AudioClip m_audioLevel;
    [SerializeField] private AudioClip m_audioCollision;

    [SerializeField] private ParticleSystem m_particlesEngine;
    [SerializeField] private ParticleSystem m_particlesEngine2;
    [SerializeField] private ParticleSystem m_particlesSuccess;
    [SerializeField] private ParticleSystem m_particlesDeath;

    public RocketState State => m_state;

    #region Messages

    // Start is called before the first frame update
    private void Start()
    {
        Screen.fullScreen = false;

        m_state = RocketState.Alive;
        m_rigidBody = gameObject.GetComponent<Rigidbody>();
        m_transform = gameObject.GetComponent<Transform>();
        m_audioSource = gameObject.GetComponent<AudioSource>();

        m_headLight = transform.Find("Head Light").GetComponent<Light>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (m_state != RocketState.Alive) return;

        RespondToThrustInput();
        RespondToRotateInput();

        if (Debug.isDebugBuild)
        {
            RespondToDebug();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (m_state != RocketState.Alive || m_disableCollision) return;

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
        m_state = RocketState.Collided;

        m_headLight.enabled = false;

        EngineEffectsStop();

        m_audioSource.PlayOneShot(m_audioCollision);
        m_particlesDeath.Play();

        Invoke(nameof(ReloadScene), m_levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        m_state = RocketState.Transcending;

        EngineEffectsStop();

        m_audioSource.PlayOneShot(m_audioLevel);
        m_particlesSuccess.Play();

        Invoke(nameof(LoadNextScene), m_levelLoadDelay);
    }

    #endregion


    #region Private methods

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetAxis("Vertical") > 0)
        {
            ApplyThrust();
        }
        else 
        {
            EngineEffectsStop();
        }
    }

    private void RespondToRotateInput()
    {
        m_rigidBody.freezeRotation = true;

        //if (Input.GetKey(KeyCode.A))
        if (Input.GetAxis("Horizontal") < 0)
        {
            m_transform.Rotate(Vector3.forward * GetRotationFrame());
        }
        else if (Input.GetAxis("Horizontal") > 0)//if (Input.GetKey(KeyCode.D))
        {
            m_transform.Rotate(Vector3.back * GetRotationFrame());
        }

        m_rigidBody.freezeRotation = false;
    }

    private void RespondToDebug()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            m_disableCollision = !m_disableCollision;
        }
    }

    private void ApplyThrust()
    {
        m_rigidBody.AddRelativeForce(new Vector3(0, 1, 0) * GetSpeedFrame(), ForceMode.Force);
        EngineEffectsPlay();
    }

    private float GetRotationFrame()
    {
        return m_rcsThrust * Time.deltaTime;
    }

    private float GetSpeedFrame()
    {
        return m_speedThrust * Time.deltaTime;
    }

    private void EngineEffectsStop()
    {
        SafeAction(m_audioSource.Stop);
        SafeAction(m_particlesEngine.Stop);
        SafeAction(m_particlesEngine2.Stop);
    }

    private void EngineEffectsPlay()
    {
        SafeAction(m_particlesEngine.Play);
        SafeAction(m_particlesEngine2.Play);

        if (m_audioSource && !m_audioSource.isPlaying)
        {
            m_audioSource.PlayOneShot(m_audioEngine);
        }
    }

    private void SafeAction(Action action) => Utils.UnitySafeAction(action);

    private void LoadNextScene()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        next = next >= SceneManager.sceneCountInBuildSettings ? 0 : next;
        SceneManager.LoadScene(next);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
}
