using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMover : Mover
{

    // Variable to hold the Rigidbody Component
    private Rigidbody rb;
    private Transform tf;
    public AudioSource rotatingAudioSource;
    public AudioClip tankRotateClip;

    // Start is called before the first frame update
    public override void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
    }

    public override void Move(Vector3 direction, float speed)
    {
        // This line takes the class this component inherits, normalizes the whole vector, and moves it according to the speed per second
        Vector3 moveVector = direction.normalized * speed * Time.deltaTime;
        // This line takes the rigidbody component and moves that along with the object
        rb.MovePosition(rb.position + moveVector);
    }

    public override void Rotate(float turnSpeed)
    {
        tf.Rotate(0, turnSpeed * Time.deltaTime, 0);
    }

    public override void PlayRotateSound()
    {
        if (!rotatingAudioSource.isPlaying)
        {
            //AudioSource.PlayClipAtPoint(tankRotateClip, gameObject.transform.position);
            rotatingAudioSource.PlayOneShot(tankRotateClip);
        }
    }
}
