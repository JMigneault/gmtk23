using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public bool menuing = false;

    public TransitionManager curtains;

    public GameObject credits = null;
    public bool creditsEnabled = false;

    public InstrumentController ic = null;

    public MusicController music;
    
    // ugly hack so people don't press butons by accident while the curtain is down
    bool transitioning = false;
    float buttonDelay = 1.5f;
    float t = 0.0f;

    void OnEnable() {
      menuing = true;
      Play();
    }

    void OnDisable() {
      menuing = false;
      Stop();
    }

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
      t = 0.0f;
      creditsEnabled = enabled;
      if (enabled) {
        SetButtonsEnabled(false);
        ic.tm.Transition(null, credits);
      } else {
        transitioning = true;
        ic.tm.Transition(credits, null);
      }
    }

    void Update() {
      if (transitioning) {
        t += Time.deltaTime;
        if (t >= buttonDelay) {
          transitioning = false;
          t = 0;
          SetButtonsEnabled(true);
        }
      }
      if (creditsEnabled) {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape)) {
          SetCreditsEnabled(false);
        }
      }
    }
}
