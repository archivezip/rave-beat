using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChartButton : MonoBehaviour
{
    public Chart chart;

    public Image chartButtonImage;
    public TMPro.TextMeshProUGUI chartButtonName;

    // Start is called before the first frame update
    void Start()
    { 
        chartButtonImage.sprite = chart.songImage;
        chartButtonName.text = chart.songTitle;
    }

}
