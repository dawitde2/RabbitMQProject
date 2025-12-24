pipeline {
    agent any

    environment {
        BUILD_CONFIGURATION = 'Release'
        DOCKER_COMPOSE_FILE = 'docker-compose.yml'
    }

    stages {
        stage('Checkout') {
            steps {
                echo '🔄 Checking out code'
                checkout scm
            }
        }

        stage('Build & Publish Services') {
            steps {
                echo '⚙️ Building .NET projects'
                bat 'dotnet restore InventoryService/InventoryService.csproj'
                bat 'dotnet build InventoryService/InventoryService.csproj -c %BUILD_CONFIGURATION%'
                
                bat 'dotnet restore OrderService/OrderService.csproj'
                bat 'dotnet build OrderService/OrderService.csproj -c %BUILD_CONFIGURATION%'
                
                bat 'dotnet restore PaymentService/PaymentService.csproj'
                bat 'dotnet build PaymentService/PaymentService.csproj -c %BUILD_CONFIGURATION%'
            }
        }

        stage('Docker Compose Build') {
            steps {
                echo '🐳 Building Docker images using Docker Compose'
                bat "docker-compose -f %DOCKER_COMPOSE_FILE% build"
            }
        }

        stage('Optional: Docker Compose Up (for testing)') {
            steps {
                echo '🚀 Starting all services using Docker Compose (detached)'
                bat "docker-compose -f %DOCKER_COMPOSE_FILE% up -d"
            }
        }
    }

    post {
        success {
            echo '✅ Build & Docker Compose setup completed successfully'
        }
        failure {
            echo '❌ Build failed, check logs'
        }
    }
}
