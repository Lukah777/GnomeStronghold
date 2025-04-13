using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RestArea.cs
// Josiah Nistor
// Inherits from destination, overrides the wait enumerator, in order to update gnome stats, and job after a collision
public class RestArea : Destination
{
    [SerializeField] private bool m_claimed = false;

    // RestArea fufills the rest needs of the gnomes after a collision, and can be claimed
    protected override IEnumerator Wait(float waitTime, GameObject collison)
    {
        yield return new WaitForSeconds(waitTime);
        if (collison != null)
        {
            if (collison.CompareTag("NPC"))
            {
                GnomeAI gnomeAI = collison.GetComponent<GnomeAI>();
                Stats gnomeStats = collison.GetComponent<Stats>();
                if (gnomeAI.GetDest() == gameObject)
                {
                    if (m_health < 100 && gnomeAI.GetCurrentJob() == gnomeAI.m_construction)
                    {
                        gnomeAI.ForceSetJob(null);
                        gnomeStats.UpdateSkillLevel(gnomeAI.m_construction, 0.1);
                        m_health = 100;
                    }
                    else
                    {
                        gnomeAI.GetRestDecision().UpdateNeed(-m_updateAmmount);
                        gnomeStats.UpdateSkillLevel(gnomeAI.m_tiredness, 0.1);
                    }
                }
            }
            else if (collison.CompareTag("Enemy") && gameObject.tag != "Enemy")
            {
                m_health += collison.GetComponentInChildren<Enemy>().GetUpdateAmount();
            }
        }
    }

    // Setters and Getters
    public void SetClaimed() { m_claimed = true; }
    public bool GetClaimed() { return m_claimed; }
}
