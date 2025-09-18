using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Xim.Library.Utils
{
    public class SplittingText
    {
        public string text {  get; set; }
        public int checkingDocumentVersionId { get; set; }
    }

    public class Sentence
    {
        public string Content { get; set; }
        public int StartIndex { get; set; }
        public string Hash { get; set; }
        public int CheckingDocumentVersionId { get; set; }
        public int Order { get; set; }
        public int Type { get; set; }


        public override string ToString()
        {
            return $"Content: {Content}, StartIndex: {StartIndex}, Hash: {Hash}, " +
                   $"CheckingDocumentVersionId: {CheckingDocumentVersionId}, Order: {Order}, Type: {Type}";
        }
    }

    public class SentenceSplitter
    {
        public static List<Sentence> SplitSentences(SplittingText splittingText)
        {
            var text = splittingText.text;
            var checkingDocumentVersionId = splittingText.checkingDocumentVersionId;

            // Regular expression to split sentences based on common punctuation marks in Vietnamese
            var sentenceRegex = new Regex(@"[^.!?:]*[.!?:](?=\s|\n)", RegexOptions.Compiled);
            var matches = sentenceRegex.Matches(text);
            var sentences = new List<Sentence>();

            foreach (Match match in matches)
            {
                sentences.Add(new Sentence
                {
                    Content = match.Value.Trim(),
                    StartIndex = match.Index
                });
            }

            // Filtering and mapping sentences
            sentences = sentences
                .Where(sentence => !string.IsNullOrEmpty(sentence.Content))
                .Where(sentence => Regex.IsMatch(sentence.Content, @"\p{L}"))
                .Where(sentence => sentence.Content.Length > 30)
                .Select((sentence, index) => new Sentence
                {
                    Content = sentence.Content,
                    StartIndex = sentence.StartIndex,
                    Hash = SdbmHash(sentence.Content).ToString(),
                    CheckingDocumentVersionId = checkingDocumentVersionId,
                    Order = index + 1
                })
                .ToList();

            // Determine the type for each sentence
            foreach (var sentence in sentences)
            {
                sentence.Type = IsInQuotes(text, sentence.StartIndex)
                    ? 2
                    : sentence.Content.Contains('"')
                        ? 3
                        : 1;
            }

            return sentences;
        }

        private static int SdbmHash(string str)
        {
            int hash = 0;
            foreach (var c in str)
            {
                hash = c + (hash << 6) + (hash << 16) - hash;
            }
            return hash;
        }

        private static bool IsInQuotes(string text, int index)
        {
            // Implement logic to determine if a sentence is within quotes
            // This could involve counting the number of quotes before and after the index
            int quoteCountBefore = text.Substring(0, index).Count(c => c == '"');
            return quoteCountBefore % 2 != 0;
        }
    }

}
