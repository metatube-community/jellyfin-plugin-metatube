# AGENTS.md

## Git

- Don't credit AI tools in commits or pull requests: no `Co-Authored-By` trailer for an AI in commit messages, and no "Generated with ..." line in PR descriptions.

### Commits

- PRs are squash-merged, so commit messages need no fixed format; a plain verb phrase is fine, e.g., `Read manifest from dist branch`.

### Pull requests

- Title PRs `type(scope): summary` as commitlint's `config-conventional` expects, e.g., ``fix(translation): mark `TranslationMode` as flags``. Types are `build`, `chore`, `ci`, `docs`, `feat`, `fix`, `perf`, `refactor`, `revert`, `style`, and `test`. The scope is the lowercase area changed, e.g., `manifest`, `config`, or `emby`, and may be dropped for repo-wide changes. Start the summary in lowercase with no trailing period, and wrap code and provider names in backticks.
- Keep PR titles within about 50 characters when you can, and never over 72.
- Keep PR descriptions concise: clearly say what the PR does and roughly how, with no code snippets and no detailed reasoning. Skip the how when the change is simple. Use these sections:
  - `## Summary`: start with the related issue or PR, if any. Use `Fixes #N` (for a bug) or `Closes #N` only when the PR fully resolves the issue, since GitHub closes it on merge; otherwise use `Related to #N` or `Follows #N`. Then say briefly what the PR does and why (motivation, trade-offs). Give examples only when the case is too complex without one.
  - `## Key Changes`: bullets listing the main changes. Omit it when the Summary already covers them.
  - `## Testing`: bullets listing the tests that ran, e.g., `dotnet build`. Omit it when no tests ran.
