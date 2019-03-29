using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutController : MonoBehaviour
{
    public static List<MeshFilter> meshFilters = new List<MeshFilter>();
    public static int meshFilterCount = 0; 
    public GameObject victim;
    public Material mat;
    public Transform donerParent;
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Slicable")
        {
            MeshCut.Cut(other.gameObject, transform.position, -transform.up, mat, donerParent);
        }
    }
}
