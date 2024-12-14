import anthropic

from utils.environment import ANTHROPIC_API_KEY
from utils.exceptions import GenerationException
from logger import get_logger
from .claude_client_config import ClaudeClientConfig

logger = get_logger(__name__)


class ClaudeClientAsync:

    def __init__(self, config: ClaudeClientConfig):
        self.config: ClaudeClientConfig = config
        self.client: anthropic.AsyncAnthropic = anthropic.AsyncAnthropic(
            api_key = ANTHROPIC_API_KEY,
            max_retries = self.config.max_retries
        )

    async def generate(self, text: str) -> str:
        message = {"role": "user", "content": text}

        try:
            response: anthropic.types.Message = await self.client.messages.create(
                model = self.config.model,
                max_tokens = self.config.max_tokens,
                temperature = self.config.temperature,
                messages = [message]
            )
            return response.content[0].text
        except anthropic._exceptions.AnthropicError as e:
            logger.exception(str(e))
            raise GenerationException(
                f"Claude Client Exception: {str(e)}"
            )