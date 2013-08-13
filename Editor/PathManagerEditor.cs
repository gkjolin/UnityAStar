using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PathManager))]
public class PathManagerEditor : Editor {
	
	private PathManager _manager;
	private PathManager	Manager
	{
		get
		{
			if(_manager == null)
			{
				_manager = (PathManager)target;
			}
			return _manager;
		}
	}
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		if(GUILayout.Button("Create new field"))
		{
			Manager.CreateField(Manager.fieldWidth, Manager.fieldHeight, Manager.nodeSize, Manager.creationStartPoint);
		}
		if(GUILayout.Button("Remove field"))
		{
			Manager.RemoveOldField();
		}
	}
}
