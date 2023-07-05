
# Simple Neural Network

This repository contains a simple neural network implementation written in native C# without any third-party libraries. The purpose of this project is to demonstrate the implementation of a neural network and its application in solving two different problems: number recognition and the Titanic problem. It was developed as part of a teacher's assignment.

The inspiration for this project comes from various sources:
- The [Learning To See video series](https://www.youtube.com/playlist?list=PLiaHhY2iBX9ihLasvE8BKnS2Xg8AhY6iV)
- The [Neural Networks video series by 3Blue1Brown](https://www.youtube.com/playlist?list=PLZHQObOWTQDNU6R1_67000Dx_ZCJB-3pi)
- [Onigiri's video](https://youtu.be/GNcGPw_Kb_0) and their corresponding [repository](https://github.com/ArtemOnigiri/SimpleNN/tree/master/src), demonstrating the process of building a neural network from scratch
## Prerequisites
Before running the application, ensure that you have the following prerequisites installed:

- [Git](https://git-scm.com/downloads)
- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

## Getting Started
To run this application, follow these steps:

### 1. Clone the Repository
Open a terminal or command prompt and clone the repository using the following command:
```bash
git clone https://github.com/sdywa/simple-neural-network.git
```

### 2. Install Dependencies
Navigate to the cloned repository folder and install the necessary dependencies for reading images using the following commands:
```bash
cd simple-neural-network
dotnet restore
```

### 3. Add Required Datasets
Before running the program, you need to add the necessary datasets for training the neural network. Follow the steps below:

#### Number Recognition
Navigate to the `NeuralNetwork/Numbers` folder:
```bash
cd NeuralNetwork/Numbers
```
Download the training and test PNG datasets from [this repository](https://github.com/pjreddie/mnist-csv-png) and extract the archives in the current folder.

#### The Titanic Problem
Navigate to the `NeuralNetwork/Titanic` folder and create the `test` and `train` folders:
```bash
cd NeuralNetwork/Titanic
mkdir test && mkdir train
```
Download the input data from [Kaggle](https://www.kaggle.com/competitions/titanic/data). Place the `gender_submission.csv` and `test.csv` files in the `test` folder, and the `train.csv` file in the `train` folder.

### 4. Run the Application
To build and run the project, provide one of the following arguments at program startup:
- To train a neural network for number recognition, use the "numbers" argument:
```bash
dotnet run numbers
```
- To train a neural network for the Titanic problem, use the "titanic" argument: 
```bash
dotnet run titanic
```
