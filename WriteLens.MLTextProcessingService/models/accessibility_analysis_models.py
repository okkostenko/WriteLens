from pydantic import BaseModel
from .types.flag_types import FlagType


class TextAnalysisResultFlag(BaseModel):
    old_text: str
    suggestion: str
    severity: float
    flag_type: FlagType


class TextAnalysisResult(BaseModel):
    flags: list[TextAnalysisResultFlag]