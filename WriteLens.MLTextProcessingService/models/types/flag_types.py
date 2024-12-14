from enum import Enum

class FlagType(Enum):
    sentence_complexity = "sentenceComplexity"
    passive_voice = "passiveVoice"
    noninclusive_language = "noninclusiveLanguage"
    jargon = "jargon"

    def __str__(self) -> str:
        match self:
            case FlagType.sentence_complexity:
                return "complex sentence structure"
            case FlagType.passive_voice:
                return "passive voice"
            case FlagType.noninclusive_language:
                return "noninclusive language"
            case FlagType.jargon:
                return "jargon"
            case _:
                return self.value
            
    @staticmethod
    def from_str(flag: str) -> 'FlagType':
        if flag == "complex sentence structure":
            return FlagType.sentence_complexity
        elif flag == "passive voice":
            return FlagType.passive_voice
        elif flag == "noninclusive language":
            return FlagType.noninclusive_language
        elif flag == "jargon":
            return FlagType.jargon