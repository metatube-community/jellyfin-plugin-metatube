# AGENTS.md

## Git

- Don't credit AI tools in commits or pull requests: no `Co-Authored-By` trailer for an AI in commit messages, and no "Generated with ..." line in PR descriptions.

### Commits

- Title commits and PRs `Type(scope): summary`, e.g., `Fix(manifest): read manifest from dist branch`. Types are `Build`, `CI`, `Chore`, `Feature`, `Fix`, `Refactor`, and `Revert`. The scope is the area changed, e.g., `manifest`, `config`, or `Emby`, and may be dropped for repo-wide changes. Wrap code and provider names in backticks.
- Keep commit and PR titles within about 50 characters. This is a guideline, not a hard limit, but follow it when you can.

### Pull requests

- Keep PR descriptions concise: clearly say what the PR does and roughly how, with no code snippets and no detailed reasoning. Skip the how when the change is simple. Use these sections:
  - `## Summary`: start with the related issue or PR, if any. Use `Fixes #N` (for a bug) or `Closes #N` only when the PR fully resolves the issue, since GitHub closes it on merge; otherwise use `Related to #N` or `Follows #N`. Then say briefly what the PR does and why (motivation, trade-offs). Give examples only when the case is too complex without one.
  - `## Key Changes`: bullets listing the main changes. Omit it when the Summary already covers them.
  - `## Testing`: bullets listing the tests that ran, e.g., `dotnet build`. Omit it when no tests ran.
