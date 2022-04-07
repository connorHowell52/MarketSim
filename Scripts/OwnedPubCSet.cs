using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OwnedPubCSet : MonoBehaviour
{
    public TextMeshProUGUI compName;
    public TextMeshProUGUI compWorth;
    public TextMeshProUGUI compGain;
    public TextMeshProUGUI compSharesOwned;
    public TextMeshProUGUI sharesCost;
    public PublicCompany publicCompanyScript;
    public float priceCal;
    public int orderInPubC;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void readStringInput(string amt)
    {
        long amtToReturn = int.Parse(amt);
        publicCompanyScript.sellStock(amtToReturn, orderInPubC);
    }
    public void updatePrice(string amt)
    {
        float amtToReturn = int.Parse(amt);
        amtToReturn = amtToReturn * priceCal;
        sharesCost.text = String.Format("{0:C}", amtToReturn);
        amtToReturn = 0;
    }
    public void setData(string name, decimal worth, decimal gain, long sharesOwned, float price, int order, PublicCompany pubCScript)
    {
        compName.text = name;
        compWorth.text = String.Format("{0:C}", worth);
        compGain.text = "Gain: "+ String.Format("{0:C}", gain);
        compSharesOwned.text = "Shares Owned: "+ sharesOwned;
        priceCal = price;
        orderInPubC = order;
        publicCompanyScript = pubCScript;
    }
}
