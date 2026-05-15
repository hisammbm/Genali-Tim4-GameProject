using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxManager : MonoBehaviour
{
    public Material[] skyboxes;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeSkybox();
    }
    public void ChangeSkybox()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            int index = Random.Range(0, skyboxes.Length);
            RenderSettings.skybox = skyboxes[index];
        }
    }
}
