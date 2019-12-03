using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapViewManager : MonoBehaviour
{
    [SerializeField] private GameObject _char;
    [SerializeField] private Player _player;
    [SerializeField] private RectTransform _map;

    private void Update()
    {
        _player.RefreshPosMinimap(_map, _char);
    }
}
