# TUE 19.11

+ set up user management service:
    + create project
    + set up dependencies
    + configure project structure
+ set up postgres database:
    + write db context
    + define entities
    + run migrations
+ defile domain models
+ map domain models to db entities
+ AuthenticationService:
    + login user
    + register user
+ user repository

----------

# WED 20.11

+ user service
+ user controller:
    + login
    + register
    + get user
    + update user
    + delete user
+ user dtos
+ auth dtos
+ map dtos to commands
+ authentication

----------

# THU 21.11

+ refine document schema
+ create document models:
    + shared
+ create document entities:
    + shared
+ hash utility
+ document type cache
+ rename user management service to core service
+ document postgres repository
+ document content mongo repository
+ set up mongo db

----------

# FRI 22.11

+ document content mongo repository
+ create document models:
    + core
    + shared
+ create document entities:
    + core
    + shared
+ document service
+ add validation to the identity trying to get / update / delete document

----------

# SAT 23.11

+ document content repository:
    + add type to the aggregation
+ document types mongo repository
+ add validation to the identity trying to update / delete user
+ document controller
+ document DTOs
+ update mapper
+ check if type exists on document creation
+ split document service UpdateDocumentById method into UpdateDocumentById and UpdateDocumentContentById

----------

# MON 25.11

+ preload types when starting the app
+ debug core service:
    + user:
        + register
        + login
        + get by id
        + get me
        + update
        + delete
    + document:
        + get types
        + create
        + get many
        + get single
        + update
        + delete
    + caching
+ set up swagger documentation

----------

# WED 11.12

+ order sections
+ add search for documents
+ add pagination for documents
+ order document sections

----------

# THU 12.12

+ move pagination models to shared
+ refactoring
+ insert document score on document creation
+ delete section score and flags if the section was removed:
    + add repository for flags
    + add repository for scores
    + perform deletion / update

----------

# FRI 13.12

+ migrations for mongoDb
+ debug

----------

# SAT 14.12

+ debug
+ clean up mess
- write unit tests for core service
- documentation
* filters