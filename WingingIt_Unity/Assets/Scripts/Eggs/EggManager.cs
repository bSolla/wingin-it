﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EggManager : MonoBehaviour
{
    EggInfo[] commonEggs;
    EggInfo[] rareEggs;
    EggInfo[] legendaryEggs;

    bool eggDroped;
    bool eggPicked;
    EggInfo eggInfo;
    GameObject currentEgg;

    LevelManager lm;

    [SerializeField] float commonExp=20;
    [SerializeField] float rareExp = 50;
    [SerializeField] float legendaryExp = 100;

    //Kine variables
    public DateTime currentTime, oldTime;
    public GameObject eggPrefab;
    Vector3 dropTrans;
    public bool dropEgg;

    public EggInfo[] CommonEggs { get => commonEggs; set => commonEggs = value; }
    public EggInfo[] RareEggs { get => rareEggs; set => rareEggs = value; }
    public EggInfo[] LegendaryEggs { get => legendaryEggs; set => legendaryEggs = value; }


    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    //                                  M E T H O D S 
    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    private void Awake()
    {
        lm = GetComponent<LevelManager>();

        Material[] commonMaterials = Resources.LoadAll<Material>("Common Materials");
        Material[] rareMaterials = Resources.LoadAll<Material>("Rare Materials");
        Material[] legendaryMaterials = Resources.LoadAll<Material>("Legendary Materials");

        CommonEggs = new EggInfo[commonMaterials.Length];
        RareEggs = new EggInfo[rareMaterials.Length];
        LegendaryEggs = new EggInfo[legendaryMaterials.Length];

        int i = 0; 
        foreach (Material mat in commonMaterials)
        {
            CommonEggs[i] = new EggInfo();
            CommonEggs[i].eggMaterial = mat;
            CommonEggs[i].type = EggInfo.EggType.Common;
            CommonEggs[i].owned = false;
            i++;
        }

        i = 0; 
        foreach (Material mat in rareMaterials)
        {
            RareEggs[i] = new EggInfo();
            RareEggs[i].eggMaterial = mat;
            RareEggs[i].type = EggInfo.EggType.Rare;
            RareEggs[i].owned = false;
            i++;
        }

        i = 0; 
        foreach (Material mat in legendaryMaterials)
        {
            LegendaryEggs[i] = new EggInfo();
            LegendaryEggs[i].eggMaterial = mat;
            LegendaryEggs[i].type = EggInfo.EggType.Legendary;
            LegendaryEggs[i].owned = false;
            i++;
        }
    }

    //When you change scenes eggs disapear so the scripts thinks that there is an egg when its not, so each time a scene is loaded the bool is set to false
    private void OnLevelWasLoaded(int level)  //Maybe better if this is done in another way
    {
        eggDroped = false; //If we make the eggs apears always in the nest we can just remove this method but I'll keep ot here for now
        eggPicked = false;
    }

    
    void Start()
    {
        print(System.DateTime.Now);
        oldTime = DateTime.Now;
    }


    void Update()
    {
        CheckNewTime();

        if (Input.GetMouseButtonDown(0))
        {
            if (eggPicked)
            {
                Destroy(currentEgg);
                eggPicked = false;
            }
            if (eggDroped)
            {
                PickUpEgg();
            }            
        }        
    }


    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<       P I C K   U P   E G G S   M E T H O D S       >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    void PickUpEgg()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.tag=="Egg")
            {
                currentEgg.transform.localScale *= 3;
                currentEgg.transform.position = new Vector3(0, 5, 0);

                EggInfo info = currentEgg.GetComponent<EggInfo>();
                AddToColection(info);

                switch (info.type)
                {
                    case EggInfo.EggType.Common:
                        lm.AddExp(commonExp);
                        break;

                    case EggInfo.EggType.Rare:
                        lm.AddExp(rareExp);
                        break;

                    case EggInfo.EggType.Legendary:
                        lm.AddExp(legendaryExp);
                        break;

                    default:
                        break;
                }

                //Change this to another way to destroy them, zoom with the camera, bool that you are looking at them and a new method to rotate them
                eggDroped = false;
                eggPicked = true;
            }
        }
    }



    void AddToColection(EggInfo pickedEgg)
    {
        if (!pickedEgg.owned)
        {
            switch (pickedEgg.type)
            {
                case EggInfo.EggType.Common:
                    CommonEggs[pickedEgg.eggNum].owned = true;
                    break;

                case EggInfo.EggType.Rare:
                     RareEggs[pickedEgg.eggNum].owned = true;
                    break;

                case EggInfo.EggType.Legendary:
                     LegendaryEggs[pickedEgg.eggNum].owned = true;
                    break;

                default:
                    break;
            }
        }
    }



    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<        E G G   D R O P   M E T H O D S      >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


        //I think i did a mesh with something while i was copping the script because it spawns the egg always in the same place and when there are still eggs in the scene


    //Checks if its time to drop an egg
    private void CheckNewTime()
    {
        // string currScene = GetComponent<GameManager>().currentSceneName;
        currentTime = DateTime.Now;
        if (!eggDroped && (GameManager.instance.CurrentSceneName == "Outside" || GameManager.instance.CurrentSceneName == "Inside"))
        {
            if (oldTime.Date < currentTime.Date)
            {
                return;
            }
            else
            {
                if (oldTime.Hour < currentTime.Hour)
                {
                    DropAnEgg();
                }
                else
                {
                    if (currentTime.Minute - oldTime.Minute >= 0.9f || Input.GetKeyDown(KeyCode.E)) // ????????? Check this later
                    {
                        DropAnEgg();
                    }
                }
            }
        }
    }

    //Instantiates an egg
    private void DropAnEgg()
    {
        eggDroped=true;
        oldTime = currentTime;

        // dropTrans = GameObject.FindGameObjectWithTag("Chicken").transform.position;
        dropTrans = this.gameObject.transform.GetChild(0).GetChild(0).gameObject.transform.position;
        dropTrans = new Vector3(dropTrans.x + 2, 0.5f, dropTrans.z + 2);

        GameObject newEgg = Instantiate(eggPrefab, dropTrans, transform.rotation);
        eggInfo=newEgg.GetComponent<EggInfo>();
        ChooseRandomEgg();
        currentEgg = newEgg;        

        //GetComponent<EggPickUp>().eggCol = newEgg.GetComponent<Collider>();
    }

    
    //First it decides the egg type (common, rare...), then choose a random egg from the array and sets the variables to the egg in the scene
    void ChooseRandomEgg()
    {
        //_rend = GetComponentInChildren<Renderer>();

        float rndEggValue = UnityEngine.Random.value;


        if (rndEggValue >= 0.4f)      //Common - 60% Drop rate
        {
            print("I dropped a common egg");
            eggInfo.type = EggInfo.EggType.Common;
        }
        else if (rndEggValue > 0.1f)        // Rare - 30% Drop rate
        {
            print("I dropped a rare egg");
            eggInfo.type = EggInfo.EggType.Rare;
        }
        else //Legendary - 10% drop rate
        {
            print("I dropped a legendary egg");
            eggInfo.type = EggInfo.EggType.Legendary;
        }



        if (eggInfo.type == EggInfo.EggType.Common)
        {
            int num = UnityEngine.Random.Range(0, CommonEggs.Length);
            eggInfo.eggMaterial = CommonEggs[num].eggMaterial;
            eggInfo.owned= CommonEggs[num].owned;
            eggInfo.eggNum = num;
        }
        else if (eggInfo.type == EggInfo.EggType.Rare)
        {
            int num = UnityEngine.Random.Range(0, RareEggs.Length);
            eggInfo.eggMaterial = RareEggs[num].eggMaterial;
            eggInfo.owned = RareEggs[num].owned;
            eggInfo.eggNum = num;
        }
        else
        {
            int num = UnityEngine.Random.Range(0, LegendaryEggs.Length);
            eggInfo.eggMaterial = LegendaryEggs[num].eggMaterial;
            eggInfo.owned = LegendaryEggs[num].owned;
            eggInfo.eggNum = num;
        }
    }
}