using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// StorageArea.cs
// Josiah Nistor
// Inherits from destination, overrides the wait enumerator, in order to update gnome stats, and job after a collision
public class StorageArea : Destination
{
    [SerializeField] private int m_gold;
    [SerializeField] private int m_food;
    [SerializeField] private int m_drinks;
    [SerializeField] private int m_wood;
    [SerializeField] private int m_stone;
    [SerializeField] private int m_fiber;
    [SerializeField] private int m_leather;
    [SerializeField] private int m_metal;

    [SerializeField] private List<Object> m_objects = new List<Object> { };
    [SerializeField] private List<Material> m_allMaterials = new List<Material> { };

    [SerializeField] private GameObject m_materials;
    [SerializeField] private GameObject m_materialPrefab;

    // Find materials
    new private void Start()
    {
        base.Start();
        m_materials = GameObject.Find("Materials");
    }

    // Remove null items from list
    private void Update()
    {
        if (m_objects.Count > 1)
        {
            foreach (Object obj in m_objects)
            {
                if (obj == null)
                    m_objects.Remove(obj);
            }
        }
    }

    // StorageArea stores items that the gnomes have gathered, untill they are needed
    protected override IEnumerator Wait(float waitTime, GameObject collison)
    {
        yield return new WaitForSeconds(waitTime);

        if (collison != null && collison.CompareTag("Enemy"))
        {
            m_health += collison.GetComponentInChildren<Enemy>().GetUpdateAmount();
        }
        else if (collison != null && collison.CompareTag("NPC"))
        {
            if (collison.gameObject.GetComponent<GnomeAI>().GetDest() == gameObject)
            {
                GnomeAI gnomeAI = collison.GetComponent<GnomeAI>();
                Stats gnomeStats = collison.GetComponent<Stats>();
                Mood gnomeMood = collison.GetComponent<Mood>();
                GnomeInventory gnomeInv = collison.GetComponent<GnomeInventory>();
                Material newMat = gnomeInv.GetCarryingMaterial();
                gnomeInv.SetCarryingMaterial(null);

                Job completedJob = gnomeAI.GetCompletedJob();
                Job FavJob = gnomeStats.GetFavJob();
                int personalityIndex = gnomeAI.GetPersonality().GetPersonalityIndex();
                List<Object> items = gnomeInv.GetItems();

                if (m_health < 100 && collison.CompareTag("NPC") && collison.GetComponent<GnomeAI>().GetCurrentJob() == gnomeAI.m_construction)
                {
                    gnomeAI.ForceSetJob(null);
                    collison.GetComponent<Stats>().UpdateSkillLevel(gnomeAI.m_construction, 0.1);
                    m_health = 100;
                }
                else if (newMat)
                {
                    if (completedJob == gnomeAI.m_gatherFood)
                    {
                        AddFood(newMat);
                        if (FavJob == gnomeAI.m_gatherFood && personalityIndex == 1)
                        {
                            AddFood(DuplicateMaterial(newMat));
                        }
                    }
                    else if (completedJob == gnomeAI.m_gatherWater)
                    {
                        AddDrinks(newMat);
                        if (FavJob == gnomeAI.m_gatherWater && personalityIndex == 1)
                        {
                            AddDrinks(DuplicateMaterial(newMat));
                        }
                    }
                    else if (completedJob == gnomeAI.m_gatherWood)
                    {
                        AddWood(newMat);
                        if (FavJob == gnomeAI.m_gatherWood && personalityIndex == 1)
                        {
                            AddWood(DuplicateMaterial(newMat));
                        }
                    }
                    else if (completedJob == gnomeAI.m_gatherStone)
                    {
                        AddStone(newMat);
                        if (FavJob == gnomeAI.m_gatherStone && personalityIndex == 1)
                        {
                            AddStone(DuplicateMaterial(newMat));
                        }
                    }
                    else if (completedJob == gnomeAI.m_gatherFiber)
                    {
                        AddFiber(newMat);
                        if (FavJob == gnomeAI.m_gatherFiber && personalityIndex == 1)
                        {
                            AddFiber(DuplicateMaterial(newMat));
                        }
                    }
                }
                else if (completedJob == gnomeAI.m_storeItem)
                {
                    Object obj = items.Last();
                    m_objects.Add(obj);
                    items.Remove(obj);
                }
                else if (completedJob == gnomeAI.m_storeDrink)
                {
                    Object obj = items.Last();
                    m_objects.Add(obj);
                    m_drinks++;
                    items.Remove(obj);
                }
                else if (completedJob == gnomeAI.m_storeMeal)
                {
                    Object obj = items.Last();
                    m_objects.Add(obj);
                    m_food++;
                    items.Remove(obj);
                }
                else if (completedJob == gnomeAI.m_getItem)
                {
                    ItemType itemWant = collison.gameObject.GetComponent<Want>().GetItemWant();
                   foreach(Object obj in m_objects)
                   {
                        if (obj.GetObjectType() == itemWant)
                        {
                            if (itemWant == ItemType.Clothing)
                                gnomeInv.SetCloths((Clothing)obj);
                            else if (itemWant == ItemType.Armor)
                                gnomeInv.SetArmor((Armor)obj);
                            else if (itemWant == ItemType.Tools)
                                gnomeInv.SetTools((Tools)obj);
                            else if (itemWant == ItemType.Crafts)
                                items.Add(obj);
                            m_objects.Remove(obj);
                            break;
                        }    
                   }
                }
                else
                {
                    if (gnomeAI.GetNeedWater())
                    {
                        Decision thirstDecision = gnomeAI.GetThirstDecision();
                        bool drank = false;
                        thirstDecision.UpdateNeed(-m_updateAmmount);
                        foreach (Object obj in m_objects)
                        {
                            if (obj.GetObjectType() == ItemType.PreparedDrinks)
                            {
                                thirstDecision.UpdateNeed(-m_updateAmmount);
                                m_objects.Remove(obj);
                                m_drinks--;
                                drank = true;
                                break;
                            }
                        }
                        if (m_drinks > 0 && !drank)
                        {
                            foreach (Material material in m_allMaterials)
                            {
                                if (material.GetMatType() == type.Drink)
                                {
                                    m_allMaterials.Remove(material);
                                    m_drinks--;
                                    break;
                                }
                            }
                        }
                    }
                    else if (gnomeAI.GetNeedFood())
                    {
                        Decision foodDecision = gnomeAI.GetFoodDecision();
                        bool ate = false;
                        foodDecision.UpdateNeed(-m_updateAmmount);
                        foreach (Object obj in m_objects)
                        {
                            if (obj.GetObjectType() == ItemType.PreparedMeals)
                            {
                                foodDecision.UpdateNeed(-m_updateAmmount);
                                m_objects.Remove(obj);
                                m_food--;
                                ate = true;
                                break;
                            }
                        }
                        if (m_food > 0 && !ate)
                        {
                            foreach (Material material in m_allMaterials)
                            {
                                if (material.GetMatType() == type.Food)
                                {
                                    m_allMaterials.Remove(material);
                                    m_food--;
                                    break;
                                }
                            }
                        }

                    }
                    gnomeAI.ResetNeeds();
                }
                gnomeInv.ClearCarryingMaterial();
                gnomeAI.SetCompletedJob(null);
                gnomeAI.ForceSetJob(null);
                m_ocupier = null;
            }
        }
        
    }

    // Duplicate material when gathered if proper personality
    private Material DuplicateMaterial(Material oldMat)
    {
        Material material = Instantiate(m_materialPrefab, m_materials.transform).GetComponent<Material>();
        material.SetMatType(oldMat.GetMatType());
        material.SetMaterial(oldMat.GetMaterial());
        return material;
    }

    // Add Diffrent types of materials to storage
    private void AddFood(Material newMat)
    {
        m_food++;
        m_allMaterials.Add(newMat);
        if (newMat.GetMaterial() == mat.Rabbit)
        {
            m_leather++;
            Material material = Instantiate(m_materialPrefab, m_materials.transform).GetComponent<Material>();
            material.SetMatType(type.Leather);
            material.SetMaterial(mat.Rabbit);
            m_allMaterials.Add(material);
        }
    }
    private void AddDrinks(Material newMat)
    {
        m_drinks++;
        m_allMaterials.Add(newMat);
    }
    private void AddWood(Material newMat)
    {
        m_wood++;
        m_allMaterials.Add(newMat);
    }
    private void AddStone(Material newMat)
    {
        m_stone++;
        m_allMaterials.Add(newMat);
        int rand = Random.Range(0, 2);
        if (rand == 1)
        {
            m_metal++;
            Material material = Instantiate(m_materialPrefab, m_materials.transform).GetComponent<Material>();
            material.SetMatType(type.Metal);
            if (newMat.GetMaterial() == mat.Sandstone)
            {
                material.SetMaterial(mat.Copper);
            }
            else if (newMat.GetMaterial() == mat.Granite)
            {
                material.SetMaterial(mat.Iron);
            }
            else if (newMat.GetMaterial() == mat.Slate)
            {
                material.SetMaterial(mat.Silver);
            }
            m_allMaterials.Add(material);

        }
    }
    private void AddFiber(Material newMat)
    {
        m_fiber++;
        m_allMaterials.Add(newMat);
    }

    // Getters
    public int GetFood() { return m_food; }
    public int GetDrinks() {  return m_drinks; }
    public int GetWood() {  return m_wood; }
    public int GetStone() {  return m_stone; }
    public int GetFiber() {  return m_fiber; }
    public int GetLeather() {  return m_leather; }
    public int GetMetal() {  return m_metal; }
    public int GetGold() { return m_gold; }
    public List<Object> GetObjects() { return m_objects; }
    public List<Material> GetMaterials() { return m_allMaterials; }

    // Remove an amount of a material from storage based on type
    public void UpdateMaterial(int amount, type matType)
    {
        foreach (Material material in m_allMaterials)
        {
            if (material.GetMatType() == matType)
            {
                if (type.Drink == matType)
                    m_drinks += amount;
                else if (type.Food == matType)
                    m_food += amount;
                else if (type.Metal == matType)
                    m_metal += amount;
                else if (type.Leather == matType)
                    m_leather += amount;
                else if (type.Fiber == matType)
                    m_fiber += amount;
                else if (type.Stone == matType)
                    m_stone += amount;
                else if (type.Wood == matType)
                    m_wood += amount;

                m_allMaterials.Remove(material);
                break;
            }
        }
    }
    public void UpdateGold(int gold) {  m_gold += gold; }
}
