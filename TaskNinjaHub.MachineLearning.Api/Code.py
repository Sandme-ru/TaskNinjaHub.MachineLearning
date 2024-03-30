import torch
from transformers import BertTokenizer, BertModel

def main(text):
    # �������� ������������ � ������ RuBERT
    tokenizer = BertTokenizer.from_pretrained("DeepPavlov/rubert-base-cased")
    model = BertModel.from_pretrained("DeepPavlov/rubert-base-cased")

    # ����������� � ��������������� ��������� ������
    inputs = tokenizer(text, return_tensors="pt", padding=True, truncation=True)

    # ���������� ������ RuBERT � ������
    outputs = model(**inputs)

    # ��������� ����������� ������
    embeddings = outputs.last_hidden_state

    # ������� ����������� ������
    return embeddings.tolist()