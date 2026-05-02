using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveX, moveY;
    private Quaternion angle;
    [SerializeField] private float movementSpeed = 5f, smoothTurn;
    [SerializeField] private float minX = -9.9f, maxX = 9.9f;
    [SerializeField] private float minY = -3f, maxY = 9.7f;
    void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, moveY, 0) * movementSpeed * Time.deltaTime;
        transform.position += movement;

        angle.x = Mathf.Lerp(angle.x, moveX * movementSpeed, smoothTurn * Time.deltaTime);
        angle.y = Mathf.Lerp(angle.y, moveY * movementSpeed, smoothTurn * Time.deltaTime);

        angle.x = Mathf.Clamp(angle.x, -55, 55);
        angle.y = Mathf.Clamp(angle.y, -25, 25);

        transform.rotation = Quaternion.Euler(-angle.y, 0, -angle.x);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY),
            transform.position.z
        );
    }
}
