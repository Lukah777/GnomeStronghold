using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

// Crafts.cs
// Josiah Nistor
// Inherits from Object, initalizes item type, name,  material, and value
public class Crafts : Object
{
    [SerializeField] private List<string> m_crafts = new List<string>() { "Figurine", "Ring", "Earring", "Amulet", "Bracelet", "Crown", "Scepter" };

    // Crafts contains names for diffrent types of armor, and modifiers based on material
    public void Initalize()
    {
        InitializeObject();
        m_itemType = ItemType.Crafts;

        int randName = Random.Range(0, m_crafts.Count);

        m_name = m_material.GetMaterial().ToString() + " " + m_crafts[randName];

        if (m_material.GetMaterial() == mat.Slate || m_material.GetMaterial() == mat.Pine)
            m_value *= 3;
        else if (m_material.GetMaterial() == mat.Granite || m_material.GetMaterial() == mat.Birtch)
            m_value *= 2;
    }
}
