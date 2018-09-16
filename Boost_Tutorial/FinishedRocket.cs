using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    Rigidbody rigidbody;
    

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] AudioSource audiosource;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip victory;


    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem thrustingParticles;



    enum State {
        Alive, Dying, Transcending
    }
    State state = State.Alive;


    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
    //TODO somewhere stop sound.
	void Update () {
        if (state == State.Alive) {
            RespondToThrust();
            Rotate();
        }
	}

    void OnCollisionEnter(Collision collision) {

        if (state != State.Alive) {
            return;
        }

        switch(collision.gameObject.tag) {
            case "friendly": 
                    print("hit friendly");
                break;
            case "Finish":
                state = State.Transcending;
                audiosource.Stop();
                audiosource.PlayOneShot(victory);
                successParticles.Play();
                Invoke("LoadNextScene", 1f); //paramertise this
                break;
            default:
                state = State.Dying;
                audiosource.Stop();
                audiosource.PlayOneShot(death);
                explosionParticles.Play();
                Invoke("LoadPreviousLevel", 1f);
                print("Dead");
                return;

        }
    }

    private void LoadNextScene() {
        SceneManager.LoadScene(1);
    }

    private void LoadPreviousLevel() {
        SceneManager.LoadScene(0);
    }

    private void Rotate() {
     
        rigidbody.freezeRotation = true; // take manual control of rotation


        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        } else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidbody.freezeRotation = false; // let physics take control of rotation now.
    }

    private void RespondToThrust() {
    
        if (Input.GetKey(KeyCode.Space)) { //can thrust while rotating.
            ApplyThrust();
   
        } else {
            audiosource.Stop();
            thrustingParticles.Stop();
        }
    }

    private void ApplyThrust() {
        float thrustingThisFrame = mainThrust * Time.deltaTime;
        rigidbody.AddRelativeForce(Vector3.up * thrustingThisFrame);
        if (!audiosource.isPlaying) {
            audiosource.PlayOneShot(mainEngine);
            thrustingParticles.Play();
        }
    }
}
