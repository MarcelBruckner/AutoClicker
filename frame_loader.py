import subprocess
import os 
from PIL import Image
from time import sleep
import pymsgbox
from pathlib import Path
import pandas as pd
from tkinter import *

choices = ['Yes', 'No', 'Cancel']
columns = ['sample', 'label']

def list_files(path):
    all_files = os.listdir(path)
    samples = [path + file for file in all_files if 'sample_' in file]
    return samples
    
def show_file(path):
    im = Image.open(path)
    viewer = subprocess.Popen(['ristretto', path])
    return viewer

def ask_if_fishing(path):
    viewer = show_file(path)
    isFishing = pymsgbox.confirm('Fishing?', buttons=choices)
    if isFishing == choices[0]:
        isFishing = 1
    elif isFishing == choices[1]:
        isFishing = 0
    else:
        isFishing = -1
    viewer.terminate()
    viewer.kill()
    return isFishing

def add_labeled_sample(df, path, sample=None, label=None):
    if sample and label:
        df = df.append({columns[0]: sample, column[1]: label}, ignore_index=True)
    df.to_csv(path, index=False)

def open_labels_file(path):
    df = None
    try:
        df = pd.read_csv(path)
    except:
        df = pd.DataFrame(columns=columns)
    
    add_labeled_sample(df, path)
    print(df)
    return df, df[columns[0]]

def label_samples(samples_folder):
    files = list_files(samples_folder)
    labels_file = samples_folder + 'labels.csv'

    for file in files:
        df, seen_samples = open_labels_file(labels_file)

        if not file in seen_samples:
            isFishing = ask_if_fishing(file)
            if isFishing < 0:
                return
            add_labeled_sample(df, labels_file, file, isFishing)
