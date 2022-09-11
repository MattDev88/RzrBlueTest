using System.Text.RegularExpressions;

namespace Task_1
{
    public class Anagram
    {
        /// <summary>
        /// Execute anagram test
        /// </summary>
        /// <param name="word1"></param>
        /// <param name="word2"></param>
        /// <returns></returns>
        public bool Execute(string word1, string word2)
        {
            if(word1.Length < 2 || word2.Length < 2)
            {
                return false;
            }
            return CharArraySorter(word1) == CharArraySorter(word2);
        }

        /// <summary>
        /// Cleans, converts to char array, orders and returns a new string
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private static string CharArraySorter(string word)
        {
            char[] i = Cleans(word)
                .ToCharArray()
                .OrderBy(m => m)
                .ToArray();

            return new string(i);
        }


        /// <summary>
        /// Replace anthting that is not alpha characters
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private static string Cleans(string word)
        {
            return Regex.Replace(word, "[^a-zA-Z]", string.Empty).Normalize();

        }

    }
}