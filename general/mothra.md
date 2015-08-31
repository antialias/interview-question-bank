# Mothra

## Repo: https://github.com/1stdibs/mothra

## Setting up debugging in phpStorm for Mothra:

1. Go to the Run menu and select Edit Configurations... A dialog will appear.
2. In the dialog that appears, select the plus ("+") sign to add a new configuration. A drop down will appear of configuration types. Select Mocha.
3. Enter a name for your config in the Name field.
4. The Node interpreter field should point to your installation of Node.
5. Leave the Node options field blank.
6. In the Working Directory field, enter the path to your local installation of mothra. This is probably /Users/[name]/projects/mothra.
7. In the Mocha Package field, enter the path to this mothra installation of mocha. This is probably /Users/[name]/projects/mothra/node_modules/mocha.
8. Leave the user interface field as BDD.
9. You can leave the Test directory field blank or you can point it to a directory of only the tests you want to run.
10. Click Apply, then OK.

You can now add breakpoints to your code. When you want to debug mothra, set phpStorm to use this configuration, and click the bug icon to the right of the play icon.

If you are running against an environment other than qa, modify the envObj line in setup.js to point to the proper environment. Just be careful not to commit this!

```sh
config = {
    envObj: envs.qa,
    localServices: {}
};
```