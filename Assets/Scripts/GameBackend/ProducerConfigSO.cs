using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ProducerConfigSO")]
public class ProducerConfigSO : ScriptableObject
{
    [SerializeField]
    public string ProducerName;

    [SerializeField]
    public double BasePurchasePrice;

    [SerializeField]
    public double BaseLevelPrice;

    [SerializeField]
    public double BaseProduction;
}
