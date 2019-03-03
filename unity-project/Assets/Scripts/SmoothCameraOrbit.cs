//
// based on: http://www.unifycommunity.com/wiki/index.php?title=MouseOrbitZoom
//

using UnityEngine;
using System.Collections;


public class SmoothCameraOrbit : MonoBehaviour{


  public  Transform   target          = null;
  public  Vector3     targetOffset    = Vector3.zero;
  public  float       distance        = 5.0f;
  public  float       startDistance   = 28f;
  public  float       maxDistance     = 20;
  public  float       minDistance     = 0.6f;
  public  float       xSpeed          = 200.0f;
  public  float       ySpeed          = 200.0f;
  public  int         yMinLimit       = -90;
  public  int         yMaxLimit       = 90;
  public  float       zoomDampening   = 5.0f;
	public  float       autoRotate      = 1;

  private float       xDeg            = 0.0f;
  private float       yDeg            = 0.0f;
  private float       currentDistance = 0.0f;
	private float       idleTimer       = 0.0f;
	private float       idleSmooth      = 0.0f;
  private Quaternion  currentRotation;
  private Quaternion  desiredRotation;
  private Quaternion  rotation;
  private Vector3     position;



  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     Start
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
  public void Start(){

    //If there is no target, create a temporary target at 'distance' from the cameras current viewpoint
    if ( !target ){

      GameObject go = new GameObject("Cam Target");
      go.transform.position = transform.position + (transform.forward * distance);
      target = go.transform;
    
    }//END if

    currentDistance = distance;
          
    //be sure to grab the current rotations as starting points.
    position = transform.position;
    rotation = transform.rotation;
    currentRotation = transform.rotation;
    desiredRotation = transform.rotation;

    xDeg = Vector3.Angle(Vector3.right, transform.right );
    yDeg = Vector3.Angle(Vector3.up, transform.up );
    position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
  
  }//END Init



  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     LateUpdate
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/     
  void LateUpdate(){

    // If middle mouse and left alt are selected? ORBIT
    if ( Input.GetMouseButton(0) ){

      if ( Input.touchCount > 0 ){

        Touch touch = Input.GetTouch(0);

        xDeg += touch.deltaPosition.x * xSpeed * 0.002f;
        yDeg -= touch.deltaPosition.y * ySpeed * 0.002f;

      } else {

        xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

      }//END if/else


      // Clamp the vertical axis for the orbit
      yDeg = ClampAngle( yDeg, yMinLimit, yMaxLimit );

      // Update camera rotation
      desiredRotation     = Quaternion.Euler( yDeg, xDeg, 0 );
      currentRotation     = transform.rotation;
      rotation            = Quaternion.Lerp( currentRotation, desiredRotation, Time.deltaTime * zoomDampening );
      transform.rotation  = rotation;
      
      // Reset idle timers
      idleTimer   = 0;
      idleSmooth  = 0;
      
    }else{

      // Smooth idle rotation
      //
      idleTimer += Time.deltaTime;
      if ( idleTimer > autoRotate && autoRotate > 0 ) {

        idleSmooth  += (Time.deltaTime + idleSmooth) * 0.005f;
        idleSmooth   = Mathf.Clamp( idleSmooth, 0, 1 );
        xDeg        += xSpeed * 0.001f * idleSmooth;
      
      }//END if

      yDeg                = ClampAngle( yDeg, yMinLimit, yMaxLimit );
      desiredRotation     = Quaternion.Euler( yDeg, xDeg, 0 );
      currentRotation     = transform.rotation;
      rotation            = Quaternion.Lerp( currentRotation, desiredRotation, Time.deltaTime * zoomDampening * 2 );
      transform.rotation  = rotation;

    }//END if/else

    //Orbit Position
    position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
    transform.position = position;

  }//END LateUpdate



  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     Zoom
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
  public void Zoom(float pZoomAmount){

    distance += pZoomAmount;

    if ( distance > maxDistance )
      distance = maxDistance;

    if ( distance < minDistance )
      distance = minDistance;

    currentDistance = distance;

  }//END Zoom



  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
  /// 
  ///     ClampAngle
  ///------------------------------------------
  /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/ 
  private static float ClampAngle(float angle, float min, float max){

    if ( angle < -360 )
      angle += 360;

    if ( angle > 360 )
      angle -= 360;

    return Mathf.Clamp( angle, min, max );
  
  }//END ClampAngle

}//END class SmoothCameraOrbit
