using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// start -> menu
// menu -> tutorial
// tutorial -> menu
// menu -> play
// play -> score
// score -> menu


public class TransitionManager : MonoBehaviour
{

  enum curt {
    STILL, LOWERING, RAISING, WAITING
  };

  public Transform curtain;
  public float speed = 1.0f;
  public float waitTime = 0.5f;

  private float progress = 0.0f;
  public float dist = 31.0f;

  private curt s = curt.STILL;

  private GameObject before = null;
  private GameObject after = null;
  bool transitioning = false;

  private void Reset() {
    s = curt.STILL;
    before = null;
    after = null;
    transitioning = false;
  }

  public void DropCurtain() {
    progress = 0.0f;
    s = curt.LOWERING;
  }

  public void RaiseCurtain() {
    progress = 0.0f;
    s = curt.RAISING;
  }

  public void Transition(GameObject before, GameObject after) {
    transitioning = true;
    this.before = before;
    this.after = after;
    DropCurtain();
  }

  // Update is called once per frame
  void Update()
  {
    if (s == curt.WAITING) {
      progress += Time.deltaTime;
      if (progress >= waitTime) {
        RaiseCurtain();
      }
    }
    if (s == curt.LOWERING || s == curt.RAISING) {
      int dir = (s == curt.LOWERING) ? -1 : 1;
      float d = speed * Time.deltaTime;
      progress += d;
      curtain.position = new Vector3(curtain.position.x, curtain.position.y + (d * dir), curtain.position.z);
      if (progress >= dist) {
        if (transitioning && s == curt.LOWERING) {
          if (before != null) {
            before.SetActive(false);
          }
          if (after != null) {
            after.SetActive(true);
          }
          progress = 0;
          s = curt.WAITING;
        } else {
          Reset();
        }
      }
    }
  }
}
