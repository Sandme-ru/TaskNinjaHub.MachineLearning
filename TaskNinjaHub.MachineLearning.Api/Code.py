import torch
from transformers import BertTokenizer, BertModel

def main(text):
    # Загрузка токенизатора и модели RuBERT
    tokenizer = BertTokenizer.from_pretrained("DeepPavlov/rubert-base-cased")
    model = BertModel.from_pretrained("DeepPavlov/rubert-base-cased")

    # Токенизация и предварительная обработка текста
    inputs = tokenizer(text, return_tensors="pt", padding=True, truncation=True)

    # Применение модели RuBERT к тексту
    outputs = model(**inputs)

    # Получение эмбеддингов текста
    embeddings = outputs.last_hidden_state

    # Возврат эмбеддингов текста
    return embeddings.tolist()