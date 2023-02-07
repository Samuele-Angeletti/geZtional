using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    #region SINGLETON
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance != null)
                    return instance;

                GameObject go = new GameObject("GameManager");
                return go.AddComponent<GameManager>();
            }
            else
                return instance;
        }
        set
        {
            instance = value;
        }
    }
    #endregion

    [SerializeField] CameraController controller;

    
    public InputControls inputActions;
    public CameraController CameraController => controller;
    public PlayerController PlayerController;

    public List<Settings> Settings;

    public Settings CurrentSetting;

    private void Awake()
    {
        inputActions = new InputControls();
        inputActions.Player.Enable();

        inputActions.Player.CameraMovesWASD.performed += CameraMovesPerformed;
        inputActions.Player.CameraMovesARROWS.performed += CameraMovesPerformed;
        inputActions.Player.CameraRotation.started += CameraRotationStarted;
        inputActions.Player.CameraRotation.canceled += CameraRotationCanceled;
        inputActions.Player.Fast.started += FastStarted;
        inputActions.Player.Fast.canceled += FastCanceled;
        inputActions.Player.CameraZoom.started += CameraZoomStarted;
        inputActions.Player.CameraZoom.canceled += CameraZoomCanceled;
        inputActions.Player.MouseRight.started += MouseRightStarted;
        inputActions.Player.MouseRight.canceled += MouseRightCanceled;
        inputActions.Player.MouseLeft.started += MouseLeftStarted;
        inputActions.Player.MouseLeft.canceled += MouseLeftCanceled;
        inputActions.Player.DoubleLeftMouseClick.performed += DoubleLeftMouseClickPerformed;
        inputActions.Player.MouseRight.performed += MouseRightPerformed;
    }

    public Settings SetInitialSetting(EDifficultMode difficultMode, EFactionType factionType)
    {
        CurrentSetting = Settings.Find(x => x.User == factionType && x.difficultMode == difficultMode);
        PlayerController.Faction = factionType;
        UIManager.Instance.Faction = factionType;
        GlobalResourcesManager.Instance.SetInitialResources(CurrentSetting);
        return CurrentSetting;
    }

    private void MouseRightPerformed(InputAction.CallbackContext obj)
    {
        controller.RightClickedPerfomed();
    }

    private void DoubleLeftMouseClickPerformed(InputAction.CallbackContext obj)
    {
        controller.DoubleClick();
    }

    private void MouseLeftCanceled(InputAction.CallbackContext obj)
    {
        controller.LeftClick(false);
    }

    private void MouseLeftStarted(InputAction.CallbackContext obj)
    {
        controller.LeftClick(true);
    }

    private void MouseRightCanceled(InputAction.CallbackContext obj)
    {
        controller.RightClicked(false);
    }

    private void MouseRightStarted(InputAction.CallbackContext obj)
    {
        controller.RightClicked(true);
    }

    private void CameraZoomCanceled(InputAction.CallbackContext obj)
    {
        controller.SetZoom(Vector2.zero);
    }

    private void CameraZoomStarted(InputAction.CallbackContext obj)
    {
        controller.SetZoom(obj.ReadValue<Vector2>());
    }

    private void CameraRotationCanceled(InputAction.CallbackContext obj)
    {
        controller.SetRotation(Vector2.zero);
    }

    private void CameraRotationStarted(InputAction.CallbackContext obj)
    {
        controller.SetRotation(obj.ReadValue<Vector2>());
    }

    private void FastCanceled(InputAction.CallbackContext obj)
    {
        controller.SetFast(Vector2.zero);
    }

    private void FastStarted(InputAction.CallbackContext obj)
    {
        controller.SetFast(obj.ReadValue<Vector2>());
    }

    private void CameraMovesPerformed(InputAction.CallbackContext obj)
    {
        controller.SetDirection(obj.ReadValue<Vector3>());
    }
}
[Serializable]
public class Settings
{
    public EDifficultMode difficultMode;
    public EFactionType User;
    public EFactionType AI;

    [Header("Zombies")]
    public float StartVirus;
    public int MaxZombies;
    public int StartZombies;

    [Header("Humans")]
    public float StartGold;
    public int MaxHumans;
    public int StartHumans;
}

public enum EDifficultMode
{
    Easy,
    Medium,
    Hard,
    Thriller
}