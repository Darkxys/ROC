using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManagerScreen : MonoBehaviour
{
    public GameObject menuSave;
    public GameObject menuSetting;
    
    public void Save()
    {
        menuSave.SetActive(false);

        GameObject go = GameObject.FindGameObjectWithTag("Saver");
        SaveHandler save = go.GetComponent<SaveHandler>();
        save.SaveGame();

        SetInteractable(true);

        GameObject.Destroy(go);
        SceneManager.LoadScene("MenuPrincipal", LoadSceneMode.Single);
    }

    public void QuitSave()
    {
        menuSave.SetActive(false);

        SetInteractable(true);

        GameObject go = GameObject.FindGameObjectWithTag("Saver");
        GameObject.Destroy(go);

        SceneManager.LoadScene("MenuPrincipal", LoadSceneMode.Single);
    }

    public void Cancel()
    {
        menuSave.SetActive(false);

        SetInteractable(true);
    }

    public void ShowSave()
    {
        menuSave.SetActive(true);
        SetInteractable(false);
    }

    public void SaveAndReturn()
    {
        menuSave.SetActive(false);

        SaveHandler save = GameObject.FindGameObjectWithTag("Saver").GetComponent<SaveHandler>();
        save.SaveGame();

        SetInteractable(true);
    }
    private void SetInteractable(bool isInteractible)
    {
        foreach (Transform child in menuSetting.transform)
        {
            Button btn = child.gameObject.GetComponent<Button>();

            if (btn != null)
                btn.interactable = isInteractible;

        }
    }
}
