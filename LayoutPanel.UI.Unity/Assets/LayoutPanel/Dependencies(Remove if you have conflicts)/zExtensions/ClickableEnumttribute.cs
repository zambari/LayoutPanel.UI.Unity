// The purpose for this script was to avoid a nasty bug in UnityEditor 
// no matter what your Run In Background setting - unity will stop rendering when any dropdown
// is opened - this includes top menu (and there isnt much we can do about it)
// but it also affects any enum dropdowns, and I like those a lot, so I wrote this one-liner
// which adds [CliickableEnum] decorator, which will display your custom enum as a toolbar
// zambari 2017
using UnityEngine;
public class ClickableEnumAttribute : PropertyAttribute{}

// of course the actual line is in the drawer