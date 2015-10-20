# Front-end Code Quality

At 1stdibs we should strive to write high-quality and bug-free code. High-quality code is code that is easy to understand, simple to maintain, and covered by tests. It also needs to be free of defects. We need to balance quality with the knowledge that the only code that counts, is code that ships.

### Clear code
Clever code isn't better code. Generally, it's better -- i.e. more maintainable -- to write a few more lines if it makes the code more readable, understandable and explicit.

Here's a very simple example, but I think it illustrates my point:

```javascript
    // a tad too clever
    var result = {
        foo: 'bar',
        baz: 'qux'
    }['foo'];
    
    var obj = {
        foo: 'bar',
        baz: 'qux'
    };
    var result = obj.foo; // clearer; more explicit
```

Of course, clarity is a moving target (and the [cause of much debate](https://www.reddit.com/r/javascript/comments/1bl24h/explicit_vs_clever/)). Code that seems overly clever today is a well-understood convention tomorrow. What seems clear to me, may look like jibberish to you. When in doubt, consult our [JavaScript style guide](https://github.com/1stdibs/javascript).

Clarity isn't simply how you organize your code, but how you choose different building blocks. For example, when deciding on how to call a function with a specific context (i.e. the value of `this`) should you use `Function.prototype.bind` or `$.proxy`? (Correct answer: `Function.prototype.bind`.)

**some rules of thumb**

* Choose standard parts of the language. For example, we're moving towards native `Promise` and `fetch`. 
* Choose libraries that are already in our stack.
* In the event you need a new library, choose an open source library that is well-documented, well-tested and has a community on Github. Otherwise, it may be better to write, test and document it yourself.

You see where I'm going with this: documentation is important. Which brings me to...

### Document your code

Your goal should be to document the code you write using [JSDoc style comments](http://usejsdoc.org/about-getting-started.html). If you're using PHPStorm or WebStorm, there's a handy plugin that will generate a template for you. See [JetBrain's documentation](https://www.jetbrains.com/webstorm/help/creating-jsdoc-comments.html). For Sublime Test users, check out [this plugin](https://github.com/Warin/Sublime/tree/master/DocBlockr).

At 1stdibs we're currently not generating JSDocs, but the format is well-understood and sets us up for generated documentation in the future. Plus, PHPStorm and WebStorm love it and offer better inline documentation and auto-complete if JSDocs are present. 

You should write your variable names and function names so they're self-documenting. For example, it's obvious what this function does by reading its name on invocation. 

```javascript
var id = getId(model);
```

The example function definition:

```javascript
    /**
     * @param {object} model A model with an id property
     * @param {number} model.id
     * @returns {number|undefined}
     */
    var getId = function (model) {
        return model.id;
    };
```

In this example, you can leave out the `@description` field since the function is self-documenting. It would be redundant to write "@description Retrieves the id of model." That doesn't mean you don't write JSDocs for self-documenting functions. You should document arguments, return values and other non-obvious information. 

If you're writing a library package that is meant to be shared across teams (or open-sourced), you should include a Readme that documents the API and offers examples of its usage.


### Eliminate code smell

> A code smell is a surface indication that usually corresponds to a deeper problem in the system 
    
   -- [CodeSmell](http://martinfowler.com/bliki/CodeSmell.html) by Martin Fowler

This is a big topic, so we won't go deep here. Check out [this blog post and video](http://elijahmanor.com/javascript-smells/), which are excellent.
  
As a tl;dr to the above, below are a couple of common problems to look out for (But, seriously, [watch the video](https://youtu.be/JVlfj7mQZPo)). 

* Too much complexity -- if a program's or function's [cyclomatic complexity](http://dictionary.reference.com/browse/cyclomatic-complexity) gets too great it means code is hard to follow and debug. Eslint provides a way to test for too much complexity.
* copy and paste problems -- following already erroneous patterns or reusing a pattern you don't fully understand in a faulty way (also called [cargo-culting](https://en.wikipedia.org/wiki/Cargo_cult_programming))

But, seriously, [watch the video](https://youtu.be/JVlfj7mQZPo).

### Test your code

The importance of testing your code cannot be stressed enough. As a developer you should be confident that your code works in all imagined scenarios when it leaves your hands. When you assert that your pull request is ready to be merged, you should feel confident that it your code could go directly to production. 

There are a few ways to test your code, you should be most focused on automated tests.  

#### automated tests

You should spend the bulk of your **testing time** writing automated tests. As a front-end developer, this translates into writing unit tests.  

##### unit tests

You're not writing quality code if you're not writing unit tests.

Time spent manually testing your code is lost time. Every time you modify the code, you need to spend that time testing again. And again. And again. However, unit tests, once written, can be run over and over without wasting more of your valuable time. 

In addition, not only do unit tests exercise your code, they are one of the best ways to document your intentions since there are concrete assertions *in the code* as to how the code is meant to function. 

On the front-end, unit tests run on each pull request and during each build. However, you should be running the tests before pushing code to your fork. Our two main repos both provide npm scripts to run the tests. Simply run `npm test` after you `npm install`.

##### functional tests

At 1stdibs, we have the luxury of an automated test team. They write functional tests using [webdriver.io](http://webdriver.io/) and [selenium](http://www.seleniumhq.org/). These tests are collectively dubbed "[Mechagodzilla](https://github.com/1stdibs/mecha-godzilla)" (mg for short). Mg runs against 1stdibs.com, dealer tools and internal tools on Chrome, Safari and Internet Explorer 11 after qa, stage and production builds. 

We chose JS-based webdriver.io to leverage our front-end JavaScript strength. If you can't stomach doing manual tests -- which are required at times -- then  writing mg tests could help count down on manaul testing time. There are instructions in the [mg repo](https://github.com/1stdibs/mecha-godzilla) on how run tests against locally installed browsers. Instructions on running against IE on a Windows VM are coming soon.

#### manual tests

Now to the hard part: manual testing. Everyone hates testing manually. It is literally a waste of time (as noted above). Unfortunately, for fine-grained interactions, CSS layout specifications, complex UI or just simple smoketests, manual testing is required.
 
At 1stdibs, you should -- at a minimum -- test the happy path and page layouts in the [desktop browsers that we support](./browser-support.md): Safari, Chrome, Firefox, Internet Explorer 11 and Microsoft Edge. Additionally, for 1stdibs.com you'll need to test in Mobile Safari and Mobile Chrome.

CSS inconsistencies have always made it required that a good front-end dev test cross browser (though this has gotten better in recent years). The introduction of es6 to our stack has made this especially important (and hard). There is a [mix of es6 support](https://kangax.github.io/compat-table/es6/) across browser vendors. We've had bugs caused by es6 code not being transpiled properly. Things work fine in one browser, which happens to support some es6 feature, only to fail spectacularly in another that doesn't.

To ensure you're covering all your testing bases, download and install the latest versions of Chrome, Firefox and Safari. Get IE11 and MSEdge by downloading the VMs from [modern.ie](https://dev.modern.ie/tools/vms/mac/). And, last but not least, install Xcode to get access to the iOS simulator. You'll need an [Apple developer account](https://developer.apple.com/).

### Time estimates

Our organization needs good estimates from developers so that we can stay on schedule, make realistic ROI decisions and staffing plans. Needless to say, when you're engineering with quality in mind, you need to factor in the time it takes for tests. A good rule of thumb: if you think completing a specific feature would take 1 time unit, adding tests adds an additional 0.5 to 1. You should always estimate based on solid testing principles: unit tests and time for manual or mg tests. 

However, business goals can sometimes override good engineering practices. As engineers part of our job is to balance important business initiatives against the risk of shipping code that may not be perfectly tested or has other quality issues. 

You will need to cut corners from time to time. 

Deploying code without unit tests is considered a form of tech debt. When practicalities force you to make that decision it's important to communicate to other team members that the team is now taking on that debt. Make a ticket and put it in your team's backlog. There will be a time when the crunch clears up and you'll be able to pay down the debt.
