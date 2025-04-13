using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WaterDest.cs
// Josiah Nistor
// Inherits from destination, overrides the wait enumerator, in order to update gnome stats, and job after a collision
public class WaterDest : Destination
{
    [SerializeField] private Material m_material;

    // WaterDest adds water to gnomes inventory, and disables that destination
    protected override IEnumerator Wait(float waitTime, GameObject collison)
    {
        yield return new WaitForSeconds(waitTime);
        if (collison != null) 
        {
            GnomeAI gnomeAI = collison.gameObject.GetComponent<GnomeAI>();
            if (gnomeAI.GetDest() == gameObject)
            {
                gnomeAI.SetCompletedJob(gnomeAI.m_gatherWater);
                collison.GetComponent<GnomeInventory>().SetCarryingMaterial(m_material);
                gnomeAI.GetThirstDecision().UpdateNeed(-m_updateAmmount);
                collison.GetComponent<Stats>().UpdateSkillLevel(gnomeAI.m_gatherWater, 0.1);
            }
        }
    }
}
