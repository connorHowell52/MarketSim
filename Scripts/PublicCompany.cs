using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PublicCompany : MonoBehaviour
{
    public Transform contentContainer;
    public GameObject pubCPrefab;
    private PublicCompanySet setDataScript;
    public PubC[] publicCompaniesList = new PubC[5];
    public Button advanceTimeButton;
    private AdvanceTime advanceTimeScript;
    public GameObject home;
    // Start is called before the first frame update
    void Start()
    {
        advanceTimeScript = advanceTimeButton.GetComponent<AdvanceTime>();
        populateData();
        updateScreen();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void nextWeekPubC()
    {
        float randomStockVariation = 1;
        float randomDelta;
        for (int i = 0; i < publicCompaniesList.Length; i++)
        {
            //this will change the price by a random percent between -5 and 5
            if (publicCompaniesList[i].priceEarnings > 5 || publicCompaniesList[i].priceEarnings < 75)
            {
                randomDelta = UnityEngine.Random.Range(1, 5);
                if (randomDelta == 1)
                {
                    randomStockVariation = UnityEngine.Random.Range(301, 501);
                }
                else if (randomDelta > 1 && randomDelta < 4)
                {
                    randomStockVariation = UnityEngine.Random.Range(-250, 301);
                }
                else
                {
                    randomStockVariation = UnityEngine.Random.Range(-500, -249);
                }
            }
            else if (publicCompaniesList[i].priceEarnings <= 5)
            {
                randomStockVariation = UnityEngine.Random.Range(0, 501);
            }
            else if (publicCompaniesList[i].priceEarnings >= 75)
            {
                randomStockVariation = UnityEngine.Random.Range(-500, 0);
            }
            float priceMult = (((randomStockVariation / 10000) * publicCompaniesList[i].beta) + 1);
            publicCompaniesList[i].price = (float)Math.Round((double)publicCompaniesList[i].price * priceMult, 2);
            //this will vary the weekly income of the company
            randomDelta = UnityEngine.Random.Range(1, 11);
            if (randomDelta == 1)
            {
                float randomMult = UnityEngine.Random.Range(160, 301);
                float newWeekIncome = (((randomMult) / 10000)) * publicCompaniesList[i].lastWeekIncome * publicCompaniesList[i].beta;
                publicCompaniesList[i].lastWeekIncome = (long)newWeekIncome + publicCompaniesList[i].lastWeekIncome;
            }
            else if (randomDelta > 1 && randomDelta <= 6)
            {
                float randomMult = UnityEngine.Random.Range(1, 150);
                float newWeekIncome = (((randomMult) / 10000)) * publicCompaniesList[i].lastWeekIncome * publicCompaniesList[i].beta;
                publicCompaniesList[i].lastWeekIncome = (long)newWeekIncome + publicCompaniesList[i].lastWeekIncome;
            }
            else if (randomDelta > 6 && randomDelta <= 9)
            {
                float randomMult = UnityEngine.Random.Range(-1, -150);
                float newWeekIncome = (((randomMult) / 10000)) * publicCompaniesList[i].lastWeekIncome * publicCompaniesList[i].beta;
                publicCompaniesList[i].lastWeekIncome = (long)newWeekIncome + publicCompaniesList[i].lastWeekIncome;
            }
            else
            {
                float randomMult = UnityEngine.Random.Range(-160, -301);
                float newWeekIncome = (((randomMult) / 10000)) * publicCompaniesList[i].lastWeekIncome * publicCompaniesList[i].beta;
                publicCompaniesList[i].lastWeekIncome = (long)newWeekIncome + publicCompaniesList[i].lastWeekIncome;
            }
            publicCompaniesList[i].thisQuarterIncome += publicCompaniesList[i].lastWeekIncome;
            publicCompaniesList[i].incomeYTD += publicCompaniesList[i].lastWeekIncome;
            shiftArrayOne(publicCompaniesList[i].priceRange);
            publicCompaniesList[i].priceRange[0] = publicCompaniesList[i].price;
            publicCompaniesList[i].high = getHighNum(publicCompaniesList[i].priceRange);
            publicCompaniesList[i].low = getLowNum(publicCompaniesList[i].priceRange, publicCompaniesList[i].high);
            publicCompaniesList[i].marketCap = (long)(publicCompaniesList[i].sharesOutstanidng * publicCompaniesList[i].price);
            if (publicCompaniesList[i].merged)
            {
                advanceTimeScript.updateMoney(publicCompaniesList[i].lastWeekIncome);
                advanceTimeScript.moneyStats[3] += publicCompaniesList[i].lastWeekIncome;
                advanceTimeScript.weeklyIncome.Add(new AdvanceTime.income { name = publicCompaniesList[i].name, category = 3, amount = publicCompaniesList[i].lastWeekIncome });
            }

            if (endOfQuarter())
            {
                //does the calculations for the ended quarter
                long incomeOverYear = (publicCompaniesList[i].lastThreeQuarters[0] + publicCompaniesList[i].lastThreeQuarters[1] + publicCompaniesList[i].lastThreeQuarters[2] + publicCompaniesList[i].thisQuarterIncome);
                publicCompaniesList[i].priceEarnings = Math.Round(publicCompaniesList[i].price / (incomeOverYear / publicCompaniesList[i].sharesOutstanidng), 2);
                if (!publicCompaniesList[i].merged) {
                    double dividendPaid = publicCompaniesList[i].dividend * publicCompaniesList[i].sharesOwned;
                    advanceTimeScript.updateMoney((decimal)dividendPaid);
                    advanceTimeScript.moneyStats[0] += (decimal)dividendPaid;
                    advanceTimeScript.weeklyIncome.Add(new AdvanceTime.income { name = publicCompaniesList[i].name, category = 0, amount = (decimal)dividendPaid });
                }
                shiftArrayOne(publicCompaniesList[i].lastThreeQuarters);
                //does calculation for the end of year
                if (advanceTimeScript.nextWeek.ToString("MM") == "01" && advanceTimeScript.nextWeek.AddDays(-7).ToString("MM") == "12")
                {
                    publicCompaniesList[i].year1Income = publicCompaniesList[i].year2Income;
                    publicCompaniesList[i].year2Income = publicCompaniesList[i].incomeYTD;
                    publicCompaniesList[i].incomeYTD = 0;
                    publicCompaniesList[i].yearOverYear = Math.Round(((publicCompaniesList[i].year2Income - publicCompaniesList[i].year1Income) / (double)publicCompaniesList[i].year1Income) * 100, 2);
                    publicCompaniesList[i].assets = (long)(publicCompaniesList[i].assets*((.03*publicCompaniesList[i].beta)+1));
                    publicCompaniesList[i].liabilities = (long)(publicCompaniesList[i].liabilities * ((.015 * publicCompaniesList[i].beta) + 1));
                    publicCompaniesList[i].equity = publicCompaniesList[i].assets - publicCompaniesList[i].liabilities;
                    publicCompaniesList[i].bookValue = Math.Round(((double)publicCompaniesList[i].equity / publicCompaniesList[i].sharesOutstanidng), 2);
                    publicCompaniesList[i].dividend = Math.Round(((double)publicCompaniesList[i].year2Income * (double)(publicCompaniesList[i].dividendRatio)) / (double)publicCompaniesList[i].sharesOutstanidng, 2);
                }
                publicCompaniesList[i].lastThreeQuarters[0] = publicCompaniesList[i].thisQuarterIncome;
                publicCompaniesList[i].thisQuarterIncome = 0;
            }
        }
        updateScreen();
    }
    private float getHighNum(float[] a)
    {
        float[] temp1 = (float[])a.Clone();
        float temp2 = 0;
        for(int i = 0; i < temp1.Length; i++)
        {
            if (temp1[i] > temp2)
            {
                temp2 = temp1[i];
            }
        }
        return temp2;
    }
    private float getLowNum(float[] a, float highest)
    {
        float[] temp1 = (float[])a.Clone();
        float temp2 = highest;
        for (int i = 0; i < temp1.Length; i++)
        {
            if (temp1[i] < temp2)
            {
                temp2 = temp1[i];
            }
        }
        return temp2;
    }
    private void shiftArrayOne(long[] a)
    {
        long[] temp1 = (long[])a.Clone();
        for (int i = 0; i < a.Length-1; i++)
        {
            a[i + 1] = temp1[i];
            
        }
    }
    private void shiftArrayOne(float[] a)
    {
        float[] temp1 = (float[])a.Clone();
        for (int i = 0; i < a.Length - 1; i++)
        {
            a[i + 1] = temp1[i];

        }
    }
    private bool endOfQuarter()
    {
        if (advanceTimeScript.nextWeek.ToString("MM") == "04" && advanceTimeScript.nextWeek.AddDays(-7).ToString("MM") == "03")
        {
            return true;
        }else if (advanceTimeScript.nextWeek.ToString("MM") == "07" && advanceTimeScript.nextWeek.AddDays(-7).ToString("MM") == "06")
        {
            return true;
        }
        else if (advanceTimeScript.nextWeek.ToString("MM") == "10" && advanceTimeScript.nextWeek.AddDays(-7).ToString("MM") == "09")
        {
            return true;
        }
        else if (advanceTimeScript.nextWeek.ToString("MM") == "01" && advanceTimeScript.nextWeek.AddDays(-7).ToString("MM") == "12")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void populateData()
    {
        PubC solaceBank = new PubC
        {
            name = "Solace Bank",
            price = 41.61f,
            year2Income = 18187780516,
            year1Income = 16526991833,
            sharesOutstanidng = 8093557244,
            assets = 2492815730275,
            equity = 283103958392,
            high = 43.42f,
            low = 39.82f,
            incomeYTD = 0,
            thisQuarterIncome = 0,
            beta = .8f,
        };
        long[] lastThreeQuarters = {solaceBank.year2Income/4, solaceBank.year2Income/4, solaceBank.year2Income/4};
        solaceBank.lastThreeQuarters = lastThreeQuarters;
        solaceBank.priceRange = populateArray(solaceBank.low, solaceBank.high, 52);
        solaceBank.high = getHighNum(solaceBank.priceRange);
        solaceBank.low = getLowNum(solaceBank.priceRange, solaceBank.high);
        solaceBank.priceEarnings = Math.Round(solaceBank.price / (solaceBank.year2Income / solaceBank.sharesOutstanidng), 2);
        solaceBank.liabilities = solaceBank.assets - solaceBank.equity;
        solaceBank.bookValue = Math.Round(((double)solaceBank.equity / solaceBank.sharesOutstanidng),2);
        solaceBank.marketCap = (long)(solaceBank.price * solaceBank.sharesOutstanidng);
        solaceBank.yearOverYear = Math.Round(((solaceBank.year2Income - solaceBank.year1Income)/(double)solaceBank.year1Income)*100, 2);
        solaceBank.dividend = Math.Round(((double)solaceBank.year2Income * (double)(solaceBank.dividendRatio))/(double)solaceBank.sharesOutstanidng, 2);
        solaceBank.lastWeekIncome = solaceBank.year2Income / 52;
        solaceBank.order = 0;
        publicCompaniesList[0] = solaceBank;

        PubC microdesk = new PubC
        {
            name = "Micro Desk",
            price = 177.15f,
            year2Income = 61994168845,
            year1Income = 45791577618,
            sharesOutstanidng = 10093557244,
            assets = 332879394833,
            equity = 142168297754,
            high = 179.84f,
            low = 155.54f,
            incomeYTD = 0,
            thisQuarterIncome = 0,
            beta = 1.05f,
            dividendRatio = .25f
        };
        long[] lastThreeQuartersMicro = { microdesk.year2Income / 4, microdesk.year2Income / 4, microdesk.year2Income / 4 };
        microdesk.lastThreeQuarters = lastThreeQuartersMicro;
        microdesk.priceRange = populateArray(microdesk.low, microdesk.high, 52);
        microdesk.high = getHighNum(microdesk.priceRange);
        microdesk.low = getLowNum(microdesk.priceRange, microdesk.high);
        microdesk.priceEarnings = Math.Round(microdesk.price / (microdesk.year2Income / microdesk.sharesOutstanidng), 2);
        microdesk.liabilities = microdesk.assets - microdesk.equity;
        microdesk.bookValue = Math.Round(((double)microdesk.equity / microdesk.sharesOutstanidng), 2);
        microdesk.marketCap = (long)(microdesk.price * microdesk.sharesOutstanidng);
        microdesk.yearOverYear = Math.Round(((microdesk.year2Income - microdesk.year1Income) / (double)microdesk.year1Income) * 100, 2);
        microdesk.dividend = Math.Round(((double)microdesk.year2Income * (double)(microdesk.dividendRatio)) / (double)microdesk.sharesOutstanidng, 2);
        microdesk.lastWeekIncome = microdesk.year2Income / 52;
        microdesk.order = 2;
        publicCompaniesList[2] = microdesk;

        PubC firstEnergy = new PubC
        {
            name = "First Energy",
            price = 71.93f,
            year2Income = 3833684665,
            year1Income = 4182926151,
            sharesOutstanidng = 987317370,
            assets = 122935696466,
            equity = 29445838885,
            high = 77.19f,
            low = 68.13f,
            incomeYTD = 0,
            thisQuarterIncome = 0,
            beta = .57f,
            dividendRatio = .667f
        };
        long[] lastThreeQuartersFirstEnergy = { firstEnergy.year2Income / 4, firstEnergy.year2Income / 4, firstEnergy.year2Income / 4 };
        firstEnergy.lastThreeQuarters = lastThreeQuartersFirstEnergy;
        firstEnergy.priceRange = populateArray(firstEnergy.low, firstEnergy.high, 52);
        firstEnergy.high = getHighNum(firstEnergy.priceRange);
        firstEnergy.low = getLowNum(firstEnergy.priceRange, firstEnergy.high);
        firstEnergy.priceEarnings = Math.Round(firstEnergy.price / (firstEnergy.year2Income / firstEnergy.sharesOutstanidng), 2);
        firstEnergy.liabilities = firstEnergy.assets - firstEnergy.equity;
        firstEnergy.bookValue = Math.Round(((double)firstEnergy.equity / firstEnergy.sharesOutstanidng), 2);
        firstEnergy.marketCap = (long)(firstEnergy.price * firstEnergy.sharesOutstanidng);
        firstEnergy.yearOverYear = Math.Round(((firstEnergy.year2Income - firstEnergy.year1Income) / (double)firstEnergy.year1Income) * 100, 2);
        firstEnergy.dividend = Math.Round(((double)firstEnergy.year2Income * (double)(firstEnergy.dividendRatio)) / (double)firstEnergy.sharesOutstanidng, 2);
        firstEnergy.lastWeekIncome = firstEnergy.year2Income / 52;
        firstEnergy.order = 3;
        publicCompaniesList[3] = firstEnergy;

        PubC pioneerInsurance = new PubC
        {
            name = "Pioneer Insurance",
            price = 76.59f,
            year2Income = 4882530676,
            year1Income = 5427407495,
            sharesOutstanidng = 900000000,
            assets = 127721365338,
            equity = 29445838885,
            high = 78.56f,
            low = 70.78f,
            incomeYTD = 0,
            thisQuarterIncome = 0,
            beta = .85f,
            dividendRatio = .299f
        };
        long[] lastThreeQuartersPioneer = { pioneerInsurance.year2Income / 4, pioneerInsurance.year2Income / 4, pioneerInsurance.year2Income / 4 };
        pioneerInsurance.lastThreeQuarters = lastThreeQuartersPioneer;
        pioneerInsurance.priceRange = populateArray(pioneerInsurance.low, pioneerInsurance.high, 52);
        pioneerInsurance.high = getHighNum(pioneerInsurance.priceRange);
        pioneerInsurance.low = getLowNum(pioneerInsurance.priceRange, pioneerInsurance.high);
        pioneerInsurance.priceEarnings = Math.Round(pioneerInsurance.price / (pioneerInsurance.year2Income / pioneerInsurance.sharesOutstanidng), 2);
        pioneerInsurance.liabilities = pioneerInsurance.assets - pioneerInsurance.equity;
        pioneerInsurance.bookValue = Math.Round(((double)pioneerInsurance.equity / pioneerInsurance.sharesOutstanidng), 2);
        pioneerInsurance.marketCap = (long)(pioneerInsurance.price * pioneerInsurance.sharesOutstanidng);
        pioneerInsurance.yearOverYear = Math.Round(((pioneerInsurance.year2Income - pioneerInsurance.year1Income) / (double)pioneerInsurance.year1Income) * 100, 2);
        pioneerInsurance.dividend = Math.Round(((double)pioneerInsurance.year2Income * (double)(pioneerInsurance.dividendRatio)) / (double)pioneerInsurance.sharesOutstanidng, 2);
        pioneerInsurance.lastWeekIncome = pioneerInsurance.year2Income / 52;
        pioneerInsurance.order = 1;
        publicCompaniesList[1] = pioneerInsurance;

        PubC healthHarbor = new PubC
        {
            name = "Health Harbor",
            price = 62.49f,
            year2Income = 7287022705,
            year1Income = 9086699791,
            sharesOutstanidng = 3559104593,
            assets = 99279802438,
            equity = 25904340217,
            high = 69.92f,
            low = 60.58f,
            incomeYTD = 0,
            thisQuarterIncome = 0,
            beta = 1.08f,
            dividendRatio = .482f
        };
        long[] lastThreeQuartersHealth = { healthHarbor.year2Income / 4, healthHarbor.year2Income / 4, healthHarbor.year2Income / 4 };
        healthHarbor.lastThreeQuarters = lastThreeQuartersHealth;
        healthHarbor.priceRange = populateArray(healthHarbor.low, healthHarbor.high, 52);
        healthHarbor.high = getHighNum(healthHarbor.priceRange);
        healthHarbor.low = getLowNum(healthHarbor.priceRange, healthHarbor.high);
        healthHarbor.priceEarnings = Math.Round(healthHarbor.price / (healthHarbor.year2Income / healthHarbor.sharesOutstanidng), 2);
        healthHarbor.liabilities = healthHarbor.assets - healthHarbor.equity;
        healthHarbor.bookValue = Math.Round(((double)healthHarbor.equity / healthHarbor.sharesOutstanidng), 2);
        healthHarbor.marketCap = (long)(healthHarbor.price * healthHarbor.sharesOutstanidng);
        healthHarbor.yearOverYear = Math.Round(((healthHarbor.year2Income - healthHarbor.year1Income) / (double)healthHarbor.year1Income) * 100, 2);
        healthHarbor.dividend = Math.Round(((double)healthHarbor.year2Income * (double)(healthHarbor.dividendRatio)) / (double)healthHarbor.sharesOutstanidng, 2);
        healthHarbor.lastWeekIncome = healthHarbor.year2Income / 52;
        healthHarbor.order = 4;
        publicCompaniesList[4] = healthHarbor;


    }
    public void publicMerger(int order)
    {
        decimal cost = (decimal)(publicCompaniesList[order].price * (publicCompaniesList[order].sharesOutstanidng - publicCompaniesList[order].sharesOwned));
        if (cost <= advanceTimeScript.userComp.cashATM)
        {
            advanceTimeScript.updateMoney((long)-cost);
            publicCompaniesList[order].merged = true;
            publicCompaniesList[order].sharesOwned = publicCompaniesList[order].sharesOutstanidng;
            publicCompaniesList[order].percentOwned = Math.Round((double)(publicCompaniesList[order].sharesOwned / (double)publicCompaniesList[order].sharesOutstanidng)*100, 4);
            if (publicCompaniesList[order].averageCost == 0)
            {
                publicCompaniesList[order].averageCost = publicCompaniesList[order].price;
            }
            else
            {
                decimal newPercentage = ((decimal)publicCompaniesList[order].sharesOutstanidng) / publicCompaniesList[order].sharesOwned;
                decimal oldPercentage = 1 - newPercentage;
                publicCompaniesList[order].averageCost = (float)((newPercentage * (decimal)publicCompaniesList[order].price) + (oldPercentage * (decimal)publicCompaniesList[order].averageCost));
            }
        }
    }
    public void buyStock(long amountToBuy, int order)
    {
        decimal cost = (decimal)publicCompaniesList[order].price * amountToBuy;
        if (amountToBuy < (publicCompaniesList[order].sharesOutstanidng-publicCompaniesList[order].sharesOwned) && amountToBuy > 0)
        {
            if (advanceTimeScript.userComp.cashATM < cost) {
                int amountUserCanBuy = (int)Math.Floor(advanceTimeScript.userComp.cashATM/(decimal)publicCompaniesList[order].price);
                amountToBuy = amountUserCanBuy;
                cost = (decimal)publicCompaniesList[order].price * amountToBuy;
            }
            publicCompaniesList[order].sharesOwned += amountToBuy;
            if(publicCompaniesList[order].averageCost == 0)
            {
                publicCompaniesList[order].averageCost = publicCompaniesList[order].price;
            }
            else
            {
                decimal newPercentage = ((decimal)amountToBuy)/publicCompaniesList[order].sharesOwned;
                decimal oldPercentage = 1 - newPercentage;
                publicCompaniesList[order].averageCost = (float)((newPercentage * (decimal)publicCompaniesList[order].price) + (oldPercentage*(decimal)publicCompaniesList[order].averageCost));
            }
            advanceTimeScript.updateMoney((long)-cost);
            publicCompaniesList[order].percentOwned = Math.Round((double)(publicCompaniesList[order].sharesOwned/(double)publicCompaniesList[order].sharesOutstanidng)*100, 4);
        }
    }
    public void sellStock(long amountToSell, int order)
    {
        decimal cost = 0;
        if (amountToSell <= publicCompaniesList[order].sharesOwned)
        {
            cost = (decimal)publicCompaniesList[order].price * amountToSell;
        }
        else
        {
            amountToSell = publicCompaniesList[order].sharesOwned;
            cost = (decimal)publicCompaniesList[order].price * publicCompaniesList[order].sharesOwned;
        }
        if (amountToSell > 0)
        {
            publicCompaniesList[order].merged = false;
        }
        publicCompaniesList[order].sharesOwned -= amountToSell;
        advanceTimeScript.updateMoney(cost);
        advanceTimeScript.moneyStats[2] = (decimal)(publicCompaniesList[order].price - publicCompaniesList[order].averageCost) * amountToSell;
        advanceTimeScript.weeklyIncome.Add(new AdvanceTime.income { name = publicCompaniesList[order].name, category = 2, amount = (decimal)(publicCompaniesList[order].price - publicCompaniesList[order].averageCost) * amountToSell});
        if(publicCompaniesList[order].sharesOwned == 0)
        {
            publicCompaniesList[order].averageCost = 0;
        }
        publicCompaniesList[order].percentOwned = Math.Round((double)(publicCompaniesList[order].sharesOwned / (double)publicCompaniesList[order].sharesOutstanidng) * 100, 4);
    }
    public void updateScreen()
    {
        var children = new List<GameObject>();
        foreach (Transform child in contentContainer) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));

        for (int i = 0; i < publicCompaniesList.Length; i++)
        {
            //populates the screen with the companies set up in populate data
            setDataScript = pubCPrefab.GetComponent<PublicCompanySet>();
            PubC comp = publicCompaniesList[i];
            setDataScript.setData(comp.name, comp.priceEarnings, comp.bookValue, comp.marketCap, comp.yearOverYear, comp.dividend, comp.price, comp.high, comp.low, home, i, comp.percentOwned, comp.merged);
            var itemToGen = Instantiate(pubCPrefab);
            itemToGen.transform.SetParent(contentContainer);
            itemToGen.transform.localScale = Vector2.one;
        }
    }
    public class PubC
    {
        public string name;
        public float price;
        public double priceEarnings;
        public double bookValue;
        public float high;
        public float low;
        public long marketCap;
        public double yearOverYear;
        public double dividend;
        public long year1Income;
        public long year2Income;
        public bool merged = false;
        public long assets;
        public long liabilities;
        public long equity;
        public long lastWeekIncome;
        public long incomeYTD;
        public long thisQuarterIncome;
        public long sharesOutstanidng;
        public float beta;
        public float[] priceRange = new float[52];
        public long[] lastThreeQuarters = new long[3];
        public long sharesOwned = 0;
        public double percentOwned = 0;
        public float dividendRatio = .333f;
        public float averageCost = 0;
        public int order;
    }
    private float[] populateArray(float min, float max, int nums)
    {
        float[] toReturn = new float[nums];
        for(int i = 0; i<toReturn.Length; i++)
        {
            toReturn[i] = (float)Math.Round((double)UnityEngine.Random.Range(min*100, max*100)/100,2);
        }
        return toReturn;
    }
    public decimal determinePubCWorth()
    {
        decimal toReturn = 0;
        for (int i = 0; i < publicCompaniesList.Length; i++)
        {
            toReturn += (decimal)publicCompaniesList[i].price * (decimal)publicCompaniesList[i].sharesOwned;
        }
        return toReturn;
    }
    public decimal getUserPubStats()
    {
        decimal assets = 0;
        for(int i = 0; i < publicCompaniesList.Length; i++)
        {
            if (publicCompaniesList[i].merged)
            {
                assets += ((decimal)publicCompaniesList[i].price*publicCompaniesList[i].sharesOwned);
            }
            else
            {
                assets += (publicCompaniesList[i].sharesOwned*(decimal)publicCompaniesList[i].price);
            }
        }
        return assets;
    }
}
