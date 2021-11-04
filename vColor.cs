using System;

namespace CustomProgram
{


    /// <summary>
    /// My color class that holds a RGB and HSV representation
    /// </summary>
    public class vColor
    {

        private readonly double _r; //between 0 and 255
        private readonly double _g; //between 0 and 255
        private readonly double _b; //between 0 and 255
        private readonly double _h; //between 0 and 360
        private readonly double _s; //between 0 and 1
        private readonly double _v; //between 0 and 1
        /// <summary>
        /// Creates a vColor object from RGB values
        /// </summary>
        /// <returns>Returns a vColor object</returns>
        static public vColor RGB(double r, double g, double b)
        {
            r = Crunch(r, 0, 255);
            g = Crunch(g, 0, 255);
            b = Crunch(b, 0, 255);
            RgbToHsv(r, g, b, out double h, out double s, out double v);
            return new vColor(r, g, b, h, s, v);
        }
        /// <summary>
        /// Creates a vColor object from HSV values
        /// </summary>
        /// <returns>Returns a vColor object</returns>
        static public vColor HSV(double h, double s, double v)
        {
            h = GetValidHue(h);
            s = Crunch(s, 0, 1);
            v = Crunch(v, 0, 1);
            HsvToRgb(h, s, v, out double r, out double g, out double b);
            return new vColor(r, g, b, h, s, v);
        }

        private vColor(double r, double g, double b, double h, double s, double v)
        {
            _r = r;
            _g = g;
            _b = b;
            _h = h;
            _s = s;
            _v = v;
        }

        public double R => _r;
        public double G => _g;
        public double B => _b;
        public double H => _h;
        public double S => _s;
        public double V => _v;

        private static double GetValidHue(double hue)
        {
            while (hue > 360 || hue < 0)
            {
                if (hue > 360)
                {
                    hue -= 360;
                }
                else if (hue < 0)
                {
                    hue += 360;
                }
            }
            return hue;
        }
        /// <summary>
        /// Limits a value within the specified range
        /// </summary>
        /// <param name="number"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>Returns the range limited value</returns>
        private static double Crunch(double number, double min, double max)
        {
            if (number > max)
            {
                return max;
            }
            else if (number < min)
            {
                return min;
            }
            else
            {
                return number;
            }
        }
        /// <summary>
        /// Converts from RGB to HSV
        /// </summary>
        //FROM: https://www.cs.rit.edu/~ncs/color/t_convert.html
        private static void RgbToHsv(double r, double g, double b, out double h, out double s, out double v)
        {
            double delta, min;
            h = 0;

            min = Math.Min(Math.Min(r, g), b);
            v = Math.Max(Math.Max(r, g), b);
            delta = v - min;

            if (v == 0.0)
                s = 0;
            else
                s = delta / v;

            if (s == 0)
                h = 0.0;

            else
            {
                if (r == v)
                    h = (g - b) / delta;
                else if (g == v)
                    h = 2 + (b - r) / delta;
                else if (b == v)
                    h = 4 + (r - g) / delta;

                h *= 60;

                if (h < 0.0)
                    h = h + 360;
            }
            v = v / 255;
        }
        /// <summary>
        /// Converts from HSV to RGB
        /// </summary>
        //FROM http://www.splinter.com.au/converting-hsv-to-rgb-colour-using-c/
        private static void HsvToRgb(double h, double S, double V, out double r, out double g, out double b)
        {
            // ######################################################################
            // T. Nathan Mundhenk
            // mundhenk@usc.edu
            // C/C++ Macro HSV to RGB

            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            r = R * 255.0;
            g = G * 255.0;
            b = B * 255.0;
        }
    }
}
