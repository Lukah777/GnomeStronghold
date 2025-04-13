using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Want.cs
// Josiah Nistor
// Tracks what items that gnome wants, and assigned them to get those items
public class Want : MonoBehaviour
{
    [SerializeField] private GameObject m_bed = null;
    [SerializeField] private Decision m_storeageDecision;
    [SerializeField] private Decision m_restDecision;
    [SerializeField] private int m_timer = 60;
    private ItemType m_itemWant;

    // Tick down the timer, if time is up select a want in order of importance
    public bool WantUpdate(GnomeInventory gnomeInventory, List<GameObject> storageLocations, Decision m_restDecision)
    {
        m_timer--;
        if (m_timer <= 0)
        {
            m_timer = 60;

            if (m_bed == null)
            {
                m_bed = m_restDecision.FindClosestNonClaimedBed();
            }

            bool found = false;
            if (gnomeInventory.GetCloths() == null)
            {
                m_itemWant = ItemType.Clothing;
                found = m_storeageDecision.FindNearestItem(m_itemWant);
            }
            if (gnomeInventory.GetArmor() == null && !found)
            {
                m_itemWant = ItemType.Armor;
                found = m_storeageDecision.FindNearestItem(m_itemWant);
            }
            if (gnomeInventory.GetTools() == null && !found)
            {
                m_itemWant = ItemType.Tools;
                found = m_storeageDecision.FindNearestItem(m_itemWant);
            }
            if (!found)
            {
                m_itemWant = ItemType.Crafts;
                found = m_storeageDecision.FindNearestItem(m_itemWant);
            }

            return found;
        }
        return false;
    }

    // Setters and Getters
    public void SetBed(GameObject bed) { m_bed = bed; }
    public Vector3 GetBedLocation() 
    {
        if (m_bed != null)
            return m_bed.transform.position;
        else
            return new Vector3(10, 0, 0);
    }
    public ItemType GetItemWant() { return m_itemWant; }
}
