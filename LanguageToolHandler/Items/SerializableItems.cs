using LanguageToolHandler.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LanguageToolHandler.Items
{
    public class DetectedLanguage
    {
        [JsonPropertyName("language")]
        public string? Language { get; set; }

        [JsonPropertyName("rate")]
        public double? Rate { get; set; }
    }

    public class Context
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }
    }

    public class Replacement
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }

    public class Category
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class Rule
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("issueType")]
        public string? IssueType { get; set; }

        [JsonPropertyName("category")]
        public Category? Category { get; set; }

        [JsonPropertyName("isPremium")]
        public bool IsPremium { get; set; }
    }

    public class Type
    {
        [JsonPropertyName("typeName")]
        public string? TypeName { get; set; }
    }

    public class Match
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("shortMessage")]
        public string? ShortMessage { get; set; }

        [JsonPropertyName("replacements")]
        public List<Replacement>? Replacements { get; set; }

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }

        [JsonPropertyName("context")]
        public Context? Context { get; set; }

        [JsonPropertyName("sentence")]
        public string? Sentence { get; set; }

        [JsonPropertyName("type")]
        public Type? Type { get; set; }

        [JsonPropertyName("rule")]
        public Rule? Rule { get; set; }

        [JsonPropertyName("ignoreForIncompleteSentence")]
        public bool IgnoreForIncompleteSentence { get; set; }

        [JsonPropertyName("contextForSureMatch")]
        public int ContextForSureMatch { get; set; }
    }

    public class Software
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("version")]
        public string? Version { get; set; }

        [JsonPropertyName("buildDate")]
        public string? BuildDate { get; set; }

        [JsonPropertyName("apiVersion")]
        public int ApiVersion { get; set; }

        [JsonPropertyName("premium")]
        public bool Premium { get; set; }

        [JsonPropertyName("premiumHint")]
        public string? PremiumHint { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }

    public class Warnings
    {
        [JsonPropertyName("incompleteResults")]
        public bool IncompleteResults { get; set; }
    }

    public class DetectedLanguageResult
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("detectedLanguage")]
        public DetectedLanguage? DetectedLanguage { get; set; }
    }

    public class SentenceRange
    {
        [JsonPropertyName("from")]
        public int From { get; set; }

        [JsonPropertyName("to")]
        public int To { get; set; }
    }

    public class ExtendedSentenceRange
    {
        [JsonPropertyName("from")]
        public int From { get; set; }

        [JsonPropertyName("to")]
        public int To { get; set; }

        [JsonPropertyName("detectedLanguages")]
        public List<DetectedLanguage>? DetectedLanguages { get; set; }
    }

    public class LanguageToolResult
    {
        [JsonPropertyName("software")]
        public Software? Software { get; set; }

        [JsonPropertyName("warnings")]
        public Warnings? Warnings { get; set; }

        [JsonPropertyName("language")]
        public DetectedLanguageResult? Language { get; set; }

        [JsonPropertyName("matches")]
        public List<Match>? Matches { get; set; }

        [JsonPropertyName("sentenceRanges")]
        public List<List<int>>? SentenceRanges { get; set; }

        [JsonPropertyName("extendedSentenceRanges")]
        public List<ExtendedSentenceRange>? ExtendedSentenceRanges { get; set; }
    }

}