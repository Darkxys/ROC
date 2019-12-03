using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Ennemy _go;
    private GameObject _interfaceTransition;

    public GameObject Interface
    {
        set
        {
            _interfaceTransition = value;
        }
    }

    private void Awake()
    {
        _go = this.GetComponent<Ennemy>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_go.vie <= 0)
        {
            _interfaceTransition.SetActive(true);
            GameObject.Destroy(this);
        }
    }
}
