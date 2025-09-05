# DotnetTestTask

ASP.NET Core 8 backend project implementing a tree management API and exception journal according to the provided technical specification.

## Features

- **Tree Management**
  - Create a new tree (automatically created if not exists)
  - Add nodes with parent-child relationships
  - Rename nodes
  - Delete nodes (only leaf nodes can be deleted)
  - Retrieve the entire tree structure

- **Exception Journal**
  - Logs all exceptions during REST API request processing
  - Stores event ID, timestamp, request parameters, stack trace
  - Custom `SecureException` for business logic errors
  - Middleware captures exceptions and formats API responses according to specification

- **Database**
  - PostgreSQL (default, can be swapped with MS SQL or MySQL)
  - Code-first approach using EF Core
  - Trees and nodes are persisted with constraints and relationships
  - Recursive SQL query (via Dapper) for tree reconstruction

## API Endpoints

### Tree

- `POST /api.user.tree.get?treeName={name}`  
  Returns the entire tree with all nodes.

- `POST /api.user.tree.node.create?treeName={name}&parentNodeId={id}&nodeName={name}`  
  Creates a new node under the specified parent.

- `POST /api.user.tree.node.rename?treeName={name}&nodeId={id}&newNodeName={name}`  
  Renames the specified node.

- `POST /api.user.tree.node.delete?treeName={name}&nodeId={id}`  
  Deletes the specified node (only if it is a leaf).

### Journal

- `POST /api.user.journal.getRange`  
  Returns journal records with pagination and filtering.

- `POST /api.user.journal.getSingle?id={id}`  
  Returns details of a specific journal entry.

## Exception Handling

### SecureException

Used for business rule violations.  

Response format:
```json
{
  "type": "Secure",
  "id": "638136064526554554",
  "data": { "message": "You have to delete all children nodes first" }
}
