using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// FoodDest.cs
// Josiah Nistor
// Inherits from destination, overrides the wait enumerator, in order to update gnome stats, and job after a collision
public class FoodDest : Destination
{
    [SerializeField] private Material m_material;

    // FoodDest adds food to gnomes inventory, and disables that destination
    protected override IEnumerator Wait(float waitTime, GameObject collison) 
    {
        yield return new WaitForSeconds(waitTime);
        if (collison != null && !collison.CompareTag("Enemy"))
        {
            GnomeAI gnomeAI = collison.GetComponent<GnomeAI>();
            if (gnomeAI.GetDest() == gameObject)
            {
                gnomeAI.SetCompletedJob(gnomeAI.m_gatherFood);
                collison.GetComponent<GnomeInventory>().SetCarryingMaterial(m_material);
                gnomeAI.GetFoodDecision().UpdateNeed(-m_updateAmmount);
                collison.GetComponent<Stats>().UpdateSkillLevel(gnomeAI.m_gatherFood, 0.1);
                gameObject.SetActive(false);
                m_ground.SetNavMeshUpdate();
            }
        }
    }
}
