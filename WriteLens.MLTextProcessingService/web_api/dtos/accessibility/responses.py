from pydantic import BaseModel
from .enums import FlagTypeEnumDto


# class TextAnalysisResultFlag(BaseModel):
#     old_text: str
#     suggestion: str
#     severity: float
#     flag_type: FlagType


# class TextAnalysisResult(BaseModel):
#     flags: list[TextAnalysisResultFlag]

class TextAnalysisResultFlagResponseDto(BaseModel):
    old_text: str
    suggestion: str
    severity: float
    flag_type: FlagTypeEnumDto

class TextAnalysisResultResponseDto(BaseModel):
    flags: list[TextAnalysisResultFlagResponseDto]

    @staticmethod
    def from_model(model: BaseModel) -> 'TextAnalysisResultResponseDto':
        return TextAnalysisResultResponseDto(**model.model_dump(mode='json'))
