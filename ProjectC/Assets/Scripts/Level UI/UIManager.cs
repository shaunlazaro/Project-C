using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject HPObject;
    private Text HPText;
    // Start is called before the first frame update
    void Start()
    {
        HPText = HPObject.GetComponent<Text>();
    }

    public void UpdateHP(float current,float max)
    {
        HPText.text = string.Format("HP: {0}/{1}",current,max);
    }

}
