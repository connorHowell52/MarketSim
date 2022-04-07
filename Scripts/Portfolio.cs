using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Portfolio : MonoBehaviour
{
    public Image[] imagesPieChart;
    public TextMeshProUGUI cashPercentText;
    public TextMeshProUGUI pubCPercentText;
    public TextMeshProUGUI privCPercentText;
    public TextMeshProUGUI portfolioWorth;
    public Transform contentContainer;
    public GameObject privCBanner;
    public GameObject pubCBanner;
    public GameObject ownedPrivC;
    public GameObject ownedPubC;
    public GameObject home;
    private Bank bankScript;
    public Button advanceTimeButton;
    public AdvanceTime advanceTimeScript;
    // Start is called before the first frame update
    void Start()
    {
        bankScript = home.GetComponent<Bank>();
        advanceTimeScript = advanceTimeButton.GetComponent<AdvanceTime>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updatePortfolio(decimal cashAMT, decimal pubCAMT, decimal privCAMT, PrivateCompany privateScript, PublicCompany publicScript)
    {
        float[] percentages = getPercentages(privCAMT, pubCAMT, cashAMT);
        float total = 0;
        for(int i = 0; i<imagesPieChart.Length; i++)
        {
            total += percentages[i];
            imagesPieChart[i].fillAmount = total;
        }
        cashPercentText.text = "Cash: " + Math.Round(percentages[2]*100,2)+"%";
        pubCPercentText.text = "Public Companies: " + Math.Round(percentages[1]*100,2)+"%";
        privCPercentText.text = "Private Companies: " + Math.Round(percentages[0]*100,2)+"%";

        List<PublicCompany.PubC> ownedPublicList = new List<PublicCompany.PubC>();
        List<PublicCompany.PubC> mergedPublicList = new List<PublicCompany.PubC>();
        List<PrivateCompany.PrivC> ownedPrivateList = new List<PrivateCompany.PrivC>();
        for(int i = 0; i<publicScript.publicCompaniesList.Length; i++)
        {
            PublicCompany.PubC companyToAdd = publicScript.publicCompaniesList[i];
            if (companyToAdd.sharesOwned > 0 && !companyToAdd.merged) {
                ownedPublicList.Add(companyToAdd);
            }else if (companyToAdd.merged)
            {
                mergedPublicList.Add(companyToAdd);
            }
        }
        for(int i = 0; i < privateScript.privC1.Length; i++)
        {
            PrivateCompany.PrivC companyToAdd = privateScript.privC1[i];
            if (companyToAdd.owned)
            {
                ownedPrivateList.Add(companyToAdd);
            }
        }
        for (int i = 0; i < privateScript.privC2.Length; i++)
        {
            PrivateCompany.PrivC companyToAdd = privateScript.privC2[i];
            if (companyToAdd.owned)
            {
                ownedPrivateList.Add(companyToAdd);
            }
        }

        var children = new List<GameObject>();
        foreach (Transform child in contentContainer) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        var itemToGen = Instantiate(pubCBanner);
        itemToGen.transform.SetParent(contentContainer);
        itemToGen.transform.localScale = Vector2.one;

        for(int i = 0; i<ownedPublicList.Count; i++)
        {
            OwnedPubCSet setDataScript = ownedPubC.GetComponent<OwnedPubCSet>();
            PublicCompany.PubC comp = ownedPublicList[i];
            decimal worth = ((decimal)comp.price * (decimal)comp.sharesOwned);
            decimal cost = (((decimal)comp.sharesOwned * (decimal)comp.averageCost));
            decimal gain = (worth - cost);
            setDataScript.setData(comp.name, worth, gain, comp.sharesOwned, comp.price, comp.order, publicScript);
            itemToGen = Instantiate(ownedPubC);
            itemToGen.transform.SetParent(contentContainer);
            itemToGen.transform.localScale = Vector2.one;
        }

        itemToGen = Instantiate(privCBanner);
        itemToGen.transform.SetParent(contentContainer);
        itemToGen.transform.localScale = Vector2.one;

        for(int i = 0; i<ownedPrivateList.Count; i++)
        {
            OwnedPrivCSet setDataScript = ownedPrivC.GetComponent<OwnedPrivCSet>();
            PrivateCompany.PrivC comp = ownedPrivateList[i];
            decimal worth = (decimal)comp.currentCost;
            decimal cost = (decimal)comp.ownedCost;
            decimal gain = (worth - cost);
            setDataScript.setData(comp.name, worth, gain, comp.lastWeekIncome, comp.listedForSale, privateScript, comp.privCNum, comp.privCInList);
            itemToGen = Instantiate(ownedPrivC);
            itemToGen.transform.SetParent(contentContainer);
            itemToGen.transform.localScale = Vector2.one;
        }
        for (int i = 0; i < mergedPublicList.Count; i++)
        {
            OwnedPubCSet setDataScript = ownedPubC.GetComponent<OwnedPubCSet>();
            PublicCompany.PubC comp = mergedPublicList[i];
            decimal worth = ((decimal)comp.price * (decimal)comp.sharesOwned);
            decimal cost = (((decimal)comp.sharesOwned * (decimal)comp.averageCost));
            decimal gain = (worth - cost);
            setDataScript.setData(comp.name, worth, gain, comp.sharesOwned, comp.price, comp.order, publicScript);
            itemToGen = Instantiate(ownedPubC);
            itemToGen.transform.SetParent(contentContainer);
            itemToGen.transform.localScale = Vector2.one;
        }
    }
    private float[] getPercentages(decimal privCAMT, decimal pubCAMT, decimal cashAMT)
    {
        decimal totalAMT = privCAMT + pubCAMT + cashAMT;
        advanceTimeScript.userComp.assets = totalAMT;
        advanceTimeScript.userComp.stockWorth = pubCAMT;
        advanceTimeScript.userComp.ownedCompworth = privCAMT;
        portfolioWorth.text = String.Format("{0:C}", totalAMT-bankScript.solace.loaned);
        float privCPer = (float)(privCAMT / totalAMT);
        float pubCPer = (float)(pubCAMT / totalAMT);
        float cashPer = (float)(cashAMT / totalAMT);
        float[] toReturn = {privCPer, pubCPer, cashPer};
        return toReturn;

    }
}
