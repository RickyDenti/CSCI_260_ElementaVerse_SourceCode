using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFloating : MonoBehaviour
{
    private float degreePerSecond = 45.0f;
    private float amplitude = 0.5f;
    private Vector3 offset = new Vector3();
    private Vector3 temp = new Vector3();

    void Start()
    {
        // Set starting position
        offset = transform.position;
    }

    void Update()
    {
        // Rotate y
        transform.Rotate(new Vector3(0, Time.deltaTime * degreePerSecond, 0), Space.World);
        
        // Float up/down
        temp = offset;
        temp.y += Mathf.Sin(Time.fixedTime * Mathf.PI) * amplitude;

        transform.position = temp;
    }
}
