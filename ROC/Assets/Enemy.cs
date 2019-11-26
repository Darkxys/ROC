using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Space]
    [Header("Stats Enemy")]
    public float PointsDeVie;
    public float Niveau = 2;
    public float VITTESSE_DE_DEPLACEMENTS = 1f;

    private void Awake()
    {
        PointsDeVie = Niveau * 100;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
