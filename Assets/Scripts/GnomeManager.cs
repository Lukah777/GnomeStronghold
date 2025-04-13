using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// GnomeManager.cs
// Josiah Nistor
// Handels most UI, displaying gnomes stats, discription, gnome inventory of items, and handels input to assign jobs to selected gnomes
public class GnomeManager : MonoBehaviour
{
    [SerializeField] private GameObject m_gnomes;
    [SerializeField] private TMP_Text m_name;
    [SerializeField] private TMP_Text m_job;
    [SerializeField] private TMP_Text m_health;
    [SerializeField] private TMP_Text m_hunger;
    [SerializeField] private TMP_Text m_thirst;
    [SerializeField] private TMP_Text m_energy;
    [SerializeField] private TMP_Text m_socialization;
    [SerializeField] private TMP_Text m_creativity;
    [SerializeField] private TMP_Text m_belief;

    [SerializeField] private Color m_gnomeColor;

    [SerializeField] private GameObject m_itemInvenvtory;
    [SerializeField] private GameObject m_JobNamePrefab;

    [SerializeField] private GameObject m_discription;
    [SerializeField] private GameObject m_stats;
    [SerializeField] private GameObject m_gnomeInv;

    [SerializeField] private TMP_Text m_name2;
    [SerializeField] private TMP_Text m_discriptionText;
    [SerializeField] private TMP_Text m_modifier;
    [SerializeField] private TMP_Text m_gatherFood;
    [SerializeField] private TMP_Text m_gatherWater;
    [SerializeField] private TMP_Text m_gatherWood;
    [SerializeField] private TMP_Text m_gatherStone;
    [SerializeField] private TMP_Text m_gatherFiber;
    [SerializeField] private TMP_Text m_tiredness;
    [SerializeField] private TMP_Text m_socialitySkill;
    [SerializeField] private TMP_Text m_beliefSkill;
    [SerializeField] private TMP_Text m_creativitySkill;
    [SerializeField] private TMP_Text m_brewingSkill;
    [SerializeField] private TMP_Text m_cookingSkill;
    [SerializeField] private TMP_Text m_fightingSkill;
    [SerializeField] private TMP_Text m_constructionSkill;

    [SerializeField] private TMP_Text m_name3;
    [SerializeField] private TMP_Text m_armorText;
    [SerializeField] private TMP_Text m_clothsText;
    [SerializeField] private TMP_Text m_toolText;

    [SerializeField] private GameObject m_itemsLocation;
    [SerializeField] private GameObject m_itemSlotPrefab;

    [SerializeField] private Image m_assignBedImg;
    [SerializeField] private Image m_setFavJobImg;

    [SerializeField] private Image m_gatherFoodImg;
    [SerializeField] private Image m_gatherWaterImg;
    [SerializeField] private Image m_gatherWoodImg;
    [SerializeField] private Image m_gatherStoneImg;
    [SerializeField] private Image m_gatherFiberImg;
    [SerializeField] private Image m_restImg;
    [SerializeField] private Image m_socializeImg;
    [SerializeField] private Image m_workImg;
    [SerializeField] private Image m_prayImg;
    [SerializeField] private Image m_fightImg;
    [SerializeField] private Image m_repairImg;

    [SerializeField] private WorldManager m_worldManager;

    private GameObject m_selectedGnome;
    private List<GameObject> m_selectedObjects = new List<GameObject>();

    private List<Object> m_objects = new List<Object>();

    private bool m_assignMode = false;
    private bool m_setFavMode = false;
    private bool m_timer = false;
    private int m_idle_timer = 180;

    // If you clicked a gnome, or distructable resource select it and highlight it, keep track of idle timer, and Update Game UI about the selected gnome, and highlight any currently toggled buttons
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10000))
            {
                GameObject hitGameObject = hit.transform.gameObject;
                m_idle_timer = 180;
                if (m_assignMode && m_selectedGnome != null)
                {
                    if (hitGameObject.GetComponent<RestArea>() != null)
                    {
                        m_selectedGnome.GetComponent<Want>().SetBed(hitGameObject);
                        m_assignMode = false;
                        m_assignBedImg.color = Color.white;
                    }
                }

                if (hitGameObject.CompareTag("NPC"))
                {
                    m_selectedGnome = hitGameObject;
                    m_objects.Clear();
                    foreach (Transform child in m_itemsLocation.transform)
                    {
                        Destroy(child.gameObject);
                    }
                }
                else if (m_selectedGnome != null)
                {
                    m_selectedGnome.GetComponent<Renderer>().material.SetColor("_Color", m_gnomeColor);
                    m_selectedGnome = null;
                }

                if (hitGameObject.CompareTag("Distructable") && m_selectedObjects.Contains(hitGameObject))
                {
                    m_selectedObjects.Remove(hitGameObject);
                    if (hitGameObject.GetComponent<WoodDest>() != null)
                    {
                        hitGameObject.GetComponent<Renderer>().material.SetColor("_Color", hitGameObject.GetComponent<WoodDest>().GetColor());
                    }
                    else if (hitGameObject.GetComponent<StoneDest>() != null)
                    {
                        hitGameObject.GetComponent<Renderer>().material.SetColor("_Color", hitGameObject.GetComponent<StoneDest>().GetColor());
                    }

                }
                else if (hitGameObject.CompareTag("Distructable"))
                {
                    m_selectedObjects.Add(hitGameObject);
                }
            }
        }
        else
        {
            if (m_timer == false && m_idle_timer <= 0)
            {
                SceneManager.LoadScene("GS_DE", LoadSceneMode.Single);
            }
            else if (m_timer == false)
            {
                m_timer = true;
                StartCoroutine(WaitAndDecayIdleTimer());
            }
        }

        foreach (GameObject gnome in m_worldManager.GetSocialLocations())
        {
            if (gnome != null)
                gnome.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }

        m_gatherFoodImg.color = Color.white;
        m_gatherWaterImg.color = Color.white;
        m_gatherWoodImg.color = Color.white;
        m_gatherStoneImg.color = Color.white;
        m_gatherFiberImg.color = Color.white;
        m_restImg.color = Color.white;
        m_socializeImg.color = Color.white;
        m_workImg.color = Color.white;
        m_prayImg.color = Color.white;
        m_fightImg.color = Color.white;
        m_repairImg.color = Color.white;

        // UI info on selected gnome
        if (m_selectedGnome != null)
        {
            GnomeAI gnomeAI = m_selectedGnome.GetComponent<GnomeAI>();
            Stats gnomeStats = m_selectedGnome.GetComponent<Stats>();
            GnomeInventory gnomeInv = m_selectedGnome.GetComponent<GnomeInventory>();
            m_selectedGnome.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

            m_name.text = "Name: " + m_selectedGnome.GetComponent<Discription>().GetName();
            m_job.text = "Current Job: " + gnomeAI.GetCurrentJob();
            m_health.text = "Health: " + (gnomeAI.GetHealthDecision().GetMaxNeed() - gnomeAI.GetHealthDecision().GetNeed());
            m_hunger.text = "Hunger: " + gnomeAI.GetFoodDecision().GetNeed();
            m_thirst.text = "Thirst: " + gnomeAI.GetThirstDecision().GetNeed();
            m_energy.text = "Tiredness: " + gnomeAI.GetRestDecision().GetNeed();
            m_socialization.text = "Social Need: " + gnomeAI.GetSocialDecision().GetNeed();
            m_creativity.text = "Creativity: " + gnomeAI.GetCreativeDecision().GetNeed();
            m_belief.text = "Belief: " + gnomeAI.GetReligiousDecision().GetNeed();

            m_name2.text = "Name: " + m_selectedGnome.GetComponent<Discription>().GetName();
            m_discriptionText.text = "Discription: " + m_selectedGnome.GetComponent<Discription>().GetDiscription();
            m_modifier.text = "Modifiers: " + m_selectedGnome.GetComponent<Flaw>().GetModifiers() + m_selectedGnome.GetComponent<Personality>().GetModifiers();

            List<Job> skillList = gnomeStats.GetSkillList();
            m_gatherFood.text = gnomeStats.GetSkillLevelText(skillList[0]);
            m_gatherWater.text = gnomeStats.GetSkillLevelText(skillList[1]);
            m_tiredness.text = gnomeStats.GetSkillLevelText(skillList[2]);
            m_socialitySkill.text = gnomeStats.GetSkillLevelText(skillList[3]);
            m_creativitySkill.text = gnomeStats.GetSkillLevelText(skillList[4]);
            m_beliefSkill.text = gnomeStats.GetSkillLevelText(skillList[5]);
            m_gatherWood.text = gnomeStats.GetSkillLevelText(skillList[6]);
            m_gatherStone.text = gnomeStats.GetSkillLevelText(skillList[7]);
            m_gatherFiber.text = gnomeStats.GetSkillLevelText(skillList[8]);
            m_brewingSkill.text = gnomeStats.GetSkillLevelText(skillList[9]);
            m_cookingSkill.text = gnomeStats.GetSkillLevelText(skillList[10]);
            m_fightingSkill.text = gnomeStats.GetSkillLevelText(skillList[11]);
            m_constructionSkill.text = gnomeStats.GetSkillLevelText(skillList[12]);

            m_name3.text = "Name: " + m_selectedGnome.GetComponent<Discription>().GetName();
            if (gnomeInv.GetArmor() != null)
                m_armorText.text = "Armor: " + gnomeInv.GetArmor().GetDiscription();
            if (gnomeInv.GetCloths() != null)
                m_clothsText.text = "Cloths: " + gnomeInv.GetCloths().GetDiscription();
            if (gnomeInv.GetTools() != null)
                m_toolText.text = "Tool: " + gnomeInv.GetTools().GetDiscription();

            List<Object> objects = gnomeInv.GetItems();
            if (objects.Count < m_objects.Count)
            {
                m_objects.Clear();
                foreach (Transform child in m_itemsLocation.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            foreach (Object newObj in objects)
            {
                bool same = false;
                foreach (Object obj in m_objects)
                {
                    if (newObj == obj)
                        same = true;
                }
                if (!same)
                {
                    m_objects.Add(newObj);
                    GameObject newItemSlot = Instantiate(m_itemSlotPrefab, m_itemsLocation.transform);
                    Transform newItemSlotTransform = newItemSlot.transform;
                    newItemSlotTransform.GetChild(0).GetComponent<TextMeshProUGUI>().text = newObj.GetName();
                    newItemSlotTransform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "This craft made of " + newObj.GetMaterial().GetMaterial().ToString() + " and is of " + newObj.GetDiscription() + " quality";
                }
            }

            // Highlight buttons
            string favJobName = m_selectedGnome.GetComponent<Stats>().GetFavJob().GetName();
            if (favJobName == ": Gather Food")
                m_gatherFoodImg.color = Color.red;
            else if (favJobName == ": Gather Water")
                m_gatherWaterImg.color = Color.red;
            else if (favJobName == ": Tiredness")
                m_restImg.color = Color.red;
            else if (favJobName == ": Sociality")
                m_socializeImg.color = Color.red;
            else if (favJobName == ": Creativity")
                m_workImg.color = Color.red;
            else if (favJobName == ": Belief")
                m_prayImg.color = Color.red;
            else if (favJobName == ": Gather Wood")
                m_gatherWoodImg.color = Color.red;
            else if (favJobName == ": Gather Stone")
                m_gatherStoneImg.color = Color.red;
            else if (favJobName == ": Gather Fiber")
                m_gatherFiberImg.color = Color.red;
            else if (favJobName == ": Brewing")
                m_workImg.color = Color.red;
            else if (favJobName == ": Cooking")
                m_workImg.color = Color.red;
            else if (favJobName == ": Fighting")
                m_fightImg.color = Color.red;
            else if (favJobName == ": Construction")
                m_repairImg.color = Color.red;
        }

        if (m_selectedObjects.Count > 0)
        {
            foreach (GameObject destination in m_selectedObjects)
            {
                if (destination == null)
                {
                  m_selectedObjects.Remove(destination);
                    break;
                }
                destination.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            }
        }

        // ends game in no gnomes left
        int gnomeNum = m_worldManager.GetSocialLocations().Count();
        foreach (GameObject gnome in m_worldManager.GetSocialLocations())
        {
            if (gnome == null)
                gnomeNum--;
        }

        if (gnomeNum == 0)
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);

    }

    // Decay idle timer
    private IEnumerator WaitAndDecayIdleTimer()
    {
        yield return new WaitForSeconds(1f);
        m_idle_timer--;
        m_timer = false;
    }

    // Assign job to selected gnome or a random gnome if none slected
    private void AssignJob(Job job, bool repeat = false)
    {
        if (m_selectedGnome != null)
        {
            if (m_setFavMode)
            {
                m_setFavMode = false;
                m_setFavJobImg.color = Color.white;
                m_selectedGnome.GetComponent<Stats>().SetForceFavJob(job);
                return;
            }
            else
            {
                m_selectedGnome.GetComponent<GnomeAI>().ForceSetJob(job);
                m_worldManager.UpdateSelectedLocations(m_selectedObjects);
                return;
            }
        }
        else
        {
            foreach (GameObject gnome in m_worldManager.GetSocialLocations())
            {
                if (gnome.GetComponent<GnomeAI>().SetJob(job))
                {
                    break;
                }
            }
        }
        return;
    }


    // UI Buttons
    public void GnomeInventory()
    {
        m_discription.SetActive(false);
        m_stats.SetActive(false);
        m_gnomeInv.SetActive(true);
    }
    public void AssignBed()
    {
        m_assignMode = true;
        m_assignBedImg.color = Color.red;
    }
    public void GatherFood()
    {   
        if (m_selectedGnome != null)
            AssignJob(m_selectedGnome.GetComponent<GnomeAI>().m_gatherFood);
    }
    public void GatherWater()
    {
        if (m_selectedGnome != null)
            AssignJob(m_selectedGnome.GetComponent<GnomeAI>().m_gatherWater);
    }
    public void Rest()
    {
        if (m_selectedGnome != null)
            AssignJob(m_selectedGnome.GetComponent<GnomeAI>().m_tiredness);
    }
    public void Socialize()
    {
        if (m_selectedGnome != null)
            AssignJob(m_selectedGnome.GetComponent<GnomeAI>().m_sociality);
    }
    public void Work()
    {
        if (m_selectedGnome != null)
            AssignJob(m_selectedGnome.GetComponent<GnomeAI>().m_creativity);
    }
    public void Pray()
    {
        if (m_selectedGnome != null)
            AssignJob(m_selectedGnome.GetComponent<GnomeAI>().m_belief);
    }
    public void GatherWood()
    {
        if (m_selectedGnome != null)
            AssignJob(m_selectedGnome.GetComponent<GnomeAI>().m_gatherWood);
    }
    public void GatherStone()
    {
        if (m_selectedGnome != null)
            AssignJob(m_selectedGnome.GetComponent<GnomeAI>().m_gatherStone);
    }
    public void GatherFiber()
    {
        if (m_selectedGnome != null)
            AssignJob(m_selectedGnome.GetComponent<GnomeAI>().m_gatherFiber);
    }
    public void Fight()
    {
        if (m_selectedGnome != null)
            AssignJob(m_selectedGnome.GetComponent<GnomeAI>().m_fighting);
    }
    public void Repair()
    {
        if (m_selectedGnome != null)
            AssignJob(m_selectedGnome.GetComponent<GnomeAI>().m_construction);
    }
    public void SetFavJob()
    {
        if (!m_setFavMode)
        {
            m_setFavMode = true;
            m_setFavJobImg.color = Color.red;
        }
        else
        {
            m_setFavMode = false;
            m_setFavJobImg.color = Color.white;
        }

    }
    public void Discripton()
    {
        m_discription.SetActive(true);
        m_stats.SetActive(false);
        m_gnomeInv.SetActive(false);
    }
    public void Stats()
    {
        m_discription.SetActive(false);
        m_stats.SetActive(true);
        m_gnomeInv.SetActive(false);
    }

    // Getter
    public List<GameObject> GetSelectedObjects() { return m_selectedObjects; }
}
