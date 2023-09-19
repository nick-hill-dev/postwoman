# Postwoman

Manage collections of API requests and execute them locally, much like how Postman's scratch pad let you do it before the feature was taken away.

You do not need to create an account or upload your collections.

You can import and export singular API requests and whole collections.

The tool can import and convert Postman collections.

Written in C# against WPF, targeting Windows.

This project is in the early stages. It works with my own legacy Postman collections, but it may not yet work with yours.

## Features

- Can add collections and each collection can support any number of custom API requests.
- Can send API requests to servers and display the response headers and body.
- Variables can be defined on the collection level and enclosed in double curly braces I.E. `{{baseUrl}//delete-my-thing`.
- Requests support custom defined headers, GET parameters, and body content.
- Some groundwork is in place to import Postman collections. Please raise requests if you are struggling to import yours. PRs with code changes are even better!

## Limitations

- No simplified `Authentication` header capabilities yet.
- GET parameters can be made more easily editable.
- No support yet for custom JS code that you might be used to with Postman.
- No code proxy generation yet.
- Postman import is not as comprehensive as it could be.

## Aspirations

- Ability to import Swagger API definitions.

## Contributions

I'm happy to accept contributions.

## License

Postwoman was written by Nick Hill and is released under the MIT license. See LICENSE for more information.