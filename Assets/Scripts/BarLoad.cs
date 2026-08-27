using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarLoad : MonoBehaviour


{

    [SerializeField] private HingeJoint2D startJoint;
    [SerializeField] private HingeJoint2D endJoint;
    [SerializeField] private SpriteRenderer barSpriteRenderer;
    MaterialPropertyBlock propertyBlock;

    float startJointCurrentLoad = 0;
    float endJointCurrentLoad = 0;

    public void UpdateMaterial() 
    {
        if (startJoint != null) startJointCurrentLoad = startJoint.reactionForce.magnitude / startJoint.breakForce;
        if (endJoint != null) endJointCurrentLoad = endJoint.reactionForce.magnitude / endJoint.breakForce;
        float maxLoad = Mathf.Max(startJointCurrentLoad, endJointCurrentLoad);

        propertyBlock = new MaterialPropertyBlock();
        barSpriteRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_Load", maxLoad);
        barSpriteRenderer.SetPropertyBlock(propertyBlock);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 1) UpdateMaterial();
    }
}
