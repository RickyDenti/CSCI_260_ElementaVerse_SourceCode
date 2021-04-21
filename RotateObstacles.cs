using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObstacles : MonoBehaviour
{
    public float degreePerSecond;
    
    private Vector3 offset = new Vector3();

    void Start()
    {
        offset = transform.position; // Set starting position
    }

    void Update()
    {
        transform.Rotate(new Vector3(0, Time.deltaTime * degreePerSecond, 0), Space.World); // Rotate y
    }
}
