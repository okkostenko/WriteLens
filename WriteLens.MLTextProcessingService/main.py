import os
from web_api import app

if __name__ == "__main__" and os.getenv("ENV") == "dev":
    import uvicorn
    uvicorn.run(app, port=8000)