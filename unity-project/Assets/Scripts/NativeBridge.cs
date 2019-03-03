using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NativeBridge : MonoBehaviour{


  public  SmoothCameraOrbit   camOrbit  = null;




  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     ZoomIn
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
  public void ZoomIn(string pZoomAmount){

    float zoom = float.Parse( pZoomAmount );

    camOrbit.Zoom( -zoom );

  }//END ZoomIn



  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     ZoomOut
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
  public void ZoomOut(string pZoomAmount){

    float zoom = float.Parse( pZoomAmount );

    camOrbit.Zoom( zoom );

  }//END ZoomOut

   
}//END class NativeBridge
