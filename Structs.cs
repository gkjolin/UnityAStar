using UnityEngine;
using System.Collections;

public enum PathFollowerTypes{ foot, track, wheel, mech, flying }
public enum PathType{ road, field, desert, forest, rocks, water, swamp, mountain, city }

public struct PathData
{
	public PathNode[] Nodes{ get; private set; }
	public Vector3[] VectorPath{ get; private set; }
	public PathFollowerTypes TargetFollowerType{ get; private set; }
	public int Cost{ get; private set; }
	
	public PathData(PathNode[] nodes, PathFollowerTypes targetType, int cost)
	{
		Nodes = nodes;
		var length = nodes.Length;
		VectorPath = new Vector3[length];
		for(int i = 0; i < length; i++)
		{
			VectorPath[i] = nodes[i].Trans.position;
		}
		TargetFollowerType = targetType;
		Cost = cost;
	} 
}

public struct Vec2
{
	public int x;
	public int y;
	
	public Vec2(int x, int y)
	{
		this.x = x;
		this.y = y;
	}
	
	public float SqrDistance(Vec2 a, Vec2 b)
	{
		a.x = a.x - b.x;
		a.y = a.y - a.y;
		return a.x * a.x + a.y * a.y;
	}
}
