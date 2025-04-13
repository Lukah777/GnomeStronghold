using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

// Ground.cs
// Josiah Nistor
// Spawns new resources, enemies, and rebakes navmesh 
public class Ground : MonoBehaviour
{
    [SerializeField] private GameObject m_food1;
    [SerializeField] private GameObject m_food2;
    [SerializeField] private GameObject m_food3;
    [SerializeField] private GameObject m_foods;
    [SerializeField] private GameObject m_wood1;
    [SerializeField] private GameObject m_wood2;
    [SerializeField] private GameObject m_wood3;
    [SerializeField] private GameObject m_woods;
    [SerializeField] private GameObject m_fiber;
    [SerializeField] private GameObject m_fiber2;
    [SerializeField] private GameObject m_fiber3;
    [SerializeField] private GameObject m_fibers;
    [SerializeField] private GameObject m_enemy;
    [SerializeField] private GameObject m_largeEnemy;
    [SerializeField] private GameObject m_enemys;

    private bool m_updateNavMesh = false;
    private bool m_timer = false;
    private bool m_timer2 = false;
    private NavMeshSurface m_meshSurface;

    // Asign NavMeshSurface
    private void Start()
    {
        m_meshSurface = GetComponent<NavMeshSurface>();
    }

    // If timers not running rebuild navmesh, and spawn resources
    private void Update()
    {
        if (m_timer == false)
        {
            m_timer = true;
            StartCoroutine(RebuildNavMesh());
        }
        if (m_timer2 == false)
        {
            m_timer2 = true;
            StartCoroutine(SpawnResources());
        }
    }

    // Rebuild nav mesh once a second if requested
    private IEnumerator RebuildNavMesh()
    {
        yield return new WaitForSeconds(1f);
        if (m_updateNavMesh == true)
        {
            m_meshSurface.BuildNavMesh();
            m_updateNavMesh = false;
        }
        
        m_timer = false;
    }

    // Spawn a random resources or enemy encouter once every 2 minutes
    private IEnumerator SpawnResources()
    {
        yield return new WaitForSeconds(120f);
        int rand = Random.Range(0, 12);
        Vector3 location = new Vector3(Random.Range(5f, 35f), 0f, Random.Range(-50f, 50f));
        if (rand == 0)
        { 
            Instantiate(m_wood1, location, Quaternion.identity, m_woods.transform);
        }
        else if (rand == 1)
        {
            Instantiate(m_wood2, location, Quaternion.identity, m_woods.transform);
        }
        else if (rand == 2)
        {
            Instantiate(m_wood3, location, Quaternion.identity, m_woods.transform);
        }
        else if (rand == 3)
        {
            Instantiate(m_food1, location, Quaternion.identity, m_foods.transform);
        }
        else if (rand == 4)
        {
            Instantiate(m_food2, location, Quaternion.identity, m_foods.transform);
        }
        else if (rand == 5)
        {
            Instantiate(m_food3, location, Quaternion.identity, m_foods.transform);
        }
        else if (rand == 6)
        {
            Instantiate(m_fiber, location, Quaternion.identity, m_fibers.transform);
        }
        else if (rand == 7)
        {
            Instantiate(m_fiber2, location, Quaternion.identity, m_fibers.transform);
        }
        else if (rand == 8)
        {
            Instantiate(m_fiber3, location, Quaternion.identity, m_fibers.transform);
        }
        else if (rand == 9)       
        {
            Instantiate(m_enemy, location, Quaternion.identity, m_enemys.transform);
        }
        else if (rand == 10)
        {
            Instantiate(m_largeEnemy, location, Quaternion.identity, m_enemys.transform);
        }
        else
        {
            Instantiate(m_enemy, location, Quaternion.identity, m_enemys.transform);
            Instantiate(m_enemy, location + new Vector3(5, 0, 5), Quaternion.identity, m_enemys.transform);
            Instantiate(m_enemy, location + new Vector3(-5, 0, 5), Quaternion.identity, m_enemys.transform);
            Instantiate(m_enemy, location + new Vector3(5, 0, -5), Quaternion.identity, m_enemys.transform);
            Instantiate(m_enemy, location + new Vector3(-5, 0, -5), Quaternion.identity, m_enemys.transform);
        }
        m_updateNavMesh = true;
        m_timer2 = false;
    }

    // Setter
    public void SetNavMeshUpdate()
    {
        m_updateNavMesh = true;
    }
}
