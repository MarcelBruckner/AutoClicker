from django.contrib import admin
from django.urls import include, path

base = 'django/'
urlpatterns = [
    path(base + 'polls/', include('polls.urls')),
    path(base + 'admin/', admin.site.urls),
]

