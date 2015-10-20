# Front-end Code Quality

At 1stdibs we should strive to write high-quality and bug-free code. High-quality code is code that is easy to understand, test and maintain. Bug-free speaks for itself. We also need to keep in mind that the only code that counts, is the code that ships.

### Clear code
Clever code isn't better code. It's usually better -- i.e. more maintainable -- to write a few more lines if it makes the code more readable, understandable and explicit.

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

Clarity isnt' simply how you organize your code, but how you choose the different building blocks. For example, when deciding on how to call a function with a specific context (i.e. the value of `this`) should you use `Function.bind` or `$.proxy`? (Hint: `Function.bind`.)

**some rules of thumb**

* Choose standard parts of the language. For example, we should be moving towards native `Promise` and `fetch`. 
* Choose libraries that are already in our stack
* In the event you need a new library, choose an open source library that is well-documented, well-tested and has a community on Github. Otherwise, it may be better to write it, test it -- and document it -- yourself.

You see where I'm going with this: documentation is important. Which brings me to...

### Document your own code

Whenever possible, document your code using [JSDoc style comments](http://usejsdoc.org/about-getting-started.html). If you're using PHPStorm or WebStorm, there's a handy plugin that will generate a template for you. See [JetBrain's documentation](https://www.jetbrains.com/webstorm/help/creating-jsdoc-comments.html). 

At 1stdibs we're not currently generating JSDocs, but the format is well-understood and sets us up for generated documentation in the future. Plus, PHPStorm and WebStorm love it and offer  better inline documentation and auto-complete if JSDocs are present. 

You should write your variable names and function names so they're self-documenting.

For example, it's obvious what this function does by reading its name on invocation. 

```javascript
var id = getId(model);
```

The example definition:

```javascript
    /**
     * 
     * @param model A model with an id property
     * @returns {number|undefined}
     */
    var getId = function (model) {
        return model.id;
    };
```

Note that it's a good idea to have JSDocs documenting the arguments and return value. In this example, you can leave out the @description field since the function is self-documenting. It would be redundant to write "@description Retrieves the id of `model`."

Further, if you're writing a library package that is meant to be shared across teams, you should include a Readme.md that documents the API and offers examples of how to use your library.


### Eliminate code smell

> A code smell is a surface indication that usually corresponds to a deeper problem in the system 
    
   -- [CodeSmell](http://martinfowler.com/bliki/CodeSmell.html) by Martin Fowler

This is a big topic, so we won't go in deep on here. Check out [this blog post and video](http://elijahmanor.com/javascript-smells/), which are excellent.
  
As a tl;dr to the above, here are couple common problems: 

* Too much complexity -- if a function's cyclomatic complexity gets too great it means code is hard to follow and debug. Eslint provides a way to test for too much complexity.
* copy and paste -- following already erroneous patterns or reusing a pattern you don't fully understand in a faulty way (also called cargo-culting)

But, seriously, [watch the video](https://youtu.be/JVlfj7mQZPo).

### Test your code

Testing your code cannot be stressed enough. As a developer you should be confident that your code works in all imagined scenarios when it leaves your hands. When you assert that your pull request is ready to be merged, you should feel confident that it could go directly to production. 

There are a few ways to test your code, you should be most focused on automated tests.  

#### automated tests

You should spend the bulk of your **testing time** writing automated tests. As a front-end developer, this translates into writing unit tests.  

##### unit tests

You're not writing quality code if you're not writing unit tests.

Time spent manually testing your code is lost time. Every time you modify the code, you need to spend that time testing again. And again. And again. However, unit tests, once written, can be run over and over without incurring more of time cost. 

In addition, not only do unit tests exercise your code, they are one of the best ways to document the developer's intention (since there are concrete assertions *in the code* as to how the code is meant to function). 

On the front-end, unit tests run on each pull request and during each build. However, you should be running the tests before pushing code to your fork. Our two main repos both provide npm scripts to run the tests. Simply run `npm test` after you `npm install`.

##### functional tests

At 1stdibs, we have the luxury of an automated test team. They write functional tests using [webdriver.io](http://webdriver.io/) and [selenium](http://www.seleniumhq.org/). These tests are collectively dubbed "[Mechagodzilla](https://github.com/1stdibs/mecha-godzilla)" (or mg for short). Mg runs against 1stdibs.com, dealer tools and internal tools on Chrome, Safari and Internet Explorer 11 after qa, stage and production builds. 

We chose JS-based webdriver.io to leverage our front-end JavaScript strength. If you can't stomache doing manual tests -- which are required at times -- then  writing mg tests could be an alternative. There are instructions in the mg repo on how run tests against locally installed browsers. Instructions on running against IE on a Windows VM will come soon.

#### manual tests

Now to the hard part: manual testing. Everyone hates testing manually. It is literally a waste of time (as noted above). Unfortunately, for fine-grained interactions, complex UI or just simple smoketests, manual testing is sometimes required.
 
At 1stdibs, if you don't have mg tests running against supported browsers, you should -- at a minimum -- test the happy path in the [browsers that we support](./browser-support.md): Safari, Chrome, Firefox and Internet Explorer 11.

The introduction of es6 to our stack has made this especially important (and hard). There is a [mix of es6 support](https://kangax.github.io/compat-table/es6/) across browser vendors. We've had bugs caused by es6 not being transpiled properly but things work fine in one browser only to fail spectacularly in another.

Download and install the latest versions of Chrome, Firefox and Safari. Download the VirtualBox VM for IE11 from [modern.ie](https://dev.modern.ie/tools/vms/mac/). 

### Time estimates

Our organization needs good estimates from developers so that we can stay on schedule, make realistic ROI decisions and staffing plans. Needless to say, when you're engineering with quality in mind, you need to factor in the time it takes for tests. A good rule of thumb: if you think completing a specific feature would take 1 time unit, adding tests adds an additional 0.5 to 1.
 
You should always estimate based on solid testing principles: unit tests and time for manual or mg tests. 

However, depending on business goals, you will need to cut corners from time to time. If you need to ship code without unit tests this is called tech debt. It should always be called out to your other team members (especially the product manager) when it needs to happen and a ticket should be created to pay it down.
