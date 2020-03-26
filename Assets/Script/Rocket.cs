using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;

    AudioSource audioSource;
    [SerializeField] AudioClip thrustAudio;
    [SerializeField] AudioClip deathAudio;
    [SerializeField] AudioClip winAudio;

    [SerializeField] ParticleSystem thrustParticleSystem;
    [SerializeField] ParticleSystem deathParticleSystem;
    [SerializeField] ParticleSystem winParticleSystem;

    [SerializeField] float rotateThrust = 100f;
    [SerializeField] float thrustSpeed = 10f;
    [SerializeField] float delayTime = 3f;

    private bool delayed = false; //alive dead transitioning

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }


    private void FixedUpdate()
    {
        if (!delayed)
        {
            thrust();
            rotate();
        }
        else
        {
            //audioSource.Stop();
        }
    }

    private void thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(thrustAudio);
                thrustParticleSystem.Play();
            }
            rigidBody.AddRelativeForce(Vector3.up * Time.deltaTime * thrustSpeed);
        }
        else
        {
            audioSource.Stop();
            thrustParticleSystem.Stop();
        }
    }

    private void rotate()
    {
        rigidBody.freezeRotation = true;

        
        float rotationFPS = rotateThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationFPS);
            //rigidBody.AddRelativeTorque(Vector3.forward * rotationFPS);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationFPS);
            //rigidBody.AddRelativeTorque(Vector3.back * rotationFPS);
        }

        rigidBody.freezeRotation = false;

    }

    private void OnCollisionEnter(Collision collision)
    {

        string tag = collision.gameObject.tag;
        if (tag != "friendly")
        {
            int index = SceneManager.GetActiveScene().buildIndex;
            if (tag == "finish")
            {
                if (SceneManager.sceneCountInBuildSettings > index + 1) //next level
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(winAudio);
                    winParticleSystem.Play();
                    delayed = true;
                    Invoke("loadNextScene", delayTime);
                }
                else //restart
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(winAudio);
                    winParticleSystem.Play();
                    delayed = true;
                    Invoke("restart", delayTime);
                }
            }else
            {
                audioSource.Stop();
                audioSource.PlayOneShot(deathAudio);
                deathParticleSystem.Play();
                delayed = true;
                Invoke("reloadScene", delayTime);
            }
        }
    }

    private void loadNextScene()
    {
        delayed = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    private void restart()
    {
        SceneManager.LoadScene(0);
    }
    private void reloadScene()
    {
        //this.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }





}
