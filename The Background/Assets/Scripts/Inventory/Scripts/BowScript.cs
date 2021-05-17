using CMF;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BowScript : MonoBehaviour
{
    [Header("Bow Stats")]
    float _charge;
    public Animator animatorBow;
    public float chargeMax;
    public float chargeRate;
    public KeyCode FireButton;
    public bool UseAirTime;
    private Mover moverPlayer;
    [Range(0, 1)]
    public float TimeSlomo;
    [Tooltip("time it takes to go from normal to slomo")]
    public float TimeToSlomo;

    [Space]

    [Header("Spawning Arrow")]
    public Transform SpawnNormal;
    public Rigidbody arrowObjNormal;
    public GameObject arrowObj_ModelNormal;

    [Space]

    [Header("fire Arrow")]
    public Rigidbody arrowObjFire;
    public GameObject arrowObj_ModelFire;

    [Space]

    private bool loaded;
    public LayerMask Hittable;
    public int SelectedArrow = 0;

    private void Start()
    {
        moverPlayer = GetComponentInParent<Mover>();
        Physics.GetIgnoreLayerCollision(10, 10);
    }
    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            if (hit.distance > 3 && loaded)
            {
                UseAirTime = true;
            }
            else { UseAirTime = false; }
        }
            
        if (UseAirTime)
        {
            Time.timeScale = Mathf.Lerp(1, TimeSlomo, TimeToSlomo);
            
        }
        else { Time.timeScale = 1; }
        int previousSelectedWeapon = SelectedArrow;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (SelectedArrow >= transform.childCount - 1)
            {
                SelectedArrow = 0;
            }
            else
            {
                SelectedArrow++;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (SelectedArrow <= 0)
            {
                SelectedArrow = transform.childCount - 1;
            }
            else
            {
                SelectedArrow--;
            }
        }


        if (Input.GetKeyDown(FireButton) && !loaded)
        {
            if(SelectedArrow == 0)
            {
                Instantiate(arrowObj_ModelNormal, SpawnNormal.position, SpawnNormal.rotation, this.GetComponentInChildren<Transform>().GetChild(0));
            }
            if (SelectedArrow == 1)
            {
                Instantiate(arrowObj_ModelFire, SpawnNormal.position, SpawnNormal.rotation, this.GetComponentInChildren<Transform>().GetChild(0));
            }
        }
        if (Input.GetKey(FireButton) && _charge < chargeMax)
        {
            
            _charge += Time.deltaTime * chargeRate;
            //Debug.Log(_charge.ToString());
            loaded = true;
        }
        if (Input.GetKeyUp(FireButton) && loaded)
        {
            if (SelectedArrow == 0)
            {
                Destroy(this.GetComponentInChildren<Transform>().GetChild(0).GetChild(0).gameObject);
                loaded = false;
                Rigidbody arrow = Instantiate(arrowObjNormal, SpawnNormal.position, SpawnNormal.rotation) as Rigidbody;
                arrow.AddForce(SpawnNormal.forward * (_charge + arrow.velocity.magnitude) , ForceMode.Impulse);
            }
            if (SelectedArrow == 1)
            {
                Destroy(this.GetComponentInChildren<Transform>().GetChild(0).GetChild(0).gameObject);
                loaded = false;
                Rigidbody arrow = Instantiate(arrowObjFire, SpawnNormal.position, SpawnNormal.rotation) as Rigidbody;
                arrow.AddForce(SpawnNormal.forward * (_charge + arrow.velocity.magnitude), ForceMode.Impulse);
            }
            _charge = 0;
            
        }
        animatorBow.SetFloat("Charge", _charge);


        
    }

}
