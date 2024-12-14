import os
import json
from utils.environment import VALID_API_KEYS_MAPPING_PATH
from utils.exceptions import ApiKeyDoesNotExist

class ApiKeySettings:

    def __init__(self) -> ...:
        self.api_keys: dict[str, list[str]] = {}
        self.load()

    def load(self) -> None:
        with open(VALID_API_KEYS_MAPPING_PATH, 'rb') as f:
            self.api_keys = json.loads(f.read())

    async def get_item_async(self, key: str) -> str:
        await self.__validate_key_exists(key)
        return self.api_keys[key]
    
    async def check_key_exists(self, key: str) -> bool:
        return key in self.api_keys
    
    async def __validate_key_exists(self, key: str) -> None:
        if not await self.check_key_exists(key):
            raise ApiKeyDoesNotExist(
                f"Api Key Does Not Exist: \
                API Key '{key}' does not exist"
            )