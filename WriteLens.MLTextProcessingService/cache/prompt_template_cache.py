import os
from utils.environment import PROMPT_TEMPLATES_PATH
from utils.exceptions import PromptTemplateNotFound


class PromptTemplateCache:

    def __init__(self) -> None:
        self.items: dict[str, str] = {}
        self.load()

    def load(self) -> None:
        templates_files = os.listdir(PROMPT_TEMPLATES_PATH)
        for file_path in templates_files:
            template_name = '.'.join(file_path.split('.')[:-1])
            with open(os.path.join(PROMPT_TEMPLATES_PATH, file_path), 'r') as f:
                self.items[template_name] = f.read()

    async def get_item_async(self, key: str) -> str:
        await self.__validate_key_exists(key)
        return self.items[key]
    
    async def __validate_key_exists(self, key: str) -> None:
        if key not in self.items:
            raise PromptTemplateNotFound(
                f"Prompt Template Not Found: \
                Prompt with name '{key}' does not exist"
            )