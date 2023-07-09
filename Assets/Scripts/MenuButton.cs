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
      Debug.Log("down");
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

    public void OnMouseUp() {
      GetComponent<SpriteRenderer>().sprite = normalSprite;
      sc.SetMuted(false);

      if (pressed) {
        switch (b) {
          case bType.PLAY:
            mc.menuing = false;
            mc.gameObject.SetActive(false);
            mc.Stop();
            mc.ic.StartGenerating();
          break;
          case bType.TUTORIAL:
            mc.menuing = false;
            mc.gameObject.SetActive(false);
            mc.Stop();
            mc.ic.tutorializer.StartTutorial();
          break;
          case bType.CREDITS:
            mc.EnableCredits();
          break;
        }
      }
      pressed = false;
    }
}
