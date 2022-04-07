using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PublicCompanySet : MonoBehaviour
{
    public TextMeshProUGUI nameObject;
    public TextMeshProUGUI peBVPSMC;
    public TextMeshProUGUI yoyDivRange;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI range;
    private GameObject home;
    public PublicCompany publicCompanyScript;
    public int orderInPublicC;
    public TextMeshProUGUI percentOwnedText;
    public TextMeshProUGUI sharesCost;
    public float priceCal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void mergePubC()
    {
        publicCompanyScript.publicMerger(orderInPublicC);
    }
    public void updatePrice(string amt)
    {
        float amtToReturn = int.Parse(amt);
        amtToReturn = amtToReturn * priceCal;
        sharesCost.text = String.Format("{0:C}", amtToReturn);
        amtToReturn = 0;
    }
    public void ReadStringInput(string amt)
    {
        long amtToReturn = int.Parse(amt);
        publicCompanyScript.buyStock(amtToReturn, orderInPublicC);
    }
    public void setData(string compName, double priceEarnings, double bookV, long marketCap, double yearOY, double div, float price, float low, float high, GameObject setHome, int orderInPublic, double percentOwned, bool merged)
    {
        nameObject.text = compName;
        string marketCapAdj = getMarketCap(marketCap);
        if (merged)
        {
            priceCal = 0;
            peBVPSMC.text = "PE: - BVPS: - Market Cap: -";
            yoyDivRange.text = "YOY: - Dividend: -";
            range.text = "52 Week Range: -";
        }
        else
        {
            priceCal = price;
            peBVPSMC.text = "PE: " + priceEarnings + " BVPS: " + bookV + " Market Cap: " + marketCapAdj;
            yoyDivRange.text = "YOY: " + yearOY + " Dividend: " + div;
            range.text = "52 Week Range: " + high + "-" + low;
        }
        home = setHome;
        orderInPublicC = orderInPublic;
        publicCompanyScript = home.GetComponent<PublicCompany>();
        priceText.text = price.ToString();
        percentOwnedText.text = "Percent Owned: " + percentOwned.ToString() + "%";

    }
    private string getMarketCap(long marketCap)
    {
        if ((marketCap / 1000000000 > 1) && (marketCap / 1000000000) < 1000)
        {
            return Math.Round((double)marketCap / 1000000000,2) + "B";
        }
        else if ((marketCap / 1000000 > 1) && (marketCap / 1000000) < 1000)
        {
            return Math.Round((double)marketCap / 1000000,2) + "M";
        }
        else
        {
            return Math.Round((double)marketCap / 1000000000000,2) + "T";
        }
    }
}
