using UnityEngine;
using System.Collections;

abstract public class PathTypeState
{
	virtual public PathType Type { get{ return PathType.road; } }
	protected PathNode node;
	virtual public void OnEnterState(PathNode sender)
	{
		node = sender;
		node.SetSharedMaterial((int)Type);
	}
	
	virtual public void OnExitState(){}
	
	virtual public bool Walkable(PathFollowerTypes follower)
	{
		return false;
	}
}

