using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class InteractArea : MonoBehaviour
{
    public List<Iinteractive> Interactives { get; } = new();

    public void OnTriggerEnter(Collider other)
    {
        var interactive = other.GetComponent<Iinteractive>();
        if (interactive != null){
            Interactives.Add(interactive);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        var interactive = other.GetComponent<Iinteractive>();
        if (interactive != null && Interactives.Contains(interactive))
        {
            Interactives.Remove(interactive);
        }
    }
}
