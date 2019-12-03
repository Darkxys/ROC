using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{ 
    [SerializeField]private Sprite img1;
    [SerializeField]private Sprite img2;
    [SerializeField]private Sprite img3;
    [SerializeField]private Sprite img4;
    [SerializeField]private Sprite img5;
    [SerializeField]private Sprite img6;
    [SerializeField]private Sprite img7;
    [SerializeField]private Sprite img8;
    [SerializeField]private Sprite img9;


    private List<Sprite> lstImages = new List<Sprite>();

    public Items item;

    private ItemsHandler _itemHandler;

    private void Awake()
    {

        _itemHandler = GameObject.FindGameObjectWithTag("ListeContent").GetComponent<ItemsHandler>();


    }
    private void Start()
    {
        lstImages.Add(img1);
        lstImages.Add(img2);
        lstImages.Add(img3);
        lstImages.Add(img4);
        lstImages.Add(img5);
        lstImages.Add(img6);
        lstImages.Add(img7);
        lstImages.Add(img8);
        lstImages.Add(img9);
        gameObject.transform.GetChild(0).GetComponent<Image>().sprite = lstImages[item.Id-1];

    }

    public void UPDStats()
    {
        _itemHandler.selectedItem = item;
        _itemHandler.updateSeletedItem();
        
    }

 



}
