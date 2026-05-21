using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HillsManager : MonoBehaviour
{
    public GameObject[] hills;
    public static HillsManager instance;  
    public float speed = 10f;
    public Transform spawnPoint;
    public Transform spawnPoint2;
    //public Material skybox;

    void Awake()
        {
            instance = this;
        }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnHill()
    {
        int index = Random.Range(0, hills.Length);
        Instantiate(hills[index], spawnPoint);
    }
}


