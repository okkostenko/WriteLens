# For more information, please refer to https://aka.ms/vscode-docker-python
FROM python:3.12.4-slim


EXPOSE 8000

ENV PYTHONUNBUFFERED=1

COPY ./requirements.txt .

RUN apt-get update

RUN python -m pip install --upgrade pip

# RUN python -m pip --no-cache-dir install -r requirements.txt
RUN python -m pip install -r requirements.txt


WORKDIR /app
COPY . /app

RUN mkdir -p /app/static


CMD ["gunicorn", "--bind", "0.0.0.0:8000", "--workers", "4", "-k", "uvicorn.workers.UvicornWorker", "api:app"]