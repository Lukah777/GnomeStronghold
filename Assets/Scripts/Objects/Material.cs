using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum type { Food, Drink, Wood, Stone, Fiber, Metal, Leather}
public enum mat { Berries, Lettuce, Rabbit, Water, Oak, Pine, Birtch, Granite, Slate, Sandstone, Cotton, Hemp, Flax, Copper, Iron, Silver}

// Material.cs
// Josiah Nistor
// Material contains a type and material for better sorting of materials, that can be assigned to objects
public class Material : MonoBehaviour
{
    [SerializeField] private type m_type;
    [SerializeField] private mat m_material;

    // Getters and Setters
    public type GetMatType() { return m_type; }
    public void SetMatType(type type) { m_type = type; }
    public mat GetMaterial() { return m_material; }
    public void SetMaterial(mat mat) { m_material = mat; }
}
