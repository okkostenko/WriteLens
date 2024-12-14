from models.types.flag_types import FlagType
from cache import PromptTemplateCache

class SystemPromptGenerator:

    def __init__(self, templates_cache: PromptTemplateCache, template_name: str = 'default') -> None:
        self.templates_cache: PromptTemplateCache = templates_cache
        self.template_name: str = template_name
    
    async def generate(self, flags: list[FlagType]) -> str:
        template = await self.templates_cache.get_item_async(self.template_name)
        template = template.replace("{flags}", '\n'.join('-' + str(f) for f in flags))
        return template