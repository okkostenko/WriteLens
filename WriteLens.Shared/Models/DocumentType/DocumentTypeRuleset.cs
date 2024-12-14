namespace WriteLens.Shared.Models;


public class DocumentTypeRuleset
{
    public double ReadabilityThreshold { get; set; }

    public double ComplexityPenalty { get; set; }

    public int MaxSentenceLength { get; set; }

    public bool AllowPassiveVoice { get; set; }

    public bool AllowJargon { get; set; }

    public double JargonPenalty { get; set; }

    public double PassiveVoicePenalty { get; set; }

    public double SentenceComplexityPenalty { get; set; }

    public double NonInclusiveLanguagePenalty { get; set; }

    public double MaxComplexityScore { get; set; }

    public double MinSectionReadabilityScore { get; set; }

    public int MaxFlagsPerSection { get; set; }
}