using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnPlay : MonoBehaviour
{
    public void Play()
    {
        SaveHandler handler = GameObject.FindGameObjectWithTag("Saver").GetComponent<SaveHandler>();
        handler.Play();
    }
    public void Delete()
    {
        ScreenManager handler = GameObject.Find("ScreenManager").GetComponent<ScreenManager>();
        handler.Delete();
    }
}
