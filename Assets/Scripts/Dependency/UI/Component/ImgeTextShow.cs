using UnityEngine;
using BanpoFri;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class ImgeTextShow : MonoBehaviour
{
    public GameObject OnRoot;

    public GameObject OffRoot;
    
    public Image Image;

    public TextMeshProUGUI Text;   

    public void Set(Sprite sprite, string text)
    {
        Image.sprite = sprite;
        Text.text = text;
    }
}

