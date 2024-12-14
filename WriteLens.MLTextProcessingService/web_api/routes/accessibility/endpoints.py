from fastapi import APIRouter, Request, HTTPException
from fastapi.responses import JSONResponse
from celery.result import AsyncResult

from application.services import AccessibilityService
from cache import PromptTemplateCache
from models.types.flag_types import FlagType
from task_queue import task_queue, create_task
from ...dependencies import PromptTemplateCacheDep
from ...dtos.accessibility.requests import *
from ...dtos.accessibility.responses import *
from ...dtos.task.responses import *


accessibility_router = APIRouter(prefix='/accessibility')

@accessibility_router.get(
    "/task/{task_id}/status",
    response_class=JSONResponse,
    response_model=TaskStatusResponseDto
)
async def get_status(task_id):
    task_result = AsyncResult(task_id, app=task_queue)
    return TaskStatusResponseDto(status=task_result.status)


@accessibility_router.get(
    "/task/{task_id}/result",
    response_class=JSONResponse,
    response_model=TaskResultResponseDto
)
async def get_result(task_id):
    task_result = AsyncResult(task_id)
    if issubclass(type(task_result.result), Exception):
        raise task_result.result
    
    return TaskResultResponseDto(
        task_id=task_id,
        result=TextAnalysisResultResponseDto(**task_result.result)
    )


@accessibility_router.post(
    '/analyze',
    response_class=JSONResponse,
    response_model = TaskCreatedResponseDto
)
async def analyze_Accessibility(
    body: AnalyzeTextAccessabilityRequestDto
):
    task = create_task.delay(
        body.text,
        [flag.value for flag in body.flags]
    )

    return TaskCreatedResponseDto(task_id=task.id)