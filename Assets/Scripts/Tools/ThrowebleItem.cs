using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowebleItem : QuadraticBezierCurveMover
{
	[SerializeField] private ItemSettings Settings;

	public void Throw (Vector3 start, Vector3 end) 
	{
		Vector3 controllPoint = new Vector3 ();
		controllPoint.x = (start.x + end.x) / 2;
		controllPoint.z = (start.z + end.z) / 2;
		controllPoint.y = (start.y + end.y) / 2 + Random.Range (Settings.MinAddY,Settings.MaxAddY);
	}

	[System.Serializable]
	private class ItemSettings 
	{
		public float MinAddY;
		public float MaxAddY;
		public float MinFlyTime;
		public float MaxFlyTime;
	}
}
