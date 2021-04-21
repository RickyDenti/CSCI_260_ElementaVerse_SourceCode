using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemies : MonoBehaviour
{
    private float speed = 5.0f;
    private float zBound = 37f;

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        if (transform.position.z > zBound)
        {
            Destroy(gameObject);
        }
    }
}
