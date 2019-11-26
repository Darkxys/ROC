using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarderEcranRef : MonoBehaviour
{
    [SerializeField]
    public GameObject troll;

    // Start is called before the first frame update
    void Start()
    {
        troll.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
