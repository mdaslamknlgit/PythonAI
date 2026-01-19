from typing import List, Dict
from ai.client import AIClient

class LLMFactExtractor:
    def __init__(self):
        self.ai = AIClient()

    def extract(self, text: str) -> List[Dict]:
        """
        Given free-form text, return structured facts.
        Placeholder implementation.
        """
        # Later this will call the LLM with a strict prompt
        # For now, return deterministic dummy data
        return [
            {
                "source": "conversation",
                "content": text
            }
        ]
