using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bank : MonoBehaviour
{
    public Banks solace = new Banks();
    private float depositRate = 0;
    private float fedRate = 0;
    private int weeksUntilAdj;
    public Button advanceTimeButton;
    private AdvanceTime advanceTimeScript;
    [SerializeField] TextMeshProUGUI loanB1;
    [SerializeField] TextMeshProUGUI maxLoanB1;
    [SerializeField] TextMeshProUGUI loanRateB1;
    [SerializeField] TextMeshProUGUI depositRateB1;
    public void setUpBank()
    {
        advanceTimeScript = advanceTimeButton.GetComponent<AdvanceTime>();
        populateBanks();
        loanB1.text = "Loaned: " + String.Format("{0:C}", solace.loaned);
        maxLoanB1.text = "Max Loan: "+ String.Format("{0:C}", solace.maxLoan);
        loanRateB1.text = "Loan Rate: " + solace.loanRate;
        depositRateB1.text = "Deposit Rate: "+depositRate;

    }
    public void nextWeekBank()
    {

        float randomRate;
        if(advanceTimeScript.nextWeek.AddDays(-7).ToString("MM") != advanceTimeScript.nextWeek.ToString("MM"))
        {
            makePayment();
        }
        if (weeksUntilAdj == 0)
        {
            randomRate = UnityEngine.Random.Range(-50, 51);
            fedRate += (randomRate / 100);
            if (fedRate > 8.5)
            {
                randomRate = UnityEngine.Random.Range(101, 201);
                fedRate -= randomRate/100;
            }if(fedRate < 0.51)
            {
                randomRate = UnityEngine.Random.Range(101, 201);
                fedRate += randomRate / 100;
            }
            randomRate = (UnityEngine.Random.Range(250, 400));
            solace.loanRate = fedRate + (randomRate / 100);
            randomRate = UnityEngine.Random.Range(750, 950);
            solace.maxLoan = (advanceTimeScript.userComp.equity-solace.loaned) * ((decimal)randomRate / 1000);
            if (solace.maxLoan < 0)
            {
                solace.maxLoan = 0;
            }
            depositRate = fedRate;
            if (depositRate > .6)
            {
                randomRate = UnityEngine.Random.Range(10, 50);
                depositRate -= (randomRate / 100);
            }
            else
            {
                randomRate = UnityEngine.Random.Range(5, 9);
                depositRate -= (randomRate / 100);
            }
            solace.loanRate = (float)Math.Round(solace.loanRate, 2);
            depositRate = (float)Math.Round(depositRate, 2);
            weeksUntilAdj = UnityEngine.Random.Range(3, 9);
        }
        else
        {
            randomRate = UnityEngine.Random.Range(750, 950);
            solace.maxLoan = (advanceTimeScript.userComp.equity-solace.loaned) * ((decimal)randomRate / 1000);
            if (solace.maxLoan < 0)
            {
                solace.maxLoan = 0;
            }
            weeksUntilAdj -= 1;
        }
        loanB1.text = "Loaned: " + String.Format("{0:C}", solace.loaned);
        maxLoanB1.text = "Max Loan: " + String.Format("{0:C}", solace.maxLoan);
        loanRateB1.text = "Loan Rate: " + solace.loanRate;
        depositRateB1.text = "Deposit Rate: " + depositRate;
    }
    public void populateBanks()
    {
        fedRate = UnityEngine.Random.Range(10, 550);
        float randomRate = 0;
        fedRate /= 100;
        solace = new Banks();
        randomRate = (UnityEngine.Random.Range(250, 400));
        solace.loanRate = fedRate + (randomRate/100);
        randomRate = UnityEngine.Random.Range(750, 950);
        solace.maxLoan = (advanceTimeScript.userComp.assets-solace.loaned) * ((decimal)randomRate / 1000);

        depositRate = fedRate;
        if(depositRate > .6)
        {
            randomRate = UnityEngine.Random.Range(10, 50);
            depositRate -= (randomRate / 100);
            depositRate = (float)Math.Round(depositRate, 2);
        }
        else
        {
            randomRate = UnityEngine.Random.Range(5, 9);
            depositRate -= (randomRate / 100);
            depositRate = (float)Math.Round(depositRate, 2);
        }
        weeksUntilAdj = UnityEngine.Random.Range(11, 19);
    }
    public class Banks
    {
        public float loanRate;
        public decimal maxLoan;
        public decimal loaned = 0;
    }
    public decimal getBankLoans()
    {
        decimal toReturn = 0;
        toReturn += solace.loaned;
        return toReturn;
    }
    public void takeLoan(String amt)
    {
       decimal amtToLoan = Math.Round(decimal.Parse(amt),2);
        if (amtToLoan <= solace.maxLoan)
        {
            advanceTimeScript.userComp.liabilities += amtToLoan;
            solace.loaned += amtToLoan;
            advanceTimeScript.updateMoney(amtToLoan);
        }
        else
        {
            amtToLoan = solace.maxLoan;
            advanceTimeScript.userComp.liabilities += amtToLoan;
            solace.loaned += amtToLoan;
            advanceTimeScript.updateMoney(amtToLoan);
        }
        solace.maxLoan -= amtToLoan;
        loanB1.text = "Loaned: " + String.Format("{0:C}", solace.loaned);
        maxLoanB1.text = "Max Loan: " + String.Format("{0:C}", solace.maxLoan);
        loanRateB1.text = "Loan Rate: " + solace.loanRate;
        depositRateB1.text = "Deposit Rate: " + depositRate;
    }
    public void payLoan(String amt)
    {
        decimal amtToPay = Math.Round(decimal.Parse(amt), 2);
        if (amtToPay <= advanceTimeScript.userComp.cashATM && amtToPay >= solace.loaned)
        {
            amtToPay = solace.loaned;
            advanceTimeScript.userComp.liabilities -= amtToPay;
            solace.loaned -= amtToPay;
            advanceTimeScript.updateMoney(-amtToPay);
        }
        else if(amtToPay <= advanceTimeScript.userComp.cashATM && amtToPay < solace.loaned)
        {
            advanceTimeScript.userComp.liabilities -= amtToPay;
            solace.loaned -= amtToPay;
            advanceTimeScript.updateMoney(-amtToPay);
        }
        else
        {
            amtToPay = advanceTimeScript.userComp.cashATM;
            if (amtToPay>solace.loaned)
            {
                amtToPay = solace.loaned;
            }
            advanceTimeScript.userComp.liabilities -= amtToPay;
            solace.loaned -= amtToPay;
            advanceTimeScript.updateMoney(-amtToPay);
        }
        solace.maxLoan += amtToPay;
        loanB1.text = "Loaned: " + String.Format("{0:C}", solace.loaned);
        maxLoanB1.text = "Max Loan: " + String.Format("{0:C}", solace.maxLoan);
        loanRateB1.text = "Loan Rate: " + solace.loanRate;
        depositRateB1.text = "Deposit Rate: " + depositRate;
    }
    public void makePayment()
    {
        decimal rate = ((decimal)depositRate / 100) / 12;
        decimal intrestEarned = advanceTimeScript.userComp.cashATM * rate;
        advanceTimeScript.updateMoney(intrestEarned);
        advanceTimeScript.moneyStats[1] += intrestEarned;
        advanceTimeScript.weeklyIncome.Add(new AdvanceTime.income { name = "Interest Earned", category = 1, amount = intrestEarned });

        decimal minPayment = solace.loaned * (decimal)(solace.loanRate/1200);
        solace.loaned -= Math.Round(minPayment,2);
        advanceTimeScript.updateMoney(-minPayment);
        advanceTimeScript.moneyStats[1] -= minPayment;
        advanceTimeScript.weeklyIncome.Add(new AdvanceTime.income { name = "Loan Interest", category = 1, amount = -minPayment });
        rate = ((decimal)solace.loanRate / 100) / 12;
        decimal intrest = (decimal)rate * solace.loaned;
        solace.loaned += intrest;
    }
}
