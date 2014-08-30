using UnityEngine;
using System.Collections;

/// <summary>
/// Recycler.
/// </summary>
public class Recycler : MonoBehaviour {
	/// <summary>
	/// The is pooled.
	/// </summary>
	[System.NonSerialized]
	public bool isPooled = false;
	
	protected void Recycle () {
		Pool.Recycle (gameObject);

	}
}
