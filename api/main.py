from fastapi import FastAPI
from fastapi.responses import StreamingResponse
from dotenv import load_dotenv

from agent.agent import Agent

load_dotenv()

app = FastAPI(title="PythonAI API")
agent = Agent()


@app.get("/")
def root():
    return {"status": "PythonAI API running"}


@app.get("/health")
def health():
    return {"status": "healthy"}


@app.post("/ask")
def ask(prompt: str):
    return {"response": agent.handle(prompt)}


@app.post("/ask-stream")
def ask_stream(prompt: str):

    def event_generator():
        try:
            for text in agent.stream(prompt):
                # SSE framing
                yield f"data: {text}\n\n"

            yield "data: [DONE]\n\n"

        except Exception as e:
            yield f"data: [ERROR] {str(e)}\n\n"
            yield "data: [DONE]\n\n"

    return StreamingResponse(
        event_generator(),
        media_type="text/event-stream",
        headers={
            "Cache-Control": "no-cache",
            "Connection": "keep-alive",
        }
    )
