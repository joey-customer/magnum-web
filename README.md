# magnum-web

## Environment Variables

It is the good idea not to hardcode any sensitive data such as username, password or key in the source code. We should pass the neccessary setting via environment variables. Below are the environment variables needed for the application.

**MAGNUM_CERTIFICATE_FILE** - Https .pfx certificate file location. This one can be blank if MAGNUM_CERTIFICATE_ON = 'N'.
**MAGNUM_CERTIFICATE_ON** - Set it to 'Y' in the production mode, set to 'N' in development. If missing then it is equivalent to set it to 'N'.
**MAGNUM_CERTIFICATE_PASSWORD** - Password to access .pfx file defined by MAGNUM_CERTIFICATE_FILE.
**MAGNUM_DB_PASSWORD** - Password to connect to database (Firebase).
**MAGNUM_DB_USERNAME** - Username to connect to database (Firebase).
**MAGNUM_FIREBASE_BUCKET** - Firebase storage url.
**MAGNUM_FIREBASE_URL** - Firbase database url.
**MAGNUM_SMTP_HOST** - SMTP host.
**MAGNUM_SMTP_PASSWORD** - SMTP password.
**MAGNUM_SMTP_PORT** - SMTP port.
**MAGNUM_SMTP_USER** - SMTP user.
