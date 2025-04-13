using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// StoneDest.cs
// Josiah Nistor
// Inherits from destination, overrides the wait enumerator, in order to update gnome stats, and job after a collision
public class StoneDest : Destination
{
    [SerializeField] private Color m_color;
    [SerializeField] private Material m_material;

    // StoneDest adds stone to gnomes inventory, and disables that destination
    protected override IEnumerator Wait(float waitTime, GameObject collison)
    {
        yield return new WaitForSeconds(waitTime);
        if (collison != null)
        {
            GnomeAI gnomeAI = collison.GetComponent<GnomeAI>();
            if (gnomeAI.GetDest() == gameObject)
            {
                gnomeAI.SetCompletedJob(gnomeAI.m_gatherStone);
                collison.GetComponent<GnomeInventory>().SetCarryingMaterial(m_material);
                collison.GetComponent<Stats>().UpdateSkillLevel(gnomeAI.m_gatherStone, 0.1);
                gameObject.SetActive(false);
                m_ground.SetNavMeshUpdate();
            }
        }
    }

    // Gettter
    public Color GetColor() { return m_color; }
}