from pydantic import BaseModel

class OpenAIClientConfig(BaseModel):
    model: str = "gpt-4o-mini-2024-07-18"
    max_tokens: int = 4096
    temperature: int = 0
    system_prompt: str
    max_retries: int = 3