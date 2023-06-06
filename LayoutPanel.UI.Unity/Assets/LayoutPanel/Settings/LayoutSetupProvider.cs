using System;
using System.Collections.Generic;

using UnityEngine;

namespace Z.LayoutPanel
{
	[ExecuteInEditMode]
	public class LayoutSetupProvider : ScriptbaleObjectInstanceProvider<LayoutSetup> //, IRequestInitEarly
	{
		// static LayoutBorderControl instance;
		static List<IBorderControlListener> borderDraggers;

		public Action<LayoutSetup> onChanged;

		private static LayoutSetupProvider _instance;

		public static LayoutSetupProvider instance
		{
			get
			{
				if (_instance == null) _instance = GameObject.FindObjectOfType<LayoutSetupProvider>();
				return _instance;
			}
		}

		public int borderSize
		{
			get { return setup.borderSize; }
			set
			{
				setup.borderSize = value;
				OnValidate();
			}
		}

		public static LayoutSetup setup => instance?.collection;

		public bool inerseqr;

		float lastSet = -1;

		public bool topEnabled
		{
			get { return setup.borderSetup.topBorder; }
			set
			{
				setup.borderSetup.topBorder = value;
				OnValidate();
			}
		}

		public float borderAlphaHovered
		{
			get { return setup.borderSetup.hoveredAlpha; }
			set
			{
				setup.borderSetup.hoveredAlpha = value;
				OnValidate();
			}
		}

		public float borderAlphaNormal
		{
			get { return setup.borderSetup.normalAlpha; }
			set
			{
				setup.borderSetup.normalAlpha = value;
				OnValidate();
			}
		}

		public float spacingFloat
		{
			get { return setup.borderSize; }
			set
			{
				setup.borderSize = (int)(value);
				OnValidate();
			}
		}

		public float horizonttaldistance
		{
			get { return setup.panelSpacingOffserHoriz; }
			set
			{
				setup.panelSpacingOffserHoriz = (int)(value);
				OnValidate();
			}
		}

		public float distanceBorders
		{
			get { return setup.borderSetup.offsetSide; }
			set
			{
				setup.borderSetup.offsetSide = (int)value;
				OnValidate();
			}
		}

		public float borderSizeFloat
		{
			get
			{
				if (lastSet == -1) return setup.borderSetup.borderSize / 30f;

				return lastSet;
			}
			set
			{
				lastSet = value;
				setup.borderSetup.borderSize = (int)(value * 30);
				OnValidate();
			}
		}

		//
		private static void BroadcastSetup(LayoutSetup thisSetup)
		{
			if (borderDraggers != null)
				for (int i = borderDraggers.Count - 1; i >= 0; i--)
				{
					if (borderDraggers[i] == null) borderDraggers.RemoveAt(i);
					borderDraggers[i].UpdateLayoutSetupObject(thisSetup);
				}
		}

		//
		public static void Subscribe(IBorderControlListener source)
		{
			if (borderDraggers == null) borderDraggers = new List<IBorderControlListener>();
			if (!borderDraggers.Contains(source)) borderDraggers.Add(source);
			if (instance != null && instance.collection != null) source?.UpdateLayoutSetupObject(instance.collection);
		}

		public static void UnsSubscribe(IBorderControlListener source)
		{
			if (borderDraggers == null) borderDraggers = new List<IBorderControlListener>();
			if (borderDraggers.Contains(source)) borderDraggers.Remove(source);
		}

		public static bool isReady
		{
			get { return instance != null; }
		}

		private void OnValidate()
		{
			if (gameObject.PrefabModeIsActive()) return;
#if UNITY_EDITOR
			UnityEditor.EditorApplication.delayCall += BroadcastEvents;
#else
		BroadcastEvents();
#endif
		}

		protected void OnEnable()
		{
			if (setup != null)
			{
				setup.onChanged += BroadcastEvents;

				//setup.onChanged += () => onChanged?.Invoke(setup);
			}
		}

		protected void OnDisable()
		{
			if (setup != null)
			{
				setup.onChanged -= BroadcastEvents;
			}
		}

		[LPExposeMethodInEditor]
		void BroadcastEvents()
		{
			BroadcastSetup(setup);
		}

		/// <inheritdoc />
		public void Init(MonoBehaviour awakenSource)
		{
			OnEnable();
		}
	}
}
