﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StatusMenu : MonoBehaviour
{
    public int hunger = 100, thirst = 100, happiness = 100;
    float tHunger = 60, tThirst = 60, tHappiness = 60;
    public GameObject menuUI;
    // public float realTime;
    // public DateTime currTime, lastTime;
    public Text chickenNameUi;
    public String chickenName;
    public Slider sliderHunger, sliderThirst, sliderHappiness;
    public bool isOpen;


    void Start()
    {
        CloseMenu();
        print (hunger + " " + thirst + " " + happiness);
    }

    void Update()
    {
        // if(isOpen)
        // {
              
        // }

        // realTime = DateTime.Now;
        // currTime = DateTime.Now;
        // print(currTime.TimeOfDay);
        UpdateHunger();
        UpdateThirst();
        UpdateHappiness();
        if(isOpen)
        {
            if(Input.GetMouseButtonUp(0))
            {
                CloseMenu();
            }
              
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Collider col = this.gameObject.GetComponent<Collider>();
            if(Physics.Raycast(ray, out hit, 100))
            {
                if(hit.collider == col && Input.GetMouseButtonUp(0))
                {
                    // print ("Hit? " + gameObject.name);

                    OpenMenu();
                }
            }
        }
    }

    void OpenMenu()
    {
        menuUI.SetActive(true);
        isOpen = true;
        sliderHunger.value = hunger;
        sliderThirst.value = thirst;
        sliderHappiness.value = happiness;
        chickenNameUi.text = chickenName;   




    }
    void CloseMenu()
    {
        menuUI.SetActive(false);
        isOpen = false;
    }

    void UpdateHunger ()        //Constantly updating and checking if the hunger should go down
    {
        tHunger -= Time.deltaTime;
        if(tHunger <= 0)
        {
            tHunger = 30;
            hunger -= 60 * 60;           // how long it should take before it drops, minute
            sliderHunger.value = hunger;

        }
        // print (tHunger);

    }
    void UpdateThirst ()    //Constantly updating and checking if the thirst should go down
    {
        tThirst -= Time.deltaTime;
        if(tThirst <= 0)
        {
            tThirst = 5;
            thirst -= 60 * 5;           // how long it should take before it drops, minute
            sliderThirst.value = thirst;

        }
        // print (tThirst);

    }
    void UpdateHappiness ()             //Constantly updating and checking if the happiness should go down
    {
        tHappiness -= Time.deltaTime;
        if(tHappiness <= 0)
        {
            tHappiness = 10;
            happiness -= 60 * 15;           // how long it should take before it drops, minute
            sliderHunger.value = happiness;

        }
        // print (tHunger);

    }
}
