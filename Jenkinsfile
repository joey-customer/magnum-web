pipeline {
    agent any 
    
    parameters {
        string(name: 'SONAR_LOGIN_KEY', defaultValue: '6b3024e6cc1576028a5bba761b1009ffb850b6f5', description: 'Sonarqube logon key')
    }    
    
    environment {
        PRODUCT_NAME = 'MagnumWeb'
        PUBLISH_FLAG = 'FALSE'
        BUILT_VERSION = '1.1.1-SNAPSHOT'
        DOCKER_VERSION = 'latest'
        SONAR_SCANNER = '/home/tomcat/.dotnet/tools/dotnet-sonarscanner'
        COVERLET = '/home/tomcat/.dotnet/tools/coverlet'
        UNIT_TEST_ASSEMBLY = './MagnumTest/bin/Debug/netcoreapp2.2/MagnumTest.dll'
        PACKAGE_PATH = './sources/bin/Release'
    }

    stages {
        stage('Start Code Analysis') {            
            steps {                
                sh "${env.SONAR_SCANNER} begin \
                    /key:pjamenaja_magnum_web \
                    /o:pjamenaja \
                    /v:${env.BUILT_VERSION} \
                    /d:sonar.host.url=https://sonarcloud.io \
                    /d:sonar.branch.name=${env.BRANCH_NAME} \
                    /d:sonar.cs.opencover.reportsPaths=./coverage.opencover.xml \
                    /d:sonar.javascript.exclusions=**/bootstrap/**,**/jquery/**,**/jquery-validation/**,**/jquery-validation-unobtrusive/** \
                    /d:sonar.login=${params.SONAR_LOGIN_KEY}"

                sh "echo [${env.BUILT_VERSION}]"
            }
        }

        stage('Build') {
            steps {
                sh "dotnet build -p:Version=${env.BUILT_VERSION}"
            }
        }

        stage('Unit Test') {
            steps {
                sh "${env.COVERLET} ${env.UNIT_TEST_ASSEMBLY} --target 'dotnet' --targetargs 'test . --no-build' --format opencover"
            }
        } 

        stage('End Code Analysis') {
            steps {
                sh "${env.SONAR_SCANNER} end /d:sonar.login=${params.SONAR_LOGIN_KEY}"
            }         
        } 

        stage('Docker Packaging') {
            steps {
                sh "cd Docker"
                sh "./make_docker.bash"
            }         
        }                  
    }
}
