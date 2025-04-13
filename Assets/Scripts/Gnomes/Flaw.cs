using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Flaw.cs
// Josiah Nistor
// Generates flaw, and modifies stats based on that
public class Flaw : MonoBehaviour
{
    [SerializeField] private GnomeAI m_gnomeAI;
    [SerializeField] private List<string> m_flaw = new List<string>() { "A glutton", "Alcoholic", "Lazy", "Co-dependent", "Obsessive", "A zelot" };
    [SerializeField] private int m_generalMod = 2;

    private int m_randFlawIndex;
    private string m_modifiers;

    // Generate random flaw and modify relevent stat modifer
    void Start()
    {
        // randomize
        m_randFlawIndex = Random.Range(0, m_flaw.Count);

        if (m_randFlawIndex == 0)
        {
            m_modifiers = m_flaw[m_randFlawIndex] + ": double hunger. \n";
            m_gnomeAI.SetFoodMod(m_gnomeAI.GetFoodMod() * m_generalMod);
        }
        else if (m_randFlawIndex == 1)
        {
            m_modifiers = m_flaw[m_randFlawIndex] + ": double thrist. \n";
            m_gnomeAI.SetThirstMod(m_gnomeAI.GetThirstMod() * m_generalMod);
        }
        else if (m_randFlawIndex == 2)
        {
            m_modifiers = m_flaw[m_randFlawIndex] + ": double energy use. \n";
            m_gnomeAI.SetRestMod(m_gnomeAI.GetRestMod() * m_generalMod);
        }
        else if (m_randFlawIndex == 3)
        {
            m_modifiers = m_flaw[m_randFlawIndex] + ": double need for socializtion. \n";
            m_gnomeAI.SetSocialMod(m_gnomeAI.GetSocialMod() * m_generalMod);
        }
        else if (m_randFlawIndex == 4)
        {
            m_modifiers = m_flaw[m_randFlawIndex] + ": double need for creative expression. \n";
            m_gnomeAI.SetCreativeMod(m_gnomeAI.GetCreativeMod() * m_generalMod);
        }
        else if (m_randFlawIndex == 5)
        {
            m_modifiers = m_flaw[m_randFlawIndex] + ": double need for belief. \n";
            m_gnomeAI.SetReligiousMod(m_gnomeAI.GetReligiousMod() * m_generalMod);
        }
    }

    // Getters
    public string GetFlaw() { return m_flaw[m_randFlawIndex]; }
    public string GetModifiers() { return m_modifiers; }

}
