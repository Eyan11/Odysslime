using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeInput : MonoBehaviour
{
    private InputMap inputMap;

    private Vector2 moveInput;
    private Vector2 lookInput;
    //private int moveBlockVertInput;

    //button hold inputs
    private bool lockCamInput;

    //button down inputs
    private bool jumpInput;
    private bool possessInput;
    private bool returnToKingInput;
    private bool abilityInput;

    private void Awake() {
        //create a new Input Map object and enable the King Slime input
        inputMap = new InputMap();
        inputMap.Slime.Enable();
    }

    private void Update() {

        //gets vector2 input from input map
        moveInput = inputMap.Slime.Move.ReadValue<Vector2>();
        lookInput = inputMap.Slime.Look.ReadValue<Vector2>();

        //float value is 1 when holding and 0 when not holding
        //need to do this to tell if a button is being HELD DOWN
        lockCamInput = inputMap.Slime.LockCamera.ReadValue<float>() > 0.1;

        //variables are true during the first frame the input is pressed
        jumpInput = inputMap.Slime.Jump.triggered;
        possessInput = inputMap.Slime.Possess.triggered;
        returnToKingInput = inputMap.Slime.ReturnToKing.triggered;
        abilityInput = inputMap.Slime.Ability.triggered;
    }

    //---------- Methods to return input to other slime scripts ----------\\

    public Vector2 GetMoveInput() {
        return moveInput;
    }

    public Vector2 GetLookInput() {
        return lookInput;
    }
    
    public bool GetLockCamInput() {
        return lockCamInput;
    }

    public bool GetJumpInput() {
        return jumpInput;
    }

    public bool GetPossessInput() {
        return possessInput;
    }

    public bool GetReturnToKingInput() {
        return returnToKingInput;
    }

    public bool GetAbilityInput() {
        return abilityInput;
    }
}
