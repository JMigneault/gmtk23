using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public Sprite[] sprites;

    public int strang;
    public NColor color;
    public float speed;
    public float speedY = 0.0f;
    private InstrumentController ic;

    private bool played = false;

    private SpriteRenderer sr = null;

    // Animations
    private float animT = 0;

    // Played correct animation
    private bool pcStart = false;
    public float pcLength = 0.25f;
    public float pcMaxScale = 1.5f;

    private bool miStart = false;
    private float miLength = 1.0f;
    private float miNextRot;
    public float miRotProp = 0.25f;

    private bool piStart = false;
    private float piLength = 0.4f;
    private int piNFrames = 2;
    public Sprite[] piF1Sprites; // [color][frame]
    public Sprite[] piF2Sprites; // [color][frame]

    public float mcLength = 1.5f;
    private bool mcStart = false;
    public float mcHoleX = -2.13f;
    public float mcFadeProp = 0.3f; // the proportion of the beginning of the animation that we fade during
    public float mcWaitProp = 0.15f;
    public Sprite mcGoldSprite;
    public Vector3 mcTarget = new Vector3(1.688f, 11.372f, 0f);
    private float mcSpeed = 0.0f;

    public void Init(int strang, NColor col, InstrumentController ic) {
      sr = GetComponentInChildren<SpriteRenderer>();
      float ypos = (strang - 1.5f) * ic.strangHeight;
      this.transform.position = new Vector3(ic.totalLength, ypos, 0);
      this.strang = strang;
      this.color = col;
      this.speed = ic.getNoteSpeed();
      this.ic = ic;
      sr.sprite = sprites[(int)col];
    }

    public void StartPlayedCorrectlyAnim() {
      pcStart = true;
      speed = 0.0f;
      transform.position = new Vector3(0.0f, transform.position.y, transform.position.z);
      float s = pcMaxScale;
      transform.localScale = new Vector3(s, s, s);
    }

    void PlayedCorrectlyAnim() {
      float animPropProgress = animT / pcLength;
      sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1 - animPropProgress);
      animT += Time.deltaTime;
      if (animT > pcLength) {
        Destroy(gameObject);
      }
    }

    public void StartMutedIncorrectlyAnim() {
      miStart = true;
      sr.color = Color.black;
      speed /= 4.0f;
      speedY = -1.5f; // TODO param
      miNextRot = miRotProp;
    }

    void MutedIncorrectlyAnim() {
      float animPropProgress = animT / miLength;
      miNextRot -= Time.deltaTime / miLength;

      if (miNextRot <= 0) {
        transform.eulerAngles = transform.eulerAngles - new Vector3(0, 0, 90.0f);
        miNextRot = miRotProp;
      }

      sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1 - animPropProgress);
      animT += Time.deltaTime;
      if (animT > miLength) {
        Destroy(gameObject);
      }
    }

    public void StartPlayedIncorrectlyAnim() {
      piStart = true;
      speed = 0.0f;
      transform.position = new Vector3(0.0f, transform.position.y, transform.position.z);
      sr.sprite = piF1Sprites[(int)color];
    }

    void PlayedIncorrectlyAnim() {
      float animPropProgress = animT / piLength;
      if (animPropProgress > 0.5f) { // NOTE: only works for 2 frames
        sr.sprite = piF2Sprites[(int)color];
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f - (animPropProgress - 0.5f) / 0.5f);
      }
      animT += Time.deltaTime;
      if (animT > piLength) {
        Destroy(gameObject);
      }
    }

    public void StartMutedCorrectlyAnim() {
      mcStart = true;
      sr.sprite = mcGoldSprite;
      speed = 0;
      float totalTime = mcLength * (1.0f - mcWaitProp); // this is annoying
      mcSpeed = (mcTarget - transform.position).magnitude / totalTime;
    }

    void MutedCorrectlyAnim() {
      float animPropProgress = animT / mcLength;
      float alpha = (animPropProgress >= mcFadeProp) ? 1 : (1 - (animPropProgress / mcFadeProp)) / 2.0f + 0.5f;
      sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
      if (animPropProgress >= mcWaitProp) {
        Vector3 toTarget = mcTarget - transform.position;
        if (toTarget.magnitude < Mathf.Epsilon) {
          Destroy(gameObject);
        }
        Vector3 dir = (mcTarget - transform.position).normalized;
        transform.position = transform.position + dir * mcSpeed * Time.deltaTime;
      }
      animT += Time.deltaTime;
      if (animT > mcLength) {
        Destroy(gameObject);
      }
    }

    // Update is called once per frame
    void Update()
    {
      Vector3 currPos = this.transform.position;
      transform.position = new Vector3(currPos.x - speed * Time.deltaTime, currPos.y + speedY * Time.deltaTime, currPos.z);
      if (!played && transform.position.x <= 0) {
        ic.PlayNote(this);
        played = true;
      }
      if (transform.position.x <= -5) { // kys
        Destroy(gameObject);
      }
      if (pcStart) {
        PlayedCorrectlyAnim();
      }
      if (miStart) {
        MutedIncorrectlyAnim();
      }
      if (piStart) {
        PlayedIncorrectlyAnim();
      }
      if (mcStart) {
        MutedCorrectlyAnim();
      }
    }
}
