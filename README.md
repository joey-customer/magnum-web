# magnum-web

## Environment Variables

It is the good idea not to hardcode any sensitive data such as username, password or key in the source code. We should pass the neccessary setting via environment variables. Below are the environment variables needed for the application.

In general we can set MAGNUM_CERTIFICATE_ON to 'N' or remove it in the development environment then the MAGNUM_CERTIFICATE_FILE and MAGNUM_CERTIFICATE_PASSWORD can be discarded as well.

* MAGNUM_CERTIFICATE_FILE - Https .pfx certificate file location.
* MAGNUM_CERTIFICATE_ON - Set it to 'Y' in the production mode, set to 'N' in development. 
* MAGNUM_CERTIFICATE_PASSWORD - Password to access .pfx file defined by MAGNUM_CERTIFICATE_FILE.
* MAGNUM_DB_PASSWORD - Password to connect to database (Firebase).
* MAGNUM_DB_USERNAME - Username to connect to database (Firebase).
* MAGNUM_FIREBASE_BUCKET - Firebase storage url.
* MAGNUM_FIREBASE_URL - Firbase database url.
* MAGNUM_SMTP_HOST - SMTP host.
* MAGNUM_SMTP_PASSWORD - SMTP password.
* MAGNUM_SMTP_PORT - SMTP port.
* MAGNUM_SMTP_USER - SMTP user.
