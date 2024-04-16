using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class MechanismBase : MonoBehaviour
{
    public abstract void Activate();

    public abstract void Deactivate();
}
