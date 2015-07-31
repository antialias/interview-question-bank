[autocomplete]: /assets/git-completion.sh

# Git

Some general Git tidbits

## Git autocomplete

Download the [this file][autocomplete] for tab autocomplete for Git from the command line and follow the directions from the top which are:

Move the [autocomplete file][autocomplete] into your home directory:

```sh
~/Downloads $ mv ./git-completion.sh ~/.git-completion.sh
```

Add the following line to your `.bash_profile`/`.bashrc`/`.zshrc`: 

```sh
source ~/.git-completion.sh
```
