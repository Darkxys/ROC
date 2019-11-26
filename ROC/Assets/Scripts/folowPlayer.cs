using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class folowPlayer : MonoBehaviour
{

    private Vector3 posCamera;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            posCamera = player.transform.position;
            posCamera.z = -5;
            GetComponent<Transform>().position = posCamera;
        }
    }
}
