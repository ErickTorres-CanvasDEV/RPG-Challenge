﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    private Camera cam;
    [SerializeField] private Image imgAim;
    [SerializeField]private GameObject takeItemPrefab;
    [SerializeField]private Transform takeItemContainer;
    [SerializeField]private Image imageHpBar;
    [SerializeField]private GameObject pausePanel;
    [SerializeField]private GameObject optionsPanel;
    [SerializeField]private GameObject gameOverPanel;

    [Header("Dialog")]
    [SerializeField]private GameObject dialogPanel;
    [SerializeField]private TextMeshProUGUI txtNameNPC;
    [SerializeField]private TextMeshProUGUI txtDialog;
    
    [Header("Tutorial")]
    [SerializeField]private string firstTutorialString;
    [SerializeField]private GameObject tutorialPanel;
    [SerializeField]private TextMeshProUGUI txtTutorial;
    [SerializeField]private bool hasTutorial;

    [Header("Craft")]
    public GameObject craftPanel;

    [Header("Victory")]
    [SerializeField]private GameObject victoryPanel;

    [SerializeField]private Button primaryPauseButton;
    [SerializeField]private Button primaryGameoverButton;
    [SerializeField]private Button primaryVictoryButton;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        cam = Camera.main;
        if(hasTutorial)
        {
            OpenTutorialPanel(firstTutorialString);
        }
    }

    public void OpenDialogPanel(string name, string text)
    {
        GameController.Instance.ChangeGameState(GameState.DIALOG);
        dialogPanel.SetActive(true);
        SetDialogText(name, text);
    }

    public void SetDialogText(string name, string text)
    {
        txtNameNPC.text = name;
        txtDialog.text = text;
    }

    public bool CheckIfDialog()
    {
        return dialogPanel.activeSelf;
    }

    public void Quit()
    {
        FadeController.Instance.Exit();
    }

    public void CloseDialogPanel()
    {
        GameController.Instance.ChangeGameState(GameState.GAMEPLAY);
        dialogPanel.SetActive(false);
        txtNameNPC.text = "";
        txtDialog.text = "";
    }

    public void OpenVictoryPanel()
    {
        victoryPanel.SetActive(true);
        primaryVictoryButton.Select();
    }

    public void OpenCraftPanel()
    {
        GameController.Instance.ChangeGameState(GameState.PAUSE);
        craftPanel.SetActive(true);
    }

    public void OpenTutorialPanel(string text)
    {
        tutorialPanel.SetActive(true);
        txtTutorial.text = text;
        StopCoroutine(HideTutorialPanel());
        StartCoroutine(HideTutorialPanel());
    }

    IEnumerator HideTutorialPanel()
    {
        yield return new WaitForSeconds(7f);
        tutorialPanel.SetActive(false);
        txtTutorial.text = "";
    }

    public void TakeItemHUD(Collectable item)
    {
        TakeItemNotification temp = Instantiate(takeItemPrefab, takeItemContainer).GetComponent<TakeItemNotification>();
        temp.UpdateNotification(item.itemIcon, item.itemName);
    }

    public void SetTargetHUD()
    {
        imgAim.enabled = !imgAim.enabled;
    }

    public void OpenPause()
    {
        pausePanel.SetActive(true);
        optionsPanel.SetActive(false);
        SetSelectedButton(primaryPauseButton);
        GameController.Instance.ChangeGameState(GameState.PAUSE);
    }

    public void UpdateHpBar(float currentHealth, float maxHealth)
    {
        imageHpBar.fillAmount = currentHealth / maxHealth;
    }

    public void SetSelectedButton(Button button)
    {
        button.Select();
    }

    public bool isPause()
    {
        return pausePanel.activeSelf;
    }

    public void OpenGameOver()
    {
        gameOverPanel.SetActive(true);
        primaryGameoverButton.Select();
    }

    public void QuitToTitle()
    {
        gameOverPanel.SetActive(false);
        FadeController.Instance.ChangeScene("title");
    }

    public void TryAgain()
    {
        FadeController.Instance.ReloadScene();
    }

    public void ClosePause()
    {
        pausePanel.SetActive(false);
         optionsPanel.SetActive(false);
        GameController.Instance.ChangeGameState(GameState.GAMEPLAY);
    }
}
