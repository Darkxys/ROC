using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarresHud : MonoBehaviour
{

   private float DoseRemplissage;

   [SerializeField]
   private Text valueText;

   [SerializeField]
   private float lerpSpeed;
   [SerializeField]
   private Image content;

   public float MaxValue { get; set; }

   public float Value
   {
      set
      {
         string[] tmp = valueText.text.Split(':');
         valueText.text = tmp[0] + ": " + value + "/" + MaxValue;
         DoseRemplissage = Map(value, 0, MaxValue);
      }
   }


   // Start is called before the first frame update
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {
      Handlebar();

   }

   private void Handlebar()
   {
      if (content == null)
      {
         return;
      }
      if (DoseRemplissage != content.fillAmount)
      {
         content.fillAmount = Mathf.Lerp(content.fillAmount, DoseRemplissage, Time.deltaTime * lerpSpeed);

      }
   }

   private float Map(float valeur, float min, float max)
   {
      return valeur / max;

   }
}
