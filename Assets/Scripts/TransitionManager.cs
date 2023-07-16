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
  private float originalSpeed = 1.0f;
  public float waitTime = 0.5f;

  public float upY = 35.0f;
  public float downY = 0.0f;

  private curt s = curt.STILL;

  private GameObject before = null;
  private GameObject after = null;
  bool transitioning = false;

  float t = 0.0f;

  void Start() {
    originalSpeed = speed;
  }

  private void Reset() {
    t = 0.0f;
    s = curt.STILL;
    before = null;
    after = null;
    transitioning = false;
    speed = originalSpeed;
  }

  public void DropCurtain() {
    s = curt.LOWERING;
  }

  public void RaiseCurtain() {
    if (before != null) {
      before.SetActive(false);
    }
    if (after != null) {
      after.SetActive(true);
    }
    s = curt.RAISING;
  }

  public void Transition(GameObject before, GameObject after, bool openingSeq = false) {
    Debug.Log("Transitioning from: " + before + " to: " + after);
    transitioning = true;
    this.before = before;
    this.after = after;
    if (openingSeq) {
      speed = 20.0f;
      RaiseCurtain();
    } else {
      DropCurtain();
    }
  }

  // Update is called once per frame
  void Update()
  {
    float dp = speed * Time.deltaTime;

    switch (s) {
    case curt.STILL:
      break;
    case curt.WAITING:
      t += Time.deltaTime;
      if (t >= waitTime) {
        t = 0.0f;
        RaiseCurtain();
      }
      break;
    case curt.LOWERING:
      curtain.position = new Vector3(curtain.position.x, curtain.position.y - dp, curtain.position.z);
      if (curtain.localPosition.y <= downY) {
        s = curt.WAITING;
      }
      break;
    case curt.RAISING:
      curtain.position = new Vector3(curtain.position.x, curtain.position.y + dp, curtain.position.z);
      if (curtain.localPosition.y >= upY) {
        s = curt.STILL;
        Reset();
      }
      break;
    }
  }
}
