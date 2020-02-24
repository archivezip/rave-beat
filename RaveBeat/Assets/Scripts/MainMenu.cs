using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    static private bool gameFirstStart = true;

    public GameObject titleUi;
    public GameObject mainMenuUi;
    public GameObject chartSelectUi;

    public GameObject menuFirstSelected;
    public GameObject pakFirstSelected;

    public GameObject chartButtonPrefab;
    public GameObject chartButtonsContainer;
    private Chart[] charts;

    public void AnyKeyPressed()
    {
        titleUi.SetActive(false);
        mainMenuUi.SetActive(true);
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(menuFirstSelected);
        gameFirstStart = false;
    }

    public void ChartSelectPressed()
    {
        mainMenuUi.SetActive(false);
        chartSelectUi.SetActive(true);
        int x = 0;
        charts = Resources.LoadAll<Chart>("Charts");
        foreach (Chart chart in charts)
        {
            GameObject button = Instantiate(chartButtonPrefab) as GameObject;
            button.transform.SetParent(chartButtonsContainer.transform);
            button.transform.localScale = new Vector3(1f, 1f, 1f);
            button.GetComponent<ChartButton>().chart = chart;
            button.GetComponent<Button>().onClick.AddListener(() => LoadChart());
            if (x == 0) GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(button);
            x++;
        }
    }

    public void ChartMakerPressed()
    {

    }

    public void OptionsPressed()
    {

    }

    public void QuitPressed()
    {
        Application.Quit();
    }

    public void LoadChart()
    {
        GameObject chart =  GameObject.Find("EventSystem").GetComponent<EventSystem>().currentSelectedGameObject;
        PlayerPrefs.SetString("chart", chart.GetComponent<ChartButton>().chart.songTitle);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    // Start is called before the first frame update
    void Start()
    {
        if(gameFirstStart) 
        {
            titleUi.SetActive(true);
            GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(pakFirstSelected); 
        }
        else 
        {
            titleUi.SetActive(false);
            mainMenuUi.SetActive(true);
            GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(menuFirstSelected); 
        }
        
    }

}
