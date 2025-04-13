using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

// GnomeInventory.cs
// Josiah Nistor
// Contains a list of items in the gnome's inventory, and equiped items
public class GnomeInventory : MonoBehaviour
{
    [SerializeField] private Armor m_armor;
    [SerializeField] private Clothing m_cloths;
    [SerializeField] private Tools m_tool;
    [SerializeField] private List<Object> m_items = new List<Object>();
    [SerializeField] private Material m_carrying;

    // Setters and Gettters
    public void SetArmor(Armor armor)
    {
        m_armor = armor;
        Decision healthDecision = gameObject.GetComponent<GnomeAI>().GetHealthDecision();
        mat armorMaterial = m_armor.GetMaterial().GetMaterial();
        if (armorMaterial == mat.Rabbit)
            healthDecision.SetUpdateNeedMod(1);
        else if (armorMaterial == mat.Copper)
            healthDecision.SetUpdateNeedMod(3);
        else if (armorMaterial == mat.Iron)
            healthDecision.SetUpdateNeedMod(4);
        else if (armorMaterial == mat.Silver)
            healthDecision.SetUpdateNeedMod(5);
    }
    public Armor GetArmor() { return m_armor; }
    public void SetCloths(Clothing cloths) { m_cloths = cloths; }
    public Clothing GetCloths() { return m_cloths; }
    public void SetTools(Tools tool) 
    { 
        m_tool = tool;
        GnomeAI gnomeAI = gameObject.GetComponent<GnomeAI>();
        string toolName = m_tool.GetName();
        if (toolName == "Knife")
            gnomeAI.GetFoodDecision().SetUpdateMod(gnomeAI.GetFoodDecision().GetUpdateMod() * 2);
        else if (toolName == "Bucket")
            gnomeAI.GetThirstDecision().SetUpdateMod(gnomeAI.GetThirstDecision().GetUpdateMod() * 2);
        else if (toolName == "Blanket")
            gnomeAI.GetRestDecision().SetUpdateMod(gnomeAI.GetRestDecision().GetUpdateMod() * 2);
        else if (toolName == "Scroll")
            gnomeAI.GetSocialDecision().SetUpdateMod(gnomeAI.GetSocialDecision().GetUpdateMod() * 2);
        else if (toolName == "Chisel")
            gnomeAI.GetCreativeDecision().SetUpdateMod(gnomeAI.GetCreativeDecision().GetUpdateMod() * 2);
        else if (toolName == "Idol")
            gnomeAI.GetReligiousDecision().SetUpdateMod(gnomeAI.GetReligiousDecision().GetUpdateMod() * 2);
        else if (toolName == "Axe")
            gameObject.GetComponent<Stats>().SetAttack(gameObject.GetComponent<Stats>().GetAttack() * 2);
        else if (toolName == "Picaxe")
            gameObject.GetComponent<Stats>().SetAttack(gameObject.GetComponent<Stats>().GetAttack() * 2);
        else if (toolName == "Masher")
            gnomeAI.GetThirstDecision().SetUpdateMod(gnomeAI.GetThirstDecision().GetUpdateMod() * 2);
        else if (toolName == "Spoon")
            gnomeAI.GetFoodDecision().SetUpdateMod(gnomeAI.GetFoodDecision().GetUpdateMod() * 2);
        else if (toolName == "Sword")
            gameObject.GetComponent<Stats>().SetAttack(gameObject.GetComponent<Stats>().GetAttack() * 2);
        else if (toolName == "Hammer")
            gameObject.GetComponent<Stats>().SetAttack(gameObject.GetComponent<Stats>().GetAttack() * 2);
        else if (toolName == "Scythe")
            gameObject.GetComponent<Stats>().SetAttack(gameObject.GetComponent<Stats>().GetAttack() * 2);
    }
    public Tools GetTools() { return m_tool; }
    public List<Object> GetItems() { return m_items; }
    public Material GetCarryingMaterial() { return m_carrying; }
    public void SetCarryingMaterial(Material mat) { m_carrying = mat; }
    public void ClearCarryingMaterial() {  m_carrying = null; }
}
