import json
from typing import Any
from utils.exceptions import JsonParserException


class JsonParser:

    def __init__(self) -> None:
        pass

    @staticmethod
    async def parse_response(response: str) -> dict[str, Any]:
        try:
            response_json: dict[str, list] = json.loads(await JsonParser.clean_response(response))
            return response_json
        except Exception as e:
            raise JsonParserException(
                f"Text Analysis Exception: \
                Something went wrong while parsing the response {str(e)}"
            )

    @staticmethod
    async def clean_response(response: str) -> str:
        if "```json" in response:
            start_idx = response.index("```json") + 7
            end_index = response.rindex("```")
        elif "```" in response:
            start_idx = response.index("```") + 3
            end_index = response.rindex("```")
        else:
            start_idx = 0
            end_index = len(response)

        return response[start_idx : end_index]