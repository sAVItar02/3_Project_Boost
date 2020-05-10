﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

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
        Thrust();
        HandleRotation();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("WOKKAY");
                break;
            
            default:
                print("DED");
                break;
        }
    }

    private void Thrust()

    {
        if( Input.GetKey(KeyCode.Space) )
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if(!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else{
            audioSource.Stop();
        }
    }

    private void HandleRotation()
    {
        rigidBody.freezeRotation = true; //take manual control of rotation

        float rotationThisframe = rcsThrust * Time.deltaTime;
        
        if( Input.GetKey(KeyCode.A) )
        {
            transform.Rotate(Vector3.forward * rotationThisframe);
        }
        else if( Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisframe);
        }

        rigidBody.freezeRotation = false; //let the physics take control
    }
}
