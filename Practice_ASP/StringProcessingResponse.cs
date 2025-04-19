namespace Practice_ASP
{
    public class StringProcessingResponse
    {
        public string ProcessedString { get; set; }
        public Dictionary<char, int> CharRepetitions { get; set; }
        public string LongestVowelSubstring { get; set; }
        public string QuickSortedString { get; set; }
        public string TreeSortedString { get; set; }
        public string ShortenedString { get; set; }
        public char? RemovedChar { get; set; }
        public int? RemovedCharIndex { get; set; }
    }
}
