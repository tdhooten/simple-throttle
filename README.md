# Simple Throttle
This small library provides a simple thread-safe object which throttles an arbitrary set of actions, rate limiting them to the specified number of actions within the specified timespan.

## Installation
Simple Throttle is available via NuGet and can be installed easily via the command line:

```
dotnet add package SimpleThrottle
```

## Usage
First add the library to your using statements:

```
using SimpleThrottle;
```

Then the provided Throttler object can be instantiated as follows:

```
var throttler = new Throttler(int requestLimit, int timespanInMilliseconds);
```

If you wish to restrict multiple different actions to a single rate limit, you can also easily wrap a single Throttler object in a service class and provide it to various methods using dependency injection.

After your Throttler object has been instantiated, simply invoke the MakeRequest method immediately before calling each action you wish to rate limit like so:

```
foreach (var object in objects)
{
    throttler.MakeRequest();
    await SomeClass.RateLimitedAction(object);
}
```

That's it!
