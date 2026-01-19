import os
from dotenv import load_dotenv
from openai import OpenAI

load_dotenv()

class AIClient:
    def __init__(self):
        api_key = os.getenv("OPENAI_API_KEY")
        if not api_key:
            raise ValueError("OPENAI_API_KEY not set")

        self._client = OpenAI(api_key=api_key)

        # LOCKED, FINAL system prompt
        self.system_prompt = """
You are a professional software assistant.

Always format answers using proper Markdown:
- Use headings (##) for sections
- Use bullet points or numbered lists
- Use short paragraphs
- Never write lists inline in a single sentence
- Prefer clarity over verbosity
"""

    def ask(self, prompt: str) -> str:
        response = self._client.chat.completions.create(
            model="gpt-4o-mini",
            temperature=0.2,
            messages=[
                {"role": "system", "content": self.system_prompt},
                {"role": "user", "content": prompt}
            ]
        )
        return response.choices[0].message.content.strip()

    def stream(self, prompt: str):
        stream = self._client.chat.completions.create(
            model="gpt-4o-mini",
            temperature=0.2,
            messages=[
                {"role": "system", "content": self.system_prompt},
                {"role": "user", "content": prompt}
            ],
            stream=True
        )

        for chunk in stream:
            if not chunk.choices:
                continue

            delta = chunk.choices[0].delta
            if delta and delta.content:
                yield delta.content
