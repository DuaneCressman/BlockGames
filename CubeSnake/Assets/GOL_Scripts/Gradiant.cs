using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.GOL_Scripts
{
    public static class Gradient
    {
        private const int MAX_LEN = 1530;


        /* Method Name: GetGradient
        * Parameters: double val - the value on the gradient to be found    
        *             double maxVal - the maximum value 
        * Return: Color - the colour corresponding to the input
        * 
        * Purpose: This method will calculate where on a gradient the input would be.
        *          It allows 2 inputs so that you can enter a ratio instead of a set gradient value.
        */
        public static Color GetGradient(double val, double maxVal)
        {
            //get the ratio of the 2 values and use that to find the corresponding value on the gradient.
            return GetGradient((val / maxVal) * MAX_LEN);
        }


        /* Method Name: GetGradient
         * Parameters: double input - the value of the gradient              
         * Return: Color - the colour corresponding to the input
         * 
         * Purpose: This method will calculate where on a gradient the input would be.
         */
        public static Color GetGradient(double input)
        {
            //get which section of the gradient
            int position = (int)(input / 255);

            //get how far into this section this value is
            int spacing = (int)(input % 255);

            //set defaults for the 3 color channels
            float R = 0;
            float G = 0;
            float B = 0;

            //the gradient is split into 7 sections 
            switch (position)
            {
                //Red on
                //Blue increasing
                case 0:
                    R = 255;
                    B = spacing;
                    break;

                //Blue on
                //Red decreasing 
                case 1:
                    R = 255 - spacing;
                    B = 255;
                    break;

                //Blue on
                //Green increasing
                case 2:
                    G = spacing;
                    B = 255;
                    break;

                //Blue decreasing
                //Green on
                case 3:
                    B = 255 - spacing;
                    G = 255;
                    break;

                //Green on
                //Red increasing
                case 4:
                    G = 255;
                    R = spacing;
                    break;

                //Red on
                //Green decreasing
                case 5:
                    R = 255;
                    G = 255 - spacing;
                    break;


                case 6:

                    //catch if the highest possible value was entered
                    if (spacing == 0)
                    {
                        R = 255;
                        B = 255;
                    }

                    break;

                default:
                    break;
            }



            //return the corresponding color
            return new Color(R / 255f, G / 255f, B / 255f);
        }

        /* Method Name: GetDistance
         * Parameters: double X1 - First values X coordinate 
         *             double Y1 - First values Y coordinate 
         *             double X2 - Second values X coordinate 
         *             double Y2 - Second values Y coordinate              
         * Return: double - The distance between 2 points
         * 
         * Purpose: This method will use the Pythagorean thermo to find the distance between 2 points
         */
        //public static double GetDistance(double X1, double Y1, double X2, double Y2)
        //{
        //    return Math.Sqrt(Math.Pow(Y2 - Y1, 2) + Math.Pow(X2 - X1, 2));
        //}
    }
}
