using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneSwitcher : MonoBehaviour
{
    public void GotoMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void GotoMenuScene()
    {
        SceneManager.LoadScene("menu");
    }
}
