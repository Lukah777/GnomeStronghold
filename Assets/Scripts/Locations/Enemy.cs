using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy.cs
// Josiah Nistor
// Inherits from destination, overrides the wait enumerator, in order to update gnome stats, and job after a collision
public class Enemy : Destination
{
    // Enemy damages both enemy and gnome after a collision
    protected override IEnumerator Wait(float waitTime, GameObject collison)
    {
        yield return new WaitForSeconds(waitTime);
        if (collison != null)
        {
            if (collison.CompareTag("NPC"))
            {
                GnomeAI gnomeAI = collison.GetComponent<GnomeAI>();
                Stats gnomeStats = collison.GetComponent<Stats>();
                if (gnomeAI.GetCurrentJob() == gnomeAI.m_fighting)
                {
                    m_health -= gnomeStats.GetAttack();
                    gnomeAI.GetHealthDecision().UpdateNeed(-m_updateAmmount);
                }
                else
                    gnomeAI.GetHealthDecision().UpdateNeed(-m_updateAmmount);
                gnomeStats.UpdateSkillLevel(gnomeAI.m_fighting, 0.1);

                if (m_health <= 0)
                {
                    gnomeAI.ForceSetJob(null);
                    Destroy(gameObject);
                }
            }
        }
    }
}
