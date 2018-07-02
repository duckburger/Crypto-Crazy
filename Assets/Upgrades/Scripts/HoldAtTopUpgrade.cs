using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HoldAtTopUpgrade : MonoBehaviour, ISpecialUpgrade {


    public MiningController minContr;
    public MapController mapController;
    public GameEvent activationEvent;

    public TextMeshProUGUI titleField;
    public TextMeshProUGUI descriptionField;
    public TextMeshProUGUI miscTextField;

    public Image icon;
    public Button upgradeButton;
    public TextMeshProUGUI buttonText;
    public Image upgradeLevelUI;
    public TextMeshProUGUI currentEffectText;

    // Use this for initialization
    void Start()
    {
        minContr = FindObjectOfType<MiningController>();
        ResetDataForNewApartment();
    }

    public void ResetDataForNewApartment()
    {
        mapController = FindObjectOfType<MapController>();
    }

    public void ApplyUpgrade()
    {
        minContr.AddHoldAtTopTime()
    }
}
