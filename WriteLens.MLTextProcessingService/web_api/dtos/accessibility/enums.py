from enum import Enum

class FlagTypeEnumDto(Enum):
    sentence_complexity = "sentenceComplexity"
    passive_voice = "passiveVoice"
    noninclusive_language = "noninclusiveLanguage"
    jargon = "jargon"

    @staticmethod
    def all() -> list['FlagTypeEnumDto']:
        return [
            FlagTypeEnumDto.sentence_complexity,
            FlagTypeEnumDto.passive_voice,
            FlagTypeEnumDto.noninclusive_language,
            FlagTypeEnumDto.jargon
        ]