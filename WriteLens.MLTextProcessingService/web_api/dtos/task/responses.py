from pydantic import BaseModel
from ..accessibility.responses import TextAnalysisResultResponseDto

class TaskCreatedResponseDto(BaseModel):
    """Represents the acknoledgment
    of the analysis task creation.
    """

    task_id: str
    """The ID of the analysis task."""

class TaskStatusResponseDto(BaseModel):
    """Represents the status of the analysis."""

    status: str
    """The status of the analysis."""

class TaskResultResponseDto(BaseModel):
    """Represents the result of the analysis."""

    task_id: str
    """The ID of the analysis task."""

    result: TextAnalysisResultResponseDto
    """The result of the analysis."""