from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from fastapi.responses import JSONResponse
from utils.environment import ALLOWED_ORIGINS
from .routes import rest_routers
from .dependencies import api_key_settings
from .middleware import APIKeyValidationMiddleware

app = FastAPI(version="v1")

app.add_middleware(
    CORSMiddleware,
    allow_origins=ALLOWED_ORIGINS,
    allow_methods=["*"],
    allow_headers=["*"],
    allow_credentials=True
)

app.add_middleware(
    APIKeyValidationMiddleware,
    api_key_settings=api_key_settings
)

@app.get('/health')
async def health_check():
    return JSONResponse({'status': 'OK'})

for router in rest_routers:
    app.include_router(
        router,
        prefix=f"/api/{app.version}",
        tags=[router.prefix.split("/")[-1]]
    )