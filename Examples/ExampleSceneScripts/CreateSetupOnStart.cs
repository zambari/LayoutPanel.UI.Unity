namespace zUI.LayoutPanelTools.Examples
{
    using System.Collections;

    using UnityEngine;

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
}