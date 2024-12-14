from typing import Callable, Any, MutableMapping, Awaitable
from fastapi import Request, Response, HTTPException
from starlette.middleware.base import BaseHTTPMiddleware, DispatchFunction, RequestResponseEndpoint
from starlette.types import ASGIApp
from settings import ApiKeySettings


class APIKeyValidationMiddleware(BaseHTTPMiddleware):

    def __init__(
            self,
            app: Callable[
                [MutableMapping[str, Any],Callable[[], Awaitable[MutableMapping[str, Any]]], Callable[[MutableMapping[str, Any]], Awaitable[None]]],
                Awaitable[None]],
            api_key_settings: ApiKeySettings,
            dispatch: Callable[
                [Request, Callable[[Request], Awaitable[Response]]],
                Awaitable[Response]] | None = None,
        ) -> None:
        super().__init__(app, dispatch)
        self.api_key_settings = api_key_settings

    async def dispatch(self, request: Request, call_next: Callable[[Request], Awaitable[Response]]) -> Response:
        api_key_header: str | None = request.headers.get("X-API-KEY")
        if not api_key_header:
            raise HTTPException(401, detail="X-API-KEY Header is missing")
        if not await self.api_key_settings.check_key_exists(api_key_header):
            raise HTTPException(401, detail="Invalid API Key")
        
        audience_header: str | None = request.headers.get("X-Audience")
        if not audience_header:
            raise HTTPException(403, detail="X-Audience Header is missing")
        if audience_header not in await self.api_key_settings.get_item_async(api_key_header):
            raise HTTPException(403, detail="Invalid Audience")
        
        response: Response = await call_next(request)
        return response