import openai

from utils.environment import OPENAI_API_KEY, OPENAI_ORGANIZATION_ID
from utils.exceptions import GenerationException
from logger import get_logger
from .openai_client_config import OpenAIClientConfig

logger = get_logger(__name__)


class OpenAIClientAsync:

    def __init__(self, config: OpenAIClientConfig):
        self.config: OpenAIClientConfig = config
        self.client: openai.AsyncOpenAI = openai.AsyncOpenAI (
            api_key = OPENAI_API_KEY,
            organization = OPENAI_ORGANIZATION_ID,
            max_retries = self.config.max_retries
        )

    async def generate(self, text: str) -> str:
        messages = [
            {"role": "system", "content": self.config.system_prompt},
            {"role": "user", "content": text}
        ]

        try:
            response = await self.client.chat.completions.create(
                model = self.config.model,
                max_tokens = self.config.max_tokens,
                temperature = self.config.temperature,
                store = False,
                messages = messages
            )
            return response.choices[0].message.content
        except openai._exceptions.APIError as e:
            logger.exception(e)
            raise GenerationException(
                f"OpenAI Client Exception: ({e.type}) {e.message}"
            )