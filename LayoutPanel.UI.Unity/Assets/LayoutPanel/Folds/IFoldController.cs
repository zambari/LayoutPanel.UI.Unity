namespace Z.LayoutPanel
{
	using System;
	using System.Threading.Tasks;

	using UnityEngine;

	public interface IFoldController
	{
		void SetFold(bool newFold);

		bool isFolded { get; }

		Task SetFoldAsync(bool newFold);

		Action<bool> onFold { get; set; }

		void Fold();

		void UnFold();

		GameObject gameObject { get; }

		string name { get; }

		Transform transform { get; }
	}
}
