using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;
using UnityEngine;

public class SpatialAwarenessController : MonoBehaviour
{
    private IReadOnlyList<IMixedRealitySpatialAwarenessMeshObserver> observers;
    
    private void Start()
    {
        IMixedRealityDataProviderAccess dataProviderAccess = CoreServices.SpatialAwarenessSystem as IMixedRealityDataProviderAccess;
        if (dataProviderAccess != null)
        {
            observers = dataProviderAccess.GetDataProviders<IMixedRealitySpatialAwarenessMeshObserver>();
        }
    }

    public void NonVisible()
    {
        // Get the first Mesh Observer available, generally we have only one registered
        //var observer = CoreServices.GetSpatialAwarenessSystemDataProvider<IMixedRealitySpatialAwarenessMeshObserver>();
        foreach (IMixedRealitySpatialAwarenessMeshObserver observer in observers)
        {
            observer.DisplayOption = SpatialAwarenessMeshDisplayOptions.None;
        }
    } 
    
    public void Visible()
    {
        foreach (IMixedRealitySpatialAwarenessMeshObserver observer in observers)
        {
            observer.DisplayOption = SpatialAwarenessMeshDisplayOptions.Visible;
        }
    }
    
    public void Occlusion()
    {
        foreach (IMixedRealitySpatialAwarenessMeshObserver observer in observers)
        {
            observer.DisplayOption = SpatialAwarenessMeshDisplayOptions.Occlusion;
        }
    }
}
