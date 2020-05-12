using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip WinSound;
    [SerializeField] AudioClip CrashSound;

    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem deathParticle;

    enum State {Dying, Transcending, Alive};
    State state = State.Alive;

    Rigidbody rigidBody;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if( state == State.Alive)
        {
            RespondToThrustInput();
            HandleRotation();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if( state != State.Alive) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //Do Nothing 
                break;

            case "Finish":
                StartSuccessSequence();
                break;    
            
            default:
               StartdeathSequence(); 
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(WinSound);
        successParticle.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartdeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(CrashSound);
        deathParticle.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()

    {
        if( Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            ApplyThrust();
        }
        else{
            audioSource.Stop();
            //mainEngineParticle.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if(!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if(!mainEngineParticle.isPlaying)
        {
            mainEngineParticle.Play();
        }
    }

    private void HandleRotation()
    {
        rigidBody.freezeRotation = true; //take manual control of rotation

        float rotationThisframe = rcsThrust * Time.deltaTime;
        
        if( Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationThisframe);
        }
        else if( Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationThisframe);
        }

        rigidBody.freezeRotation = false; //let the physics take control
    }
}
