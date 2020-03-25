using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField] float rotateThrust = 100f;
    [SerializeField] float thrustSpeed = 10f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }


    private void FixedUpdate()
    {
        thrust();
        rotate();
    }

    private void thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!audioSource.isPlaying)
                audioSource.Play();

            rigidBody.AddRelativeForce(Vector3.up * Time.deltaTime * thrustSpeed);
        }
        else
        {
            audioSource.Stop();
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
        if(collision.gameObject.tag != "friendly")
        {
            this.gameObject.SetActive(false);
        }
    }


}
