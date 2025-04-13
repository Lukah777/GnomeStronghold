using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Curve { Linear, Exponentnal, SquareRoot, Sigmoid }


// Decision.cs
// Josiah Nistor
// Base decision class that is modified in the inspector to create each decision that the AI uses to track its many needs, and called using templates
public class Decision : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent m_agent;
    [SerializeField] protected Vector3 m_homePos;
    [SerializeField] protected float m_need = 0;
    [SerializeField] protected float m_updateNeedModifier = 0;
    [SerializeField] protected float m_scoredNeedThreshold = 0.8f;
    [SerializeField] protected float m_needMax = 100;
    [SerializeField] protected GameObject m_storageAreas;
    [SerializeField] private Curve m_curve;
    [SerializeField] private int m_x = 2;
    [SerializeField] protected Decision m_healthDecision;

    [SerializeField] private GnomeAI m_gnomeAI;

    private InventoryManager m_InvManger;
    private WorldManager m_worldManager;

    protected List<GameObject> m_locations;
    protected float m_updateMod = 1;

    // Initializes refrences to world and inventory managers, and gives a refrence to locations from the world manager
    public void Initialize(WorldManager worldManager, InventoryManager invManager, string nameToFind = "")
    {
        m_worldManager = worldManager;
        m_InvManger = invManager;

        if (nameToFind != "")
        {
            m_locations = m_worldManager.FindLocation(nameToFind);
        }
    }
    
    // Finds the closest and set pathfinding destination, using established or given locations
    public virtual bool FindClosestAndMove(int noneFoodWater = 0, List<GameObject> locations = null)
    {
        m_homePos = gameObject.GetComponent<Want>().GetBedLocation();

        if (locations == null)
            locations = m_locations;

        Vector3 closest = new Vector3(1000, 1000, 1000);
        foreach (GameObject location in locations)
        {
            if (location != null && location.activeSelf && location.transform.parent.gameObject != gameObject)
            {
                GameObject IsOcupied = location.GetComponent<Destination>().GetIsOcupied();
                if (IsOcupied == gameObject || IsOcupied == null)
                {
                    float x = gameObject.transform.position.x;
                    float z = gameObject.transform.position.z;
                    float locX = location.transform.position.x;
                    float locZ = location.transform.position.z;
                    if ((Mathf.Abs(x - locX) + Mathf.Abs(z - locZ))
                        < (Mathf.Abs(x - closest.x) + Mathf.Abs(z - closest.z)))
                        {
                        if (location.GetComponent<Workshop>() != null)
                        {
                            Job workshopGetCompletedJob = location.GetComponent<Workshop>().GetCompletedJob(m_gnomeAI);

                            if (!workshopGetCompletedJob.workshopBeavior(m_InvManger))
                                continue;  
                        }
                        
                        closest = location.transform.position;
                        if (m_gnomeAI)
                            m_gnomeAI.SetDest(location);
                    }
                }
            } 
        }
        if (m_worldManager.GetStorageLocations() != null && noneFoodWater != 0)
        {
            foreach (GameObject location in m_worldManager.GetStorageLocations())
            {
                if (location != null)
                {
                    GameObject IsOcupied = location.GetComponent<Destination>().GetIsOcupied();
                    if (IsOcupied == gameObject || IsOcupied == null)
                    {
                        bool cont = false;
                        if (noneFoodWater == 1)
                        {
                            if (location.GetComponent<StorageArea>().GetFood() > 0)
                                cont = true;
                        }
                        else if (noneFoodWater == 2)
                        {
                            if (location.GetComponent<StorageArea>().GetDrinks() > 0)
                                cont = true;
                        }

                        if (cont == true && (Mathf.Abs(gameObject.transform.position.x - location.transform.position.x) + Mathf.Abs(gameObject.transform.position.z - location.transform.position.z))
                            < (Mathf.Abs(gameObject.transform.position.x - closest.x) + Mathf.Abs(gameObject.transform.position.z - closest.z)))
                        {
                            closest = location.transform.position;
                            if (m_gnomeAI)
                                m_gnomeAI.SetDest(location);
                        }
                    }
                }
            }
        }
        if (closest != new Vector3(1000, 1000, 1000))
        {
            m_agent.destination = closest;
            return true;
        }
        else
            m_agent.destination = m_homePos;
        return false;
    }

    // Check if need is over the threshold then find the closest location to deal with that need and damage if over max need 
    public bool UpdateDecision(int i =  0)
    {
        if (m_need > m_needMax)
        {
            m_healthDecision.UpdateNeed(1);
            FindClosestAndMove(i);
            return true;
        }

        if (ScoreNeed() > m_scoredNeedThreshold)
        {
            FindClosestAndMove(i);
            return true;

        }
        return false;
    }

    // Health only: Check if health is over threshold and if so find the closest hospital, and if over max health need destroy the gnome
    public bool UpdateHealth()
    {
        if (m_need > m_needMax)
        {
            Destroy(gameObject);
            return true;
        }
        if (ScoreNeed() > m_scoredNeedThreshold)
        {
            FindClosestAndMove();
            return true;
        }
        return false;
    }

    // Score need based on type of curve and modifiers
    public float ScoreNeed()
    {
        float normalizedNeed = m_need / m_needMax;
        float result = 0;
        if (m_curve == Curve.Linear)
            result = normalizedNeed;
        else if (m_curve == Curve.Exponentnal)
            result = Mathf.Pow(normalizedNeed, m_x);
        else if (m_curve == Curve.SquareRoot)
            result = Mathf.Sqrt(normalizedNeed);
        else if (m_curve == Curve.Sigmoid)
            result = (1 / (1 + Mathf.Pow(normalizedNeed / (1 - normalizedNeed), -m_x)));

        return result;
    }

    // Tick down up the need by the change and and update mod
    public void UpdateNeed(double change)
    {
        float mod = 1;
        if (change > 0)
            mod = m_updateMod;

        m_need += (float)((change - m_updateNeedModifier) * mod);
        if (m_need < 0)
            m_need = 0;
    }

    // Setters and Getters
    public void SetUpdateMod(float mod) { m_updateMod = mod; }

    public void SetUpdateNeedMod(float newNeedMod) { m_updateNeedModifier = newNeedMod; }

    public float GetUpdateMod() { return m_updateMod; }

    public float GetNeed() { return m_need; }

    public float GetMaxNeed() { return m_needMax; }

    public void SetMaxNeed(float maxNeed)
    {
        m_needMax = maxNeed;
        m_need = m_needMax;
    }

    // Only used by rest: Looks through rest locations for beds that have not been claimed
    public GameObject FindClosestNonClaimedBed()
    {
        foreach (GameObject restArea in m_worldManager.GetRestLocations())
        {
            if (!restArea.GetComponent<RestArea>().GetClaimed())
            {
                restArea.GetComponent<RestArea>().SetClaimed();
                return restArea;
            }
        }
        return null;
    }

    // Only used by storage: Looks through storages to find a specific item type
    public bool FindNearestItem(ItemType itemType)
    {
        m_worldManager.GetStorageLocationsWithItem().Clear();
        foreach (GameObject storage in m_worldManager.GetStorageLocations())
        {
            foreach (Object obj in storage.GetComponent<StorageArea>().GetObjects())
            {
                if (obj.GetObjectType() == itemType)
                    m_worldManager.GetStorageLocationsWithItem().Add(storage);
            }
        }
        if (m_worldManager.GetStorageLocationsWithItem().Count > 0)
        {
            m_gnomeAI.SetCompletedJob(m_gnomeAI.m_getItem);
            FindClosestAndMove(0, m_worldManager.GetStorageLocationsWithItem());
            return true;
        }
        return false;
    }
}
