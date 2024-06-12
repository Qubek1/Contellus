using System.Globalization;
using Entities;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour
{
    private void Update()
    {
        GetComponent<Text>().text = Game.Instance.points.ToString();
    }
}