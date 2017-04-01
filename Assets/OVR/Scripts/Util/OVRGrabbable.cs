/************************************************************************************

Copyright   :   Copyright 2014 Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.3 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

http://www.oculus.com/licenses/LICENSE-3.3

Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/

using System;
using UnityEngine;

/// <summary>
/// An object that can be grabbed and thrown by OVRGrabber.
/// </summary>
public class OVRGrabbable : MonoBehaviour
{
    [SerializeField]
    protected bool m_allowOffhandGrab = true;
    [SerializeField]
    protected bool m_snapPosition = false;
    [SerializeField]
    protected bool m_snapOrientation = false;
    [SerializeField]
    protected Transform m_snapOffset;
    [SerializeField]
    protected Collider[] m_grabPoints = null;

    protected bool m_grabbedKinematic = false;
    protected Collider m_grabbedCollider = null;
    protected OVRGrabber m_grabbedBy = null;

	/// <summary>
	/// If true, the object can currently be grabbed.
	/// </summary>
    public bool allowOffhandGrab
    {
        get { return m_allowOffhandGrab; }
    }

	/// <summary>
	/// If true, the object is currently grabbed.
	/// </summary>
    public bool isGrabbed
    {
        get { return m_grabbedBy != null; }
    }

	/// <summary>
	/// If true, the object's position will snap to match snapOffset when grabbed.
	/// </summary>
    public bool snapPosition
    {
        get { return m_snapPosition; }
    }

	/// <summary>
	/// If true, the object's orientation will snap to match snapOffset when grabbed.
	/// </summary>
    public bool snapOrientation
    {
        get { return m_snapOrientation; }
    }

	/// <summary>
	/// An offset relative to the OVRGrabber where this object can snap when grabbed.
	/// </summary>
    public Transform snapOffset
    {
        get { return m_snapOffset; }
    }

	/// <summary>
	/// Returns the OVRGrabber currently grabbing this object.
	/// </summary>
    public OVRGrabber grabbedBy
    {
        get { return m_grabbedBy; }
    }

	/// <summary>
	/// The transform at which this object was grabbed.
	/// </summary>
    public Transform grabbedTransform
    {
        get { return m_grabbedCollider.transform; }
    }

	/// <summary>
	/// The Rigidbody of the collider that was used to grab this object.
	/// </summary>
    public Rigidbody grabbedRigidbody
    {
        get { return m_grabbedCollider.attachedRigidbody; }
    }

	/// <summary>
	/// The contact point(s) where the object was grabbed.
	/// </summary>
    public Collider[] grabPoints
    {
        get { return m_grabPoints; }
    }

	/// <summary>
	/// Notifies the object that it has been grabbed.
	/// </summary>
	virtual public void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        try
        {
            ThrowLogic obj1 = transform.GetChild(0).GetComponent<ThrowLogic>();
            ThrowLogic obj2 = transform.GetChild(1).GetComponent<ThrowLogic>();
            obj1.GetComponent<Rigidbody>().isKinematic = true;
            obj2.GetComponent<Rigidbody>().isKinematic = true;

        }
        catch { }
        m_grabbedBy = hand;
        m_grabbedCollider = grabPoint;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

	/// <summary>
	/// Notifies the object that it has been released.
	/// </summary>
	virtual public void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        try
        {
            ThrowLogic obj1 = transform.GetChild(0).GetComponent<ThrowLogic>();
            ThrowLogic obj2 = transform.GetChild(1).GetComponent<ThrowLogic>();
            obj1.GetComponent<Rigidbody>().isKinematic = false;
            obj2.GetComponent<Rigidbody>().isKinematic = false;
            obj1.transform.parent = null;
            obj2.transform.parent = null;
            obj1.Throw(8000);
            obj2.Throw(8000);
        }
        catch { }
        m_grabbedBy = null;
        m_grabbedCollider = null;
    }

    void Awake()
    {
        if (m_grabPoints.Length == 0)
        {
            // Get the collider from the grabbable
            Collider collider = this.GetComponent<Collider>();
            if (collider == null)
            {
				throw new ArgumentException("Grabbables cannot have zero grab points and no collider -- please add a grab point or collider.");
            }

            // Create a default grab point
            m_grabPoints = new Collider[1] { collider };
        }
    }

    void Start()
    {
        m_grabbedKinematic = GetComponent<Rigidbody>().isKinematic;
    }

    void OnDestroy()
    {
        if (m_grabbedBy != null)
        {
            // Notify the hand to release destroyed grabbables
            m_grabbedBy.ForceRelease(this);
        }
    }
}
