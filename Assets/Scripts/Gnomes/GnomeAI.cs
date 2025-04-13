using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

enum StartingJob { None, GatherFood, GatherWater, GatherFiber, Fight}

// GnomeAI.cs
// Josiah Nistor
// Contains all relevent info and scripts pertaining to the AI, decisions and tracks needs that behavior, and handels jobs assigned to them
public class GnomeAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent m_agent;
    [SerializeField] private Stats m_stats;
    [SerializeField] private Status m_status;
    [SerializeField] private Mood m_mood;
    [SerializeField] private Personality m_personality;
    [SerializeField] private GnomeInventory m_inventory;
    [SerializeField] private Want m_want;

    [SerializeField] private Job m_currentJob;
    [SerializeField] private Job m_prevJob;
    [SerializeField] private Job m_completedJob;

    [SerializeField] private StartingJob m_startingJob;

    [SerializeField] private Decision m_foodDecision;
    [SerializeField] private Decision m_thirstDecision;
    [SerializeField] private Decision m_restDecision;
    [SerializeField] private Decision m_socialDecision;
    [SerializeField] private Decision m_creativeDecision;
    [SerializeField] private Decision m_religiousDecision;
    [SerializeField] private Decision m_healthDecision;
    [SerializeField] private Decision m_decision;

    private InventoryManager m_InvManger;
    private WorldManager m_worldManager;

    private double m_foodMod = 1;
    private double m_thirstMod = 1;
    private double m_restMod = 1;
    private double m_socialMod = 0.5;
    private double m_creativeMod = 0.5;
    private double m_religiousMod = 0.5;

    protected double m_OGRestMod;
    protected double m_OGSocialMod;
    protected double m_OGReligiousMod;

    private GameObject m_setDestination = null;

    private bool m_timer = false;
    private int m_socialCooldown = 0;

    private bool m_needWater = false;
    private bool m_needFood = false;

    // Jobs
    public GatherFood m_gatherFood = new GatherFood();
    public GatherWater m_gatherWater = new GatherWater();
    public Tiredness m_tiredness = new Tiredness();
    public Sociality m_sociality = new Sociality();
    public Creativity m_creativity = new Creativity();
    public Belief m_belief = new Belief();
    public GatherWood m_gatherWood = new GatherWood();
    public GatherStone m_gatherStone = new GatherStone();
    public Brewing m_brewing = new Brewing();
    public Cooking m_cooking = new Cooking();
    public Fighting m_fighting = new Fighting();
    public Construction m_construction = new Construction();
    public StoreItem m_storeItem = new StoreItem();
    public StoreDrink m_storeDrink = new StoreDrink();
    public StoreMeal m_storeMeal = new StoreMeal();
    public GetItem m_getItem = new GetItem();
    public GatherFiber m_gatherFiber = new GatherFiber();

    // Find needed objects from the scene, set mods, initialize decisions, initialize jobs, and set starting jobs
    private void Start()
    {
        m_InvManger = FindFirstObjectByType<InventoryManager>();
        m_worldManager = FindFirstObjectByType<WorldManager>();

        m_setDestination = null;

        m_OGRestMod = m_restMod;
        m_OGSocialMod = m_socialMod;
        m_OGReligiousMod = m_religiousMod;

        m_foodDecision.Initialize(m_worldManager, m_InvManger, "Foods");
        m_thirstDecision.Initialize(m_worldManager, m_InvManger, "Waters");
        m_restDecision.Initialize(m_worldManager, m_InvManger, "Beds");
        m_socialDecision.Initialize(m_worldManager, m_InvManger, "Gnomes");
        m_creativeDecision.Initialize(m_worldManager, m_InvManger, "Workshops");
        m_religiousDecision.Initialize(m_worldManager, m_InvManger, "Shrines");
        m_healthDecision.Initialize(m_worldManager, m_InvManger, "Healths");
        m_decision.Initialize(m_worldManager, m_InvManger);

        m_gatherFood.Initialize(m_foodDecision, this);
        m_gatherWater.Initialize(m_thirstDecision, this);
        m_tiredness.Initialize(m_restDecision, this);
        m_sociality.Initialize(m_socialDecision, this);
        m_creativity.Initialize(m_creativeDecision, this);
        m_belief.Initialize(m_religiousDecision, this);
        m_gatherWood.Initialize(m_decision, this);
        m_gatherStone.Initialize(m_decision, this);
        m_brewing.Initialize(m_creativeDecision, this);
        m_cooking.Initialize(m_creativeDecision, this);
        m_fighting.Initialize(m_decision, this);
        m_construction.Initialize(m_decision, this);
        m_storeItem.Initialize(m_decision, this);
        m_storeDrink.Initialize(m_decision, this);
        m_storeMeal.Initialize(m_decision, this);
        m_getItem.Initialize(m_decision, this);
        m_gatherFiber.Initialize(m_decision, this);

        if (m_startingJob == StartingJob.GatherFood)
            m_stats.SetForceFavJob(m_gatherFood);
        else if (m_startingJob == StartingJob.GatherWater)
            m_stats.SetForceFavJob(m_gatherWater);
        else if (m_startingJob == StartingJob.GatherFiber)
            m_stats.SetForceFavJob(m_gatherFiber);
        else if (m_startingJob == StartingJob.Fight)
            m_stats.SetForceFavJob(m_fighting);
    }

    // Once per second tick the wait update needs and jobs
    private void Update()
    {
        if (m_timer == false)
        {
            m_timer = true;
            StartCoroutine(WaitAndUpdateNeedsAndJobs());
        }
    }

    // Tick up needs, Score needs, Update modifiers, Compare scored decicions and update if needed, Update selected locations, Do current jobs or favorite job, Update want need
    private IEnumerator WaitAndUpdateNeedsAndJobs()
    {
        yield return new WaitForSeconds(1f);

        // update needs
        m_foodDecision.UpdateNeed(m_foodMod);
        m_thirstDecision.UpdateNeed(m_thirstMod);
        m_restDecision.UpdateNeed(m_restMod);
        m_socialDecision.UpdateNeed(m_socialMod);
        m_creativeDecision.UpdateNeed(m_creativeMod);
        m_religiousDecision.UpdateNeed(m_religiousMod);
         
        // score needs
        float scoredHunger = m_foodDecision.ScoreNeed();
        float scoredThirst = m_thirstDecision.ScoreNeed();
        float scoredEnergy = m_restDecision.ScoreNeed();
        float scoredSocialNeed = m_socialDecision.ScoreNeed();
        float scoredCreativity = m_creativeDecision.ScoreNeed();
        float scoredBelief = m_religiousDecision.ScoreNeed();
        float scoredHealth = m_healthDecision.ScoreNeed();

        // update modifiers
        m_status.UpdateStatus();
        m_mood.UpdateMood();
        m_stats.UpdateStats();
        m_restMod = m_OGRestMod;
        m_socialMod = m_OGSocialMod;
        m_religiousMod = m_OGReligiousMod;

        if (m_socialCooldown > 0)
            m_socialCooldown--;

        m_timer = false;

        // scored decisions
        if (m_healthDecision.UpdateHealth())
        {
            yield break;
        }
        else if (scoredHunger > scoredThirst && scoredHunger > scoredEnergy && scoredHunger > scoredSocialNeed && scoredHunger > scoredCreativity && scoredHunger > scoredBelief && m_foodDecision.UpdateDecision(1))
        {
            m_needFood = true;
            yield break;
        }
        else if (scoredThirst > scoredEnergy && scoredThirst > scoredSocialNeed && scoredThirst > scoredCreativity && scoredThirst > scoredBelief && m_thirstDecision.UpdateDecision(2))
        {
            m_needWater = true;
            yield break;
        }
        else if (scoredEnergy > scoredSocialNeed && scoredEnergy > scoredCreativity && scoredEnergy > scoredBelief && m_restDecision.UpdateDecision())
        {
            yield break;
        }
        else if (scoredBelief > scoredSocialNeed && scoredBelief > scoredCreativity && m_religiousDecision.UpdateDecision())
        {
            yield break;
        }
        else if (scoredSocialNeed > scoredCreativity && m_socialDecision.UpdateDecision())
        {
            yield break;
        }
        else if (m_creativeDecision.UpdateDecision())
        {
            yield break;
        }


        m_needWater = false;
        m_needFood = false;

        // store item
        if (m_completedJob != null)
        {
            m_decision.FindClosestAndMove(0, m_worldManager.GetStorageLocations());
            yield break;
        }

        // do task if fill qualifications
        if ((m_currentJob != null && m_currentJob != m_prevJob && m_setDestination == null ) || m_currentJob == m_fighting)
        {
            m_currentJob.FindClosesestAndMove();
            yield break;
        }

        // if no job do favorite job
        if (m_currentJob == null)
        {
            m_setDestination = null;
            m_currentJob = m_stats.GetFavJob();
        }

        // update want if nothing you can do
        if (m_want.WantUpdate(m_inventory, m_worldManager.GetStorageLocations(), m_restDecision) || m_currentJob == null)
            yield break;
    }

    // Set job if no current job, and update selected desinations
    public bool SetJob(Job job)
    {
        if (m_currentJob == null)
        {
            m_setDestination = null;
            m_currentJob = job;
            return true;
        }
        return false;
    }

    // Set job regardless of current job, and update selected desinations
    public bool ForceSetJob(Job job)
    {
        m_setDestination = null;
        m_currentJob = job;
        return true;
    }

    // Setters, and Getters
    public void SetCompletedJob(Job job)
    {
        m_setDestination = null;
        m_completedJob = job;
        m_currentJob = null;
        m_prevJob = null;
    }
    public Job GetCurrentJob() { return m_currentJob; }
    public Job GetCompletedJob() { return m_completedJob; }
    public bool GetNeedWater() { return m_needWater; }
    public bool GetNeedFood() { return m_needFood; }
    public void ResetNeeds()
    {
        m_needWater = false;
        m_needFood = false;
    }
    public GameObject GetDest() { return m_setDestination; }
    public void SetDest(GameObject dest) { m_setDestination = dest; }
    public Decision GetFoodDecision() { return m_foodDecision; }
    public Decision GetThirstDecision() { return m_thirstDecision; }
    public Decision GetRestDecision() { return m_restDecision; }
    public Decision GetSocialDecision() { return m_socialDecision; }
    public Decision GetCreativeDecision() { return m_creativeDecision; }
    public Decision GetReligiousDecision() { return m_religiousDecision; }
    public Decision GetHealthDecision() { return m_healthDecision; }
    public NavMeshAgent GetAgent() { return m_agent; }
    public double GetFoodMod() { return m_foodMod; }
    public void SetFoodMod(double foodMod) { m_foodMod = foodMod; }
    public double GetThirstMod() { return m_thirstMod; }
    public void SetThirstMod(double thirstMod) { m_thirstMod = thirstMod; }
    public double GetRestMod() { return m_restMod; }
    public void SetRestMod(double restMod) { m_restMod = restMod; }
    public double GetSocialMod() { return m_socialMod; }
    public void SetSocialMod(double socialMod) { m_socialMod = socialMod; }
    public double GetCreativeMod() { return m_creativeMod; }
    public void SetCreativeMod(double creativeMod) { m_creativeMod = creativeMod; }
    public double GetReligiousMod() { return m_religiousMod; }
    public void SetReligiousMod(double religiousMod) { m_religiousMod = religiousMod; }
    public Personality GetPersonality() { return m_personality; }
    public WorldManager GetWorldManager() { return m_worldManager; }
}