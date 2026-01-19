from memory.facts_store import FactsStore

store = FactsStore()
store.add({"type": "test", "value": "hello"})
print(store.get_all())
