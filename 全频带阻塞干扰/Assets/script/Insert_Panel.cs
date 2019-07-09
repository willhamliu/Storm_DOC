using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Insert_Panel : MonoBehaviour {

    public Text Audo_Value;
    public Slider Audo_Slider;
    public AudioSource BGM;

    public void Start()
    {
        Audo_Value.text = (Audo_Slider.value*100).ToString();
        Audo_Slider.onValueChanged.AddListener((float value) =>Audo_Adjust(value));
    }
    public void Audo_Adjust( float value)
    {
        Audo_Value.text = ((int)(value*100)).ToString();
        BGM.volume = Audo_Slider.value;
    }
}
