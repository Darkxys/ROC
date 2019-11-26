using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoom : MonoBehaviour
{
    
    void Update()
    {
      if (Input.GetAxis ("Mouse ScrollWheel") > 0 && GetComponent<Camera>().orthographicSize > 2)
      {
         GetComponent<Camera>().orthographicSize -= 1;
      }

      if (Input.GetAxis("Mouse ScrollWheel") < 0 && GetComponent<Camera>().orthographicSize < 10)
      {
         GetComponent<Camera>().orthographicSize += 1;
      }
      
    }
}
