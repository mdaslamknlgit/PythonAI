import json
import os
from typing import List, Dict

class FactsStore:
    def __init__(self, file_path: str = "facts.json"):
        self.file_path = file_path
        if not os.path.exists(self.file_path):
            self._write([])

    def _read(self) -> List[Dict]:
        try:
            with open(self.file_path, "r", encoding="utf-8") as f:
                content = f.read().strip()
                if not content:
                    return []
                return json.loads(content)
        except json.JSONDecodeError:
            return []

    def _write(self, facts: List[Dict]):
        with open(self.file_path, "w", encoding="utf-8") as f:
            json.dump(facts, f, indent=2)

    def get_all(self) -> List[Dict]:
        return self._read()

    def add(self, fact: Dict):
        facts = self._read()
        facts.append(fact)
        self._write(facts)
