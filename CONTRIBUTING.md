# Git workflow

Do not commit or push directly to `master`/`main` or `develop`.

1. Start a feature branch from `develop`: `git switch develop` then `git switch -c feature/<work-name>`.
2. Commit and push only the feature branch: `git push -u origin feature/<work-name>`.
3. Create a pull request whose base branch is `develop`.
4. Merge the approved pull request into `develop`. A release pull request from `develop` to the protected production branch is a separate step.

Repository administrators should enable GitHub Branch Protection for `master` (or `main` if renamed) and `develop`, requiring pull requests before merge and disabling direct pushes.
