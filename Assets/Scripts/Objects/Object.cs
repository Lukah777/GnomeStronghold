using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ItemType { Clothing, Crafts, PreparedDrinks, PreparedMeals, Tools, Armor}

// Object.cs
// Josiah Nistor
// Object is the base class, that other items inherit
public class Object : MonoBehaviour
{
    [SerializeField] protected string m_name;
    [SerializeField] protected string m_discription;
    [SerializeField] protected int m_value;
    [SerializeField] protected Material m_material;
    [SerializeField] protected ItemType m_itemType;

    protected List<string> m_quality = new List<string>() { "average", "well-crafted", "finely-crafted", "superior", "exceptional", "masterful", "artifact" };

    // Object contains the info about the item, and generates it value, and quality
    public void InitializeObject()
    {
        int randQuality = Random.Range(0, m_quality.Count);
        int randVal = Random.Range(1, 10);

        m_discription = m_quality[randQuality];
        m_value = randVal * randQuality+1;
    }

    // Getters and Setters
    public string GetName() { return m_name; }
    public string GetDiscription() { return m_discription; }
    public int GetValue() { return m_value; }
    public Material GetMaterial() { return m_material; }
    public void SetMaterial(Material type) { m_material = type; }
    public ItemType GetObjectType() { return m_itemType; }
}
