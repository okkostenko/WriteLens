from typing import Any
from cache import PromptTemplateCache
from models.types.flag_types import FlagType
from models.accessibility_analysis_models import TextAnalysisResult, TextAnalysisResultFlag
from helpers import SystemPromptGenerator, JsonParser
from ..clients import OpenAIClientConfig, OpenAIClientAsync
from logger import get_logger

logger = get_logger(__name__)


class AccessibilityService:

    def __init__(self, cache: PromptTemplateCache) -> None:
        self.cache: PromptTemplateCache = cache

    async def analyze_async(self, text: str, flags: list[FlagType]) -> TextAnalysisResult:
        logger.info("Analyzing text")
        logger.debug(self.cache)
        system_prompt: str = await SystemPromptGenerator(self.cache).generate(flags)
        client = OpenAIClientAsync(
            OpenAIClientConfig(system_prompt = system_prompt)
        )

        response: str = await client.generate(text)
        detected_flags: dict[str, Any] = await JsonParser.parse_response(response)
        
        return TextAnalysisResult(
            flags = [
                TextAnalysisResultFlag(
                    old_text=flag['old_text'],
                    suggestion=flag['replacement'],
                    severity=flag['severity'],
                    flag_type=FlagType.from_str(flag['flag_type'])
                )
                for flag in detected_flags['flags']
            ]
        )
    
    

