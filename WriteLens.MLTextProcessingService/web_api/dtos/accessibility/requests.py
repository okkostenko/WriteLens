from pydantic import BaseModel
from .enums import FlagTypeEnumDto

class AnalyzeTextAccessabilityRequestDto(BaseModel):
    text: str
    flags: list[FlagTypeEnumDto] = FlagTypeEnumDto.all()