using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using PrepositionChecker;
using SpelingCheckAPI.Interfaces;
using Microsoft.Extensions.ML;
using CommonCode.CheckResults;

namespace SpelingCheckAPI.Services
{
    public class PrepositionCheckService : IPrepositionCheckService
    {
        private readonly PredictionEnginePool<PrepositionChecker_ML.ModelInput, PrepositionChecker_ML.ModelOutput> _predictionEnginePool;
        private readonly string[] _prepositions = { "s", "so", "z", "zo" };

        public PrepositionCheckService(PredictionEnginePool<PrepositionChecker_ML.ModelInput, PrepositionChecker_ML.ModelOutput> predictionEnginePool)
        {
            _predictionEnginePool = predictionEnginePool;
        }
        public async Task<bool> IsInstrumental(string word)
        {
            var sampleData = new PrepositionChecker_ML.ModelInput()
            {
                Word = word
            };
            var result = await Task.Run(() => _predictionEnginePool.Predict(sampleData));
            if (result.PredictedLabel == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<SpellingCheckResult>> CheckPrepositionsInText(string text)
        {
            var result = new List<SpellingCheckResult>();
            var words = FindWordsAfterPrepositions(text, _prepositions);
            foreach (var preposition in words.Keys)
            {
                switch (preposition)
                {
                    case "s":
                    case "so":
                    case "S":
                    case "So":
                        foreach (var word in words[preposition])
                        {
                            if (!await IsInstrumental(word.word))
                            {
                                var newPreposition = ChangePreposition(preposition);
                                result.Add(new SpellingCheckResult
                                {
                                    ErrorSentence = $"{preposition} {word.word}",
                                    Suggestion = $"{newPreposition} {word.word}",
                                    Index = word.prepositionIndex,
                                    Length = word.length,
                                    Message = $"Nesprávna predložka {preposition}"
                                });
                            }
                        }
                        break;
                    case "z":
                    case "zo":
                    case "Z":
                    case "Zo":
                        foreach (var word in words[preposition])
                        {
                            if (await IsInstrumental(word.word))
                            {
                                var newPreposition = ChangePreposition(preposition);
                                result.Add(new SpellingCheckResult
                                {
                                    ErrorSentence = $"{preposition} {word.word}",
                                    Suggestion = $"{newPreposition} {word.word}",
                                    Index = word.prepositionIndex,
                                    Length = word.length,
                                    Message = $"Nesprávna predložka {preposition}"

                                });
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        private static Dictionary<string, List<(string word, int prepositionIndex, int length)>> FindWordsAfterPrepositions(string text, string[] prepositions)
        {
            string pattern = $@"\b({string.Join("|", prepositions)})\b\s+(\w+)";
            MatchCollection matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);
            var words = new Dictionary<string, List<(string word, int prepositionIndex, int length)>>();
            foreach (Match match in matches)
            {
                string preposition = match.Groups[1].Value;
                string word = match.Groups[2].Value;
                if (!words.ContainsKey(preposition))
                {
                    words[preposition] = new List<(string word, int prepositionIndex, int length)>();
                }
                if (char.IsDigit(word[0]))
                {
                    string subText = text.Substring(match.Index + match.Length);
                    var subMatch = Regex.Match(subText, @"\b\w+\b", RegexOptions.IgnoreCase);
                    if (subMatch.Success)
                    {
                        word = subMatch.Captures[0].Value;
                    }
                }
                words[preposition].Add((word, match.Index, match.Length));
            }
            return words;
        }

        private string ChangePreposition(string preposition)
        {
            switch (preposition[0])
            {
                case 's':
                    return 'z' + preposition.Substring(1);
                case 'S':
                    return 'Z' + preposition.Substring(1);
                case 'z':
                    return 's' + preposition.Substring(1);
                case 'Z':
                    return 'S' + preposition.Substring(1);
                default:
                    return preposition;
            }
        }
    }
}
