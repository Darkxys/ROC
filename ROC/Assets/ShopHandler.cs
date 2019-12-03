using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHandler : MonoBehaviour
{
    private GameObject player;
    private PlayerStats playerStats;
    [SerializeField] private Text goldText;
    [SerializeField] private ItemsHandler itemHandler;
    [SerializeField] private Text err;

    private void Awake()
    {
        playerStats = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerStats>();
        player = GameObject.FindGameObjectWithTag("player");




    }
    public void buyItem()
    {
        if (itemHandler.selectedItem != null)
        {
            if (itemHandler.selectedItem.Prix <= playerStats.Gold)
            {
                playerStats.Gold -= itemHandler.selectedItem.Prix;
                playerStats.ItemsPos.Add(itemHandler.selectedItem.Id);
            }
            else
            {
                err.enabled = true;
                err.text = "Vous etes trop pauvre pour vous procurer cet item";
            }

        }
        else
        {
            err.enabled = true;
            err.text = "Aucun item selectionné";
        }
        UPDStat();

    }

    public void UPDStat()
    {
        goldText.text = playerStats.Gold.ToString();

    }

    public void effacerErr()
    {
        err.enabled = false;
    
    }


}
