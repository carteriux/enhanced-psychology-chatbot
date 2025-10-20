# Use the official lightweight Python image.
# https://hub.docker.com/_/python
FROM python:3.11.10-slim
ARG PORT=5000
ENV PORT=${PORT}

# Allow statements and log messages to immediately appear in the Knative logs
ENV PYTHONUNBUFFERED True
ENV PROJECT_ID="chatbots-452017"
ENV COLLECTION_ID="psycology-activities-docs"
ENV CHAT_HISTORY="Psycology-bot-history"
ENV DATABASE="(default)"
ENV LOCATION="us-south1"
ENV TEST_USER_ID="Test-id"
ENV BUCKET_NAME="actividades-pdfs"
ENV LANGSMITH_TRACING="true"
ENV LANGSMITH_ENDPOINT="https://api.smith.langchain.com"
ENV LANGSMITH_API_KEY="[REDACTED]"
ENV LANGSMITH_PROJECT="asistente digital psicoterapia"

# Copy local code to the container image.
ENV APP_HOME /app
WORKDIR $APP_HOME
COPY . ./

# Install production dependencies.
RUN pip install --no-cache-dir -r requirements.txt

# Run the web service on container startup. Here we use the gunicorn
# webserver, with one worker process and 8 threads.
# For environments with multiple CPU cores, increase the number of workers
# to be equal to the cores available.
# Timeout is set to 0 to disable the timeouts of the workers to allow Cloud Run to handle instance scaling.
CMD exec gunicorn --bind :$PORT --workers 1 --threads 8 --timeout 0 main:app