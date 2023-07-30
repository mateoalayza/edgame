using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class opcion_but : MonoBehaviour
{
    public TextMeshProUGUI text_obj;
    
    public void settext(string txt)
    {
        text_obj.text = txt;
    }

   
}
