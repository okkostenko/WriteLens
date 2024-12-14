from fastapi import Depends
from cache import PromptTemplateCache
from settings import ApiKeySettings

prompt_template_cache = PromptTemplateCache()
api_key_settings = ApiKeySettings()

def get_prompt_template_cache() -> PromptTemplateCache:
    return prompt_template_cache

PromptTemplateCacheDep = Depends(get_prompt_template_cache)