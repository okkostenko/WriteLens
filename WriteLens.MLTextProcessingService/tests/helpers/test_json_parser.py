import pytest
import asyncio
from helpers import JsonParser
from utils.exceptions import JsonParserException

class TestJsonParser:

    def test_valid_makrdown_formated_json(self) -> None:
        test_case = (
            """
            You are a professional text editor. Your task is to analyze the text, inclosed in ''', and select parts of it which can be described by one of the following:
            * jargon
            * passive voice detection
            * not inclusive language
            ```json
            {
                "flags": [
                    {
                        "old_text": "the part of the text that was found to satisfy the checks",
                        "replacement": "the replacement text",
                        "severity": 0.1,
                        "flag_type": "jargon"
                    }
                ]
            }
            ```
            """,

            {
                "flags": [
                    {
                        "old_text": "the part of the text that was found to satisfy the checks",
                        "replacement": "the replacement text",
                        "severity": 0.1,
                        "flag_type": "jargon"
                    }
                ]
            }
        )
        result = asyncio.run(JsonParser.parse_response(test_case[0]))
        assert result == test_case[1]

    def test_valid_makrdown_formated_json_no_language_provided(self) -> None:
        test_case = (
            """
            ```
            {
                "flags": [
                    {
                        "old_text": "the part of the text that was found to satisfy the checks",
                        "replacement": "the replacement text",
                        "severity": 0.1,
                        "flag_type": "jargon"
                    }
                ]
            }
            ```
            """,

            {
                "flags": [
                    {
                        "old_text": "the part of the text that was found to satisfy the checks",
                        "replacement": "the replacement text",
                        "severity": 0.1,
                        "flag_type": "jargon"
                    }
                ]
            }
        )
        result = asyncio.run(JsonParser.parse_response(test_case[0]))
        assert result == test_case[1]

    def test_valid_json_string(self) -> None:
        test_case = (
            """
            {
                "flags": [
                    {
                        "old_text": "the part of the text that was found to satisfy the checks",
                        "replacement": "the replacement text",
                        "severity": 0.1,
                        "flag_type": "jargon"
                    }
                ]
            }
            """,

            {
                "flags": [
                    {
                        "old_text": "the part of the text that was found to satisfy the checks",
                        "replacement": "the replacement text",
                        "severity": 0.1,
                        "flag_type": "jargon"
                    }
                ]
            }
        )
        result = asyncio.run(JsonParser.parse_response(test_case[0]))
        assert result == test_case[1]

    def test_raises_exception(self) -> None:
        test_case = """
            You are a professional text editor. Your task is to analyze the text, inclosed in ''', and select parts of it which can be described by one of the following:
            * jargon
            * passive voice detection
            * not inclusive language
            * complex sentence structure

            I want you to write the replacement text (suggestion) and calculate the relative severity for each of the found parts. If the sentence structure is complex, select it all as well as the part of it that satisfy other checks, and as a replacement divide the sentence into the more readable and accessible ones.
            It is very important that you return your response as JSON of the following structure:
            """
        
        with pytest.raises(JsonParserException) as exc:
            asyncio.run(JsonParser.parse_response(test_case))

        assert exc.type is JsonParserException