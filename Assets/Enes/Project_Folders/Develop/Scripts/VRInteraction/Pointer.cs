using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
{
    public float m_DefaultLenght = 5.0f;
    public GameObject laser;
    public VRInputModule m_InputModule;
    private LineRenderer m_LineRenderer = null;
    private void Awake()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    
    private void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        PointerEventData data = m_InputModule.GetData();

        if (VRInputModule.m_CurrentObject&& VRInputModule.m_CurrentObject.layer==5)
        {
            Debug.Log(VRInputModule.m_CurrentObject.layer);
            float targetLenght = data.pointerCurrentRaycast.distance == 0 ? m_DefaultLenght : data.pointerCurrentRaycast.distance;
            RaycastHit hit = CreateRaycast(targetLenght);
            Vector3 endPosition = transform.position + (transform.forward * targetLenght);

            if (hit.collider != null)
            {
                endPosition = hit.point;
            }

            InUI();
            laser.transform.position = endPosition;
            m_LineRenderer.SetPosition(0, transform.position);
            m_LineRenderer.SetPosition(1, endPosition);
        }
        else
        {
           OutUI();
        }

        
    }
    private  RaycastHit CreateRaycast(float lenght)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position,transform.forward);
        Physics.Raycast(ray,out hit,m_DefaultLenght,LayerMask.GetMask("UI"));
        return hit;

    }

    void InUI()
    {
        m_LineRenderer.enabled = true;
        laser.SetActive(true);
    }
    void OutUI()
    {
        m_LineRenderer.enabled = false;
        laser.SetActive(false);
    }
}
