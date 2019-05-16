using System;
using UnityEngine;

namespace CCS.EnumerationFlags
{

    public class EnumFlags<T> where T : struct, IConvertible
    {

        private static bool CheckIfEnum()
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("T must be an Enum type Variable");
            }
            return true;

        }
        /// <summary>
        /// Checks to see if the sent value contains the desired flag.
        /// </summary>
        public static bool HasFlag(T valueToCheck, T flagToCheck)
        {
            if (CheckIfEnum())
            {
                int value = (int)(object)valueToCheck;
                int intLookingForFlag = (int)(object)flagToCheck;
                return ((value & intLookingForFlag) != 0);
            }
            return false;
        }

        /// <summary>
        /// Checks to see if the sent value contains all desired flags.
        /// </summary>
        public static bool HasAllFlags(T value, params T[] checkFlags)
        {
           
            if (CheckIfEnum())
            {
                foreach (T flag in checkFlags)
                {
                    
                    if (!HasFlag(value, flag))
                        return false;
                }

            }
            return true;
        }
    }
}
