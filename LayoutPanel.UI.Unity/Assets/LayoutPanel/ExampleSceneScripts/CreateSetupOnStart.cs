using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zUI;
namespace zUI.Examples
{

    [RequireComponent(typeof(LayoutCreator))]
    public class CreateSetupOnStart : MonoBehaviour
    {

        IEnumerator Start()
        {
			yield return null;
			yield return null;
            var creator = GetComponent<LayoutCreator>();
            creator.CreateSetup();
        }

    }
}