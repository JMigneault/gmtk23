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

    public void EnableCredits() {
      creditsEnabled = true;
      credits.SetActive(true);
    }

    public void EnableScore() {
      scoreEnabled = true;
      score.SetActive(true);
    }

    void Update() {
      if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape)) {
        if (creditsEnabled) {
          creditsEnabled = false;
          credits.SetActive(false);
        }
        if (scoreEnabled) {
          scoreEnabled = false;
          score.SetActive(false);
        }
      }
    }
}
