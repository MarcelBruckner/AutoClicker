#!/bin/bash

NAME="autoclicker"                              #Name of the application (*)
DJANGODIR=/var/www/web/RS-Bots/autoclicker             # Django project directory (*)
SOCKFILE=/var/www/web/RS-Bots/run/gunicorn.sock        # we will communicate using this unix socket (*)
# SOCKFILE=/usr/local/etc/gunicorn.sock        # we will communicate using this unix socket (*)
USER=www-data                                        # the user to run as (*)
GROUP=www-data	                                     # the group to run as (*)
NUM_WORKERS=3                                     # how many worker processes should Gunicorn spawn (*)
DJANGO_SETTINGS_MODULE=autoclicker.settings             # which settings file should Django use (*)
DJANGO_WSGI_MODULE=autoclicker.wsgi                     # WSGI module name (*)

echo "Starting $NAME as `whoami`"

# Activate the virtual environment
cd $DJANGODIR
source /var/www/web/RS-Bots/autoclickerEnv/bin/activate
export DJANGO_SETTINGS_MODULE=$DJANGO_SETTINGS_MODULE
export PYTHONPATH=$DJANGODIR:$PYTHONPATH

# Create the run directory if it doesn't exist
RUNDIR=$(dirname $SOCKFILE)
test -d $RUNDIR || mkdir -p $RUNDIR

echo "$PWD"


# Start your Django Unicorn
# Programs meant to be run under supervisor should not daemonize themselves (do not use --daemon)
exec /var/www/web/RS-Bots/autoclickerEnv/bin/gunicorn ${DJANGO_WSGI_MODULE}:application \
  --name $NAME \
  --workers $NUM_WORKERS \
  --user $USER \
  --bind=unix:$SOCKFILE

