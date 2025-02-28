using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class SpineSlotHide : StateMachineBehaviour
{
    public string slotName;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SkeletonMecanim skeletonMecanim = animator.GetComponent<SkeletonMecanim>();

        if(skeletonMecanim != null)
        {
            var skeleton = skeletonMecanim.Skeleton;
            var slot = skeleton.FindSlot(slotName);

            if(slot != null)
            {
                Debug.Log("Áö¿ò");
                slot.Attachment = null;
            }
        }
    }
}
