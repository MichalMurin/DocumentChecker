using CommonCode.CheckResults;
using System.Text.Json.Serialization;

namespace CommonCode.ApiModels
{
    public static class LanguageToolParser
    {
        /// <summary>
        /// Transforms the LanguageToolApiResult to a list of SpellingCheckResult.
        /// </summary>
        /// <param name="lTresult">The LanguageToolApiResult to transform.</param>
        /// <returns>A list of SpellingCheckResult.</returns>
        public static List<SpellingCheckResult>? TransformLtResultToCheckResult(LanguageToolApiResult? lTresult)
        {
            if (lTresult is not null && lTresult.Matches is not null)
            {
                var result = new List<SpellingCheckResult>();
                foreach (var match in lTresult.Matches)
                {
                    var newResult = new SpellingCheckResult
                    {
                        Message = match.Message ?? "No message to show",
                        ShortMessage = match.ShortMessage ?? "No message to show",
                        ErrorSentence = match.Sentence ?? "No sentence to show",
                        Index = match.Offset,
                        Length = match.Length
                    };
                    if (match.Replacements is not null && match.Replacements.Count > 0)
                    {
                        newResult.Suggestion = match.Replacements[0].Value ?? string.Empty;
                    }
                    result.Add(newResult);
                }
                return result;
            }
            else
            {
                return null;
            }
        }

    }


    public class DetectedLanguage
    {
        /// <summary>
        /// The detected language.
        /// </summary>
        [JsonPropertyName("language")]
        public string? Language { get; set; }

        /// <summary>
        /// The rate of the detected language.
        /// </summary>
        [JsonPropertyName("rate")]
        public double? Rate { get; set; }
    }

    public class Context
    {
        /// <summary>
        /// The text of the context.
        /// </summary>
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        /// <summary>
        /// The offset of the context.
        /// </summary>
        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// The length of the context.
        /// </summary>
        [JsonPropertyName("length")]
        public int Length { get; set; }
    }

    public class Replacement
    {
        /// <summary>
        /// The value of the replacement.
        /// </summary>
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }

    public class Category
    {
        /// <summary>
        /// The ID of the category.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// The name of the category.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class Rule
    {
        /// <summary>
        /// The ID of the rule.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// The description of the rule.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// The issue type of the rule.
        /// </summary>
        [JsonPropertyName("issueType")]
        public string? IssueType { get; set; }

        /// <summary>
        /// The category of the rule.
        /// </summary>
        [JsonPropertyName("category")]
        public Category? Category { get; set; }

        /// <summary>
        /// Indicates if the rule is premium.
        /// </summary>
        [JsonPropertyName("isPremium")]
        public bool IsPremium { get; set; }
    }

    public class Type
    {
        /// <summary>
        /// The type name.
        /// </summary>
        [JsonPropertyName("typeName")]
        public string? TypeName { get; set; }
    }

    public class Match
    {
        /// <summary>
        /// The message of the match.
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        /// <summary>
        /// The short message of the match.
        /// </summary>
        [JsonPropertyName("shortMessage")]
        public string? ShortMessage { get; set; }

        /// <summary>
        /// The list of replacements for the match.
        /// </summary>
        [JsonPropertyName("replacements")]
        public List<Replacement>? Replacements { get; set; }

        /// <summary>
        /// The offset of the match.
        /// </summary>
        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// The length of the match.
        /// </summary>
        [JsonPropertyName("length")]
        public int Length { get; set; }

        /// <summary>
        /// The context of the match.
        /// </summary>
        [JsonPropertyName("context")]
        public Context? Context { get; set; }

        /// <summary>
        /// The sentence of the match.
        /// </summary>
        [JsonPropertyName("sentence")]
        public string? Sentence { get; set; }

        /// <summary>
        /// The type of the match.
        /// </summary>
        [JsonPropertyName("type")]
        public Type? Type { get; set; }

        /// <summary>
        /// The rule of the match.
        /// </summary>
        [JsonPropertyName("rule")]
        public Rule? Rule { get; set; }

        /// <summary>
        /// Indicates if the match should be ignored for incomplete sentence.
        /// </summary>
        [JsonPropertyName("ignoreForIncompleteSentence")]
        public bool IgnoreForIncompleteSentence { get; set; }

        /// <summary>
        /// The context for sure match.
        /// </summary>
        [JsonPropertyName("contextForSureMatch")]
        public int ContextForSureMatch { get; set; }
    }

    public class Software
    {
        /// <summary>
        /// The name of the software.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// The version of the software.
        /// </summary>
        [JsonPropertyName("version")]
        public string? Version { get; set; }

        /// <summary>
        /// The build date of the software.
        /// </summary>
        [JsonPropertyName("buildDate")]
        public string? BuildDate { get; set; }

        /// <summary>
        /// The API version of the software.
        /// </summary>
        [JsonPropertyName("apiVersion")]
        public int ApiVersion { get; set; }

        /// <summary>
        /// Indicates if the software is premium.
        /// </summary>
        [JsonPropertyName("premium")]
        public bool Premium { get; set; }

        /// <summary>
        /// The premium hint of the software.
        /// </summary>
        [JsonPropertyName("premiumHint")]
        public string? PremiumHint { get; set; }

        /// <summary>
        /// The status of the software.
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }

    public class Warnings
    {
        /// <summary>
        /// Indicates if the results are incomplete.
        /// </summary>
        [JsonPropertyName("incompleteResults")]
        public bool IncompleteResults { get; set; }
    }

    public class DetectedLanguageResult
    {
        /// <summary>
        /// The name of the detected language.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// The code of the detected language.
        /// </summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        /// <summary>
        /// The detected language.
        /// </summary>
        [JsonPropertyName("detectedLanguage")]
        public DetectedLanguage? DetectedLanguage { get; set; }
    }

    public class SentenceRange
    {
        /// <summary>
        /// The starting index of the sentence range.
        /// </summary>
        [JsonPropertyName("from")]
        public int From { get; set; }

        /// <summary>
        /// The ending index of the sentence range.
        /// </summary>
        [JsonPropertyName("to")]
        public int To { get; set; }
    }

    public class ExtendedSentenceRange
    {
        /// <summary>
        /// The starting index of the extended sentence range.
        /// </summary>
        [JsonPropertyName("from")]
        public int From { get; set; }

        /// <summary>
        /// The ending index of the extended sentence range.
        /// </summary>
        [JsonPropertyName("to")]
        public int To { get; set; }

        /// <summary>
        /// The list of detected languages in the extended sentence range.
        /// </summary>
        [JsonPropertyName("detectedLanguages")]
        public List<DetectedLanguage>? DetectedLanguages { get; set; }
    }

    public class LanguageToolApiResult
    {
        /// <summary>
        /// The software information.
        /// </summary>
        [JsonPropertyName("software")]
        public Software? Software { get; set; }

        /// <summary>
        /// The warnings.
        /// </summary>
        [JsonPropertyName("warnings")]
        public Warnings? Warnings { get; set; }

        /// <summary>
        /// The detected language information.
        /// </summary>
        [JsonPropertyName("language")]
        public DetectedLanguageResult? Language { get; set; }

        /// <summary>
        /// The list of matches.
        /// </summary>
        [JsonPropertyName("matches")]
        public List<Match>? Matches { get; set; }

        /// <summary>
        /// The list of sentence ranges.
        /// </summary>
        [JsonPropertyName("sentenceRanges")]
        public List<List<int>>? SentenceRanges { get; set; }

        /// <summary>
        /// The list of extended sentence ranges.
        /// </summary>
        [JsonPropertyName("extendedSentenceRanges")]
        public List<ExtendedSentenceRange>? ExtendedSentenceRanges { get; set; }
    }

}