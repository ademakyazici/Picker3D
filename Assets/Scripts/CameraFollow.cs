using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    private Vector3 distance;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        distance = transform.position - player.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, player.position.y + distance.y, player.position.z + distance.z);
    }
}
