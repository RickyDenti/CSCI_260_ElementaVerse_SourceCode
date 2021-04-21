using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsCoroutine : MonoBehaviour
{
    private int timerForPlatformSet = 7;

    private bool firstSet = true;
    private bool secondSet = false;

    public GameObject firstSetPlatforms;
    public GameObject secondSetPlatforms;
    void Start()
    {
        firstSetPlatforms = GameObject.Find("First Platform Set");
        secondSetPlatforms = GameObject.Find("Second Platform Set");
    }

    void Update()
    {
        StartCoroutine(ChangePlatformVisibility(firstSet, secondSet));
        firstSet = !firstSet;
        secondSet = !secondSet;
    }

    IEnumerator ChangePlatformVisibility(bool firstActive, bool secondActive)
    {
        yield return new WaitForSeconds(timerForPlatformSet);
        firstSetPlatforms.SetActive(!firstActive);
        secondSetPlatforms.SetActive(!secondActive);
    }
}
