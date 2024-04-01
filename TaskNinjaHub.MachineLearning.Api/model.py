import tensorflow as tf
import json
from tensorflow.keras import layers, models

def create_model(input_shape):
    model = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=input_shape),
        layers.Dense(32, activation='relu'),
        layers.Dense(16, activation='relu'),
        layers.Dense(1, activation='sigmoid')
    ])

    model.compile(optimizer='adam',
                  loss='binary_crossentropy',
                  metrics=['accuracy'])
    
    return model

def train_model(X_train, y_train, epochs=10):
    X_train = tf.convert_to_tensor(X_train, dtype=tf.float32)
    y_train = tf.convert_to_tensor(y_train, dtype=tf.float32)

    numFeatures = X_train.shape[1]

    model = create_model(input_shape=(numFeatures,))
    model.fit(X_train, y_train, epochs=epochs)
    return model

def save_model(model, filepath):
    model.save(filepath)

def predict_probability(jsonData, modelFilePath):
    data = json.loads(jsonData)
    priorityId = data['PriorityId']
    informationSystemId = data['InformationSystemId']
    taskTypeId = data['TaskTypeId']
    taskExecutorId = data['TaskExecutorId']

    model = tf.keras.models.load_model(modelFilePath)
    
    # Преобразование списка в тензор
    inputs = tf.constant([[priorityId, informationSystemId, taskTypeId, taskExecutorId]], dtype=tf.float32)

    prediction = model.predict(inputs)
    return float(prediction[0][0])