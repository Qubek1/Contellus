using System;
using System.Globalization;
using Entities;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private void Update()
    {
        GetComponent<Text>().text = ((int)Game.Instance.CurrentPlayer.GetComponent<Player>().Health).ToString(CultureInfo.CurrentCulture);
    }
}