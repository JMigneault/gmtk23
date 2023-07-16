using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public enum bType {
      PLAY, TUTORIAL, CREDITS
    }

    public Sprite normalSprite;
    public Sprite depressedSprite;
    public bType b;

    public StrangController sc = null;

    private Menu mc = null;

    private bool pressed = false;

    void Start() {
      mc = transform.parent.GetComponent<Menu>();
    }

    public void OnMouseDown() {
      pressed = true;
    }

    public void OnMouseEnter() {
      GetComponent<SpriteRenderer>().sprite = depressedSprite;
      sc.SetMuted(true);
    }

    public void OnMouseExit() {
      GetComponent<SpriteRenderer>().sprite = normalSprite;
      sc.SetMuted(false);
    }

    void TransitionToTutorial() {
      mc.ic.tm.Transition(mc.gameObject, mc.ic.tutorializer.gameObject);
    }

    void TransitionToGame() {
      mc.ic.tm.Transition(mc.gameObject, mc.ic.playStarter);
    }

    void TransitionToCredits() {
      mc.SetCreditsEnabled(true);
    }

    public void OnMouseUp() {
      GetComponent<SpriteRenderer>().sprite = normalSprite;
      sc.SetMuted(false);

      if (pressed) {
        switch (b) {
          case bType.PLAY:
            TransitionToGame();
          break;
          case bType.TUTORIAL:
            TransitionToTutorial();
          break;
          case bType.CREDITS:
            TransitionToCredits();
          break;
        }
      }
      pressed = false;
    }
}
