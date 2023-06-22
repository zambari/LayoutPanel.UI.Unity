using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class ScriptbaleObjectInstanceProvider<T> : MonoBehaviour where T : ScriptableObject
{
	public Action<T> onChanged;

	public static ScriptbaleObjectInstanceProvider<T> instance;

	public T collection
	{
		get
		{
			if (_collection == null) FindCollection();
			return _collection;
		}
		set { _collection = value; }
	}

	[SerializeField]
	private T _collection;

	protected virtual void OnEnable()
	{
		if (instance == null || instance == this) instance = this;
		else
		{
			Debug.Log($"{this.GetType()} multiple instanes found", gameObject);
		}
	}

	private void Reset()
	{
		FindCollection();
	}

	public bool FindCollection()
	{
		if (_collection == null)
		{
			var collections = Resources.FindObjectsOfTypeAll<T>();
			_collection = collections.FirstOrDefault();
			if (collections.Length > 1)
			{
				Debug.Log(
					$"Warning, there were multiple collections of {typeof(T)} available, first one was chosen",
					gameObject);
			}
		}

		return _collection != null;
	}
}
