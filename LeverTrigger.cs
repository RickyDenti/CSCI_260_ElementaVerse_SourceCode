using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverTrigger : MonoBehaviour
{
    private Animator lever;
    private RotateObstacles rotateObstaclesScriptOne;
    private RotateObstacles rotateObstaclesScriptTwo;
    private float obstacleSpeedDivider;

    public bool isLeverTurned = false;

    public GameObject fireObstacleOne;
    public GameObject fireObstacleTwo;

    void Start()
    {
        lever = gameObject.GetComponent<Animator>();
        fireObstacleOne = GameObject.Find("Fire Obstacle 1");
        fireObstacleTwo = GameObject.Find("Fire Obstacle 2");
        rotateObstaclesScriptOne = fireObstacleOne.GetComponent<RotateObstacles>();
        rotateObstaclesScriptTwo = fireObstacleTwo.GetComponent<RotateObstacles>();
    }

    void FixedUpdate()
    {
        if (isLeverTurned)
        {
            LeverIsTurned();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isLeverTurned = true;
            lever.Play("Lever1");
        }
    }

    void LeverIsTurned()
    {
        obstacleSpeedDivider = 72;
        rotateObstaclesScriptOne.degreePerSecond = obstacleSpeedDivider;
        rotateObstaclesScriptTwo.degreePerSecond = obstacleSpeedDivider;
    }
}
