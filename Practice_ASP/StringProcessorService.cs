using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Practice_ASP
{
    public class StringProcessorService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<StringProcessorService> _logger;

        public StringProcessorService(
         HttpClient httpClient,
         ILogger<StringProcessorService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }


        // проверка на наличие в строке неподходящих символов
        public List<char> GetUnsuitableChars(string str)
        {
            List<char> unsuitableChars = new List<char>();
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            foreach (char c in str)
            {
                if (!alphabet.Contains(c))
                {
                    unsuitableChars.Add(c);
                }
            }
            return unsuitableChars;
        }


        // удаление из строки случайного символа
        public async Task<string> DeleteRandomChar(string str)
        {
            int randomIndex = await GetRandomNumber(str.Length - 1);

            if (randomIndex < 0 || randomIndex >= str.Length)
            {
                throw new Exception("Индекс вне допустимого диапазона");
            }

            return str.Remove(randomIndex, 1);
        }


        // получаем случайный индекс символа
        public async Task<int> GetRandomNumber(int max)
        {
            try
            {
                string apiUrl = $"http://www.randomnumberapi.com/api/v1.0/random?min=0&max={max}&count=1";
                string responseString = await _httpClient.GetStringAsync(apiUrl);

                int[] numbers = JsonSerializer.Deserialize<int[]>(responseString);
                return numbers[0];
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Ошибка API случайных чисел. Используется локальный генератор");
                Random rnd = new Random();
                return rnd.Next(0, max + 1);
            }
        }

        // Быстрая сортировка

        //функция, преобразующая входную строку в массив символов,
        //вызывающая QuickSort для этого массива и преобразующая
        //его обратно в строку уже отсортированную
        public string QuickSortString(string str)
        {
            char[] charArr = str.ToCharArray();
            QuickSort(charArr, 0, charArr.Length - 1);
            return new string(charArr);
        }


        //сортировка массива символов методом быстрой сортировки
        private void QuickSort(char[] arr, int left, int right)
        {
            if (left < right)
            {
                int supportElementIndex = Partition(arr, left, right);
                QuickSort(arr, left, supportElementIndex - 1);
                QuickSort(arr, supportElementIndex + 1, right);
            }
        }


        // функция разделения, выбирающая опорный элемент и
        // располагающая элементы меньше опорного слева, а больше – справа
        private int Partition(char[] arr, int left, int right)
        {
            char supportElement = arr[right];
            int i = left - 1;
            for (int j = left; j < right; j++)
            {
                if (arr[j] <= supportElement)
                {
                    i++;
                    Swap(arr, i, j);
                }
            }
            Swap(arr, i + 1, right);
            return i + 1;
        }

        private void Swap(char[] arr, int i, int j)
        {
            char temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

        // Сортировка деревом

        //узел дерева
        private class Node
        {
            public char Value { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }

            public Node(char value)
            {
                Value = value;
            }
        }


        //функция для сортировки деревом
        public string TreeSortString(string str)
        {
            Node root = null;
            foreach (char c in str)
            {
                root = Insert(root, c);
            }

            StringBuilder sortedString = new StringBuilder();
            InOrderTraversal(root, sortedString);
            return sortedString.ToString();
        }


        // вставка узла в дерево
        private Node Insert(Node root, char value)
        {
            if (root == null)
            {
                return new Node(value);
            }

            if (value < root.Value)
            {
                root.Left = Insert(root.Left, value);
            }
            else
            {
                root.Right = Insert(root.Right, value);
            }
            return root;
        }


        //симметричный обход дерева
        private void InOrderTraversal(Node node, StringBuilder sb)
        {
            if (node != null)
            {
                InOrderTraversal(node.Left, sb);
                sb.Append(node.Value);
                InOrderTraversal(node.Right, sb);
            }
        }


        // получение самой длинной подстроки,
        //начинающейся и заканчивающейся на гласных
        public string GetLongestVowelSubstring(string str)
        {
            string vowels = "aeiouy";
            string longestSubstring = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (vowels.Contains(str[i]))
                {
                    for (int j = i; j < str.Length; j++)
                    {
                        if (vowels.Contains(str[j]))
                        {
                            string currentSubstring = str.Substring(i, j - i + 1);
                            if (currentSubstring.Length > longestSubstring.Length)
                            {
                                longestSubstring = currentSubstring;
                            }
                        }
                    }
                }
            }
            return longestSubstring;
        }


        // подсчет повторений символов
        // для удобства переделал в словарь
        public Dictionary<char, int> GetCharsRepetitions(string str)
        {
            var repetitions = new Dictionary<char, int>();
            foreach (char c in str)
            {
                if (repetitions.ContainsKey(c))
                {
                    repetitions[c]++;
                }
                else
                {
                    repetitions[c] = 1;
                }
            }
            return repetitions;
        }


        // проверка строки на чётность
        public bool IsEven(string str)
        {
            int length = str.Length;
            if (length % 2 == 0)
            {
                return true;
            }
            else return false;
        }


        // основная обработка строки
        public string StringProcessing(string str)
        {
            if (GetUnsuitableChars(str).Count != 0)
            {
                throw new ArgumentException("Строка содержит неподходящие символы");
            }
            if (IsEven(str))
                return EvenStringProcessing(str);
            else
                return OddStringProcessing(str);
        }


        // обработка четной строки
        private string EvenStringProcessing(string str)
        {
            int halfLength = str.Length / 2;
            string firstHalf = str.Substring(0, halfLength);
            string secondHalf = str.Substring(halfLength);
            firstHalf = ReverseString(firstHalf);
            secondHalf = ReverseString(secondHalf);
            string newStr = string.Concat(firstHalf, secondHalf);
            return newStr;
        }


        // обработка нечетной строки
        private string OddStringProcessing(string str)
        {
            string reversedStr = ReverseString(str);
            string newStr = string.Concat(reversedStr, str);
            return newStr;
        }


        // переворот строки
        private string ReverseString(string str)
        {
            string newStr = "";
            for (int i = str.Length - 1; i >= 0; i--)
            {
                newStr += str[i];
            }
            return newStr;
        }
    }
}
