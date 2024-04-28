using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{

    [SerializeField] private LevelSelectManager levelScript;

    //displays the information of the current selected island
    //  this method is called on the last from of the scroll unroll animation
    public void DisplayScrollInfo() {
        levelScript.OpenScrollInfo();
    }
}
