using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SocialSpace.cs
// Josiah Nistor
// Inherits from destination, overrides the wait enumerator, in order to update gnome stats, and job after a collision
public class SocialSpace : Destination
{
    [SerializeField] private GameObject m_gnomes;
    [SerializeField] private GameObject m_gnomePrefab;

    // Sets gnomes so new gnomes are assigned to it
    new private void Start()
    {
        base.Start();
        m_gnomes = transform.parent.parent.gameObject;
    }

    // SocialSpace fufills the social needs of the gnomes after a collision, redirects the gnomes to make converatsion look good, and has a chance to spawn a new gnome
    protected override IEnumerator Wait(float waitTime, GameObject collison)
    {
        if (collison.CompareTag("NPC"))
        {
            GnomeAI gnomeAI = collison.GetComponent<GnomeAI>();
            if (collison != null || gnomeAI.GetCurrentJob() != null || !collison.CompareTag("Enemy"))
            {
                if (gnomeAI.GetDest() == gameObject)
                {
                    if (gnomeAI.GetCurrentJob() == gnomeAI.m_sociality)
                    {
                        Vector3 newDirection = Vector3.RotateTowards(gnomeAI.transform.position, transform.parent.position, Time.deltaTime, 0.0f);
                        transform.rotation = Quaternion.LookRotation(newDirection);

                        if (transform.parent.GetComponent<GnomeAI>().GetCurrentJob() != gnomeAI.m_sociality)
                            transform.parent.GetComponent<GnomeAI>().ForceSetJob(gnomeAI.m_sociality);

                        yield return new WaitForSeconds(waitTime);

                        gnomeAI.GetSocialDecision().UpdateNeed(-m_updateAmmount);
                        gnomeAI.ForceSetJob(null);

                        int rand = Random.Range(0, 50);
                        if (rand == 0)
                        {
                            Instantiate(m_gnomePrefab, new Vector3(transform.position.x, 1f, transform.position.z), transform.rotation, m_gnomes.transform);
                        }

                        collison.GetComponent<Stats>().UpdateSkillLevel(gnomeAI.m_sociality, 0.1);

                    }
                }
            }
        }
    }
}
