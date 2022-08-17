using System;

namespace Magals.DevicesControl.SDKStandart.Attributes
{
    /// <summary>
    /// Label attribute - signals that this object is a driver of interaction with some
    /// specific equipment.
    /// </summary>
    public class DriverAttribute : Attribute
    {
        public DriverAttribute(string name)
        {
            this.name = RemoveSpecChars(name);
        }

        public string name { get; set; }
        public bool enable { get; set; } = true;

        /// <summary>
        /// During the creation of a dynamic class, its name must not contain characters
        /// which are not allowed in the class name
        /// </summary>
        /// <param name="word">class name</param>
        /// <returns>cleare object name</returns>
        public static string RemoveSpecChars(string word)
        {
            return word.Replace("+", string.Empty)
                .Replace("&&", string.Empty)
                .Replace("||", string.Empty)
                .Replace("!", string.Empty)
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Replace("{", string.Empty)
                .Replace("}", string.Empty)
                .Replace("[", string.Empty)
                .Replace("]", string.Empty)
                .Replace("^", string.Empty)
                .Replace("~", string.Empty)
                .Replace("*", string.Empty)
                .Replace("?", string.Empty)
                .Replace(":", string.Empty)
                .Replace("\\", string.Empty)
                .Replace("\"", string.Empty)
                .Replace("/", string.Empty);
        }
    }

}
