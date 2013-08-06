// ****************************************************************************
// <author>James Witt Hurst</author>
// <email>JamesH@DesignForge.com</email>
// <date>2011-8-15</date>
// <project>JhLib</project>
// <web>http://www.designforge.wordpress.com</web>
// ****************************************************************************
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media;


namespace JhLib
{
    /// <summary>
    /// This class was created for use with the ColorComboBox, for allowing the user to select colors.
    /// </summary>
    public class NamedColor
    {
        #region Constructors

        public NamedColor(string sName)
        {
            _sName = sName;
            // Try to find the matching Color that goes with this name.
            NamedColor theNamedColorForThisName = NamedColor.From(sName);
            if (theNamedColorForThisName != null)
            {
                _Color = theNamedColorForThisName.Color;
            }
            else
            {
                Console.WriteLine("Oops! NamedColor(\"" + sName + "\") ctor cannot recognize name!");
            }
        }

        /// <summary>
        /// private constructor. Creates a NamedColor instance given a name (string) and a Color.
        /// </summary>
        /// <param name="sName">The name of this color, taken from Colors</param>
        /// <param name="color">The Color component</param>
        private NamedColor(string sName, Color color)
        {
            this._Color = color;
            this._sName = sName;
        }
        #endregion Constructors

        #region AllWindowsMediaColors static property
        /// <summary>
        /// Return an array of all the named colors
        /// </summary>
        public static NamedColor[] AllWindowsMediaColors
        {
            get
            {
                if (s_NamedColors == null)
                {
                    PropertyInfo[] props = typeof(Colors).GetProperties();
                    s_NamedColors = new NamedColor[props.Length];
                    for (int i = 0; i < props.Length; i++)
                    {
                        s_NamedColors[i] = new NamedColor(props[i].Name, (Color)props[i].GetValue(null, null));
                    }
                }
                return s_NamedColors;
            }
        }
        #endregion

        #region AllSystemColors static property
        /// <summary>
        /// Return an array of all the named colors
        /// </summary>
        public static List<NamedColor> AllSystemColors
        {
            get
            {
                if (s_listSystemColors == null)
                {
                    s_listSystemColors = new List<NamedColor>();
                    PropertyInfo[] props = typeof(SystemColors).GetProperties();
                    for (int i = 0; i < props.Length; i++)
                    {
                        PropertyInfo propInfo = props[i];
                        if (propInfo.Name.Contains("Color") && !propInfo.Name.Contains("ColorKey"))
                        //if (propInfo.GetType() == typeof(Color))
                        {
                            s_listSystemColors.Add(new NamedColor(props[i].Name, (Color)props[i].GetValue(null, null)));
                            //s_SystemColors[i] = new NamedColor(props[i].Name, (Color)props[i].GetValue(null, null));
                        }
                    }

                    //NamedColor c = new NamedColor("ActiveBorderColor", SystemColors.ActiveBorderColor);
                    //s_SystemColors = new NamedColor[1];
                    //s_SystemColors[0] = c;
                }
                return s_listSystemColors;
            }
        }
        #endregion

        #region Name property
        /// <summary>
        /// Get the name component.
        /// </summary>
        public string Name
        {
            get
            {
                if (_sName == null)
                {
                    _sName = "";
                }
                return _sName;
            }
        }
        #endregion

        #region NameWithSpaces property
        /// <summary>
        /// Get the name component, with spaces inserted between the parts of the name.
        /// </summary>
        public string NameWithSpaces
        {
            get
            {
                // Reformat the name so that it has spaces separating the parts
                // (as indicated by the uppercase letters)..
                if (!String.IsNullOrWhiteSpace(_sName))
                {
                    string sNameWithSpaces = _sName[0].ToString();
                    for (int i = 1; i < _sName.Length; i++)
                    {
                        sNameWithSpaces += (char.IsUpper(_sName[i]) ? " " : "") + _sName[i].ToString();
                    }
                    return sNameWithSpaces;
                }
                else
                {
                    return Name;
                }
            }
        }
        #endregion

        #region Color property
        /// <summary>
        /// Get the Color component
        /// </summary>
        public Color Color
        {
            get { return _Color; }
        }
        #endregion

        #region SolidColorBrush property
        /// <summary>
        /// Get a SolidColorBrush based upon the Color component
        /// </summary>
        public SolidColorBrush Brush
        {
            get
            {
                if (_solidColorBrush == null)
                {
                    _solidColorBrush = new SolidColorBrush(_Color);
                }
                return _solidColorBrush;
            }
        }
        #endregion

        #region ToString
        /// <summary>
        /// Override of the ToString method, to yield just the Name.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
        #endregion

        public static NamedColor Empty
        {
            get { return new NamedColor("Transparent", Colors.Transparent); }
        }

        #region ToDigitHexValueRGB
        /// <summary>
        /// Returns the color value as a string in std HTML 6-hexadecimal-digit RGB format, ie as RRGGBB
        /// (e.g., FF0000 for red)
        /// </summary>
        /// <returns></returns>
        public string ToDigitHexValueRGB()
        {
            var sb = new StringBuilder();
            sb.Append(_Color.R.ToString("X"));
            sb.Append(_Color.G.ToString("X"));
            sb.Append(_Color.B.ToString("X"));
            return sb.ToString();
        }
        #endregion

        #region IsEmpty
        /// <summary>
        /// True if this color is what we designate as a 'null' value, which in this case is what we use the "Transparent" color for.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (String.IsNullOrEmpty(this.Name) || this.Name.Equals("Transparent", StringComparison.InvariantCultureIgnoreCase) || this.Color == Colors.Transparent);
            }
        }
        #endregion

        #region From(string)
        /// <summary>
        /// Given a string that names one of the Colors, return the corresponding NamedColor
        /// (or null if no match).
        /// </summary>
        /// <param name="sName">A name of one of the colors from the Colors class</param>
        /// <returns>The appropriate NamedColor, or null if sName is not a valid color-name</returns>
        public static NamedColor From(string sName)
        {
            foreach (NamedColor namedColor in AllWindowsMediaColors)
            {
                if (String.Compare(sName, namedColor.ToString(), true) == 0)
                {
                    return namedColor;
                }
            }
            foreach (NamedColor namedColor in AllSystemColors)
            {
                if (String.Compare(sName, namedColor.ToString(), true) == 0)
                {
                    return namedColor;
                }
            }
            return null;
        }
        #endregion

        #region From(Color)
        /// <summary>
        /// Given a Color, return the corresponding NamedColor
        /// (or transparent is no match).
        /// </summary>
        /// <param name="color">The color to match up with our list of named colors</param>
        /// <returns>The appropriate NamedColor, or transparent if it's not one of the color-names</returns>
        public static NamedColor From(Color color)
        {
            foreach (NamedColor namedColor in AllWindowsMediaColors)
            {
                if (Color.AreClose(color, namedColor.Color))
                {
                    return namedColor;
                }
            }
            foreach (NamedColor namedColor in AllSystemColors)
            {
                if (Color.AreClose(color, namedColor.Color))
                {
                    return namedColor;
                }
            }
            return NamedColor.Empty;
        }
        #endregion

        #region CompareLuminance
        /// <summary>
        /// Allows a caller to sort a list of colors by brightness.
        /// Sorting this way makes the colors in the combobox drop down list
        /// look really nice.
        /// </summary>
        /// <param name="x">one color to compare</param>
        /// <param name="y">the other color to compare against</param>
        /// <returns>-1, 0, or 1 to reflect the result of the comparison</returns>
        public static int CompareLuminance(NamedColor cnx, NamedColor cny)
        {
            //TODO: This needs to be implemented properly.
            //      WPF doesn't provide a GetBrightness method!

            Color x = cnx.Color;
            Color y = cny.Color;

            float xBrightness = x.ScR + x.ScG + x.ScB;
            float yBrightness = y.ScR + y.ScG + y.ScB;

            //Color cx = Color.FromKnownColor(x);
            //Color cy = Color.FromKnownColor(y);

            //float xBrightness = x.GetBrightness();
            //float yBrightness = y.GetBrightness();

            xBrightness = (float)Math.Round(xBrightness, 5);
            yBrightness = (float)Math.Round(yBrightness, 5);

            // Compare brightness first. If brightnesses are equal, compare by Red, then Blue, then Green components.
            // This tends to lump similar colors together in the list.

            if (xBrightness > yBrightness)
            {
                return -1;
            }
            else if (xBrightness < yBrightness)
            {
                return 1;
            }
            else if (x.R > y.R)
            {
                return -1;
            }
            else if (x.R < y.R)
            {
                return 1;
            }
            else if (x.B > y.B)
            {
                return -1;
            }
            else if (x.B < y.B)
            {
                return 1;
            }
            else if (x.G > y.G)
            {
                return -1;
            }
            else if (x.G < y.G)
            {
                return 1;
            }

            return 0;
        }
        #endregion

        #region Fields

        private Color _Color;
        private string _sName;

        //TODO: This Transient attribute was for db4o. However, I would prefer not to have this class require the db4o assemblies for every project.
//        [Transient]
        private static NamedColor[] s_NamedColors;

//        [Transient]
        private static List<NamedColor> s_listSystemColors;

//        [Transient]
        private SolidColorBrush _solidColorBrush;

        #endregion Fields
    }
}
