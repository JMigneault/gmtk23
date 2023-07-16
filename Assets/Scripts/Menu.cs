using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public bool menuing = false;

    private bool tToPlay = false;
    private bool tToTutorial = false;

    public TransitionManager curtains;

    public GameObject credits = null;
    public bool creditsEnabled = false;

    public InstrumentController ic = null;

    public MusicController music;

    // transition objects
    public GameObject playTO;
    public GameObject tutorialTO;

    public void Play() {
      music.menuMusic.Play();
    }

    public void Stop() {
      music.StopMenuMusic();
    }

    public void SetButtonsEnabled(bool e) {
      foreach (MenuButton b in GetComponentsInChildren<MenuButton>()) {
        b.GetComponent<BoxCollider2D>().enabled = e;
      }
    }

    public void SetCreditsEnabled(bool enabled) {
      creditsEnabled = enabled;
      SetButtonsEnabled(!enabled);
      if (enabled) {
        ic.tm.Transition(null, credits);
      } else {
        ic.tm.Transition(credits, null);
      }

    }

    void Update() {
      if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape)) {
        if (creditsEnabled) {
          SetCreditsEnabled(false);
        }
      }
    }
}
