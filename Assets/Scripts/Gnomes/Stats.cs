using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

// Stats.cs
// Josiah Nistor
// Tracks gnome stats, generates skill levels for each job, and track those skill levels
public class Stats : MonoBehaviour
{
    [SerializeField] private GnomeAI m_gnomeAI;
    [SerializeField] private int m_attack = 25;
    [SerializeField] private float m_speed = 0f;
    [SerializeField] private float m_workSpeed = 1f;
    [SerializeField] private int m_positivity = 0;

    [SerializeField] private List<Job> m_jobSkillList = new List<Job>();
    [SerializeField] private Dictionary<Job, double> m_jobSkill = new Dictionary<Job, double>();

    private Job m_forceFavJob;
    private float m_OGWorkSpeed = 1f;

    // Add all skills to your skill list and generate random levels
    private void Start()
    {
        m_speed = m_gnomeAI.GetAgent().speed;
        m_workSpeed = m_OGWorkSpeed;

        m_jobSkillList.Add(m_gnomeAI.m_gatherFood);
        m_jobSkillList.Add(m_gnomeAI.m_gatherWater);
        m_jobSkillList.Add(m_gnomeAI.m_tiredness);
        m_jobSkillList.Add(m_gnomeAI.m_sociality);
        m_jobSkillList.Add(m_gnomeAI.m_creativity);
        m_jobSkillList.Add(m_gnomeAI.m_belief);
        m_jobSkillList.Add(m_gnomeAI.m_gatherWood);
        m_jobSkillList.Add(m_gnomeAI.m_gatherStone);
        m_jobSkillList.Add(m_gnomeAI.m_gatherFiber);
        m_jobSkillList.Add(m_gnomeAI.m_brewing);
        m_jobSkillList.Add(m_gnomeAI.m_cooking);
        m_jobSkillList.Add(m_gnomeAI.m_fighting);
        m_jobSkillList.Add(m_gnomeAI.m_construction);

        foreach (Job job in m_jobSkillList)
        {
            int rand = Random.Range(1, 11);
            m_jobSkill.Add(job, rand);
        }
    }

    // Reset stats to default
    public void UpdateStats()
    {
        //return to normal
        m_gnomeAI.GetAgent().speed = m_speed;
        m_workSpeed = m_OGWorkSpeed;
    }

    // Getters and Setters
    public int GetAttack() { return m_attack; }
    public void SetAttack(int attack) { m_attack = attack; }
    public float GetWorkSpeed() { return m_workSpeed; }
    public int GetPositivity() { return m_positivity; }
    public Job GetFavJob() 
    {
        if (m_forceFavJob != null)
        {
            return m_forceFavJob;
        }
        Job tempJob = null;
        double tempSkill = 0;
        foreach (Job job in m_jobSkill.Keys)
        {
            if (m_jobSkill[job] > tempSkill)
            {
                tempJob = job;
                tempSkill = m_jobSkill[job];
            }
        }
        return tempJob;

    }
    public string GetSkillLevelText(Job job) { return m_jobSkill[job].ToString() + job.GetName(); }
    public List<Job> GetSkillList() { return m_jobSkillList; }


    // Update stats by a given change
    public void UpdateWorkSpeed(float change)
    {
        m_workSpeed += change;
    }
    public void UpdatePositivity(int change)
    {
        m_positivity += change;
    }
    public void UpdateSkillLevel(Job job, double update)
    {

        m_jobSkill[job] += update;
    }

    // Force set current and favorite jobs
    public void SetForceFavJob(Job job)
    {
        m_gnomeAI.ForceSetJob(job);
        m_forceFavJob = job;
    }
}
