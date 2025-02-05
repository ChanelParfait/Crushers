using UnityEngine;
using System.Collections;

public class SlipperyEffect : MonoBehaviour
{
    private WheelCollider[] wheelColliders;
    private float effectTime = 5f; 
    private WheelFrictionCurve[] initialForwardFriction; 
    private WheelFrictionCurve[] initialSidewaysFriction; 

    public void Initialize(WheelCollider[] colliders)
    {
        wheelColliders = colliders;
        StoreInitialFrictionValues();
        ApplySlipperyEffect();
    }

    //Store the original values of wheel collider 
    private void StoreInitialFrictionValues()
    {
        if (wheelColliders != null && wheelColliders.Length > 0)
        {
            initialForwardFriction = new WheelFrictionCurve[wheelColliders.Length];
            initialSidewaysFriction = new WheelFrictionCurve[wheelColliders.Length];

            for (int i = 0; i < wheelColliders.Length; i++)
            {
                initialForwardFriction[i] = wheelColliders[i].forwardFriction;
                initialSidewaysFriction[i] = wheelColliders[i].sidewaysFriction;
            }
        }
    }


    //Apply the effect
    private void ApplySlipperyEffect()
    {
        if (wheelColliders != null && wheelColliders.Length > 0)
        {
            foreach (var wheelCollider in wheelColliders)
            {
                WheelFrictionCurve forwardFriction = wheelCollider.forwardFriction;
                WheelFrictionCurve sidewaysFriction = wheelCollider.sidewaysFriction;

                forwardFriction.stiffness = 0.1f; 
                sidewaysFriction.stiffness = 0.1f; 

                wheelCollider.forwardFriction = forwardFriction;
                wheelCollider.sidewaysFriction = sidewaysFriction;
            }
        }
    }

    //This method is called by the collider on exit to start restoring traction
    public void RestoreFriction() {
        StartCoroutine(ResetFrictionOverTime());
    }

    private IEnumerator ResetFrictionOverTime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < effectTime)
        {
            elapsedTime += Time.deltaTime;

            if (wheelColliders != null && wheelColliders.Length > 0)
            {
                for (int i = 0; i < wheelColliders.Length; i++)
                {
                    WheelFrictionCurve forwardFriction = wheelColliders[i].forwardFriction;
                    WheelFrictionCurve sidewaysFriction = wheelColliders[i].sidewaysFriction;

                    forwardFriction.stiffness = Mathf.Lerp(0.1f, initialForwardFriction[i].stiffness, elapsedTime / effectTime);
                    sidewaysFriction.stiffness = Mathf.Lerp(0.1f, initialSidewaysFriction[i].stiffness, elapsedTime / effectTime);

                    wheelColliders[i].forwardFriction = forwardFriction;
                    wheelColliders[i].sidewaysFriction = sidewaysFriction;
                }
            }

            yield return null;
        }
        if (wheelColliders != null && wheelColliders.Length > 0)
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                wheelColliders[i].forwardFriction = initialForwardFriction[i];
                wheelColliders[i].sidewaysFriction = initialSidewaysFriction[i];
            }
        }
        Destroy(this);
    }
}