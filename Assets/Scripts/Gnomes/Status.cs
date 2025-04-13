using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GnomeStatus { None, Injured, Tired}

// Status.cs
// Josiah Nistor
// Tracks gnome status that moidifies stats
public class Status : MonoBehaviour
{
    [SerializeField] private GnomeAI m_gnomeAI;
    [SerializeField] private Stats m_stats;
    [SerializeField] private List<GnomeStatus> m_status = new List<GnomeStatus>() { };
    [SerializeField] private double m_generalMod = 2;

    // Check then the gnome and addd a status if needed, then modify stats based on status
    public void UpdateStatus()
    {
        if (m_gnomeAI.GetHealthDecision().GetNeed() > m_gnomeAI.GetHealthDecision().GetMaxNeed() / m_generalMod)
        {
            if (!m_status.Contains(GnomeStatus.Injured))
                m_status.Add(GnomeStatus.Injured);
        }
        else
        {
            m_status.Remove(GnomeStatus.Injured);
        }

        if (m_gnomeAI.GetRestDecision().GetNeed() > m_gnomeAI.GetRestDecision().GetMaxNeed() / m_generalMod)
        {
            if (!m_status.Contains(GnomeStatus.Tired))
                m_status.Add(GnomeStatus.Tired);
        }
        else
        {
            m_status.Remove(GnomeStatus.Tired);
        }

        //modify if status
        foreach (GnomeStatus status in m_status)
        {
            if (status == GnomeStatus.Injured)
            {
                m_gnomeAI.GetAgent().speed = m_gnomeAI.GetAgent().speed / (float)m_generalMod;
            }
            else if (status == GnomeStatus.Tired)
            {
                m_stats.UpdateWorkSpeed(m_stats.GetWorkSpeed() * (float)m_generalMod);
            }
        }
    }
}
