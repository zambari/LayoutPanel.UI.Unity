using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Z.LayoutPanel
{
    public interface IProvideLayoutNameHelperSettings
    {
        LayoutNameHelperSettings GetSettings();
    }
    public interface IProvideLayoutGroupSettings
    {
        LayoutGroupSettings GetGroupSettings();
    }
}

