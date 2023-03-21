using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public delegate double UpgradeOnTick(ProducerBase p, UpgradeManagerBase um);
public delegate void UpgradeOnPurchase(ProducerBase p, UpgradeManagerBase um);
public struct Upgrade
{
    public double Cost;
    public string Description;
    public UpgradeOnTick OnTick;
    public UpgradeOnPurchase OnPurchase;
}

[RequireComponent(typeof(Collider))] // anything with upgrades needs a collider so the upgrade menu's raycast based onClick function works
public abstract class UpgradeManagerBase : MonoBehaviour
{
    public int[] UpgradeLevels { get => _UpgradeLevels; protected set => _UpgradeLevels = value; }
    protected int[] _UpgradeLevels = new int[3] { 0, 0, 0 };

    public Upgrade[,] Upgrades { get => _Upgrades; protected set => _Upgrades = value; }
    protected Upgrade[,] _Upgrades = new Upgrade[3, 5];

    public double ResetCost { get => _ResetCost; protected set => _ResetCost = value; }
    protected double _ResetCost;

    protected Core Coreptr = null;
    protected UpgradeManagerBase UMptr = null;
    protected ProducerBase Pptr = null;

    // should be bound to a UI button somehow?
    // resets the upgrade manager
    public virtual void ResetUpgrades()
    {
        if(Coreptr != null && Coreptr.Bank > ResetCost)
        {
            Coreptr.Bank -= ResetCost;
            Array.Fill(UpgradeLevels, 0);
        }
    }

    // called by producer each fixed update
    // returns income from
    public virtual double Tick()
    {
        double acc = 0;

        for(int path = 0; path < Upgrades.GetLength(0); path++)
        {
            for(int tier = 0; tier < UpgradeLevels[path]; tier++)
            {
                if(Upgrades[path, tier].OnTick != null)
                {
                    acc += Upgrades[path, tier].OnTick(Pptr, this);
                }
            }
        }

        return acc;
    }

    // should be bound to a UI button somehow?
    // purchases upgrade
    public virtual void BuyUpgrade(int path)
    {
        if (Coreptr != null && UpgradeLevels[path] < Upgrades.Rank && Coreptr.Bank > Upgrades[path,UpgradeLevels[path]+1].Cost)
        {
            UpgradeLevels[path] += 1;
            Coreptr.Bank -= Upgrades[path, UpgradeLevels[path]].Cost;
            if (Upgrades[path, UpgradeLevels[path]].OnPurchase != null)
            {
                Upgrades[path, UpgradeLevels[path]].OnPurchase(Pptr, this);
            }
        }
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        Coreptr = GameObject.Find("Core").GetComponent<Core>();
        Pptr = GetComponentInParent<ProducerBase>();
    }
}
