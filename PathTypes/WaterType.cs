using UnityEngine;
using System.Collections;

public class WaterType : PathTypeState 
{
	public override PathType Type {get { return PathType.water; }}
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
			return false;
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
