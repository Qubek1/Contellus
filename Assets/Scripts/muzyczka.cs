using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class muzyczka : MonoBehaviour
{
    AudioSource audios;
    public GameObject svalue;
    void Start()
    {
        audios = GetComponent<AudioSource>();
        //handle = GameObject.Find("In_Game_UI_Canvas").transform.Find("Options Menu").Find("Slider").Find("Fill Area").Find("Fill").gameObject;
        svalue = GameObject.Find("In_Game_UI_Canvas").transform.Find("Options Menu").Find("Slider").gameObject;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            audios.mute = !audios.mute;
        }
        audios.volume = svalue.GetComponent<Slider>().value;
    }
    
}
