using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Helper class to round the number of minutes to the lowest 5 minute interval
/// 
/// </summary>
/// 
namespace CalendarManagerLibrary {

    public class Rounder {

        #region Constructor
        public Rounder() {
            
        }
        #endregion Constructor

        #region Round minutes to the nearest multiple of 5 rounded down
        public int RoundToLowestFive(int minutes) {

            //if the number of minutes ends in 1 or 2 or 3 or 4, round down
            if ((minutes % 10) == 1 || (minutes % 10) == 2 || (minutes % 10) == 3 || (minutes % 10) == 4) {
                //if there minutes have 2 digits
                if (minutes > 10) {

                    int firstDigit = minutes;

                    while (firstDigit >= 10) {
                        firstDigit /= 10;
                    }

                    string concatenatedValue = firstDigit.ToString() + "0";

                    return Int32.Parse(concatenatedValue);
                }
                //otherwise only 1 digit so round down to 0
                else {
                    return 0;
                }
            }

            //if the number of minutes ends in 6 or 7 or 8 or 9, round down again to avoid overlap
            if ((minutes % 10) == 6 || (minutes % 10) == 7 || (minutes % 10) == 8 || (minutes % 10) == 9) {
                //if there minutes have 2 digits
                if (minutes > 10) {

                    int firstDigit = minutes;

                    while (firstDigit >= 10) {
                        firstDigit /= 10;
                    }
                    
                    string concatenatedValue = firstDigit.ToString() + "5";

                    return Int32.Parse(concatenatedValue);
                }
                //otherwise only 1 digit so round down to 5
                else {
                    return 5;
                }
            }

            //otherwise minutes ends in 0 or 5 so return the imput
            return minutes;
        }
        #endregion Round minutes to the nearest multiple of 5 rounded down
    }
}
