using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class AdvanceTime : MonoBehaviour
{
    public Text advanceDateText;
    public DateTime nextWeek;
    public TextMeshProUGUI playerMoney;
    public string test = "test";
    public GameObject home;
    private PrivateCompany privateCompanyScript;
    private PublicCompany publicCompanyScript;
    private Finance financeScript;
    private object[] privateUserCompanies;
    private object[] publicUserCompanies;
    public userCompany userComp;
    private Portfolio portfolioScript;
    private Bank bankScript;
    public Transform contentContainer;
    public setHomeScreenStats setStatsScript;
    public GameObject statsPrefab;
    public decimal[] moneyStats = new decimal[4];
    public List<income> weeklyIncome = new List<income>();
    /*
    public decimal divIncome;
    public decimal intrestIncome;
    public decimal gainsIncome;
    public decimal operationsIncome;
    */
    // Start is called before the first frame update
    void Start()
    {
        privateCompanyScript = home.GetComponent<PrivateCompany>();
        publicCompanyScript = home.GetComponent<PublicCompany>();
        portfolioScript = home.GetComponent<Portfolio>();
        financeScript = home.GetComponent<Finance>();
        bankScript = home.GetComponent<Bank>();
        DateTime thisYear = DateTime.Now;
        nextWeek = new DateTime(thisYear.Year, 1, 1, 1, 1, 1);
        advanceDateText.text = nextWeek.ToString("dd-MM-yyyy");
        userComp = setUpNewUser();
        bankScript.setUpBank();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateMoney(decimal i)
    {
        userComp.cashATM = (userComp.cashATM + i);
        playerMoney.text = String.Format("{0:C}", userComp.cashATM);
        updateStats();
        portfolioScript.updatePortfolio(userComp.cashATM, publicCompanyScript.determinePubCWorth(), privateCompanyScript.determinePrivCWorth(), privateCompanyScript, publicCompanyScript);
    }
    public void advanceNextWeek()
    {
        nextWeek = nextWeek.AddDays(7);
        advanceDateText.text = nextWeek.ToString("dd-MM-yyyy");
        privateCompanyScript.nextWeekPrivC();
        publicCompanyScript.nextWeekPubC();
        bankScript.nextWeekBank();
        portfolioScript.updatePortfolio(userComp.cashATM, publicCompanyScript.determinePubCWorth(), privateCompanyScript.determinePrivCWorth(), privateCompanyScript, publicCompanyScript);
        updateStats();
        setStats();
    }
    public class userCompany
    {
        public decimal year3Income;
        public decimal year3Interest;
        public decimal year3Div;
        public decimal year3Operational;
        public decimal year3CapitalGains;
        public decimal assets;
        public decimal cashATM;
        public decimal liabilities;
        public decimal liabilitiesPayment;
        public decimal equity;
        public decimal ownedCompworth;
        public decimal stockWorth;
        public bool winGame;
    }
    private userCompany setUpNewUser()
    {
        userCompany toReturn = new userCompany();
        toReturn.cashATM = 0;
        toReturn.liabilities = 0;
        toReturn.cashATM = (1000000);
        toReturn.equity = toReturn.cashATM;
        toReturn.assets = toReturn.cashATM;
        playerMoney.text = String.Format("{0:C}", toReturn.cashATM);
        return toReturn;
    }
    private void updateStats()
    {
        userComp.assets = 0;
        userComp.liabilities = 0;
        decimal returnedPrivCA = privateCompanyScript.getUserPrivStats();
        decimal returnedPubCA= publicCompanyScript.getUserPubStats();
        userComp.assets += returnedPrivCA + returnedPubCA + userComp.cashATM;
        userComp.liabilities += bankScript.getBankLoans();
        userComp.equity = userComp.assets - userComp.liabilities;
    }
    private void setStats()
    {
        var children = new List<GameObject>();
        foreach (Transform child in contentContainer) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        setStatsScript = statsPrefab.GetComponent<setHomeScreenStats>();
        String stringToSet = "NetIncome";
        setStatsScript.setData(stringToSet, moneyStats[0]+moneyStats[1]+moneyStats[2]+moneyStats[3]);
        var itemToGen = Instantiate(statsPrefab);
        itemToGen.transform.SetParent(contentContainer);
        itemToGen.transform.localScale = Vector2.one;
        for (int i = 0; i < weeklyIncome.Count; i++)
        {
            if (weeklyIncome[i].amount != 0)
            {
                stringToSet = weeklyIncome[i].name;
                setStatsScript = statsPrefab.GetComponent<setHomeScreenStats>();
                if (weeklyIncome[i].category == 0)
                {
                    stringToSet += " Dividends";
                }
                else if (weeklyIncome[i].category == 1)
                {
                    stringToSet += "";
                }
                else if (weeklyIncome[i].category == 2)
                {
                    stringToSet += " Gains";
                }
                else if (weeklyIncome[i].category == 3)
                {
                    stringToSet += " Operations";
                }
                setStatsScript.setData(stringToSet, weeklyIncome[i].amount);
                itemToGen = Instantiate(statsPrefab);
                itemToGen.transform.SetParent(contentContainer);
                itemToGen.transform.localScale = Vector2.one;
            }
        }
        userComp.year3Div += moneyStats[0];
        userComp.year3Interest += moneyStats[1];
        userComp.year3CapitalGains += moneyStats[2];
        userComp.year3Operational += moneyStats[3];
        userComp.year3Income += moneyStats[0] + moneyStats[1] + moneyStats[2] + moneyStats[3];
        if (nextWeek.ToString("MM") == "01" && nextWeek.AddDays(-7).ToString("MM") == "12")
        {
            financeScript.updateYearlyStats();
            userComp.year3Div = 0;
            userComp.year3Interest = 0;
            userComp.year3CapitalGains = 0;
            userComp.year3Operational = 0;
            userComp.year3Income = 0;
        }
        moneyStats[0] = 0;
        moneyStats[1] = 0;
        moneyStats[2] = 0;
        moneyStats[3] = 0;
        weeklyIncome.Clear();
    }
    public class income
    {
        public String name;
        public int category;
        public decimal amount;
    }
}
