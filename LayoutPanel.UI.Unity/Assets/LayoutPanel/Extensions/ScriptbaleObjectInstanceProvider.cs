namespace Z.LayoutPanel
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	using UnityEngine;
	using UnityEngine.Serialization;

	[ExecuteInEditMode]
	public class ScriptbaleObjectInstanceProvider<T> : MonoBehaviour where T : ScriptableObject
	{
		public T collection
		{
			get
			{
				if (_collection == null) FindCollection();
				return _collection;
			}
		}

		public static ScriptbaleObjectInstanceProvider<T> baseClassInstance;

	

		[SerializeField]
		private T _collection;

		private void Reset()
		{
			FindCollection();
		}

		protected void FindCollection()
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
		}
	}
}
