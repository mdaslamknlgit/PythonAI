from fastapi import FastAPI
from fastapi.responses import StreamingResponse
from dotenv import load_dotenv

from agent.agent import Agent

from api.auth import create_access_token, get_current_user
from fastapi import Depends


load_dotenv()

app = FastAPI(title="PythonAI API")
agent = Agent()



@app.get("/")
def root():
    return {"status": "PythonAI API running"}


@app.post("/auth/login")
def login(username: str, password: str):
    # TEMP: hardcoded user (replace later)
    if username != "admin" or password != "admin":
        raise HTTPException(status_code=401, detail="Invalid credentials")

    token = create_access_token(username)
    return {
        "access_token": token,
        "token_type": "bearer"
    }


@app.get("/health")
def health():
    return {"status": "healthy"}


@app.post("/ask")
def ask(prompt: str, user: str = Depends(get_current_user)):
    response = agent.handle(prompt)
    return {"response": response}

@app.post("/ask-stream")
def ask_stream(prompt: str,
    user: str = Depends(get_current_user)
    ):

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
