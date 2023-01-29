using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [Header("Keyboard")]
    [SerializeField] float normalSpeed;
    [SerializeField] float fastSpeed;
    [SerializeField] float movementTime;
    [SerializeField] float rotationAmount;
    [SerializeField] Vector3 zoomAmount;

    [Header("Mouse")]
    [SerializeField] float panBorderThickness;

    [Header("Limits")]
    [SerializeField] Vector2 panLimit;
    [SerializeField] float panZoomLimitH;
    [SerializeField] float panZoomLimitL;

    [Header("Selection")]
    [SerializeField] RectTransform SelectionBox;
    [SerializeField] LayerMask SelectableLayers;
    [SerializeField] LayerMask FloorLayers;

    [Header("UI Overlay")]
    [SerializeField] GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    [SerializeField] EventSystem eventSystem;

    PlayerController _playerController;
    Transform _cameraTransform;
    float _movementSpeed;
    Vector3 _newPosition;
    Vector3 _direction;
    Quaternion _newRotation;
    Vector2 _rotation;
    Vector3 _newZoom;
    Vector3 _zoom;
    Vector3 _dragStartPosition;
    Vector3 _dragCurrentPosition;
    Camera _mainCamera;
    bool _clicked;
    bool _movingByMouse;
    Vector2 startMousePosition;
    bool _dragging;
    List<Selectable> castedSelectables;



    private void Start()
    {
        _newPosition = transform.position;
        _movementSpeed = normalSpeed;
        _newRotation = transform.rotation;
        _clicked = false;
        _mainCamera = Camera.main;
        _newZoom = _mainCamera.transform.localPosition;
        _cameraTransform = _mainCamera.transform;
        _playerController = GetComponent<PlayerController>();
        _dragging = false;
        SelectionBox.gameObject.SetActive(false);
        castedSelectables = new List<Selectable>();
    }


    private void Update()
    {
        HandleMouse();
        HandleDragging();
        HandleMovement();
    }

    private void HandleDragging()
    {
        if(_dragging)
        {
            float widthDelta = Input.mousePosition.x - startMousePosition.x;
            float heightDelta = Input.mousePosition.y - startMousePosition.y;

            SelectionBox.anchoredPosition = startMousePosition + new Vector2(widthDelta / 2, heightDelta / 2);
            SelectionBox.sizeDelta = new Vector2(Mathf.Abs(widthDelta), Mathf.Abs(heightDelta));

            Bounds bounds = new Bounds(SelectionBox.anchoredPosition, SelectionBox.sizeDelta);

            
            for (int i = 0; i < _playerController.AllSelectables.Count; i++)
            {
                if (UnitIsInsideBounds(_mainCamera.WorldToScreenPoint(_playerController.AllSelectables[i].transform.position), bounds))
                {
                    if(_playerController.AllSelectables[i].FactionType == _playerController.Faction && _playerController.AllSelectables[i].SelectableType == ESelectableType.Unit && !castedSelectables.Contains(_playerController.AllSelectables[i]))
                        castedSelectables.Add(_playerController.AllSelectables[i]);
                }
                else
                {
                    castedSelectables.Remove(_playerController.AllSelectables[i]);
                }
            }
        }
    }

    private bool UnitIsInsideBounds(Vector2 position, Bounds bounds)
    {
        return position.x > bounds.min.x && position.x < bounds.max.x && position.y > bounds.min.y && position.y < bounds.max.y;
    }
    

    public void LeftClick(bool clicked)
    {
        if(clicked)
        {
            if (OverUIElement()) return;

            _dragging = true;

            SelectionBox.sizeDelta = Vector2.zero;
            SelectionBox.gameObject.SetActive(true);
            startMousePosition = Input.mousePosition;
        }
        else
        {
            _dragging = false;
            SelectionBox.sizeDelta = Vector2.zero;
            SelectionBox.gameObject.SetActive(false);
            if (OverUIElement()) return;
            if (castedSelectables.Count > 0)
            {
                _playerController.Selection(castedSelectables);
                castedSelectables.Clear();
            }
            else
            {
                Ray ray = GetRayMousePosition();
                
                if (Physics.Raycast(ray, out RaycastHit raycastHit))
                {
                    if (raycastHit.collider != null)
                    {
                        Selectable selectable = raycastHit.collider.gameObject.GetComponent<Selectable>();
                        if (selectable != null && selectable.FactionType == _playerController.Faction)
                        {
                            _playerController.Selection(new List<Selectable>() { selectable });
                        }
                        else
                            _playerController.Selection(null);
                    }
                }
            }

        }
    }

    public Ray GetRayMousePosition()
    {
        return _mainCamera.ScreenPointToRay(Input.mousePosition);
    }

    public Vector3 GetMousePosition(out RaycastHit[] hits)
    {
        Plane plane = new(Vector3.up, Vector3.zero);
        Ray ray = GetRayMousePosition();
        hits = null;
        if (plane.Raycast(ray, out float entry))
        {
            hits = new RaycastHit[5];
            Physics.RaycastNonAlloc(ray, hits);
            return ray.GetPoint(entry);
        }
        return Vector3.zero;
    }

    void HandleMouse()
    {
        if(_clicked)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = GetRayMousePosition();
            if (plane.Raycast(ray, out float entry))
            {
                _dragCurrentPosition = ray.GetPoint(entry);

                _newPosition = transform.position + _dragStartPosition - _dragCurrentPosition;
            }
        }

        //if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        //{
        //    _direction += transform.forward;
        //    _movingByMouse = true;
        //}
        //else if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        //{
        //    _direction += transform.right;
        //    _movingByMouse = true;
        //}
        //else if (Input.mousePosition.y <= panBorderThickness)
        //{
        //    _direction += -transform.forward;
        //    _movingByMouse = true;
        //}
        //else if (Input.mousePosition.x <= panBorderThickness)
        //{
        //    _direction += -transform.right;
        //    _movingByMouse = true;
        //}
        //else if (_movingByMouse)
        //{
        //    _movingByMouse = false;
        //    _direction = Vector3.zero;
        //}
        _direction = _direction.normalized;
    }

    void HandleMovement()
    {
        if (_direction != Vector3.zero)
           _newPosition += _movementSpeed * _direction;

        if(_rotation != Vector2.zero)
            _newRotation *= Quaternion.Euler(_rotation * rotationAmount);

        if (_zoom != Vector3.zero)
            _newZoom += _zoom;

        _newPosition.x = Mathf.Clamp(_newPosition.x, -panLimit.x, panLimit.x);
        _newPosition.z = Mathf.Clamp(_newPosition.z, -panLimit.y, panLimit.y);

        _newZoom.y = Mathf.Clamp(_newZoom.y, -panZoomLimitL, panZoomLimitH);
        _newZoom.z = Mathf.Clamp(_newZoom.z, -panZoomLimitH, panZoomLimitL);

        transform.SetPositionAndRotation(
            Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * movementTime), 
            Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * movementTime));

        _cameraTransform.localPosition = Vector3.Lerp(_cameraTransform.localPosition, _newZoom, Time.deltaTime * movementTime);
    }

    public void SetDirection(Vector3 direction)
    {
        if (direction.x > 0)
            _direction = transform.right;
        else if (direction.x < 0)
            _direction = transform.right * -1;
        else if (direction.y > 0)
            _direction = transform.forward;
        else if (direction.y < 0)
            _direction = transform.forward * -1;
        else
            _direction = Vector3.zero;
    }

    public void SetFast(Vector2 speedVector)
    {
        if (speedVector.x > 0 || speedVector.y > 0)
            _movementSpeed = fastSpeed;
        else
            _movementSpeed = normalSpeed;
    }

    public void SetRotation(Vector2 rotation)
    {
        if (rotation.x > 0)
            _rotation = Vector2.up;
        else if (rotation.y > 0)
            _rotation = -Vector2.up;
        else
            _rotation = Vector2.zero;
    }

    public void SetZoom(Vector2 zoom)
    {
        if (zoom.x > 0)
            _zoom = zoomAmount;
        else if (zoom.y > 0)
            _zoom = -zoomAmount;
        else
            _zoom = Vector3.zero;
    }

    public void RightClicked(bool clicked)
    {
        if (clicked)
        {
            _clicked = true;
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = GetRayMousePosition();
            if(plane.Raycast(ray, out float entry))
            {
                _dragStartPosition = ray.GetPoint(entry);
            }
        }
        else
            _clicked = false;
    }


    public void DoubleClick()
    {
        Ray ray = GetRayMousePosition();
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            Selectable selectable = raycastHit.collider.gameObject.GetComponent<Selectable>();
            if (selectable != null && selectable.FactionType == _playerController.Faction && selectable.SelectableType == ESelectableType.Unit)
            {
                ESelectableType type = selectable.SelectableType;
                List<Selectable> selectables = _playerController.AllSelectables.Where(x => x.SelectableType == type).ToList();

                Bounds bounds = new(_mainCamera.ScreenToWorldPoint(Vector3.zero), new Vector3(Screen.width / 2, Screen.height / 2));
                for (int i = 0; i < selectables.Count; i++)
                {
                    if (UnitIsInsideBounds(selectables[i].transform.position, bounds))
                    {
                        castedSelectables.Add(selectables[i]);
                    }
                }

                _playerController.Selection(castedSelectables);
            }
        }
        
    }

    public void RightClickedPerfomed()
    {
        // TODO: se con il ray cast becco un nemico, allora attacca, se becco una struttura allora boh, altrimenti move
        RaycastHit[] hits;
        Vector3 mousePosition = GetMousePosition(out hits);

        //if (hits.Any(x => x.collider.gameObject.GetComponent<Human>()))
        //    _playerController.SetDestinationAndAttack(mousePosition, hits.First(x => x.collider.gameObject.GetComponent<Human>()).collider.gameObject);
        //else
            _playerController.SetUnitsDestination(mousePosition);
    }

    private bool OverUIElement()
    {
        pointerEventData = new(eventSystem)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new();

        raycaster.Raycast(pointerEventData, results);

        return results.Count > 0;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 a = new Vector3(panLimit.x, 0, panLimit.y);
        Vector3 b = new Vector3(-panLimit.x, 0, -panLimit.y);
        Vector3 c = new Vector3(panLimit.x, 0, -panLimit.y);
        Vector3 d = new Vector3(-panLimit.x, 0, panLimit.y);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(a, 2);
        Gizmos.DrawSphere(b, 2);
        Gizmos.DrawSphere(c, 2);
        Gizmos.DrawSphere(d, 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(a, c);
        Gizmos.DrawLine(a, d);
        Gizmos.DrawLine(c, b);
        Gizmos.DrawLine(d, b);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position, 5);
    }


#endif
}
