using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickUpObjectType
{
    None,
    Key,
    Trampoline
}

[CreateAssetMenu(fileName = "PickUpData", menuName = "Object Data", order = 1)]
public class PickUpObject : ScriptableObject
{
    public PickUpObjectType type;
    public GameObject prefabObject;
    public GameObject pick2DObject;
}
