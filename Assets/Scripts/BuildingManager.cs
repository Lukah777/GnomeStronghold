using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// BuildingManager.cs
// Josiah Nistor
// Handels user UI input in order to create new builings 
public class BuildingManager : MonoBehaviour
{
    [SerializeField] private GameObject m_bedPrefab;
    [SerializeField] private GameObject m_shrinePrefab;
    [SerializeField] private GameObject m_workshopPrefab;
    [SerializeField] private GameObject m_storagePrefab;

    [SerializeField] private GameObject m_beds;
    [SerializeField] private GameObject m_shrines;
    [SerializeField] private GameObject m_workshops;
    [SerializeField] private GameObject m_storages;

    [SerializeField] private InventoryManager m_inventoryManager;

    [SerializeField] private GameObject m_highlight;

    [SerializeField] private Image BuildModeImg;

    private Vector3 m_mousePos;
    private bool m_buildMode = false;

    // Check if in build mode and display highligh and save selected position
    private void Update()
    {
        if (m_buildMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10000))
            {
                m_mousePos.x = Mathf.Round(hit.point.x);
                m_mousePos.y = 0;
                m_mousePos.z = Mathf.Round(hit.point.z);
                m_highlight.SetActive(true);
                m_highlight.transform.position = m_mousePos;
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                m_buildMode = false;
                BuildModeImg.color = Color.white;
            }
        }      
    }

    // On button press toggle build mode, color button
    public void BuildMode()
    {
        if (!m_buildMode)
        {
            m_buildMode = true;
            BuildModeImg.color = Color.red;
        }
        else
        {
            m_buildMode = false;
            BuildModeImg.color = Color.white;
            m_highlight.SetActive(false);
        }
    }

    // Build bed on button press
    public void BuildBed()
    {
        BuildStructure(1, 0, m_bedPrefab, m_beds);
    }

    // Build Shrine on button press
    public void BuildShrine()
    {
        BuildStructure(0, 1, m_shrinePrefab, m_shrines);
    }

    // Build workshop on button press
    public void BuildWorkshop()
    {
        BuildStructure(1, 1, m_workshopPrefab, m_workshops);
    }

    // Build storage on button press
    public void BuildStorage()
    {
        BuildStructure(1, 1, m_storagePrefab, m_storages);
    }

    // Build structure if you have the required materials then instantiate prefab under the parent object
    public void BuildStructure(int woodNeeded, int stoneNeeded, GameObject prefab, GameObject parent)
    {
        bool hasResources = false;

        if (woodNeeded > 0)
        {
            if (m_inventoryManager.GetWood() > 0)
                hasResources = true;
            else
                hasResources = false;
        }

        if (stoneNeeded > 0)
        {
            if (m_inventoryManager.GetStone() > 0)
                hasResources = true;
            else
                hasResources = false;
        }

        if (hasResources)
        {
            m_inventoryManager.UpdateMaterial(-woodNeeded, type.Wood);
            m_inventoryManager.UpdateMaterial(-stoneNeeded, type.Stone);
            m_highlight.SetActive(false);
            Instantiate<GameObject>(prefab, m_mousePos, Quaternion.identity, parent.transform);
        }
    }
}
