using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CameraController.cs
// Josiah Nistor
// Handels user input to move the camera around the world
public class CameraController : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed = 10f;

    // Detect keyboard input to move camera around the world
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward  * m_moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * m_moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back * m_moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)) 
        { 
            transform.position += Vector3.right * m_moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += Vector3.up * m_moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.position += Vector3.down * m_moveSpeed * Time.deltaTime;
        }
    }
}
