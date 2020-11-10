using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class ButtonFade : MonoBehaviour
{
    [SerializeField] float fadeSpeed;
    int fadeTimer = 0;
    [SerializeField] int fadeWaitingTime;
    [SerializeField] VisualEffect fadeEffect;

    [SerializeField] float rightEndPosition;

    private void FixedUpdate() 
    {
        if(fadeTimer == 0 || fadeEffect.GetFloat("Fade Position") == -rightEndPosition){return;} 
        if(fadeTimer < fadeWaitingTime){fadeTimer++;}
        else
        {
            if(fadeEffect.GetInt("Rate") == 0){fadeEffect.SetInt("Rate", 2000);}
            GetComponent<Image>().fillAmount -= fadeSpeed;

            float valueToDecrease = fadeSpeed * 
            (2/*because 0 is the middle of the button, so it needs to decrease twice as fast*/ + 
            0.7f/*added because the edges are 0.7f further out*/) * 
            GetComponent<RectTransform>().localScale.x;

            if(fadeEffect.GetFloat("Fade Position") - valueToDecrease < -rightEndPosition){fadeEffect.SetFloat("Fade Position", -rightEndPosition); fadeEffect.SetInt("Rate", 0);}
            else{fadeEffect.SetFloat("Fade Position", fadeEffect.GetFloat("Fade Position") - valueToDecrease);}
        }
    }
    public void FadeButton()
    {
        fadeEffect.SetFloat("Fade Position", rightEndPosition);
        fadeTimer++;
    }
}
