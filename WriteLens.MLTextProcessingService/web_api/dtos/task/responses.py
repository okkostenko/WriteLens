from pydantic import BaseModel
from ..accessibility.responses import TextAnalysisResultResponseDto

class TaskCreatedResponseDto(BaseModel):
    task_id: str

class TaskStatusResponseDto(BaseModel):
    status: str

class TaskResultResponseDto(BaseModel):
    task_id: str
    result: TextAnalysisResultResponseDto