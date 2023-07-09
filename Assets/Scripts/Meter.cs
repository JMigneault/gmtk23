using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meter : MonoBehaviour
{
    public float length = 34f;
    public int capacity = 100;
    public float speed = 1.0f;
    private float markerX = 0;
    private Transform marker;
    public GameObject loseScreen = null;
    int amount;

    void Start() {
      marker = transform.GetChild(0);
      markerX = marker.localPosition.x;
      amount = capacity / 2;
    }

    public void Correct(int a) {
      amount += a;
      amount = amount > capacity ? capacity : amount;
    }

    public void Incorrect(int a) {
      amount -= a;
      if (amount <= 0) {
        amount = 0;
        speed = 0;
        markerX = length * -0.5f;
        loseScreen.SetActive(true);
      }
    }

    void Update() {
      float targetX = ((amount / (float)capacity) - 0.5f) * length;
      float dx = targetX - markerX;
      if (Mathf.Abs(dx) > .1) {
        markerX += ((dx > 0) ? speed : -speed) * Time.deltaTime;
      }
      marker.localPosition = new Vector3(markerX, marker.localPosition.y, marker.localPosition.z);
    }

    public void Reset() {
      amount = capacity / 2;
    }
}
