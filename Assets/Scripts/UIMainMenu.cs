using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] GameObject ControlsPanel;
    [SerializeField] GameObject CreditsPanel;
    [SerializeField] TextMeshProUGUI SecondsSurvivedText;
    [SerializeField] TextMeshProUGUI IceBergSizeText;
    private void Start()
    {
        if (SecondsSurvivedText)
        {
            SecondsSurvivedText.text = GameManager.Get().GetSecondsSurvived().ToString();
        }
        if (IceBergSizeText)
        { 
            IceBergSizeText.text = GameManager.Get().GetMaxIceBergSize().ToString();
        }
    }
    public void StartGame()
    {
        GameManager.Get().GoToGameplayScene();
    }
    public void ReturnToMainMenu()
    {
        GameManager.Get().GoToMainMenuScene();
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void ShowControls()
    {
        if (ControlsPanel) ControlsPanel.gameObject.SetActive(true);
    }
    public void CloseControls()
    {
        if (ControlsPanel) ControlsPanel.gameObject.SetActive(false);
    }
    public void ShowCredits()
    {
        if (CreditsPanel) CreditsPanel.gameObject.SetActive(true);
    }
    public void CloseCredits()
    {
        if (CreditsPanel) CreditsPanel.gameObject.SetActive(false);
    }
}
