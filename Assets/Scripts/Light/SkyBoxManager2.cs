using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SkyBoxManager2 : MonoBehaviour
{
    public LightingManager lightingManager;
    public Material[] skyboxMaterials = new Material[5];

    private int lastAppliedIndex = -1;

    void Update()
    {

        if (lightingManager == null || skyboxMaterials.Length < 5) return;

        float currentTime = lightingManager.TimeOfDay;

        float scaledTime = (currentTime / 24f) * 5f;
        int currentIdx = Mathf.FloorToInt(scaledTime) % 5;


        if (currentIdx != lastAppliedIndex)
        {
            if (skyboxMaterials[currentIdx] != null)
            {
                RenderSettings.skybox = skyboxMaterials[currentIdx];

                DynamicGI.UpdateEnvironment();

                lastAppliedIndex = currentIdx;
            }
        }
    }
}
