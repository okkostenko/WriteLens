# TUE 26.11

+ set up project
+ repositories:
    + content
    + scores
    + document types
+ research text readability analysis methods
+ text utility
+ service

----------

# WED 27.11

+ implement readability analysis:
    + analyzers:
        + ARI
        + Flesch-Kincaid
        + Gunnig Fog
        + SMOG
    + orchestrator
+ add controller

----------

# THU 28.11

+ service
+ add request queue MassTranis
+ add redis cache
+ controller:
    + endpoints
    + DTOs

----------

# FRI 29.11

+ mapping profile
+ debug
+ add deduplication
+ request jwt token validation from core

----------

# SAT 30.11

- api gateway
- core: when document is created create score db record
- core: add endpoint to check access to the document
- rename field is_readability_analyzed to is_readability_analyzed
* add flags for words and stuff

