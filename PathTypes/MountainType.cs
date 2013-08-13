using UnityEngine;
using System.Collections;

public class MountainType : PathTypeState 
{
	public override PathType Type {get { return PathType.mountain; }}
	
	public override void OnEnterState (PathNode sender)
	{
		base.OnEnterState (sender);
	}
	
	public override void OnExitState ()
	{
		
	}
	
	public override bool Walkable (PathFollowerTypes follower)
	{
		switch(follower)
		{
		case PathFollowerTypes.flying:
			return true;
		case PathFollowerTypes.foot:
			return true;
		case PathFollowerTypes.mech:
			return false;
		case PathFollowerTypes.track:
			return false;
		case PathFollowerTypes.wheel:
			return false;
		}
		return false;
	}
}
