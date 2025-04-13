using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Destination.cs
// Josiah Nistor
// Base class for other desinations where gnomes go in order to fuffill their needs, this is inherited by other locations
public class Destination : MonoBehaviour
{
    [SerializeField] protected float m_waitTime = 2f;
    [SerializeField] protected int m_updateAmmount = 50;
    [SerializeField] protected int m_health = 100;
    protected Ground m_ground;

    protected GameObject m_ocupier = null;

    // Find ground in scene
    protected void Start()
    {
        m_ground = FindAnyObjectByType<Ground>();
    }

    // On trigger enter if NPC tag start behavior after a delay
    void OnTriggerEnter(Collider other)
    {
        if (m_ocupier != null && m_ocupier != other)
            return;
        if (other.CompareTag("NPC"))
        {
            m_ocupier = other.gameObject;
            StartCoroutine(Wait(m_waitTime, m_ocupier));
        }
    }

    // On trigger stay if NPC tag start behavior after a delay, or if enemy attack the structure
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("NPC") && m_ocupier == null)
        {
            m_ocupier = other.gameObject;
            StartCoroutine(Wait(m_waitTime, m_ocupier));
        }
        else if (other.CompareTag("Enemy") && !gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<EnemyAI>())
                other.gameObject.GetComponent<EnemyAI>().Attack(this);
        }
    }

    // reset ocupier
    void OnTriggerExit(Collider other)
    {
        m_ocupier = null;
    }

    // Base wait function
    protected virtual IEnumerator Wait(float waitTime, GameObject collison)
    {
        yield return new WaitForSeconds(waitTime);
    }

    // Getters
    public GameObject GetIsOcupied() { return m_ocupier; }
    public int GetUpdateAmount() { return m_updateAmmount; }

    // Update stat to new ammount
    public void ChangeUpdateAmount(int newAmount)
    {
        m_updateAmmount = newAmount;
    }

    // Update Health by change
    public void UpdateHealth(int change)
    {
        m_health += change;
        if (m_health <= 0)
        {
            m_ground.SetNavMeshUpdate();
            Destroy(gameObject);
        }
    }
}
