using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Sway Settings")]
    public bool canSway = true;
    public float swayAmount = 1.5f;
    public float swaySpeed = 2f;

    [Header("Rotation Settings")]
    public bool autoRotateToPlayerIfTargeted = true;
    public float rotationSpeed = 5f;
    public float rotationThresholdX = 1.5f;
    public float rotationThresholdY = 1.5f;

    private Vector3 resultPos;
    private bool hasReachedTarget = false;
    private float randomOffset;
    private Quaternion defaultRotation;
    private EnemyShooting enemyShooting;
    private bool hasSetSway = false;

    private void Start()
    {
        if (!hasSetSway)
        {
            randomOffset = Random.Range(0f, 100f);
        }
        enemyShooting = GetComponent<EnemyShooting>();
        defaultRotation = transform.rotation;
    }

    private void Update()
    {
        // 1. Pergerakan Posisi
        if (!hasReachedTarget)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, resultPos, 3f * Time.deltaTime);
            if (Vector3.Distance(transform.localPosition, resultPos) < 0.15f)
            {
                hasReachedTarget = true;
            }
        }
        else
        {
            if (canSway)
            {
                float newX = resultPos.x + Mathf.Sin((Time.time + randomOffset) * swaySpeed) * swayAmount;
                // Still Lerp the Y and Z position just in case
                float newY = Mathf.Lerp(transform.localPosition.y, resultPos.y, 3f * Time.deltaTime);
                transform.localPosition = new Vector3(newX, newY, transform.localPosition.z);
            }
        }

        // 2. Rotasi Mengikuti Player (jika bertipe Targeted)
        if (autoRotateToPlayerIfTargeted && enemyShooting != null && enemyShooting.shootType == ShootType.Targeted)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector3 directionToPlayer = player.transform.position - transform.position;
                
                if (directionToPlayer != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            // Kembali ke rotasi awal/default jika tidak membidik player
            transform.rotation = Quaternion.Lerp(transform.rotation, defaultRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void SetTargetPosition(Vector3 pos)
    {
        resultPos = pos;
        hasReachedTarget = false;
    }

    public void SetSwayOffset(float offset)
    {
        randomOffset = offset;
        hasSetSway = true;
    }
}
