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

    public void CloseAllElements () {
        addObjectPanel.SetActive(false);
        translationArrows.SetActive(false);
    }

}
