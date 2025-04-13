using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// PreparedMeals.cs
// Josiah Nistor
// Inherits from Object, initalizes item type, name,  material, and value
public class PreparedMeals : Object
{
    [SerializeField] private List<string> m_prep = new List<string>() { "Roasted", "Broiled", "Steamed", "Baked", "Grilled", "Poached", "Boiled", "Fried", "Braised", "Sautéd", "Simmered", "Blanched", "Stir fried", "Seared", "Pan roasted", "Stewed", "Basted", "Slow cooked", "Fileted" };

    // PreparedMeals contains names for diffrent types of meals, and modifiers based on material
    public void Initalize()
    {
        InitializeObject();

        m_itemType = ItemType.PreparedMeals;

        int randName = Random.Range(0, m_prep.Count);

        m_name = m_prep[randName] + " " + m_material.GetMaterial().ToString();

        if (m_material.GetMaterial() == mat.Rabbit)
            m_value *= 3;
        else if (m_material.GetMaterial() == mat.Berries)
            m_value *= 2;
    }
}
