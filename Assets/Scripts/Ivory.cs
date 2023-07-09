using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ivory : MonoBehaviour
{
  public float frameLength = 0.5f;
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
      if (t >= frameLength) {
        t = 0f;
        onFirst = !onFirst;
        if (onFirst) {
          GetComponent<SpriteRenderer>().sprite = f1;
        } else {
          GetComponent<SpriteRenderer>().sprite = f2;
        }
      }

    }
}
