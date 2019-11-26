using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate : MonoBehaviour
{
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    public void troll()
    {
        if (!GameControl.youWin)
        {
            _rectTransform.Rotate(0f, 0f, 90f);
        }
    }

    
}
