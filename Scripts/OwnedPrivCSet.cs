using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OwnedPrivCSet : MonoBehaviour
{
    public TextMeshProUGUI compName;
    public TextMeshProUGUI compWorth;
    public TextMeshProUGUI compGain;
    public TextMeshProUGUI compWeeklyIncome;
    public Text compListed;
    public PrivateCompany privateCompanyScript;
    public int list;
    public int order;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void listStock()
    {
        if (compListed.text == "Unlist")
        {
            privateCompanyScript.unList(list, order);
        }
        else
        {
            privateCompanyScript.listStock(list, order);
        }
    }
    public void setData(string name, decimal worth, decimal gain, long weeklyIncome, bool listed, PrivateCompany privCScript, int privCList, int privCOrder)
    {
        compName.text = name;
        compWorth.text = String.Format("{0:C}", worth);
        compGain.text = "Gain: "+String.Format("{0:C}", gain);
        compWeeklyIncome.text = "Weekly Income: "+String.Format("{0:C}", weeklyIncome);
        if (listed)
        {
            compListed.text = "Unlist";
        }
        else
        {
            compListed.text = "List";
        }
        list = privCList;
        order = privCOrder;
        privateCompanyScript = privCScript;
    }
}
