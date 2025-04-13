using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;

// Job.cs
// Josiah Nistor
// Base class for each job subclass that can be assigned to the gnome some of which have unique functions
public class Job 
{
    protected GnomeAI m_gnomeAI;
    protected string m_jobName;
    protected Decision m_decision;

    // Initializes decision and gnome AI refrences, as well as setting name
    public void Initialize(Decision decision, GnomeAI gnomeAI)
    {
        m_decision = decision;
        m_gnomeAI = gnomeAI;
        SetName();
    }

    // Base function, sets name for lookup later
    protected virtual void SetName()
    {
        m_jobName = "None";
    }

    // Base function, find closest and moves based on set decision
    public virtual void FindClosesestAndMove()
    {
        m_decision.FindClosestAndMove(0);
    }

    // Get name in format for UI
    public string GetName()
    {
        return ": " + m_jobName;
    }

    // Base function for workshops
    public virtual bool workshopBeavior(InventoryManager invManger)
    {
        return true;
    }
}

// All subclasses of Jobs
public class GatherFood : Job
{
    protected override void SetName()
    {
        m_jobName = "Gather Food";
    }
}
public class GatherWater : Job
{
    protected override void SetName()
    {
        m_jobName = "Gather Water";
    }
}
public class Tiredness : Job
{
    protected override void SetName()
    {
        m_jobName = "Tiredness";
    }
}
public class Sociality : Job    
{
    protected override void SetName()
    {
        m_jobName = "Sociality";
    }
}
public class Creativity : Job 
{
    protected override void SetName()
    {
        m_jobName = "Creativity";
    }
    public override bool workshopBeavior(InventoryManager invManger)
    {
        if (invManger.GetStone() <= 0 && invManger.GetWood() <= 0 && invManger.GetFiber() <= 0 && invManger.GetLeather() <= 0 && invManger.GetMetal() <= 0)
            return false;

        return true;
    }
}
public class Belief : Job
{
    protected override void SetName()
    {
        m_jobName = "Belief";
    }
}
public class GatherWood : Job
{
    protected override void SetName()
    {
        m_jobName = "Gather Wood";
    }
    public override void FindClosesestAndMove()
    {
        m_decision.FindClosestAndMove(0, m_gnomeAI.GetWorldManager().GetWoodLocations());
    }
}
public class GatherStone : Job
{
    protected override void SetName()
    {
        m_jobName = "Gather Stone";
    }
    public override void FindClosesestAndMove()
    {
        m_decision.FindClosestAndMove(0, m_gnomeAI.GetWorldManager().GetStoneLocations());
    }
}
public class Brewing : Job
{
    protected override void SetName()
    {
        m_jobName = "Brewing";
    }
    public override bool workshopBeavior(InventoryManager invManger)
    {
        if (invManger.GetDrinks() <= 0)
            return false;
        return true;
    }
}
public class Cooking : Job
{
    protected override void SetName()
    {
        m_jobName = "Cooking";
    }
    public override bool workshopBeavior(InventoryManager invManger)
    {
        if (invManger.GetFood() <= 0)
            return false;
        return true;
    }
}
public class Fighting : Job
{
    protected override void SetName()
    {
        m_jobName = "Fighting";
    }
    public override void FindClosesestAndMove()
    {
        m_decision.FindClosestAndMove(0, m_gnomeAI.GetWorldManager().GetEnemyLocations());
    }
}
public class Construction : Job
{
    protected override void SetName()
    {
        m_jobName = "Construction";
    }
    public override void FindClosesestAndMove()
    {
        m_decision.FindClosestAndMove(0, m_gnomeAI.GetWorldManager().GetStructureLocations());
    }
}
public class StoreItem : Job
{
    protected override void SetName()
    {
        m_jobName = "Store Item";
    }
}
public class StoreDrink : Job
{
    protected override void SetName()
    {
        m_jobName = "Store Drink";
    }
}
public class StoreMeal : Job
{
    protected override void SetName()
    {
        m_jobName = "Store Meal";
    }
}
public class GetItem : Job
{
    protected override void SetName()
    {
        m_jobName = "Get Item";
    }
}
public class GatherFiber : Job
{
    protected override void SetName()
    {
        m_jobName = "Gather Fiber";
    }
    public override void FindClosesestAndMove()
    {
        m_decision.FindClosestAndMove(0, m_gnomeAI.GetWorldManager().GetFiberLocations());
    }
}