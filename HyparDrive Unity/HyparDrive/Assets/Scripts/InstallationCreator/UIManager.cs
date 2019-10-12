using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager INSTANCE;

    [SerializeField]
    private GameObject addObjectPanel;

    [SerializeField]
    private GameObject translationArrows;

    [SerializeField]
    private GameObject saveScreen;
    [SerializeField]
    private GameObject loadScreen;

    public void Start () {
        if (INSTANCE == null) {
            INSTANCE = this;
        }

        CloseAllElements();
    }

    public void SetTranslationArrows ( Vector3 position ) {
        translationArrows.transform.position = position;
        translationArrows.SetActive(true);
    }

    public void HideTranslationArrows () {
        translationArrows.SetActive(false);
    }

    public void SetSpawnPanelState ( bool state ) {
        addObjectPanel.SetActive(state);
    }

    public void ToggleSpawnPanelState () {
        addObjectPanel.SetActive(!addObjectPanel.activeSelf);
    }

    public void SetSaveScreenState ( bool state ) {
        saveScreen.SetActive(state);
    }

    public void SetLoadScreenState ( bool state ) {
        loadScreen.SetActive(state);
    }

    public void CloseAllElements () {
        addObjectPanel.SetActive(false);
        translationArrows.SetActive(false);
        saveScreen.SetActive(false);
        loadScreen.SetActive(false);
    }

}
