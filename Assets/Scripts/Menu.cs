using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public bool menuing = false;

    public GameObject credits = null;
    public GameObject score = null;
    private bool creditsEnabled = false;
    private bool scoreEnabled = false;

    public InstrumentController ic = null;

    public void SetButtonsEnabled(bool e) {
      foreach (MenuButton b in GetComponentsInChildren<MenuButton>()) {
        b.GetComponent<BoxCollider2D>().enabled = e;
      }
    }

    public void EnableCredits() {
      creditsEnabled = true;
      credits.SetActive(true);
      SetButtonsEnabled(false);
    }

    public void EnableScore() {
      scoreEnabled = true;
      score.SetActive(true);
      SetButtonsEnabled(false);
    }

    void Update() {
      if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape)) {
        if (creditsEnabled) {
          creditsEnabled = false;
          credits.SetActive(false);
          SetButtonsEnabled(true);
        }
        if (scoreEnabled) {
          scoreEnabled = false;
          score.SetActive(false);
          SetButtonsEnabled(true);
        }
      }
    }
}
