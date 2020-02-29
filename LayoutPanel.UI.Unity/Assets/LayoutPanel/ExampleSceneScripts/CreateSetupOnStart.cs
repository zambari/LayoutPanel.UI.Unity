using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Z.LayoutPanel;
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
            Debug.Log("sorry this has changed");
            //     creator.CreateSetup();
        }

    }
}