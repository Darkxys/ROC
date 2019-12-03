using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private GameObject _prefabInterface;
    void OnMouseDown()
    {
        if(_prefabInterface)
            _prefabInterface.SetActive(true);
    }
}
