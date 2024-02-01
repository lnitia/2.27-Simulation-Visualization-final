using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorValueController : MonoBehaviour
{
    [SerializeField] private GameObject Conditions;
    [SerializeField] private GameObject ColorValue;
    
    void Update()
    {
        bool anyActive = CheckIfAnyActive(Conditions);
        
        ColorValue.SetActive(anyActive);
    }
    
    bool CheckIfAnyActive(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.activeSelf)
            {
                return true;
            }
        }
        
        return false;
    }
}
