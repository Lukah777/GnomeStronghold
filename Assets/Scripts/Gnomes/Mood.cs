using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GnomeMood { None, Happy, Sad }

// Mood.cs
// Josiah Nistor
// Update stats based on a mood
public class Mood : MonoBehaviour
{
    [SerializeField] private GnomeAI m_gnomeAI;
    [SerializeField] private Stats m_stats;
    [SerializeField] private List<GnomeMood> m_mood = new List<GnomeMood>() { };
    [SerializeField] private double m_generalMod = 0.5;
    private int m_prevPositivity = 0;

    // Update loops through current moods and modified the positivity value, depending on positivty value change modifiers, and clear that staus effect
    public void UpdateMood()
    {
        // modify based on status
        foreach (GnomeMood status in m_mood)
        {
            if (status == GnomeMood.Happy)
            {
                m_stats.UpdatePositivity(1);
            }
            else if (status == GnomeMood.Sad)
            {
                m_stats.UpdatePositivity(-1);
            }
        }

        if (m_stats.GetPositivity() > 0 && m_prevPositivity != m_stats.GetPositivity())
        {
            m_gnomeAI.SetSocialMod(m_gnomeAI.GetSocialMod() * m_generalMod);
            m_gnomeAI.GetComponentInChildren<SocialSpace>().ChangeUpdateAmount(25);
            m_gnomeAI.SetRestMod(m_gnomeAI.GetRestMod() * m_generalMod);
            m_mood.Remove(GnomeMood.Happy);

        }
        else if (m_stats.GetPositivity() < 0 && m_prevPositivity != m_stats.GetPositivity())
        {
            m_gnomeAI.SetSocialMod(m_gnomeAI.GetSocialMod() / m_generalMod);
            m_gnomeAI.GetComponentInChildren<SocialSpace>().ChangeUpdateAmount(-25);
            m_gnomeAI.SetRestMod(m_gnomeAI.GetRestMod() / m_generalMod);
            m_mood.Remove(GnomeMood.Sad);

        }
        m_prevPositivity = m_stats.GetPositivity();
    }

    // Adds a mood to the gnomes current moods
    public void AddMood(GnomeMood mood)
    {
        m_mood.Add(mood);
    }
}
