using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Tools.cs
// Josiah Nistor
// Inherits from Object, initalizes item type, name,  material, and value
public class Tools : Object
{
    // All types togeather
    // [SerializeField] private List<string> m_tools = new List<string>() { "Knife", "Bucket", "Blanket", "Scroll", "Chisel", "Idol", "Axe", "Picaxe", "Masher", "Spoon", "Sword", "Hammer", "Scythe"}; // order of jobs they should effect
    [SerializeField] private List<string> m_metalTools = new List<string>() { "Knife", "Chisel", "Idol", "Axe", "Picaxe", "Masher", "Spoon", "Sword", "Hammer", "Scythe" };
    [SerializeField] private List<string> m_WoodTools = new List<string>() { "Bucket", "Idol", "Masher", "Spoon" };
    [SerializeField] private List<string> m_ClothTools = new List<string>() { "Blanket", "Scroll" };

    // Tools contains names for diffrent types of tools, and picks a name based on the type of material that was assigned, then modifies value as well
    public void Initalize()
    {
        InitializeObject();

        m_itemType = ItemType.Tools;

        if (m_material.GetMatType() == type.Metal)
        {
            int randName = Random.Range(0, m_metalTools.Count);
            m_name = m_material.GetMaterial().ToString() + " " + m_metalTools[randName];

            if (m_material.GetMaterial() == mat.Silver)
                m_value *= 3;
            else if (m_material.GetMaterial() == mat.Iron)
                m_value *= 2;
        }
        else if (m_material.GetMatType() == type.Wood)
        {
            int randName = Random.Range(0, m_WoodTools.Count);
            m_name = m_material.GetMaterial().ToString() + " " + m_WoodTools[randName];

            if (m_material.GetMaterial() == mat.Pine)
                m_value *= 3;
            else if (m_material.GetMaterial() == mat.Birtch)
                m_value *= 2;
        }
        else if(m_material.GetMatType() == type.Fiber)
        {
            int randName = Random.Range(0, m_ClothTools.Count);
            m_name = m_material.GetMaterial().ToString() + " " + m_ClothTools[randName];

            if (m_material.GetMaterial() == mat.Cotton)
                m_value *= 3;
            else if (m_material.GetMaterial() == mat.Hemp)
                m_value *= 2;
        }
    }
}