from ai.client import AIClient

class Agent:
    def __init__(self):
        self.ai = AIClient()

    def handle(self, user_input: str) -> str:
        return self.ai.ask(user_input)

    def stream(self, user_input: str):
        return self.ai.stream(user_input)
