class ApiKeyDoesNotExist(Exception): ...

class TextAnalysisException(Exception): ...

class JsonParserException(TextAnalysisException): ...

class GenerationException(TextAnalysisException): ...

class PromptTemplateNotFound(TextAnalysisException): ...
