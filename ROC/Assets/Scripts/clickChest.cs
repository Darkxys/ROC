using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickChest : MonoBehaviour
{
    private GameObject ui;

    // Start is called before the first frame update
    void Awake()
    {
        ui = GameObject.FindGameObjectWithTag("coffreActif").GetComponent<GarderEcranRef>().troll;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("wow");
        ui.SetActive(true);
    }
}
