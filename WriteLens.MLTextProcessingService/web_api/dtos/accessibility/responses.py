from pydantic import BaseModel
from .enums import FlagTypeEnumDto


class TextAnalysisResultFlagResponseDto(BaseModel):
    """Represents the text accessibility rules violation flag."""
    
    old_text: str
    """The flagged text."""

    suggestion: str
    """The suggested replacement to the flagged text."""

    severity: float
    """The severity of the violation."""

    flag_type: FlagTypeEnumDto
    """The type of the flag."""
    

class TextAnalysisResultResponseDto(BaseModel):
    """Represent the result of the analysis."""

    flags: list[TextAnalysisResultFlagResponseDto]
    """The list of the determined the text
    accessibility rules violation flags"""

    @staticmethod
    def from_model(model: BaseModel) -> 'TextAnalysisResultResponseDto':
        return TextAnalysisResultResponseDto(**model.model_dump(mode='json'))
