using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class PrivateCompany : MonoBehaviour
{ 
    public Sprite bd001_9;
    public Sprite bd001_3;
    public Sprite eShop;
    public Sprite vendingMachine;
    public Sprite ribs;
    public Sprite jam;
    public Sprite superMarket;
    public Sprite iceCreamTruck;
    public Sprite bigOffice1;
    public TextMeshProUGUI privC1Cost;
    public TextMeshProUGUI privC1Name;
    public Image privC1Logo;
    public TextMeshProUGUI privC1Description;
    public TextMeshProUGUI privC1Year1;
    public TextMeshProUGUI privC1Year2;
    public TextMeshProUGUI privC1Year3;
    public RawImage privC1Year1Bar;
    public RawImage privC1Year2Bar;
    public RawImage privC1Year3Bar;
    public TextMeshProUGUI privC1NetIncome;
    public TextMeshProUGUI privC1Revenue;
    public TextMeshProUGUI privC1Expenses;
    public TextMeshProUGUI privC1AvgWeekIncome;
    public TextMeshProUGUI privC1Assets;
    public TextMeshProUGUI privC1Liabilities;
    public TextMeshProUGUI privC1Equity;
    public TextMeshProUGUI privC2Cost;
    public TextMeshProUGUI privC2Name;
    public Image privC2Logo;
    public TextMeshProUGUI privC2Description;
    public TextMeshProUGUI privC2Year1;
    public TextMeshProUGUI privC2Year2;
    public TextMeshProUGUI privC2Year3;
    public RawImage privC2Year1Bar;
    public RawImage privC2Year2Bar;
    public RawImage privC2Year3Bar;
    public TextMeshProUGUI privC2NetIncome;
    public TextMeshProUGUI privC2Revenue;
    public TextMeshProUGUI privC2Expenses;
    public TextMeshProUGUI privC2AvgWeekIncome;
    public TextMeshProUGUI privC2Assets;
    public TextMeshProUGUI privC2Liabilities;
    public TextMeshProUGUI privC2Equity;
    //privc1 contains lower risk companies while privc2 contains higher risk companies
    public PrivC[] privC1 = new PrivC[4];
    public PrivC[] privC2 = new PrivC[4];
    public Button advanceTimeButton;
    private AdvanceTime advanceTimeScript;
    public int curPrivC1;
    public int curPrivC2;
    public GameObject soldText;
    public GameObject soldText2;
    // Start is called before the first frame update
    void Start()
    {
        advanceTimeScript = advanceTimeButton.GetComponent<AdvanceTime>();
        populateData();
        refreshPrivC(); 
    }

    // Update is called once per frame
    void Update()
    {

    }
    public class PrivC
    {
        public string name;
        public long currentCost;
        public Sprite Logo;
        public string description;
        public long year3Revenue;
        public long year3Expenses;
        public long year1Income;
        public long year2Income;
        public long year3Income;
        public long assets;
        public long liabilities;
        public long equity;
        public long lastWeekIncome;
        public long incomeYTD;
        public long orignalWeeklyIncome;
        public bool owned = false;
        public long lastYearRevenue;
        public long originalRevenue;
        public float growthMultiplier = 1;
        public long ownedCost = 0;
        public bool listedForSale = false;
        public int privCNum;
        public int privCInList;
    }
    public void privateBuy(int privcNum)
    {
        PrivC compToBuy;
        if (privcNum == 1)
        {
            compToBuy = privC1[curPrivC1];
        }
        else
        {
            compToBuy = privC2[curPrivC2];
        }
        if (compToBuy.owned == false) {
            if (advanceTimeScript.userComp.cashATM >= compToBuy.currentCost)
            {
                if (privcNum == 1)
                {
                    privC1[curPrivC1].owned = true;
                    advanceTimeScript.updateMoney(-compToBuy.currentCost);
                    soldText.SetActive(true);
                    privC1[curPrivC1].ownedCost = privC1[curPrivC1].currentCost;
                }
                else
                {
                    privC2[curPrivC2].owned = true;
                    advanceTimeScript.updateMoney(-compToBuy.currentCost);
                    soldText2.SetActive(true);
                    privC2[curPrivC2].ownedCost = privC2[curPrivC2].currentCost;
                }
            }
        }
    }
    public void nextWeekPrivC()
    {
        //update week for low risk co
        for(int i = 0; i<privC1.Length; i++)
        {
            long multIncome;
            if (privC1[i].lastWeekIncome < -privC1[i].orignalWeeklyIncome)
            {
                multIncome = -privC1[i].orignalWeeklyIncome;
            }
            else
            {
                multIncome = privC1[i].lastWeekIncome;
            }
            int randomDelta = UnityEngine.Random.Range(1,11);
            if (randomDelta==1)
            {
                float randomMult = UnityEngine.Random.Range(160, 301);
                float newWeekIncome = (((randomMult) / 10000)) * multIncome;
                privC1[i].lastWeekIncome = (int)newWeekIncome + privC1[i].lastWeekIncome;
            }else if(randomDelta > 1 && randomDelta <= 6)
            {
                float randomMult = UnityEngine.Random.Range(5, 150);
                float newWeekIncome = (((randomMult) / 10000)) * multIncome;
                privC1[i].lastWeekIncome = (int)newWeekIncome + privC1[i].lastWeekIncome;
            }
            else if(randomDelta>6 && randomDelta <= 9)
            {
                float randomMult = UnityEngine.Random.Range(-5, -150);
                float newWeekIncome = (((randomMult) / 10000)) * multIncome;
                privC1[i].lastWeekIncome = (int)newWeekIncome + privC1[i].lastWeekIncome;
            }
            else
            {
                float randomMult = UnityEngine.Random.Range(-160, -301);
                float newWeekIncome = (((randomMult) / 10000)) * multIncome;
                privC1[i].lastWeekIncome = (int)newWeekIncome + privC1[i].lastWeekIncome;
            }
            if (privC1[i].liabilities > 0 && privC1[i].lastWeekIncome>0)
            {
                float lastWeeksPayment = (float)(privC1[i].lastWeekIncome * .05);
                privC1[i].liabilities -= (int)lastWeeksPayment;
                if(privC1[i].liabilities < 0)
                {
                    privC1[i].liabilities = 0;
                }
            }else if (privC1[i].lastWeekIncome < 0)
            {
                privC1[i].liabilities += privC1[i].lastWeekIncome;
            }
            privC1[i].equity = privC1[i].assets - privC1[i].liabilities;
            privC1[i].incomeYTD += privC1[i].lastWeekIncome;
            if (advanceTimeScript.nextWeek.ToString("MM") == "01" && advanceTimeScript.nextWeek.AddDays(-7).ToString("MM")=="12")
            { 
                privC1[i].year1Income = privC1[i].year2Income;
                privC1[i].year2Income = privC1[i].year3Income;
                privC1[i].year3Income = privC1[i].incomeYTD;
                double lastYearincome = privC1[i].year2Income;
                double thisYearIncome = privC1[i].year3Income;
                privC1[i].lastYearRevenue = privC1[i].year3Revenue;
                double percentChange = ((((thisYearIncome) - (privC1[i].orignalWeeklyIncome*52))) / Math.Abs(privC1[i].orignalWeeklyIncome * 52));
                privC1[i].year3Revenue = (int)(privC1[i].originalRevenue * Math.Abs(percentChange+1));
                percentChange = (((((double)privC1[i].year3Revenue) - ((double)privC1[i].lastYearRevenue))) / Math.Abs((double)privC1[i].lastYearRevenue));

                privC1[i].year3Expenses = privC1[i].year3Revenue - privC1[i].year3Income;
                privC1[i].incomeYTD = 0;
                privC1[i].assets = (privC1[i].assets * (long)1.03);
                privC1[i].equity = privC1[i].assets - privC1[i].liabilities;
                if (privC1[i].year1Income>0 && privC1[i].year3Income > 0 && privC1[i].year2Income > 0) {
                    float randomCostMult = UnityEngine.Random.Range(13, 20);
                    privC1[i].currentCost = (int)randomCostMult * privC1[i].year3Income;
                }else
                {

                    float newCost = (float)(privC1[i].currentCost * (percentChange + 1));
                    if(newCost > privC1[i].equity)
                    {
                        privC1[i].currentCost = (int)newCost;
                    }
                    else
                    {
                        privC1[i].currentCost = Math.Abs(privC1[i].equity);
                    }
                }
            }
            if (privC1[i].owned)
            {
                advanceTimeScript.updateMoney(privC1[i].lastWeekIncome);
                advanceTimeScript.moneyStats[3] += privC1[i].lastWeekIncome;
                advanceTimeScript.weeklyIncome.Add(new AdvanceTime.income { name = privC1[i].name, category = 3, amount = privC1[i].lastWeekIncome });
            }
            if (privC1[i].listedForSale)
            {
                int toSale = UnityEngine.Random.Range(1, 40);
                if(toSale == 10)
                {
                    privC1[i].owned = false;
                    privC1[i].listedForSale = false;
                    advanceTimeScript.updateMoney(privC1[i].currentCost);
                    decimal gain = privC1[i].currentCost - privC1[i].ownedCost;
                    advanceTimeScript.moneyStats[2] += gain;
                    advanceTimeScript.weeklyIncome.Add(new AdvanceTime.income { name = privC1[i].name, category = 2, amount = gain });
                }
            }
        }
        //update week for high risk co
        for (int i = 0; i < privC2.Length; i++)
        {
            if (privC2[i].lastWeekIncome > 10000000)
            {
                privC2[i].growthMultiplier = .9f;
            }
            long multIncome;
            if (privC2[i].lastWeekIncome < -privC2[i].orignalWeeklyIncome)
            {
                multIncome = -privC2[i].orignalWeeklyIncome;
            }
            else
            {
                multIncome = privC2[i].lastWeekIncome;
            }
            int randomDelta = UnityEngine.Random.Range(1, 11);
            if (randomDelta > 0 && randomDelta <= 4)
            {
                float randomMult = UnityEngine.Random.Range(400, 820)*privC2[i].growthMultiplier;
                float newWeekIncome = (((randomMult) / 10000)) * multIncome;
                privC2[i].lastWeekIncome = (int)newWeekIncome + privC2[i].lastWeekIncome; ;
            }
            else if (randomDelta > 5 && randomDelta <= 6)
            {
                float randomMult = UnityEngine.Random.Range(160, 300)*privC2[i].growthMultiplier;
                float newWeekIncome = (((randomMult) / 10000)) * multIncome;
                privC2[i].lastWeekIncome = (int)newWeekIncome + privC2[i].lastWeekIncome;
            }
            else if (randomDelta == 7)
            {
                float randomMult = UnityEngine.Random.Range(-100, -250)*privC2[i].growthMultiplier;
                float newWeekIncome = (((randomMult) / 10000)) * multIncome;
                privC2[i].lastWeekIncome = (int)newWeekIncome + privC2[i].lastWeekIncome;
            }
            else
            {
                float randomMult = UnityEngine.Random.Range(-350, -750)*privC2[i].growthMultiplier;
                float newWeekIncome = (((randomMult) / 10000)) * multIncome;
                privC2[i].lastWeekIncome = (int)newWeekIncome + privC2[i].lastWeekIncome;
            }
            if (privC2[i].lastWeekIncome > 0 && privC2[i].liabilities > 0)
            {
                float lastWeeksPayment = (float)(privC2[i].lastWeekIncome * .05);
                privC2[i].liabilities -= (int)lastWeeksPayment;
                if (privC2[i].liabilities < 0)
                {
                    privC2[i].liabilities = 0;
                }
            }
            else if (privC2[i].lastWeekIncome < 0)
            {
                privC2[i].liabilities += Math.Abs(privC2[i].lastWeekIncome);
            }
            privC2[i].equity = privC2[i].assets - privC2[i].liabilities;
            privC2[i].incomeYTD += privC2[i].lastWeekIncome;
            if (advanceTimeScript.nextWeek.ToString("MM") == "01" && advanceTimeScript.nextWeek.AddDays(-7).ToString("MM") == "12")
            {
                privC2[i].year1Income = privC2[i].year2Income;
                privC2[i].year2Income = privC2[i].year3Income;
                privC2[i].year3Income = privC2[i].incomeYTD;
                double lastYearincome = privC2[i].year2Income;
                double thisYearIncome = privC2[i].year3Income;
                double percentChange = ((((thisYearIncome) - (privC2[i].orignalWeeklyIncome * 52))) / Math.Abs(privC2[i].orignalWeeklyIncome * 52));
                privC2[i].lastYearRevenue = privC2[i].year3Revenue;
                privC2[i].year3Revenue = (int)(privC2[i].originalRevenue * (Math.Abs(percentChange)+1));
                percentChange = (((((double)privC2[i].year3Revenue) - ((double)privC2[i].lastYearRevenue))) / Math.Abs((double)privC2[i].lastYearRevenue));
                privC2[i].year3Expenses = privC2[i].year3Revenue - privC2[i].year3Income;
                privC2[i].incomeYTD = 0;
                privC2[i].assets = (((long)(privC2[i].assets * 1.03)));
                privC2[i].equity = privC2[i].assets - privC2[i].liabilities;
                if (privC2[i].year1Income > 10000000 && privC2[i].year3Income > 10000000 && privC2[i].year2Income > 10000000)
                {
                    float randomCostMult = UnityEngine.Random.Range(35, 100);
                    privC2[i].currentCost = (int)randomCostMult * privC2[i].year3Income;
                }
                else
                {
                    float newCost = (float)(privC2[i].currentCost * (percentChange + 1));
                    if (newCost > privC2[i].equity)
                    {
                        privC2[i].currentCost = (int)newCost;
                    }
                    else
                    {
                        privC2[i].currentCost = privC2[i].equity;
                    }
                }
            }
            if (privC2[i].owned) {
                advanceTimeScript.updateMoney(privC2[i].lastWeekIncome);
                advanceTimeScript.moneyStats[3] += privC2[i].lastWeekIncome;
                advanceTimeScript.weeklyIncome.Add(new AdvanceTime.income { name = privC2[i].name, category = 3, amount = privC2[i].lastWeekIncome });
            }
            if (privC2[i].listedForSale)
            {
                int toSale = UnityEngine.Random.Range(1, 40);
                if (toSale == 10)
                {
                    privC2[i].owned = false;
                    privC2[i].listedForSale = false;
                    advanceTimeScript.updateMoney(privC2[i].currentCost);
                    decimal gain = privC2[i].currentCost - privC2[i].ownedCost;
                    advanceTimeScript.moneyStats[2] += gain;
                    advanceTimeScript.weeklyIncome.Add(new AdvanceTime.income { name = privC2[i].name, category = 2, amount = gain });
                }
            }
        }
        refreshPrivC();
    }
    private void populateData()
    {
        PrivC jimsHardware = new PrivC();
        jimsHardware.name = "Jim's Hardware";
        jimsHardware.currentCost = 500000;
        jimsHardware.Logo = bd001_9;
        jimsHardware.description = "Jim's Hardware is a locally owned store that primarly focuses on the sell of household hardware for home improvment";
        jimsHardware.year3Revenue = 367637;
        jimsHardware.year3Expenses = 338226;
        jimsHardware.year1Income = 26676;
        jimsHardware.year2Income = 27957;
        jimsHardware.year3Income = 29411;
        jimsHardware.assets = 325820;
        jimsHardware.liabilities = 184290;
        jimsHardware.equity = jimsHardware.assets-jimsHardware.liabilities;
        jimsHardware.lastWeekIncome = 623;
        jimsHardware.incomeYTD = 0;
        jimsHardware.orignalWeeklyIncome = jimsHardware.lastWeekIncome;
        jimsHardware.originalRevenue = jimsHardware.year3Revenue;
        jimsHardware.privCNum = 1;
        jimsHardware.privCInList = 0;
        privC1[0] = jimsHardware;

        PrivC topIce = new PrivC();
        topIce.name = "Top Ice";
        topIce.currentCost = 1100000;
        topIce.Logo = iceCreamTruck;
        topIce.description = "Top Ice owns and operates a small fleet of ice cream trucks in beach towns.";
        topIce.year3Revenue = 88492;
        topIce.year3Expenses = 25816;
        topIce.year1Income = 53587;
        topIce.year2Income = 59542;
        topIce.year3Income = 62676;
        topIce.assets = 477462;
        topIce.liabilities = 126294;
        topIce.equity = topIce.assets - topIce.liabilities;
        topIce.lastWeekIncome = 1205;
        topIce.incomeYTD = 0;
        topIce.orignalWeeklyIncome = topIce.lastWeekIncome;
        topIce.originalRevenue = topIce.year3Revenue;
        topIce.privCNum = 1;
        topIce.privCInList = 1;
        privC1[1] = topIce;

        PrivC snackSip = new PrivC();
        snackSip.name = "Snack & Sip";
        snackSip.currentCost = 185000;
        snackSip.Logo = vendingMachine;
        snackSip.description = "Snack & Sip Vendors is a small localy owned and operated vending machine company.";
        snackSip.year3Revenue = 21452;
        snackSip.year3Expenses = 10890;
        snackSip.year1Income = 9243;
        snackSip.year2Income = 9465;
        snackSip.year3Income = 10562;
        snackSip.assets = 87475;
        snackSip.liabilities = 5347;
        snackSip.equity = snackSip.assets - snackSip.liabilities;
        snackSip.lastWeekIncome = 213;
        snackSip.incomeYTD = 0;
        snackSip.orignalWeeklyIncome = snackSip.lastWeekIncome;
        snackSip.originalRevenue = snackSip.year3Revenue;
        snackSip.privCNum = 1;
        snackSip.growthMultiplier = .98f;
        snackSip.privCInList = 2;
        privC1[2] = snackSip;

        PrivC saveMart = new PrivC();
        saveMart.name = "Save Mart";
        saveMart.currentCost = 5800900;
        saveMart.Logo = superMarket;
        saveMart.description = "SaveMart is a chain of independtly owned and operated groccery stores.";
        saveMart.year3Revenue = 18162300;
        saveMart.year3Expenses = 17799054;
        saveMart.year1Income = 312981;
        saveMart.year2Income = 333324;
        saveMart.year3Income = 363246;
        saveMart.assets = 1708625;
        saveMart.liabilities = 895319;
        saveMart.equity = saveMart.assets - saveMart.liabilities;
        saveMart.lastWeekIncome = 6986;
        saveMart.incomeYTD = 0;
        saveMart.orignalWeeklyIncome = saveMart.lastWeekIncome;
        saveMart.originalRevenue = saveMart.year3Revenue;
        saveMart.privCNum = 1;
        saveMart.growthMultiplier = .9f;
        saveMart.privCInList = 3;
        privC1[3] = saveMart;

        PrivC bitdealStudio = new PrivC();
        bitdealStudio.name = "Bitdeal Studio";
        bitdealStudio.currentCost = 290000;
        bitdealStudio.Logo = bd001_3;
        bitdealStudio.description = "Bitdeal Studio is a small startup software company. They focus on selling software that helps small buisnesses manage inventory.";
        bitdealStudio.year3Revenue = 62835;
        bitdealStudio.year3Expenses = 113672;
        bitdealStudio.year1Income = -82659;
        bitdealStudio.year2Income = -43811;
        bitdealStudio.year3Income = bitdealStudio.year3Revenue-bitdealStudio.year3Expenses;
        bitdealStudio.assets = 23783;
        bitdealStudio.liabilities = 18329;
        bitdealStudio.equity = bitdealStudio.assets - bitdealStudio.liabilities;
        bitdealStudio.lastWeekIncome = -559;
        bitdealStudio.incomeYTD = 0;
        bitdealStudio.orignalWeeklyIncome = bitdealStudio.lastWeekIncome;
        bitdealStudio.originalRevenue = bitdealStudio.year3Revenue;
        bitdealStudio.privCNum = 2;
        bitdealStudio.privCInList = 0;
        privC2[0] = bitdealStudio;

        PrivC chemek = new PrivC();
        chemek.name = "Chemek";
        chemek.currentCost = 8000000;
        chemek.Logo = bigOffice1;
        chemek.description = "Chemek is a startup biotech company founded by a small group of scientists from top universities.";
        chemek.year3Revenue = 157930;
        chemek.year3Expenses = 351026;
        chemek.year1Income = -529580;
        chemek.year2Income = -398214;
        chemek.year3Income = chemek.year3Revenue - chemek.year3Expenses;
        chemek.assets = 4201928;
        chemek.liabilities = 7489201;
        chemek.equity = chemek.assets - chemek.liabilities;
        chemek.lastWeekIncome = -3711;
        chemek.incomeYTD = 0;
        chemek.orignalWeeklyIncome = chemek.lastWeekIncome;
        chemek.originalRevenue = chemek.year3Revenue;
        chemek.growthMultiplier = 1.7f;
        chemek.privCNum = 2;
        chemek.privCInList = 1;
        privC2[1] = chemek;

        PrivC jamJar = new PrivC();
        jamJar.name = "JamJar";
        jamJar.currentCost = 1900000;
        jamJar.Logo = jam;
        jamJar.description = "JamJar seeks to become a global manufacturer of preservatives.";
        jamJar.year3Revenue = 1567690;
        jamJar.year3Expenses = 1487132;
        jamJar.year1Income = -19362;
        jamJar.year2Income = 43938;
        jamJar.year3Income = 80558;
        jamJar.assets = 937402;
        jamJar.liabilities = 540370;
        jamJar.equity = jamJar.assets - jamJar.liabilities;
        jamJar.lastWeekIncome = 1641;
        jamJar.incomeYTD = 0;
        jamJar.orignalWeeklyIncome = jamJar.lastWeekIncome;
        jamJar.originalRevenue = jamJar.year3Revenue;
        jamJar.privCNum = 2;
        jamJar.growthMultiplier = 1.5f;
        jamJar.privCInList = 2;
        privC2[2] = jamJar;

        PrivC eSeller = new PrivC();
        eSeller.name = "E-Seller";
        eSeller.currentCost = 650000;
        eSeller.Logo = eShop;
        eSeller.description = "E-Seller is an online reatiler. E-Seller is growing and specialises in clothing.";
        eSeller.year3Revenue = 1567690;
        eSeller.year3Expenses = 1487132;
        eSeller.year1Income = -15394;
        eSeller.year2Income = -37192;
        eSeller.year3Income = -25739;
        eSeller.assets = 53196;
        eSeller.liabilities = 174193;
        eSeller.equity = eSeller.assets - eSeller.liabilities;
        eSeller.lastWeekIncome = -1849;
        eSeller.incomeYTD = 0;
        eSeller.orignalWeeklyIncome = eSeller.lastWeekIncome;
        eSeller.originalRevenue = eSeller.year3Revenue;
        eSeller.privCNum = 2;
        eSeller.growthMultiplier = 1.35f;
        eSeller.privCInList = 3;
        privC2[3] = eSeller;


    }
    public void refreshPrivC()
    {
        int i = UnityEngine.Random.Range(0,privC1.Length);
        if (privC1[i].owned)
        {
            i = UnityEngine.Random.Range(0, privC1.Length);
        }
        curPrivC1 = i;
        if (privC1[i].owned)
        {
            soldText.SetActive(true);
        }
        else
        {
            soldText.SetActive(false);
        }
        //This will display a private company's data on the first private company screen
        privC1Cost.text = String.Format("{0:C}", privC1[i].currentCost);
        privC1Name.text = privC1[i].name;
        privC1Logo.sprite = privC1[i].Logo;
        privC1Description.text = privC1[i].description;
        privC1Year1.text = advanceTimeScript.nextWeek.AddYears(-3).ToString("yyyy");
        privC1Year2.text = advanceTimeScript.nextWeek.AddYears(-2).ToString("yyyy"); ;
        privC1Year3.text = advanceTimeScript.nextWeek.AddYears(-1).ToString("yyyy"); ;
        privC1Year1Bar.rectTransform.offsetMax = new Vector2(privC1Year1Bar.rectTransform.offsetMax.x, -79);
        if(privC1[i].year1Income < 0)
        {
            privC1Year1Bar.color = Color.red;
        }
        else
        {
            privC1Year1Bar.color = Color.green;
        }
        int year2Height = determineBarHeight(2, privC1[i].year1Income, privC1[i].year2Income, 79);
        privC1Year2Bar.rectTransform.offsetMax = new Vector2(privC1Year2Bar.rectTransform.offsetMax.x, -year2Height);
        if (privC1[i].year2Income < 0)
        {
            privC1Year2Bar.color = Color.red;
        }
        else
        {
            privC1Year2Bar.color = Color.green;
        }
        privC1Year3Bar.rectTransform.offsetMax = new Vector2(privC1Year3Bar.rectTransform.offsetMax.x, -determineBarHeight(3, privC1[i].year2Income, privC1[i].year3Income, year2Height));
        if (privC1[i].year3Income < 0)
        {
            privC1Year3Bar.color = Color.red;
        }
        else
        {
            privC1Year3Bar.color = Color.green;
        }
        privC1NetIncome.text = privC1[i].year3Income.ToString();
        privC1Revenue.text = privC1[i].year3Revenue.ToString();
        privC1Expenses.text = privC1[i].year3Expenses.ToString();
        privC1AvgWeekIncome.text = privC1[i].lastWeekIncome.ToString();
        privC1Assets.text = privC1[i].assets.ToString();
        privC1Liabilities.text = privC1[i].liabilities.ToString();
        privC1Equity.text = (-privC1[i].liabilities + privC1[i].assets).ToString();
        //This will display a private company's data on the second private company screen
        i = UnityEngine.Random.Range(0, privC2.Length); ;
        curPrivC2 = i;
        if (privC2[i].owned)
        {
            i = UnityEngine.Random.Range(0, privC2.Length);
        }
        if (privC2[i].owned)
        {
            soldText2.SetActive(true);
        }
        else
        {
            soldText2.SetActive(false);
        }
        privC2Cost.text = String.Format("{0:C}", privC2[i].currentCost);
        privC2Name.text = privC2[i].name;
        privC2Logo.sprite = privC2[i].Logo;
        privC2Description.text = privC2[i].description;
        privC2Year1.text = advanceTimeScript.nextWeek.AddYears(-3).ToString("yyyy");
        privC2Year2.text = advanceTimeScript.nextWeek.AddYears(-2).ToString("yyyy"); ;
        privC2Year3.text = advanceTimeScript.nextWeek.AddYears(-1).ToString("yyyy"); ;
        privC2Year1Bar.rectTransform.offsetMax = new Vector2(privC2Year1Bar.rectTransform.offsetMax.x, -79);
        if (privC2[i].year1Income < 0)
        {
            privC2Year1Bar.color = Color.red;
        }
        else
        {
            privC2Year1Bar.color = Color.green;
        }
        year2Height = determineBarHeight(2, privC2[i].year1Income, privC2[i].year2Income, 79);
        privC2Year2Bar.rectTransform.offsetMax = new Vector2(privC2Year2Bar.rectTransform.offsetMax.x, -year2Height);
        if (privC2[i].year2Income < 0)
        {
            privC2Year2Bar.color = Color.red;
        }
        else
        {
            privC2Year2Bar.color = Color.green;
        }
        privC2Year3Bar.rectTransform.offsetMax = new Vector2(privC2Year3Bar.rectTransform.offsetMax.x, -determineBarHeight(3, privC2[i].year2Income, privC2[i].year3Income, year2Height));
        if (privC2[i].year3Income < 0)
        {
            privC2Year3Bar.color = Color.red;
        }
        else
        {
            privC2Year3Bar.color = Color.green;
        }
        privC2NetIncome.text = privC2[i].year3Income.ToString();
        privC2Revenue.text = privC2[i].year3Revenue.ToString();
        privC2Expenses.text = privC2[i].year3Expenses.ToString();
        privC2AvgWeekIncome.text = privC2[i].lastWeekIncome.ToString();
        privC2Assets.text = privC2[i].assets.ToString();
        privC2Liabilities.text = privC2[i].liabilities.ToString();
        privC2Equity.text = (-privC2[i].liabilities + privC2[i].assets).ToString();
    }
    private int determineBarHeight(int barNum, double lastYearIncome, double thisYearIncome, int lastYearBarH)
    {
        //this will determine the change in the bar graph for the companies income the past three years
        double percentChange = (((thisYearIncome - lastYearIncome) * 100) / (lastYearIncome));
        if (lastYearIncome>0 && thisYearIncome < 0)
        {
            percentChange = Math.Abs(percentChange);
        }
        if (percentChange <= 3 && percentChange > 0 && lastYearBarH > 29)
        {
            return lastYearBarH-4;
        } else if (percentChange >= 3 && percentChange < 8 && lastYearBarH > 33)
        {
            return lastYearBarH-8;
        } else if (percentChange >= 8 && percentChange < 20 && lastYearBarH > 36)
        {
            return lastYearBarH-11;
        }
        else if (percentChange >= 20 && percentChange < 99 && lastYearBarH > 56)
        {
            return lastYearBarH-31;
        }
        else if (percentChange >= 99 && lastYearBarH > 62)
        {
            return lastYearBarH-37;
        }
        else if (percentChange <= 3 && percentChange > 0 && lastYearBarH > 26)
        {
            return lastYearBarH-1;
        }
        else if (percentChange <= -1 && percentChange > -5 && lastYearBarH < 163)
        {
            return lastYearBarH+3;
        }
        else if (percentChange <= -5 && percentChange > -15 && lastYearBarH < 157)
        {
            return lastYearBarH+9;
        }
        else if (percentChange <= -15 && lastYearBarH < 150)
        {
            return lastYearBarH+16;
        }
        else
        {
            return lastYearBarH;
        }
        
    }
    public decimal determinePrivCWorth()
    {
        decimal toReturn = 0;
        for (int i = 0; i < privC1.Length; i++)
        {
            if (privC1[i].owned) {
                toReturn += (decimal)privC1[i].currentCost;
            }
        }
        for (int i = 0; i < privC2.Length; i++)
        {
            if (privC2[i].owned)
            {
                toReturn += (decimal)privC2[i].currentCost;
            }
        }
        return toReturn;

    }
    public void listStock(int privCList, int privCOrder)
    {
        PrivC compToList;
        if (privCList == 1)
        {
            compToList = privC1[privCOrder];
        }
        else
        {
            compToList = privC2[privCOrder];
        }
        if(compToList.owned && !compToList.listedForSale)
        {
            if (privCList == 1)
            {
                 privC1[privCOrder].listedForSale = true;
            }
            else
            {
                privC2[privCOrder].listedForSale = true;
            }
        }
    }
    public void unList(int privCList, int privCOrder)
    {
        PrivC compToList;
        if (privCList == 1)
        {
            compToList = privC1[privCOrder];
        }
        else
        {
            compToList = privC2[privCOrder];
        }
        if (compToList.owned && compToList.listedForSale)
        {
            if (privCList == 1)
            {
                privC1[privCOrder].listedForSale = false;
            }
            else
            {
                privC2[privCOrder].listedForSale = false;
            }
        }
    }
    public decimal getUserPrivStats()
    {
        decimal assets = 0;
        for (int i = 0; i < privC1.Length; i++)
        {
            if (privC1[i].owned)
            {
                assets += privC1[i].currentCost;
            }
        }
        for (int i = 0; i < privC2.Length; i++)
        {
            if (privC2[i].owned)
            {
                assets += privC2[i].currentCost;
            }
        }
        return assets;
    }

}
