using CalendarManagerLibrary;
using System;
using Xunit;

namespace CalendarMangerXUnitTests {

    public class LibraryTests {

        [Fact]
        public void RoundsDownToNearestFive() {

            var rounder = new Rounder();

            int actual = rounder.RoundToLowestFive(59);

            int expected = 55;

            Assert.Equal(expected, actual);

        }

        [Fact]
        public void ShortUrlLength() {

            var generator = new ShortURL();

            int actual = generator.GenerateURL().Length;

            int expected = 21;

            Assert.Equal(expected, actual);

        }

        [Fact]
        public void ShortUrlNoClash() {

            var generator = new ShortURL();

            for(int i = 0; i<100; i++) {

                string url1 = generator.GenerateURL();
                string url2 = generator.GenerateURL();

                Assert.NotEqual(url1,url2);
            }
        }
        [Fact]
        public void StateTokenLength() {

            var generator = new StateToken();

            int actual = generator.GenerateToken().Length;

            int expected = 30;

            Assert.Equal(expected, actual);

        }

        [Fact]
        public void StateTokenNoClash() {

            var generator = new StateToken();

            for (int i = 0; i < 100; i++) {

                string url1 = generator.GenerateToken();
                string url2 = generator.GenerateToken();

                Assert.NotEqual(url1, url2);
            }
        }

    }
}
