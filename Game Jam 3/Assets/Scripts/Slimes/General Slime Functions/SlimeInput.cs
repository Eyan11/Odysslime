using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeInput : MonoBehaviour
{
    private InputMap inputMap;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float moveBlockVertInput;

    //button down inputs
    private bool jumpInput;
    private bool lockCamInput;
    private bool possessInput;
    private bool returnToKingInput;
    private bool abilityInput;

    //button up input
    private bool unlockCamInput;

    private void Awake() {
        //create a new Input Map object and enable the King Slime input
        inputMap = new InputMap();
        inputMap.Slime.Enable();
    }

    private void Update() {

        //gets vector2 input from input map
        moveInput = inputMap.Slime.Move.ReadValue<Vector2>();
        lookInput = inputMap.Slime.Look.ReadValue<Vector2>();

        //gets int from input map (-1 is down, 0 is no movement, 1 is up)
        moveBlockVertInput = inputMap.Slime.MoveBlockVertically.ReadValue<float>();
        Debug.Log(moveBlockVertInput);

        //variables are true during the first frame the input is pressed
        jumpInput = inputMap.Slime.Jump.triggered;
        possessInput = inputMap.Slime.Possess.triggered;
        returnToKingInput = inputMap.Slime.ReturnToKing.triggered;
        abilityInput = inputMap.Slime.Ability.triggered;
        lockCamInput = inputMap.Slime.LockCamera.triggered;

        //this is true on first frame the input is released
        unlockCamInput = inputMap.Slime.UnlockCamera.triggered;
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
    public bool GetUnlockCamInput() {
        return unlockCamInput;
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


    public float GetMoveBlockVertInput() {
        return moveBlockVertInput;
    }

}
