using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private Vector3 resultPos;

    //private void OnEnable()
    //{
    //    Vector3 offset = new Vector3(Random.Range(-posX, posX), Random.Range(-posY, posY), 0);
    //    resultPos = targetPos.localPosition + offset;
    //}

    //private void OnDisable()
    //{
    //    transform.localPosition = Vector3.zero;
    //}

    private void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, resultPos, 3f * Time.deltaTime);
    }

    public void SetTargetPosition(Vector3 pos)
    {
        resultPos = pos;
    }
}
