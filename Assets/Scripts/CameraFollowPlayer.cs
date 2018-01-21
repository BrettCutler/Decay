using UnityEngine;
using System.Collections;

public class CameraFollowPlayer : MonoBehaviour
{
  public float m_Distance = 5.0f;
  public float m_DistanceMin;
  public float m_DistanceMax;

  public float m_HeightToDistanceRatio = .8f;

  public float m_OrbitAngle = 0f;

  public Transform target;

  private const string k_CameraZoomInputController = "RightVertical";
  private const string k_CameraRotateInputController = "RightHorizontal";

  private const string k_CameraZoomInputMouse = "Mouse ScrollWheel";
  private const string k_CameraRotateInputMouse = "Mouse X";

  private float m_Height
  {
    get { return m_Distance * m_HeightToDistanceRatio; }
  }

  void LateUpdate()
  {
    float zoomInputController = Input.GetAxis( k_CameraZoomInputController );
    float zoomInputMouse = Input.GetAxis( k_CameraZoomInputMouse );

    float zoomInput = Mathf.Abs( zoomInputController ) > Mathf.Abs( zoomInputMouse ) ?
                          zoomInputController : zoomInputMouse;


    AdjustDistance( zoomInput );

    //float rotateInput = Mathf.Max( Input.GetAxis( k_CameraRotateInputController ),
    //                               Input.GetAxis( k_CameraRotateInputMouse ) );

    transform.position = target.position;

    Vector3 orbitForward = Quaternion.Euler( 0f, m_OrbitAngle, 0f) * target.forward;

    //Vector3 distanceToPlayerForward = Vector3.ProjectOnPlane( transform.forward, target.up );
    Vector3 distanceToPlayerForward = Vector3.ProjectOnPlane( orbitForward, target.up );

    // if camera is situated directly above player, will lock into position
    // Therefore, don't allow Distance to drop below Height
    transform.position -= distanceToPlayerForward * Mathf.Max( m_Distance, m_Height );

    transform.position += target.up * m_Height;

    //Vector3 left = Vector3.Cross( target.forward, target.up );

    transform.LookAt( target );
  }

  private void AdjustDistance( float change )
  {
    m_Distance = Mathf.Max( m_DistanceMin, Mathf.Min( m_DistanceMax, m_Distance + change ) );
  }
}
