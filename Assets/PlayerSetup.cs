using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField]
    private Behaviour[] ComponentDisable;

    [SerializeField]
    private GameObject playerUIPrefab;

    [HideInInspector]
    public GameObject playerUI;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void DisableComponents()
    {
        foreach(var i in ComponentDisable)
        {
            i.enabled = false;
        }
        //Afficher "En entente de l'autre joueur"
    }

    public void EnableComponents()
    {
        foreach (var i in ComponentDisable)
        {
            i.enabled = true;
        }
        //Enlever "En entente de l'autre joueur"
    }
}
