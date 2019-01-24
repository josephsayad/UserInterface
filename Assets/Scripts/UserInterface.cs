using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterface : MonoBehaviour {

  public Color unselectedColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
  public Color selectedColor = new Color(0.2122642f, 0.4006358f, 1.0f, 1.0f);
  public GameObject VRRaycaster;
  public GameObject openUIButton;

  // For Models
  private GameObject topBorder;
  private GameObject bottomBorder;
  private GameObject leftBorder;
  private GameObject rightBorder;

  private uint modelSelected = 0;
  private bool changeModel = false;

  public string selectedUI = "Models_UI";

  void Start() {
    SelectPanel();
  }

  void Update() {
    if(changeModel) {
      SelectPanel();
    }
  }

  void OnVRTriggerDown(string buttonName) {

    if(buttonName == "ExitUI") {
      gameObject.active = false;
      openUIButton.SetActive(true);
    }

    if(selectedUI == "Models_UI") {
      if(buttonName.Split('_')[0] == "Model") {
        UpdateModel(buttonName.Split('_')[1]);
      } else if(buttonName.Split('_')[0] == "Mount") {
        VRRaycaster.SendMessage("Mount", modelSelected);
      } else if(buttonName.Split('_')[0] == "Models") {
        // Do Nothing!
      } else if(buttonName.Split('_')[0] == "Environments") {
        TurnOff("Models");
        UnselectMenuPanel("Model");
        SelectMenuPanel("Environment");
        selectedUI = "Environments_UI";
      }
    } else if(selectedUI == "Environments_UI") {
      if(buttonName.Split('_')[0] == "Environments") {
        // Do Nothing!
      } else if(buttonName.Split('_')[0] == "Models") {
        TurnOn("Models");
        SelectMenuPanel("Model");
        UnselectMenuPanel("Environment");
        selectedUI = "Models_UI";
      }
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

  void TurnOff(string type) {

    switch(type) {

      case "Models":
        // Turn off model panels
        transform.Find("Model_0").gameObject.active = false;
        transform.Find("Model_1").gameObject.active = false;
        transform.Find("Model_2").gameObject.active = false;
        transform.Find("Model_3").gameObject.active = false;
        transform.Find("Model_4").gameObject.active = false;
        // Turn off mount button
        transform.Find("Mount").gameObject.active = false;
        break;

      default:
        break;
    }
  }

  void TurnOn(string type) {

    switch(type) {

      case "Models":
        // Turn on model panels
        transform.Find("Model_0").gameObject.active = true;
        transform.Find("Model_1").gameObject.active = true;
        transform.Find("Model_2").gameObject.active = true;
        transform.Find("Model_3").gameObject.active = true;
        transform.Find("Model_4").gameObject.active = true;
        // Turn on mount button
        transform.Find("Mount").gameObject.active = true;
        break;

      default:
        break;
    }
  }

  void SelectMenuPanel(string type) {

    // Reference gameObjects
    topBorder = transform.Find("Border_Top_" + type + "_Panel").gameObject;
    bottomBorder = transform.Find("Border_Bottom_" + type + "_Panel").gameObject;
    leftBorder = transform.Find("Border_Left_" + type + "_Panel").gameObject;
    rightBorder = transform.Find("Border_Right_" + type + "_Panel").gameObject;

    // Change color
    topBorder.GetComponent<Renderer>().material.color = selectedColor;
    bottomBorder.GetComponent<Renderer>().material.color = selectedColor;
    leftBorder.GetComponent<Renderer>().material.color = selectedColor;
    rightBorder.GetComponent<Renderer>().material.color = selectedColor;

  }

  void UnselectMenuPanel(string type) {

    // Reference gameObjects
    topBorder = transform.Find("Border_Top_" + type + "_Panel").gameObject;
    bottomBorder = transform.Find("Border_Bottom_" + type + "_Panel").gameObject;
    leftBorder = transform.Find("Border_Left_" + type + "_Panel").gameObject;
    rightBorder = transform.Find("Border_Right_" + type + "_Panel").gameObject;

    // Change color
    topBorder.GetComponent<Renderer>().material.color = unselectedColor;
    bottomBorder.GetComponent<Renderer>().material.color = unselectedColor;
    leftBorder.GetComponent<Renderer>().material.color = unselectedColor;
    rightBorder.GetComponent<Renderer>().material.color = unselectedColor;

  }

}
