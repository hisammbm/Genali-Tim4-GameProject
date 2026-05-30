using System.Collections;
using UnityEngine;

public class MaterialOffset : MonoBehaviour
{
    private Material targetMaterial;
    private Vector2 currentOffset = Vector2.zero;


    [SerializeField] private float stepAmount = 0.1f; 
    [SerializeField] private float timeStep = 0.5f;   
    [SerializeField] private float maxYValue = 1.0f; 

    private void Awake()
    {
        targetMaterial = GetComponent<Renderer>().sharedMaterial;
       
    }

    private void Start()
    {
        currentOffset = new Vector2(0f, 0f);
        StartCoroutine(StepOffsetRoutine());
    }

    private IEnumerator StepOffsetRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeStep);
            currentOffset.y += stepAmount;

            if (currentOffset.y >= maxYValue)
            {
                currentOffset.y = 0f;
            }
            if (targetMaterial != null)
            {
                targetMaterial.SetTextureOffset("_BaseMap", currentOffset);
            }
        }
    }
}