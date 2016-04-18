using UnityEngine;
using System.Collections;

/// <summary>
/// Derived from Unity's "ThirdPersonCharacter.cs"
/// </summary>
public class CharacterController : MonoBehaviour
{
  [SerializeField] private Rigidbody m_Rigidbody;
  [SerializeField] private float m_GroundCheckDistance = 0.1f;
  /// <summary>
  /// degrees / sec
  /// </summary>
  [SerializeField] private float m_StationaryTurnSpeed;
  /// <summary>
  /// degrees / sec
  /// </summary>
  [SerializeField] private float m_MaxSpeedTurnSpeed;

  private bool m_IsGrounded;
  private Vector3 m_GroundNormal;
  private float m_TurnAmount;
  private float m_ForwardAmount;

  void Awake()
  {
    m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    //// here example ThirdPersonCharacter.cs records distance to ground for use in jumping
  }

  public void Move( Vector3 moveDir )
  {
    // convert the world-relative moveDir vector into a local-relative
    // turn amount and forward amount required to head in the desired direction
    // note: ThirdPersonUserControl has already converted from 'inputspace' to 'worldspace'

    // sanitize
    if( moveDir.magnitude > 1f )
    {
      moveDir.Normalize();
    }

    moveDir = transform.InverseTransformDirection( moveDir );
    CheckGroundStatus();
    moveDir = Vector3.ProjectOnPlane( moveDir, m_GroundNormal );

    // split into seperate floats: rotation + forward
    m_TurnAmount = Mathf.Atan2( moveDir.x, moveDir.z );
    m_ForwardAmount = moveDir.z;

    ApplyTurnRotation();

    // different movement for grounded, airborne
    if( m_IsGrounded )
    {
      m_Rigidbody.velocity += new Vector3( 0f, 0f, m_ForwardAmount * Time.deltaTime );
    
     

      HandleGroundedMovement();
    }
    else
    {
      HandleAirborneMovement();
    }    
  }

  private void ApplyTurnRotation()
  {
    float turnSpeed = Mathf.Lerp( m_StationaryTurnSpeed, m_MaxSpeedTurnSpeed, m_ForwardAmount );
    transform.Rotate( 0f, m_TurnAmount * turnSpeed * Time.deltaTime, 0f );
  }

  private void HandleGroundedMovement()
  {
  }

  private void HandleAirborneMovement()
  {

  }

  void CheckGroundStatus()
  {
    RaycastHit hitInfo;
#if UNITY_EDITOR
    // helper to visualise the ground check ray in the scene view
    Debug.DrawLine( transform.position + ( Vector3.up * 0.1f ), transform.position + ( Vector3.up * 0.1f ) + ( Vector3.down * m_GroundCheckDistance ) );
#endif
    // 0.1f is a small offset to start the ray from inside the character
    // it is also good to note that the transform position in the sample assets is at the base of the character
    if( Physics.Raycast( transform.position + ( Vector3.up * 0.1f ), Vector3.down, out hitInfo, m_GroundCheckDistance ) )
    {
      m_GroundNormal = hitInfo.normal;
      m_IsGrounded = true;
      //// here example script sets: m_Animator.applyRootMotion = true;
    }
    else
    {
      m_IsGrounded = false;
      m_GroundNormal = Vector3.up;
      //// here example script sets: m_Animator.applyRootMotion = false;
    }
  }
}
