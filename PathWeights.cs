using UnityEngine;
using System.Collections;
using System.IO;
using MiniJSON;
using System.Collections.Generic;
using System;

public class PathWeights
{
#if UNITY_WINDOWS
	const string weightsPath = @"Data\";
#else
	const string weightsPath = @"Data/";
#endif
	const string weightFile = "PathWeights.ini";
	private static PathWeights _instance;
	private static PathWeights Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new PathWeights();
			}
			return _instance;
		}
	}
	
	private Dictionary<PathFollowerTypes, Dictionary<PathType, int>> weights;
	
	static public PathWeights WakeUp()
	{
		if(_instance == null)
		{
			_instance = new PathWeights();
		}
		return _instance;
	}
	
	private PathWeights()
	{
		Directory.CreateDirectory(weightsPath);
		if(!File.Exists(weightsPath + weightFile))
		{ 
			Debug.LogWarning("No weights file found at "+weightsPath + weightFile+ "! Creating default.");
			CreateDefaultWeightsFile();
		}
		
		
		StreamReader sr = new StreamReader(weightsPath+ weightFile);
		DeserializeWeightData(sr.ReadToEnd());
	}
	
	private void DeserializeWeightData(string data)
	{
		Dictionary<string, object> dict = (Dictionary<string, object>) Json.Deserialize(data);
		weights = new Dictionary<PathFollowerTypes, Dictionary<PathType, int>>();
		foreach(KeyValuePair<string, object> kvp in dict)
		{
			PathFollowerTypes t = (PathFollowerTypes)Enum.Parse(typeof(PathFollowerTypes), kvp.Key);
			Dictionary<string, object> wObjects = (Dictionary<string, object>)kvp.Value;
			Dictionary<PathType, int> w = new Dictionary<PathType, int>();
			foreach(KeyValuePair<string, object> d in wObjects)
			{
				var key = (PathType)Enum.Parse(typeof(PathType), d.Key);
				w.Add(key, (int)((long)d.Value));
			}
			weights.Add( t, w);
		}
	}
	
	private void CreateDefaultWeightsFile()
	{
		weights = new Dictionary<PathFollowerTypes,Dictionary<PathType, int>>();
		var enumNames = Enum.GetNames(typeof(PathType));
		Dictionary<PathType, int> defaultWeight = new Dictionary<PathType, int>();
		for(int i = 0; i < enumNames.Length; i++)
		{
			defaultWeight.Add((PathType)i, 1);
		}
		weights.Add(PathFollowerTypes.foot, defaultWeight );
		weights.Add(PathFollowerTypes.flying, defaultWeight );
		weights.Add(PathFollowerTypes.mech, defaultWeight );
		weights.Add(PathFollowerTypes.track, defaultWeight );
		weights.Add(PathFollowerTypes.wheel, defaultWeight );
		string data = Json.Serialize(weights);
		StreamWriter sw = new StreamWriter(weightsPath+ weightFile, false);
		sw.Write(data);
		sw.Close();
	}
	
	public static int GetWeight(PathFollowerTypes followerType, PathType pathType)
	{
		return Instance.weights[followerType][pathType];
	}
	
	public static Dictionary<PathType, int> GetWeights(PathFollowerTypes followerType)
	{
		return Instance.weights[followerType];	
	}
}
