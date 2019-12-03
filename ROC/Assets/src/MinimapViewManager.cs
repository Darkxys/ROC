using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapViewManager : MonoBehaviour
{
    [SerializeField] private GameObject _miniMap;

    public void OnClick()
    {
        _miniMap.SetActive(!_miniMap.activeSelf);
    }
}
