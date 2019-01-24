using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour {

  public GameObject UserInterface;

  void Start() {}
  void Update() {}

  void OnVRTriggerDown() {
    UserInterface.SetActive(true);
    gameObject.active = false;
  }
}
