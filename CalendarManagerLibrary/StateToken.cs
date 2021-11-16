using Microsoft.AspNetCore.Antiforgery;
using System;

namespace CalendarManagerLibrary {

    /// <summary>
    /// StateToken class generates an anti-forgery state token, secure enough
    /// to prevent cross-site request forgery.
    /// </summary>

    public class StateToken {

        #region Private fields
        private String tokenValue;
        #endregion Private fields

        #region Constructor
        public StateToken() {

        }
        #endregion Constructor

        #region Get token value
        public String GetTokenValue() {

            return tokenValue;
        }
        #endregion Get token value

        #region Generating token string
        public String GenerateToken() {

            //62 character options
            String [] scheme = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                                 "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                                 "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};

            Random random = new Random();

            String token = "";

            int index;

            //30 character long token
            for (int i = 0; i < 30; i++) {
                index = random.Next(scheme.Length);
                token = token+scheme[index];
            }

            tokenValue = token;

            return token;
        }
        #endregion Generating token string

    }
}
