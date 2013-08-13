using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PathNode))]
public class PathNodeEditor : Editor 
{
	private PathNode _target;
	private PathNode Node
	{
		get
		{
			if(_target == null)
			{
				_target = (PathNode)target;
			}
			return _target;
		}
	}
	public override void OnInspectorGUI ()
	{
		//EditorGUILayout.LabelField("Current type: "+Node.Type.ToString());
		var oldType = Node.Type;
		var newType = (PathType)EditorGUILayout.EnumPopup("PathType:", oldType);
		if(oldType != newType)
		{
			Node.ChangePathType(newType);
		}
	}
}
