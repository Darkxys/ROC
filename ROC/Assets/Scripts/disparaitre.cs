using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disparaitre : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 2f);
    }
}
