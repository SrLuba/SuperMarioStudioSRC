using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberFont : MonoBehaviour
{
    public List<Sprite> left, middle, right;
    public List<Image> renderers;
    public bool flag_0 = false;
    public bool timer = false;
    
    public string abecedary = "0123456789";

    public string value;

    string oldValue;
    public void updateVisuals()
    {
       string valueSTR = value.ToString();
       char[] characters = value.ToString().ToCharArray();
       for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].gameObject.SetActive((flag_0) ? (value.ToString().Length-1 >= i ? true : false) : true);
        }
       for(int i = 0; i < characters.Length; i++)
        {
            for (int a = 0; a < abecedary.Length; a++)
            {
               if (abecedary[a] == characters[i]) renderers[i].sprite = (i==0) ? (timer) ? middle[a] :  left[a] : ((i>=characters.Length-1) ? right[a] : middle[a]);
                renderers[i].SetNativeSize();   
            }
        }
    }
    void Update()
    {
        if (oldValue != value)
        {
            updateVisuals();
            oldValue = value;
        }
    }
}
