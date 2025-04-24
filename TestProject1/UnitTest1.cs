using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Practice_ASP;

namespace TestProject1
{
    
    [TestFixture]
    public class Tests
    {
        StringProcessorService _processor;
        HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _processor = new StringProcessorService(_httpClient, 
                NullLogger<StringProcessorService>.Instance,
                Options.Create(new AppConfig()));
        }

        // 1 ������� (��������� ������ � �������� �����)
        [Test]
        [TestCase("computer", "pmocretu")] // ������ ������
        [TestCase("space", "ecapsspace")] // �������� ������
        [TestCase("a", "aa")] // 1 ������
        [TestCase("", "")] // ������ ������
        public void Zadanie1Test(string str, string expected)
        {
            var actual = _processor.StringProcessing(str);
            Assert.AreEqual(expected, actual);
        }

        // 2 ������� (����� ������������ ��������)
        [Test]
        public void Zadanie2Test()
        {
            string str = "abcde123ABCD���";
            var actual = _processor.GetUnsuitableChars(str);
            Assert.AreEqual("123ABCD���", actual);
        }

        // 3 ������� (������� ����� ���������� ��������)
        [Test]
        public void Zadanie3Test()
        {
            string str = "aabbbccd";
            var actual = _processor.GetCharsRepetitions(str);
            Assert.AreEqual(2, actual['a']);
            Assert.AreEqual(3, actual['b']);
            Assert.AreEqual(2, actual['c']);
            Assert.AreEqual(1, actual['d']);
        }

        // 4 ������� (����� ����� ������� ���������,
        //������������ � �������������� �� �������)
        [Test]
        [TestCase("taxtelecom", "axteleco")]
        [TestCase("flower", "owe")]
        [TestCase("erase", "erase")]
        [TestCase("bcdfg", "")]
        public void Zadanie4Test(string str, string expected)
        {
            var actual = _processor.GetLongestVowelSubstring(str);
            Assert.AreEqual(expected, actual);
        }

        //����� ��� 5 ������� 

        [Test]
        [TestCase("practice", "acceiprt")]
        [TestCase("ba", "ab")]
        [TestCase("a", "a")]
        [TestCase("", "")]
        public void QuickSortTest(string str, string expected)
        {
            var actual = _processor.QuickSortString(str);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("practice", "acceiprt")]
        [TestCase("ba", "ab")]
        [TestCase("a", "a")]
        [TestCase("", "")]
        public void TreeSortTest(string str, string expected)
        {
            var actual = _processor.TreeSortString(str);
            Assert.AreEqual(expected, actual);
        }
    }
}