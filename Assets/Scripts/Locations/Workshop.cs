using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Workshop.cs
// Josiah Nistor
// Inherits from destination, overrides the wait enumerator, in order to update gnome stats, and job after a collision
public class Workshop : Destination
{
    // Type of workshop changes what material is need and what craft is created
    public enum WorkshopType { Workshop, Kitchen, Still }

    [SerializeField] protected WorkshopType m_workshopType;
    [SerializeField] protected Job m_completedJob;
    [SerializeField] protected GameObject m_items;
    [SerializeField] protected GameObject m_itemPrefab;

    // Finds Items in scene
    new private void Start()
    {
        base.Start();
        m_items = GameObject.Find("Items");
    }

    // Workshop fufills the social needs of the gnomes after a collision if there is a material to be used to create a craft
    protected override IEnumerator Wait(float waitTime, GameObject collison)
    {
        yield return new WaitForSeconds(waitTime);
        if (collison != null)
        {
            if (collison.CompareTag("NPC"))
            {
                GnomeAI gnomeAI = collison.gameObject.GetComponent<GnomeAI>();
                Job completedJob = gnomeAI.GetCompletedJob();
                if (gnomeAI.GetDest() == gameObject && completedJob != gnomeAI.m_storeItem
                    && completedJob != gnomeAI.m_storeDrink && completedJob != gnomeAI.m_storeMeal)
                {
                    if (m_health < 100 && gnomeAI.GetCurrentJob() == gnomeAI.m_construction)
                    {
                        gnomeAI.ForceSetJob(null);
                        m_health = 100;
                    }
                    else
                    {
                        gnomeAI.SetCompletedJob(m_completedJob);
                        gnomeAI.GetCreativeDecision().UpdateNeed(-m_updateAmmount);
                        gnomeAI.ForceSetJob(null);
                        collison.GetComponent<Stats>().UpdateSkillLevel(gnomeAI.m_creativity, 0.1);

                        GameObject obj = Instantiate(m_itemPrefab, m_items.transform);

                        List<GameObject> m_storageLocations = gnomeAI.GetWorldManager().GetStorageLocations();
                        Material mat = null;

                        if (m_completedJob == gnomeAI.m_creativity)
                        {
                            foreach (GameObject storageLocation in m_storageLocations)
                            {
                                StorageArea storageArea = storageLocation.GetComponent<StorageArea>();
                                List<Material> materials = storageArea.GetMaterials();
                                foreach (Material material in materials)
                                {
                                    if (material.GetMatType() == type.Metal)
                                    {
                                        mat = material;
                                        storageArea.UpdateMaterial(-1, type.Metal);
                                        int rand = Random.Range(0, 2);
                                        if (rand == 0)
                                            obj.AddComponent<Armor>();
                                        else
                                            obj.AddComponent<Tools>();

                                        break;
                                    }
                                    else if (material.GetMatType() == type.Leather)
                                    {
                                        mat = material;
                                        storageArea.UpdateMaterial(-1, type.Leather);
                                        obj.AddComponent<Armor>();
                                        break;
                                    }
                                    else if (material.GetMatType() == type.Fiber)
                                    {
                                        mat = material;
                                        storageArea.UpdateMaterial(-1, type.Fiber);
                                        int rand = Random.Range(0, 2);
                                        if (rand == 0)
                                            obj.AddComponent<Clothing>();
                                        else
                                            obj.AddComponent<Tools>();

                                        break;
                                    }
                                    else if (material.GetMatType() == type.Wood)
                                    {
                                        mat = material;
                                        storageArea.UpdateMaterial(-1, type.Wood);
                                        int rand = Random.Range(0, 3);
                                        if (rand == 0)
                                            obj.AddComponent<Crafts>();
                                        else
                                            obj.AddComponent<Tools>();
                                        break;
                                    }
                                    else if (material.GetMatType() == type.Stone)
                                    {
                                        mat = material;
                                        storageArea.UpdateMaterial(-1, type.Stone);
                                        obj.AddComponent<Crafts>();
                                        break;
                                    }
                                }
                                if (mat != null)
                                    break;
                            }
                            Crafts crafts = obj.GetComponent<Crafts>();
                            Clothing clothing = obj.GetComponent<Clothing>();
                            Armor armor = obj.GetComponent<Armor>();
                            Tools tools = obj.GetComponent<Tools>();
                            int val = 0;
                            if (clothing)
                            {
                                clothing.SetMaterial(mat);
                                clothing.Initalize();
                                clothing.name = clothing.GetName();
                                val = clothing.GetValue();
                                collison.GetComponent<GnomeInventory>().GetItems().Add(clothing);
                            }
                            else if (crafts)
                            {
                                crafts.SetMaterial(mat);
                                crafts.Initalize();
                                crafts.name = crafts.GetName();
                                val = crafts.GetValue();
                                crafts.GetComponent<GnomeInventory>().GetItems().Add(crafts);
                            }
                            else if (armor)
                            {
                                armor.SetMaterial(mat);
                                armor.Initalize();
                                armor.name = armor.GetName();
                                val = armor.GetValue();
                                collison.GetComponent<GnomeInventory>().GetItems().Add(armor);
                            }
                            else if (tools)
                            {
                                tools.SetMaterial(mat);
                                tools.Initalize();
                                tools.name = tools.GetName();
                                val = tools.GetValue();
                                collison.GetComponent<GnomeInventory>().GetItems().Add(tools);
                            }
                            gnomeAI.SetCompletedJob(gnomeAI.m_storeItem);
                            if (val > 45)
                            {
                                collison.GetComponent<Mood>().AddMood(GnomeMood.Happy);
                            }
                            else
                            {
                                collison.GetComponent<Mood>().AddMood(GnomeMood.Sad);
                            }
                        }
                        else if (m_completedJob == gnomeAI.m_brewing)
                        {
                            obj.AddComponent<PreparedDrinks>();
                            foreach (GameObject storageLocation in m_storageLocations)
                            {
                                StorageArea storageArea = storageLocation.GetComponent<StorageArea>();
                                List<Material> materials = storageArea.GetMaterials();
                                foreach (Material material in materials)
                                {
                                    if (material.GetMatType() == type.Drink)
                                    {
                                        mat = material;
                                        storageArea.UpdateMaterial(-1, type.Drink);
                                        break;
                                    }
                                }
                                if (mat != null)
                                    break;
                            }
                            PreparedDrinks preparedDrinks = obj.GetComponent<PreparedDrinks>();
                            preparedDrinks.SetMaterial(mat);
                            preparedDrinks.Initalize();
                            gnomeAI.SetCompletedJob(gnomeAI.m_storeDrink);
                            preparedDrinks.name = preparedDrinks.GetName();
                            if (preparedDrinks.GetValue() > 45)
                            {
                                collison.GetComponent<Mood>().AddMood(GnomeMood.Happy);
                            }
                            else
                            {
                                collison.GetComponent<Mood>().AddMood(GnomeMood.Sad);
                            }

                            collison.GetComponent<GnomeInventory>().GetItems().Add(preparedDrinks);
                        }
                        else if (m_completedJob == gnomeAI.m_cooking)
                        {
                            obj.AddComponent<PreparedMeals>();
                            foreach (GameObject storageLocation in m_storageLocations)
                            {
                                StorageArea storageArea = storageLocation.GetComponent<StorageArea>();
                                List<Material> materials = storageArea.GetMaterials();
                                foreach (Material material in materials)
                                {
                                    if (material.GetMatType() == type.Food)
                                    {
                                        mat = material;
                                        storageArea.UpdateMaterial(-1, type.Food);
                                        break;
                                    }
                                }
                                if (mat != null)
                                    break;
                            }
                            PreparedMeals preparedMeals = obj.GetComponent<PreparedMeals>();
                            preparedMeals.SetMaterial(mat);
                            preparedMeals.Initalize();
                            gnomeAI.SetCompletedJob(gnomeAI.m_storeMeal);
                            preparedMeals.name = preparedMeals.GetName();
                            if (preparedMeals.GetValue() > 45)
                            {
                                collison.GetComponent<Mood>().AddMood(GnomeMood.Happy);
                            }
                            else
                            {
                                collison.GetComponent<Mood>().AddMood(GnomeMood.Sad);
                            }

                            collison.GetComponent<GnomeInventory>().GetItems().Add(preparedMeals);
                        }                   
                    }
                }
            }
            else if (collison.CompareTag("Enemy"))
            {
                m_health += collison.GetComponentInChildren<Enemy>().GetUpdateAmount();
            }
        }
    }

    // Gets type of workshop
    public Job GetCompletedJob(GnomeAI gnomeAI)
    {
        if (m_workshopType == WorkshopType.Workshop)
            m_completedJob = gnomeAI.m_creativity;
        else if (m_workshopType == WorkshopType.Kitchen)
            m_completedJob = gnomeAI.m_cooking;
        else if (m_workshopType == WorkshopType.Still)
            m_completedJob = gnomeAI.m_brewing;

        return m_completedJob;
    }
}
