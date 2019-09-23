﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenController : MonoBehaviour
{
    public float planeX, planeZ;
    int currWalkPoint;
    public float movementSpeed = 5f;
    public GameObject[] walkingPoints;
    public GameObject foodBowl;

    Vector3 target;
    StatusMenu status;
    public bool canMove = true, isLifted = false;


    void Start()
    {
        status = GetComponent<StatusMenu>();

        target = newWalkingpoint();

    }

    void Update()
    {
        if(status.currState == StatusMenu.State.Normal && !isLifted)
        {
            StartCoroutine(movingPoint());
        }
        LiftChicken();
    }

    public IEnumerator movingPoint()
    {
        
        Vector3 moveDir = target - transform.position;

        if(canMove)
        {
            if(Vector3.Distance(transform.position, target) < 0.1f)
            {

                canMove = false;
                float t = Random.Range(1, 10);
                yield return new WaitForSeconds(t);
                target = newWalkingpoint();
                canMove = true;
            }

            transform.position += moveDir * 3 * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target - transform.position), 7f * Time.deltaTime);

        }

    }

    void LiftChicken()
    {
        // Ray ray = new Ray();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Collider col = this.gameObject.GetComponent<Collider>();

        if(Physics.Raycast(ray, out hit))
        {
            if(Input.GetMouseButtonDown(0)  && hit.collider == col)
            {
                isLifted = true;
                
            }
            if(Input.GetMouseButtonUp(0))
            {
                isLifted = false;
                target= new Vector3(transform.position.x, 0.1f, transform.position.z);

                transform.position = target;
            }
            if(isLifted)
            {
                
                    Vector3 moveDir = new Vector3(hit.point.x, 1f, hit.point.z);
                    // Vector3 moveDir = hit.point;

                    // print (moveDir);

                    transform.position = moveDir;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Camera.main.transform.position - transform.position), 7f * Time.deltaTime);

                    
            }
        }
        

    }
    public Vector3 newWalkingpoint()
    {
        // print("New target");
        float xPos = transform.position.x;
        float newX = Random.Range(xPos - 1, xPos + 1);
        if(newX > 2) newX = Random.Range(0.0f, 2.0f);
        if(newX < -2) newX = Random.Range(0.0f, -2.0f);

        float zPos = transform.position.z;
        float newZ = Random.Range(zPos - 2, zPos + 2);
        if(newZ > 2) newZ = 2;
        if(newZ < -2) newZ = -2;

        Vector3 newTarget = new Vector3(newX, 0.1f, newZ);

        return newTarget;
    }
    public void GettingFood()
    {
        Vector3 moveDir = foodBowl.transform.position - transform.position;
        if(moveDir.magnitude > 1)
        {
            transform.position += moveDir * 2 * Time.deltaTime;

            transform.rotation = Quaternion.LookRotation(foodBowl.transform.position);

        }
        if(foodBowl.GetComponent<FoodBowl>().avaliableFood > 0 &&  Vector3.Distance(transform.position, foodBowl.transform.position) < 1f)
        {
            status.hunger ++;
            foodBowl.GetComponent<FoodBowl>().avaliableFood --;
        }
       
    }

}
