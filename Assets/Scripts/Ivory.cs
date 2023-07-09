using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ivory : MonoBehaviour
{
  public float frame1Length = 0.5f;
  public float frame2Length = 0.5f;
  public Sprite f1;
  public Sprite f2;
  private bool onFirst = true;
  private float t = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
      GetComponent<SpriteRenderer>().sprite = f1;
    }

    // Update is called once per frame
    void Update()
    {
      t += Time.deltaTime;
      if (onFirst && t >= frame1Length) {
        t = 0f;
        onFirst = false;
        GetComponent<SpriteRenderer>().sprite = f2;
      } else if (!onFirst && t >= frame2Length) {
        t = 0f;
        onFirst = true;
        GetComponent<SpriteRenderer>().sprite = f1;
      }
    }
}
