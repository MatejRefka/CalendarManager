using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarManagerLibrary {

    /// <summary>
    /// Short URL generator for the for each booking session
    /// </summary>


    public class ShortURL {

        #region Constructor
        public ShortURL() {

        }
        #endregion Constructor

        #region Generating short URL string
        public String GenerateURL() {

            //62 character options
            String[] scheme = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                                 "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                                 "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};

            Random random = new Random();

            String shortURL = "";

            int index;

            //21 character long URL
            for (int i = 0; i < 21; i++) {
                index = random.Next(scheme.Length);
                shortURL = shortURL + scheme[index];
            }

            return shortURL;
        }
        #endregion Generating token string

    }

}
