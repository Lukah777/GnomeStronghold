using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Personality.cs
// Josiah Nistor
// Generate a random personality, and modifiy stats based on that
public class Personality : MonoBehaviour
{
    [SerializeField] private GnomeAI m_gnomeAI;
    [SerializeField] private Stats m_stats;
    [SerializeField] private List<string> m_personality = new List<string>() { "Optimistic", "Passionate", "Brave", "Jolly", "Stoic", "Supportive" };
    [SerializeField] private float m_generalMod = 0.75f;

    private int m_randPersonalityIndex;
    private string m_modifiers;

    // Generates and random personality and updates modifiers, and jobs accordingly, as well as gnereating a discription for the personality
    void Start()
    {
        // randomize
        m_randPersonalityIndex = Random.Range(0, m_personality.Count);

        if (m_randPersonalityIndex == 0)
        {
            m_modifiers += m_personality[m_randPersonalityIndex] + ": less social, crative and beleif drain.";
            m_gnomeAI.SetSocialMod(m_gnomeAI.GetSocialMod() * m_generalMod);
            m_gnomeAI.SetCreativeMod(m_gnomeAI.GetCreativeMod() * m_generalMod);
            m_gnomeAI.SetReligiousMod(m_gnomeAI.GetReligiousMod() * m_generalMod);
        }
        else if (m_randPersonalityIndex == 1)
        {
            m_modifiers += m_personality[m_randPersonalityIndex] + ": does preferd job twice as well.";
            if (m_stats.GetFavJob() == m_gnomeAI.m_gatherFood)
            {
                m_gnomeAI.GetFoodDecision().SetUpdateMod(1.5f);
            }
            else if (m_stats.GetFavJob() == m_gnomeAI.m_gatherWater)
            {
                m_gnomeAI.GetThirstDecision().SetUpdateMod(1.5f);
            }
            else if (m_stats.GetFavJob() == m_gnomeAI.m_tiredness)
            {
                m_gnomeAI.GetRestDecision().SetUpdateMod(1.5f);
            }
            else if (m_stats.GetFavJob() == m_gnomeAI.m_creativity)
            {
                m_gnomeAI.GetCreativeDecision().SetUpdateMod(1.5f);
            }
            else if (m_stats.GetFavJob() == m_gnomeAI.m_belief)
            {
                m_gnomeAI.GetReligiousDecision().SetUpdateMod(1.5f);
            }

        }
        else if (m_randPersonalityIndex == 2)
        {
            m_modifiers += m_personality[m_randPersonalityIndex] + ": better in combat and more health.";
            m_stats.SetAttack(m_stats.GetAttack() * 2);
            m_gnomeAI.GetHealthDecision().SetMaxNeed(150);
        }
        else if (m_randPersonalityIndex == 3)
        {
            m_modifiers += m_personality[m_randPersonalityIndex] + ": No mofications.";
        }
        else if (m_randPersonalityIndex == 4)
        {
            m_modifiers += m_personality[m_randPersonalityIndex] + ": less food, drink, and enegry drain.";
            m_gnomeAI.SetFoodMod(m_gnomeAI.GetFoodMod() * m_generalMod);
            m_gnomeAI.SetThirstMod(m_gnomeAI.GetThirstMod() * m_generalMod);
            m_gnomeAI.SetRestMod(m_gnomeAI.GetRestMod() * m_generalMod);
        }
        else if (m_randPersonalityIndex == 5)
        {
            m_modifiers += m_personality[m_randPersonalityIndex] + ": provides double social when interacting.";
            GetComponentInChildren<SocialSpace>().ChangeUpdateAmount(150);
        }
    }

    // Getters
    public int GetPersonalityIndex() { return m_randPersonalityIndex; }
    public string GetPersonality() { return m_personality[m_randPersonalityIndex]; }
    public string GetModifiers() { return m_modifiers; }

}
