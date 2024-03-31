import tensorflow as tf
from tensorflow.keras import layers, models

def create_model(input_shape):
    model = models.Sequential([
        layers.Dense(64, activation='relu', input_shape=input_shape),
        layers.Dense(32, activation='relu'),
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
