using System;
using UnityEngine;

namespace GizmosExtensions
{
    public partial class Debug : UnityEngine.Debug
    {
        public static void DrawNumber(Vector3 position, int number, Color color)
        {
            // DrawNumber(position, number, 0);
            int digits = NumPlaces(number);
            if (digits != 1)
            {
                var digitChars = number.ToString().ToCharArray();
                // Debug.Log(digitChars[0] + " " + digitChars[1] + " " + Convert.ToInt32(digitChars[0]) + " " + Convert.ToInt32(digitChars[1]));
                for (int i = 0; i < digits; i++)
                {
                    float displacement = ((digits - 1) * -1.25f) + (i * 1.25f);
                    DrawSingleNumber(position, Convert.ToInt32(digitChars[i]) - 48, displacement, color);
                }
                return;
            }
            DrawSingleNumber(position, number, 0, color);
        }

        private static void DrawSingleNumber(Vector3 position, int number, float xDisplacement, Color color)
        {
            Vector3 sevenSegmentTopLeft = new Vector3(position.x + xDisplacement - 0.5f, position.y + 1, position.z);
            Vector3 sevenSegmentTopRight = new Vector3(position.x + xDisplacement + 0.5f, position.y + 1, position.z);
            Vector3 sevenSegmentMidLeft = new Vector3(position.x + xDisplacement - 0.5f, position.y + 0, position.z);
            Vector3 sevenSegmentMidRight = new Vector3(position.x + xDisplacement + 0.5f, position.y + 0, position.z);
            Vector3 sevenSegmentLowLeft = new Vector3(position.x + xDisplacement - 0.5f, position.y - 1, position.z);
            Vector3 sevenSegmentLowRight = new Vector3(position.x + xDisplacement + 0.5f, position.y - 1, position.z);


            //Topline
            if (IsNumber(number, 0, 2, 3, 5, 6, 7, 8, 9))
            {
                DrawLine(sevenSegmentTopLeft, sevenSegmentTopRight, color);
            }
            //mid line
            if (IsNumber(number, 2, 3, 4, 5, 6, 8, 9))
            {
                DrawLine(sevenSegmentMidLeft, sevenSegmentMidRight, color);
            }
            //lowline
            if (IsNumber(number, 0, 2, 3, 5, 6, 8))
            {
                DrawLine(sevenSegmentLowLeft, sevenSegmentLowRight, color);
            }

            //topright
            if (IsNumber(number, 0, 1, 2, 3, 4, 7, 8, 9))
            {
                DrawLine(sevenSegmentTopRight, sevenSegmentMidRight, color);
            }
            //lowright
            if (IsNumber(number, 0, 1, 3, 4, 5, 6, 7, 8, 9))
            {
                DrawLine(sevenSegmentLowRight, sevenSegmentMidRight, color);
            }
            //top left
            if (IsNumber(number, 0, 4, 5, 6, 8, 9))
            {
                DrawLine(sevenSegmentTopLeft, sevenSegmentMidLeft, color);
            }
            //lowleft
            if (IsNumber(number, 0, 2, 6, 8))
            {
                DrawLine(sevenSegmentMidLeft, sevenSegmentLowLeft, color);
            }
        }

        private static bool IsNumber(int number, params int[] numbers)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (number == numbers[i])
                    return true;
            }
            return false;
        }

        public static int NumPlaces(int n)
        {
            if (n < 10) return 1;
            if (n < 100) return 2;
            if (n < 1000) return 3;
            if (n < 10000) return 4;
            if (n < 100000) return 5;
            if (n < 1000000) return 6;
            if (n < 10000000) return 7;
            if (n < 100000000) return 8;
            if (n < 1000000000) return 9;
            /*      2147483647 is 2^31-1 - add more ifs as needed
               and adjust this final return as well. */
            return 10;
        }
    }
}