using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitBox : MonoBehaviour
{
    // will allow us to access the box collider per attack
    BoxCollider hitBox;

    [SerializeField] GameObject particleHit;
  
    void Start()
    {
        hitBox = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(particleHit, transform.position, transform.rotation);
        
    }

    public void EnableHitBox()
    {
        hitBox.enabled = true;
    }
    public void DisableHitBox()
    {
        hitBox.enabled = false;
    }
}
