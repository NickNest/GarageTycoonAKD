using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Transform _holdPos;
    [SerializeField] private float _pickUpRange = 5f; 

    private GameObject _heldObj;
    private Rigidbody _heldObjRb; 
    private int _layerNumber; 

    void Start()
    {
        _layerNumber = LayerMask.NameToLayer("holdLayer");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
        {
            if (_heldObj == null) 
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, _pickUpRange))
                {
                    if (hit.transform.gameObject.tag == "canPickUp")
                    {
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                    StopClipping(); 
                    DropObject();
            }
        }
    }
    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) 
        {
            _heldObj = pickUpObj;
            _heldObjRb = pickUpObj.GetComponent<Rigidbody>();
            _heldObjRb.isKinematic = true;
            _heldObjRb.transform.parent = _holdPos.transform;
            _heldObj.layer = _layerNumber;
            Physics.IgnoreCollision(_heldObj.GetComponent<Collider>(), _player.GetComponent<Collider>(), true);
        }
    }
    void DropObject()
    {
        Physics.IgnoreCollision(_heldObj.GetComponent<Collider>(), _player.GetComponent<Collider>(), false);
        _heldObj.layer = 0;
        _heldObjRb.isKinematic = false;
        _heldObj.transform.parent = null;
        _heldObj = null;
    }
    void MoveObject()
    {
        _heldObj.transform.position = _holdPos.transform.position;
    }
    void StopClipping()
    {
        var clipRange = Vector3.Distance(_heldObj.transform.position, transform.position);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        if (hits.Length > 1)
        {
            _heldObj.transform.position += Vector3.up * 0.1f;
        }
    }
}
