using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillMovement : MonoBehaviour
{
    private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.back * HillsManager.instance.speed * Time.deltaTime;
        transform.position += movement;

        if (transform.position.z < player.transform.position.z)
        {
            Destroy(gameObject);
            HillsManager.instance.SpawnHill();
        }
    }
}
