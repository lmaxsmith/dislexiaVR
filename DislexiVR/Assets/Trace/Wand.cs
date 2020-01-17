using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour
{
    [Tooltip("The diameter of the larges collision sphere (in meters)")]
    public float roughPrecisionScale = .3f;

    //Collider roughCollider;
    //Collider midCollider;
    //Collider fineCollider;

    Collider[] castingColliders = new Collider[5];

    public bool isCasting;

    // Start is called before the first frame update
    void Start()
    {
        //roughCollider = SetupColliders(roughPrecisionScale);
        //midCollider = SetupColliders(roughPrecisionScale * .6f);
        //fineCollider = SetupColliders(roughPrecisionScale * .3f);

        //create each collider at varying sizes
        for (int i = 0; i < castingColliders.Length; i++)
        {
            castingColliders[i] = SetupColliders(roughPrecisionScale / (castingColliders.Length - i));
        }
    }

    Collider SetupColliders(float radius)
    {
        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.radius = radius;
        collider.isTrigger = true;

        return collider;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
