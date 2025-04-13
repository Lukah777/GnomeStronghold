using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// PreparedDrinks.cs
// Josiah Nistor
// Inherits from Object, initalizes item type, name,  material, and value
public class PreparedDrinks : Object
{
    [SerializeField] private List<string> m_drinks = new List<string>() { "Wine", "Beer", "Coffee", "Tea", "Cider", "Juice" };

    // PreparedDrinks contains names for diffrent types of drinks
    public void Initalize()
    {
        InitializeObject();

        m_itemType = ItemType.PreparedDrinks;

        int randName = Random.Range(0, m_drinks.Count);

        m_name = m_drinks[randName];
    }
}
