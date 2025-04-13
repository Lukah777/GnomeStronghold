using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// HeallthArea.cs
// Josiah Nistor
// Inherits from destination, overrides the wait enumerator, in order to update gnome stats, and job after a collision
public class HeallthArea : Destination
{
    // HeallthArea heals gnomes
    protected override IEnumerator Wait(float waitTime, GameObject collison)
    {
        yield return new WaitForSeconds(waitTime);
        if (collison != null)
        {
            if (collison.CompareTag("NPC"))
            {
                GnomeAI gnomeAI = collison.GetComponent<GnomeAI>();
                if (collison.gameObject.GetComponent<GnomeAI>().GetDest() == gameObject)
                {
                    if (m_health < 100 && collison.GetComponent<GnomeAI>().GetCurrentJob() == gnomeAI.m_construction)
                    {
                        gnomeAI.ForceSetJob(null);
                        collison.GetComponent<Stats>().UpdateSkillLevel(gnomeAI.m_construction, 0.1);
                        m_health = 100;
                    }
                    else
                        gnomeAI.GetHealthDecision().UpdateNeed(-m_updateAmmount);
                }
            }
            else if (collison.CompareTag("Enemy"))
            {
                m_health += collison.GetComponentInChildren<Enemy>().GetUpdateAmount();
            }
        }
    }
}
