using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Armor.cs
// Josiah Nistor
// Inherits from Object, initalizes item type, name,  material, and value
public class Armor : Object
{
    [SerializeField] private List<string> m_leatherArmor = new List<string>() { "Padded", "Vest", "Studded"};
    [SerializeField] private List<string> m_metalArmor = new List<string>() { "Chain Shirt", "Scale Mail", "Spiked Armor", "Breastplate", "Halfplate", "Ring Mail", "Chain Mail", "Splint", "Plate" };

    // Armor contains names for diffrent types of armor, and modifies value based on material
    public void Initalize()
    {
        InitializeObject();

        m_itemType = ItemType.Armor;

        if (m_material.GetMatType() == type.Leather)
        {
            int randName = Random.Range(0, m_leatherArmor.Count);
            m_name = m_leatherArmor[randName];
        }
        else if (m_material.GetMatType() == type.Metal)
        {
            int randName = Random.Range(0, m_metalArmor.Count);
            m_name = m_material.GetMaterial().ToString() + " " + m_metalArmor[randName];

            if (m_material.GetMaterial() == mat.Silver)
                m_value *= 3;
            else if (m_material.GetMaterial() == mat.Iron)
                m_value *= 2;
        }
    }
}
