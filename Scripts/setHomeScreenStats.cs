using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class setHomeScreenStats : MonoBehaviour
{
    public TextMeshProUGUI textToSet;

    public void setData(string typeOfIncome, decimal income)
    {
        textToSet.text = typeOfIncome + ": " + String.Format("{0:C}", income); ;
    }
}
