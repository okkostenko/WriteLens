import asyncio
from celery import Celery
from asgiref.sync import async_to_sync
from utils.environment import CELERY_BROKER_URL

from application.services import AccessibilityService
from cache import cache
from models.types.flag_types import FlagType
from models.accessibility_analysis_models import TextAnalysisResult

task_queue = Celery('tasks', broker=CELERY_BROKER_URL, backend=CELERY_BROKER_URL)

@task_queue.task(name="create_task")
def create_task(text: str, flags: list[str]) -> TextAnalysisResult:
    result = async_to_sync(AccessibilityService(cache).analyze_async)(
        text = text,
        flags = [FlagType(flag) for flag in flags]
    )
    return result.model_dump(mode='json')