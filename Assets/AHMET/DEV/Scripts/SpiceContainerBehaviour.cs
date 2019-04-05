using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiceContainerBehaviour : MonoBehaviour
{
    public enum SpiceType { salt, blackPepper, chiliPepper, mayonnaise, mustard, ketchup }
    public SpiceType spiceType;
    public int amountPerShake;
    public ParticleSystem particleSystem;
    public SkinnedMeshRenderer skinnedMesh;
    public bool isFluid;
    public bool canAdd;
    public GameObject ingredientObj;
    public void UseIngredient(float amount)
    {

        if (!isFluid)
        {

            if (skinnedMesh.GetBlendShapeWeight(0) >= 10f)
            {
                Debug.Log(skinnedMesh.GetBlendShapeWeight(0));
                skinnedMesh.SetBlendShapeWeight(0, skinnedMesh.GetBlendShapeWeight(0) - amount);
            }
        }
      
    }
    private void FixedUpdate()
    {
       if (canAdd)
       {
               GameObject copy = Instantiate(ingredientObj, particleSystem.transform.position, Quaternion.identity);
               copy.GetComponent<SpiceBehaviour>().spiceContainer = GetComponent<SpiceContainerBehaviour>();
           canAdd = false;
       }
    }
    public void Add()
    {
        GetComponent<IngredientItem>().Action();
    }
    public void Particle()
    {
        StartCoroutine(ActivateParticle());
    }
    public IEnumerator ActivateParticle()
    {
        particleSystem.Play();
        yield return new WaitForSeconds(1f);
        if (particleSystem.isPlaying)
        {
            particleSystem.Stop();
        }
    }
    }
