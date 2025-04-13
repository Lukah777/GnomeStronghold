using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

// EnemyAI.cs
// Josiah Nistor
// EnemyAI has the enemy wander around the world, and if within range of a target move to that
public class EnemyAI : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent m_agent;
    private int m_serchTimer = 0;
    [SerializeField] protected Vector3 m_homePos = new Vector3(10, 0, 0);
    [SerializeField] private float m_attackSpeed = 1f;
    [SerializeField] private int m_searchRange = 10;
    private bool m_timer = false;

    private WorldManager m_worldManager;

    // Find and get a refrence to the world manager in the scene
    private void Start()
    {
        m_worldManager = FindFirstObjectByType<WorldManager>();

    }

    // If any targets in range move toward them, otherwise pick a random point every 300 frames
    private void Update()
    {
        if (m_worldManager.GetEnemyTargetsLocations().Count > 0)
        {
            if (!FindClosestAndMove())
            {
                if (m_serchTimer == 0)
                {
                    m_serchTimer = 300;
                    Vector3 location = new Vector3(Random.Range(15f, 35f), 0f, Random.Range(-50f, 50f));
                    m_agent.destination = location;
                }
                else
                    m_serchTimer--;
            }
        }
    }

    // Find closest target in that is in search range
    public virtual bool FindClosestAndMove()
    {
        Vector3 closest = new Vector3(1000, 1000, 1000);
        foreach (GameObject location in m_worldManager.GetEnemyTargetsLocations())
        {
            if (location != null)
            {
                float x = gameObject.transform.position.x;
                float z = gameObject.transform.position.z;
                float locX = location.transform.position.x;
                float locZ = location.transform.position.z;
                if ((Mathf.Abs(x - locX) + Mathf.Abs(z - locZ))
                    < (Mathf.Abs(x - closest.x) + Mathf.Abs(z - closest.z))
                    && (Mathf.Abs(x - locX) + Mathf.Abs(z - locZ)) < m_searchRange)
                {
                    closest = location.transform.position;
                }
            }
        }
        if (closest != new Vector3(1000, 1000, 1000))
        {
            m_agent.destination = closest;
            return true;
        }
        return false;
    }

    // Attack the target on a timer
    public void Attack(Destination destination)
    {
        if (m_timer == false)
        {
            m_timer = true;
            StartCoroutine(Hit(destination));
        }
    }

    // After wait attack the target
    private IEnumerator Hit(Destination destination)
    {
        yield return new WaitForSeconds(m_attackSpeed);
        destination.UpdateHealth(gameObject.GetComponentInChildren<Enemy>().GetUpdateAmount());
        m_timer = false;
    }
}
