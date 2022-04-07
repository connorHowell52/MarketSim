using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Finance : MonoBehaviour
{
    /*
        Dividend Income
        Interest Income
        Capital Gains
        Operational
        NetIncome
     */
    public TextMeshProUGUI[] year3IStats;
    public TextMeshProUGUI[] year2IStats;
    public TextMeshProUGUI[] year1IStats;
    /*
        Cash
        Owned Companies
        Stocks
        TotalAssets
        Liabilities
        Equity
     */
    public TextMeshProUGUI[] year3BStats;
    public TextMeshProUGUI[] year2BStats;
    public TextMeshProUGUI[] year1BStats;
    public Button advanceTimeButton;
    public AdvanceTime advanceTimeScript;
    public GameObject home;
    // Start is called before the first frame update
    void Start()
    {
        advanceTimeScript = advanceTimeButton.GetComponent<AdvanceTime>();
    }

    // Update is called once per frame
    public void updateYearlyStats()
    {
        for(int i = 0; i<year3IStats.Length; i++)
        {
            year1IStats[i].text = year2IStats[i].text;
            year2IStats[i].text = year3IStats[i].text;
        }
        for (int i = 0; i < year3BStats.Length; i++)
        {
            year1BStats[i].text = year2BStats[i].text;
            year2BStats[i].text = year3BStats[i].text;
        }
        year3BStats[0].text = advanceTimeScript.nextWeek.AddDays(-7).ToString("yyyy");
        year3BStats[1].text = String.Format("{0:C}", advanceTimeScript.userComp.cashATM);
        year3BStats[2].text = String.Format("{0:C}", advanceTimeScript.userComp.ownedCompworth);
        year3BStats[3].text = String.Format("{0:C}", advanceTimeScript.userComp.stockWorth);
        year3BStats[4].text = String.Format("{0:C}", advanceTimeScript.userComp.assets);
        year3BStats[5].text = String.Format("{0:C}", advanceTimeScript.userComp.liabilities);
        year3BStats[6].text = String.Format("{0:C}", advanceTimeScript.userComp.equity);

        year3IStats[0].text = advanceTimeScript.nextWeek.AddDays(-7).ToString("yyyy");
        year3IStats[1].text = String.Format("{0:C}", advanceTimeScript.userComp.year3Div);
        year3IStats[2].text = String.Format("{0:C}", advanceTimeScript.userComp.year3Interest);
        year3IStats[3].text = String.Format("{0:C}", advanceTimeScript.userComp.year3CapitalGains);
        year3IStats[4].text = String.Format("{0:C}", advanceTimeScript.userComp.year3Operational);
        year3IStats[5].text = String.Format("{0:C}", advanceTimeScript.userComp.year3Income);

    }
}
