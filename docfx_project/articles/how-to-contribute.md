# How to Contribute

We want contributions. We need contributions. You should contribute!

If you find a bug please follow these easy steps:

1. [Create an Issue](https://github.com/irongut/EDlib/issues) for the bug tagged "Bug".
2. Write a regression test in the `UnitTests` project under the appropriate test class.
3. Fix it!
4. Commit & push your changes.
5. Submit a Pull Request telling me how awesome your fix is. Make sure to include "Fix #XXX" in the PR details to link the issue.

If you want a new feature then follow these steps:

1. Implement it!
2. Test it a bunch. If it's a change to an existing feature add some tests to the appropriate test class, if it's a completely new feature create a new test class.
3. Commit & push your changes.
4. Submit a Pull Request telling me how awesome and valuable this new feature is.

## Submitting Pull Requests

1. Fork the repository.
2. Check out the repository onto your machine.
3. Hack on the EDlib code.
4. Commit & push your changes.
5. Open a Pull Request on `irongut/EDlib`.

See the [GitHub docs on how to fork a repo](https://help.github.com/articles/fork-a-repo) for more details.

## Code Formatting

* Use spaces aligned to 4 characters.
* Private fields are camelCase and have a leading `_` if they are a backing field for another member.
* All other members are PascalCase.
