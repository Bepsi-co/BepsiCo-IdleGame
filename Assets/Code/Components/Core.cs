using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public double Bank { get; set; }
    private const double startingBank = 1000;

    // Start is called before the first frame update
    void Start()
    {
        Bank = startingBank;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
