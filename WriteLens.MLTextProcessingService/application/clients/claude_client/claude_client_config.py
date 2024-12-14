from pydantic import BaseModel

class ClaudeClientConfig(BaseModel):
    model: str = "claude-3-haiku-20240307"
    max_tokens: int = 4096
    temperature: int = 0
    system_prompt: str
    max_retries: int = 3