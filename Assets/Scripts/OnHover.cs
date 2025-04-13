using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// OnHover.cs
// Josiah Nistor
// Diplayes the name of whatever object the mouse hovers over
public class OnHover : MonoBehaviour
{
    [SerializeField] private TMP_Text m_name;
    private Vector3 m_mousePos;

    // Cast a ray from the camera to where the mouse is pointing and if on an object, display a name tag
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10000))
        {
            GameObject hitGameObject = hit.transform.gameObject;
            if (!hitGameObject.CompareTag("NoName"))
            {
                m_name.transform.gameObject.SetActive(true);
                Vector2 screenPos = Input.mousePosition;
                screenPos.x += 100;
                m_name.transform.position = screenPos;
                m_name.text = hitGameObject.name;
            }
            else
            {
                m_name.transform.gameObject.SetActive(false);
            }
        }
    }
}
