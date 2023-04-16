from fastapi import FastAPI
import pandas as pd
from simpletransformers.classification import ClassificationModel
from bs4 import BeautifulSoup
from typing import Optional
from pydantic import BaseModel


# function to remove HTML tags
def remove_html_tags(text):
    return BeautifulSoup(text, 'html.parser').get_text()

train_args = {
    'reprocess_input_data': True,
    'overwrite_output_dir': True,
    'sliding_window': True,
    'max_seq_length': 64,
    'num_train_epochs': 1,
    'train_batch_size': 128,
    'output_dir': '/outputs/',
    'fp16': True
}
model = ClassificationModel('roberta', './notebook/outputs/', args=train_args,use_cuda=False)
label_dict = {0: 'nothate', 1: 'hate'}


class Article(BaseModel):
    Title:str
    Body: str



app = FastAPI()

@app.post("/validate")
async def create_item(item: Article):
    item.Body=remove_html_tags(item.Body)
    print(item.Title)
    print(item.Body)
    predictions, _ = model.predict([item.Title,item.Body])
    isArticleTitleHateful=label_dict[predictions[0]]
    isArticleBodyHateful=label_dict[predictions[1]]
    return {
        "IsTItleContainsHate":isArticleTitleHateful,
        "IsBodyContainsHate":isArticleBodyHateful
    }

# uvicorn main:app --reload    