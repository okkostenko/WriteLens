using WriteLens.Shared.Types;

public static class FlagSeverityEnumHelper
{
    public static DocumentContentFlagSeverity FromDecimal(decimal severity)
    {
        if (severity <= 0.3m)
            return DocumentContentFlagSeverity.low;
        else if (severity <= 0.7m)
            return DocumentContentFlagSeverity.moderate;
        else
            return DocumentContentFlagSeverity.high;
    }
}