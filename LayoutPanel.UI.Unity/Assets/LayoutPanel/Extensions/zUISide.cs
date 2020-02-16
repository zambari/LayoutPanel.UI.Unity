using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace zUI
{

    public enum Side { Top, Left, Right, Bottom }; //reordered for inspector unicodes

    public static class SideExtensions
    {
        public static string GetObjectLabel(this Side _side)
        {
            switch (_side)
            {
                default:
                case Side.Top: return "┌═════┐"; //"╒═══╕";
                case Side.Left: return "╠──    ";//"╠═──   ";
                case Side.Right: return "       ──╣";//"  ──═╣";
                case Side.Bottom: return "└═════┘";// "╘═══╛";

            }

        }

        public static Vector2 SideDelta(this Side side, Vector2 delta)
        {

            if (side.isHorizontal())
                delta.y = 0;
            else
                delta.x = 0;

            if (side == Side.Left) delta.x = -delta.x;
            if (side == Side.Bottom) delta.y = -delta.y;
            return delta;
        }
        public static Vector2 PositionScaledByPivot(this RectTransform rect, Vector2 position)
        {
            float x = position.x * rect.pivot.x;
            float y = position.y * rect.pivot.y;

            return new Vector2(x, y);
        }
        public static Vector2 GetAnchorMin(this Side side)
        {
            switch (side)
            {
                default:
                case Side.Top: return new Vector2(0, 1);
                case Side.Left: return new Vector2(0, 0);
                case Side.Right: return new Vector2(1, 0);
                case Side.Bottom: return new Vector2(0, 0);

            }
        }
        public static Vector2 GetAnchorMax(this Side side)
        {
            switch (side)
            {
                default:
                case Side.Top: return new Vector2(1, 1);
                case Side.Left: return new Vector2(0, 1);
                case Side.Right: return new Vector2(1, 1);
                case Side.Bottom: return new Vector2(1, 0);

            }
        }
        public static bool isHorizontal(this Side side) { return side == Side.Left || side == Side.Right; }
        public static Vector2 GetPivot(this Side side, bool borderPlacedInside)
        {
            switch (side)
            {
                default:
                case Side.Top: return borderPlacedInside ? new Vector2(.5f, 1) : new Vector2(0.5f, 0);
                case Side.Left: return borderPlacedInside ? new Vector2(0, .5f) : new Vector2(1, .5f);
                case Side.Right: return borderPlacedInside ? new Vector2(1, .5f) : new Vector2(0, .5f);
                case Side.Bottom: return borderPlacedInside ? new Vector2(0.5f, 0f) : new Vector2(0.5f, 1f);

            }
        }
        public static Vector2 GetPivot(this Side side)
        {
            switch (side)
            {
                default:
                case Side.Top: return new Vector2(0.5f, 0); ;
                case Side.Left: return new Vector2(1, .5f);
                case Side.Right: return new Vector2(0, .5f);
                case Side.Bottom: return new Vector2(0.5f, 1f);

            }
        }
        // public static Vector2 GetBaseSizeDelta(this Side side)
        // {

        // }

        //   switch (side)
        //     {
        //         default:
        //         case Side.Top: return   ;
        //         case Side.Left: return     ;
        //         case Side.Right: return      ;
        //         case Side.Bottom: return     ;

        //     }
    }

    /*
            string sideStringDouble
            {
                get
                {
                    switch (_side)
                    {
                        case Side.Left: return "⇇";
                        case Side.Right: return "⇉";
                        case Side.Top: return "⇈";
                        case Side.Bottom: return "⇊";

                    }
                    return null;
                }
            }


            string sideStringHollow
            {
                get
                {
                    switch (_side)
                    {
                        case Side.Left: return "⇦";
                        case Side.Right: return "⇨";
                        case Side.Top: return "⇧";
                        case Side.Bottom: return "⇩";

                    }
                    return null;
                }
            }
            string sideString
            {
                get
                {
                    switch (_side)
                    {
                        case Side.Top: return "┌═════┐"; //"╒═══╕";
                        case Side.Left: return "╠──    ";//"╠═──   ";

                        case Side.Right: return "       ──╣";//"  ──═╣";

                        case Side.Bottom: return "┕═════┙";// "╘═══╛";

                    }
                    return null;
                }
            }

            string sideStringFour
            {
                get
                {
                    string n = sideString;
                    return n + n + n + n;

                }
            }
            */

}
