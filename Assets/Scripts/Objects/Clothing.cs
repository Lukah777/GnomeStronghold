using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Clothing.cs
// Josiah Nistor
// Inherits from Object, initalizes item type, name,  material, and value
public class Clothing : Object
{
    [SerializeField] private List<string> m_cloths = new List<string>() { "Common clothes", "Costume", "Fine clothes", "Robes", "Traveler's clothes"};

    // Clothing contains names for diffrent types of cloths, and modifies value based on material
    public void Initalize()
    {
        InitializeObject();

        m_itemType = ItemType.Clothing;

        int randName = Random.Range(0, m_cloths.Count);

        m_name = m_cloths[randName];

        if (m_material.GetMaterial() == mat.Cotton)
            m_value *= 3;
        else if (m_material.GetMaterial() == mat.Hemp)
            m_value *= 2;
    }
}