using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour {

  public Color unselectedColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
  public Color selectedColor = new Color(0.2122642f, 0.4006358f, 1.0f, 1.0f);
  public GameObject VRRaycaster;

  private GameObject topBorder;
  private GameObject bottomBorder;
  private GameObject leftBorder;
  private GameObject rightBorder;

  private uint modelSelected = 0;
  private bool changeModel = false;

  void Start() {
    SelectPanel();
  }

  void Update() {
    if(changeModel) {
      SelectPanel();
    }
  }

  void OnVRTriggerDown(string buttonName) {
    if(buttonName.Split('_')[0] == "Model") {
      UpdateModel(buttonName.Split('_')[1]);
    } else if(buttonName.Split('_')[0] == "Mount") {
      VRRaycaster.SendMessage("Mount", modelSelected);
    }
  }

  void UpdateModel(string numId) {

    UnselectPanel();

    switch(numId) {
      case "0":
        modelSelected = 0;
        break;
      case "1":
        modelSelected = 1;
        break;
      case "2":
        modelSelected = 2;
        break;
      case "3":
        modelSelected = 3;
        break;
      case "4":
        modelSelected = 4;
        break;
      default:
        break;
    };

    changeModel = true;
  }

  void SelectPanel() {
    // Reference gameObjects
    topBorder = transform.Find("Divider_" + modelSelected.ToString()).gameObject;
    bottomBorder = transform.Find("Divider_" + (modelSelected + 1).ToString()).gameObject;
    leftBorder = transform.Find("Model_Img_Border_Left_" + modelSelected.ToString()).gameObject;
    rightBorder = transform.Find("Model_Img_Border_Right_" + modelSelected.ToString()).gameObject;

    // Change color
    topBorder.GetComponent<Renderer>().material.color = selectedColor;
    bottomBorder.GetComponent<Renderer>().material.color = selectedColor;
    leftBorder.GetComponent<Renderer>().material.color = selectedColor;
    rightBorder.GetComponent<Renderer>().material.color = selectedColor;
  }

  void UnselectPanel() {
    // Reference gameObjects
    topBorder = transform.Find("Divider_" + modelSelected.ToString()).gameObject;
    bottomBorder = transform.Find("Divider_" + (modelSelected + 1).ToString()).gameObject;
    leftBorder = transform.Find("Model_Img_Border_Left_" + modelSelected.ToString()).gameObject;
    rightBorder = transform.Find("Model_Img_Border_Right_" + modelSelected.ToString()).gameObject;

    // Change color
    topBorder.GetComponent<Renderer>().material.color = unselectedColor;
    bottomBorder.GetComponent<Renderer>().material.color = unselectedColor;
    leftBorder.GetComponent<Renderer>().material.color = unselectedColor;
    rightBorder.GetComponent<Renderer>().material.color = unselectedColor;
  }

}
