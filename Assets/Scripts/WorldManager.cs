using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// WorldManager.cs
// Josiah Nistor
// Keeps track of all the lists of GameObjects and only updates lists when needed, in order to minimize find calls and look ups
public class WorldManager : MonoBehaviour
{
    [SerializeField] private GameObject m_gnomes;
    [SerializeField] private GameObject m_storages;
    [SerializeField] private GameObject m_foods;
    [SerializeField] private GameObject m_waters;
    [SerializeField] private GameObject m_beds;
    [SerializeField] private GameObject m_shrines;
    [SerializeField] private GameObject m_workshops;
    [SerializeField] private GameObject m_stones;
    [SerializeField] private GameObject m_woods;
    [SerializeField] private GameObject m_healths;
    [SerializeField] private GameObject m_enemies;
    [SerializeField] private GameObject m_fibers;

    [SerializeField] private GnomeManager m_gnomeManger;


    private int m_structureCount;
    private int m_enemyTargetsCount;
    private int m_stoneLocationCount;
    private int m_woodLocationCount;

    private List<GameObject> m_socialLocations = new List<GameObject>();
    private List<GameObject> m_storageLocations = new List<GameObject>();
    private List<GameObject> m_foodLocations = new List<GameObject>();
    private List<GameObject> m_waterLocations = new List<GameObject>();
    private List<GameObject> m_restLocations = new List<GameObject>();
    private List<GameObject> m_shrineLocations = new List<GameObject>();
    private List<GameObject> m_workshopLocations = new List<GameObject>();
    private List<GameObject> m_stoneLocations = new List<GameObject>();
    private List<GameObject> m_woodLocations = new List<GameObject>();
    private List<GameObject> m_healthLocations = new List<GameObject>();
    private List<GameObject> m_enemyLocations = new List<GameObject>();
    private List<GameObject> m_fiberLocations = new List<GameObject>();
    private List<GameObject> m_structureLocations = new List<GameObject>();
    private List<GameObject> m_enemyTargetsLocations = new List<GameObject>();
    private List<GameObject> m_storageLocationsWithItem = new List<GameObject>();

    // Fills all the lists on start
    void Start()
    {
        UpdateList<SocialSpace>(m_gnomes, m_socialLocations);
        UpdateList<StorageArea>(m_storages, m_storageLocations);
        UpdateList<FoodDest>(m_foods, m_foodLocations);
        UpdateList<WaterDest>(m_waters, m_waterLocations);
        UpdateList<RestArea>(m_beds, m_restLocations);
        UpdateList<Shrine>(m_shrines, m_shrineLocations);
        UpdateList<Workshop>(m_workshops, m_workshopLocations);
        UpdateList<HeallthArea>(m_healths, m_healthLocations);
        UpdateList<Enemy>(m_enemies, m_enemyLocations);
        UpdateList<FiberDest>(m_fibers, m_fiberLocations);
        UpdateListOffTag(m_structureLocations, "Structure");
        m_structureCount = m_structureLocations.Count;
        m_enemyTargetsLocations = m_socialLocations.Concat(m_structureLocations).ToList();
        m_enemyTargetsCount = m_enemyTargetsLocations.Count;
    }

    // Updates lists only when the parent object has a diffrent number of objects then the list
    void Update()
    {
        if (m_gnomes.transform.childCount != m_socialLocations.Count)
            UpdateList<SocialSpace>(m_gnomes, m_socialLocations);

        if (m_storages.transform.childCount != m_storageLocations.Count)
            UpdateList<StorageArea>(m_storages, m_storageLocations);

        if (m_foods.transform.childCount != m_waterLocations.Count)
            UpdateList<FoodDest>(m_foods, m_waterLocations);

        if (m_waters.transform.childCount != m_waterLocations.Count)
            UpdateList<WaterDest>(m_waters, m_waterLocations);

        if (m_beds.transform.childCount != m_restLocations.Count)
            UpdateList<RestArea>(m_beds, m_restLocations);

        if (m_shrines.transform.childCount != m_shrineLocations.Count)
            UpdateList<Shrine>(m_shrines, m_shrineLocations);

        if (m_workshops.transform.childCount != m_workshopLocations.Count)
            UpdateList<Workshop>(m_workshops, m_workshopLocations);

        if (m_stoneLocationCount != m_stoneLocations.Count)
        {
            UpdateCombinedList(m_gnomeManger.GetSelectedObjects(), m_stoneLocations);
            m_stoneLocationCount = m_stoneLocations.Count;
        }
        
        if (m_woodLocationCount != m_woodLocations.Count)
        {
            UpdateCombinedList(m_gnomeManger.GetSelectedObjects(), m_woodLocations);
            m_woodLocationCount = m_woodLocations.Count;
        }

        if (m_healths.transform.childCount != m_healthLocations.Count)
            UpdateList<HeallthArea>(m_healths, m_healthLocations);

        if (m_enemies.transform.childCount != m_enemyLocations.Count)
            UpdateList<Enemy>(m_enemies, m_enemyLocations);

        if (m_fibers.transform.childCount != m_fiberLocations.Count)
            UpdateList<FiberDest>(m_fibers, m_fiberLocations);

        if (m_structureCount != m_structureLocations.Count)
        {
            UpdateListOffTag(m_structureLocations, "Structure");
            m_structureCount = m_structureLocations.Count;
        }

        if (m_enemyTargetsCount != m_enemyTargetsLocations.Count)
        {
            m_enemyTargetsLocations = m_socialLocations.Concat(m_structureLocations).ToList();
            m_enemyTargetsCount = m_enemyTargetsLocations.Count;
        }
    }

    // Update the list by finding a object by a tags name from the scene
    private void UpdateListOffTag(List<GameObject> locations, string tag)
    {
        GameObject[] spaces = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject space in spaces)
        {
            if (!locations.Contains(space.gameObject))
            {
                locations.Add(space.gameObject);
            }
        }

    }

    // Update the list by getting the children from the parent, and adding them to the list if they are not in it yet
    private void UpdateList<T>(GameObject source, List<GameObject> locations) where T : Destination
    {
        T[] spaces = source.GetComponentsInChildren<T>();

        foreach (T space in spaces)
        {
            if (!locations.Contains(space.gameObject))
            {
                locations.Add(space.gameObject);
            }
        }
    }

    // Update the list by getting the children from the parent, and adding them to the list if they are not in it yet
    private void UpdateCombinedList(List<GameObject> source, List<GameObject> locations)
    {
        foreach (GameObject destination in source)
        {
            if (destination.GetComponent<WoodDest>() != null)
                if (!m_woodLocations.Contains(destination))
                    m_woodLocations.Add(destination);
            else if (destination.GetComponent<StoneDest>() != null)
                if (!m_stoneLocations.Contains(destination))
                    m_stoneLocations.Add(destination);
        }
    }


    // Getters
    public List<GameObject> GetSocialLocations() { return m_socialLocations; }
    public List<GameObject> GetStorageLocations() { return m_storageLocations; }
    public List<GameObject> GetFoodLocations() { return m_foodLocations; }
    public List<GameObject> GetWaterLocations() { return m_waterLocations; }
    public List<GameObject> GetRestLocations() { return m_restLocations; }
    public List<GameObject> GetShrineLocations() { return m_shrineLocations; }
    public List<GameObject> GetWorkshopLocations() { return m_workshopLocations; }
    public List<GameObject> GetStoneLocations() { return m_stoneLocations; }
    public List<GameObject> GetWoodLocations() { return m_woodLocations; }
    public List<GameObject> GetHealthLocations() { return m_healthLocations; }
    public List<GameObject> GetEnemyLocations() { return m_enemyLocations; }
    public List<GameObject> GetFiberLocations() { return m_fiberLocations; }
    public List<GameObject> GetStructureLocations() { return m_structureLocations; }
    public List<GameObject> GetEnemyTargetsLocations() { return m_enemyTargetsLocations; }
    public List<GameObject> GetStorageLocationsWithItem() { return m_storageLocationsWithItem; }

    // Update wood and stone loactions from the slected locations
    public void UpdateSelectedLocations(List<GameObject> destinations = null)
    {
        if (destinations != null && destinations.Count > 0)
        {
            foreach (GameObject destination in destinations)
            {
                if (destination.GetComponent<WoodDest>() != null)
                {
                    if (!m_woodLocations.Contains(destination))
                        m_woodLocations.Add(destination);

                }
                if (destinations[0].GetComponent<StoneDest>() != null)
                {
                    if (!m_stoneLocations.Contains(destination))
                        m_stoneLocations.Add(destination);
                }
            }
        }
    }

    // Find a location based on its name
    public List<GameObject> FindLocation(string name)
    {
        if (name == m_gnomes.name)
            return m_socialLocations;
        else if (name == m_storages.name)
            return m_storageLocations;
        else if (name == m_foods.name)
            return m_foodLocations;
        else if (name == m_waters.name)
            return m_waterLocations;
        else if (name == m_beds.name)
            return m_restLocations;
        else if (name == m_shrines.name)
            return m_shrineLocations;
        else if (name == m_workshops.name)
            return m_workshopLocations;
        else if (name == m_healths.name)
            return m_healthLocations;
        else
            return null;
    }
}               