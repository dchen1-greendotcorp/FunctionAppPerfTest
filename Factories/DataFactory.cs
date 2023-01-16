using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppPerfTest.Factories
{
    public static class DataFactory
    {
        public static dynamic CreateFakeData()
        {
            string data = "";
            return data;
        }

        public static dynamic CreateFakeCharacterData(int size)
        {
            Random rand = new Random();
            
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {

                // Generating a random number.
                int randValue = rand.Next(0, 26);

                // Generating random character by converting
                // the random number into character.
                char letter = Convert.ToChar(randValue + 65);

                // Appending the letter to string.
                stringBuilder.Append(letter);
            }

            return stringBuilder.ToString();
        }

        public static string CreateUtcData()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-fff");
        }
    }
}
