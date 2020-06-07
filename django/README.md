#Building and Running for Development
1. Setup a python virtual environment and the local_settings.py
1. Install all python requirements using "pip install -r requirements.txt"
1. Install Docker
1. Run redis using the command "docker run --name redis -d -p 6379:6379 redis "
1. Run postgres using the command "docker run --name postgres -d -e POSTGRES_PASSWORD=danny13kotson -d -p 5432:5432"
1. Load data into Django using the command "python manage.py loaddata --settings=tabletop.local_settings db.json"
1. Run the Django server using the command "python manage.py runserver --settings=tabletop.local_settings 80"
1. Run the Unity application
