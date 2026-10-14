<!-- pdac-scope: cited -->
# Project task board

## ADDED Requirements

### Requirement: Tasks are ordered by configurable status

The system MUST create tasks for a project with a configured status and non-negative board position.

#### Scenario: Create a task

- **WHEN** an operator creates a task with a project and valid status
- **THEN** the task is returned with its status, assignee and position

### Requirement: Task progression recalculates project progress

The system MUST recalculate project progress after a task changes status, counting statuses marked complete.

#### Scenario: Progress a task

- **WHEN** an operator moves a task to a completing status
- **THEN** the task is updated and the project progress reflects completed tasks

<!-- pdac:cite id="UC-PRJ-PROGRESS-STATUS-AUTOMATICALLY" digest="sha256:cd576d336c8096b0e71096d084bb2258c78c0d81522848e32a78730276caafed" -->
