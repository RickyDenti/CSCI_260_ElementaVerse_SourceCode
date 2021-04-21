using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCam : MonoBehaviour
{
    private float speed = 130f;

    private AudioSource focalPointAudio;

    private PlayerController playerController;

    public GameObject player;
    public GameObject camera;


    public AudioClip playerDestruction;

    private void Start()
    {
        focalPointAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * speed * Time.deltaTime);

        transform.position = player.transform.position; // Move focal point with player

    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.gameObject;

        if (player.activeSelf == false)
        {
            focalPointAudio.PlayOneShot(playerDestruction);
        }
    }
}
