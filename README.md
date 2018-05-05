Daisy
=========

Daisy is a [business writeable] domain specific language for business rules.  It lets business experts write the rules, and software developers define how they are implemented. Daisy itself is domain agnostic, but allows domain experts and software developers to create the rule domain together. 

### Examples:
<pre>
Any User
  Is Active
  Has Account
    Balance is less than 0
    OR Balance is greater than 250000
</pre>
This rule could be used in a banking domain to find all users with accounts outside of the FDIC coverage range

<pre>
All Widgets
    Is Verified
  OR
		NOT Obviously broken
		Looks Ok
</pre>
This rule could be used in a manufacturing domain to assert that all widgets pass a really low quality bar.

Daisy handles the boolean algebra and linking statements to functions. Domain experts write business rules in a language that not only makes sense to them, but they invent. Software engineers translate each statement into code their software understands.

### Why Use Daisy

*  Simple to write, implement and deploy, it's about as lightweight a rules engine you'll ever use
*  Domain adaptive. Daisy will work for nearly any domain you've got, since it let's you define the data model
*  Open source. Openly available well tested source means you can always see what's happening under the covers

### Using Daisy
Daisy is available as a [nuget package]

For Your Domain Experts
------------------------
Daisy can be used to create rules about a domain. For example, a rule could determine if an account is overdrawn, or if a customer engaging with your product.  All rules ultimately end up with a either a 'Yes' or 'No' answer.  In the examples below, I'll use the domain of users on a website, but Daisy could just as well be used with any other domain.

Like tubes to the internet, Daisy is made up of a series of statements. You, the domain expert, can make these statements up!  For example, if part of a rule needs to determine if an user was deactivated, you could write:
<pre>
Is Deactivated
</pre>

Use a NOT before a statement to negate it. This statement would be identical to the one above.
<pre>
NOT Is Active
</pre>

Combine statements by putting each on a separate line with and AND, or OR before it.
<pre>
Is Deactivated
AND Last use is greater than 180 days ago
OR Unsubscribed
</pre>

Statments without an AND or OR before them are implicitly considered to be ANDed with the statement before it. Unlike most programming languages, Daisy treats AND and OR with equal precedence. Thus the above statement would yield 'Yes' if the either the user is deactivated and and hasn't recently used the app or is unsubscribed.  If you want to group statements, use indents:
<pre>
Is Deactivated
OR
    Last use is greater than 180 days ago
    Unsubscribed
</pre>
This statement would yield 'Yes' if either the user is deactivated or hasn't recently used the app and is unsubscribed.

Sometimes your domain model is a little complicated and you'll need statements that break the domain down.
<pre>
Is Deactivated
OR NOT Has more than 6 sessions
    Duration is greater than 5 minutes
    More than 3 key actions
</pre>
This rule would yield 'Yes' if the user is deactivated or has no more than 6 sessions with long durations and many key actions. Statements followed by a other indented statements make those statements operate on a sub-part of the original domain. Another example is a query to find your high-roller users: 
<pre>
All purchases
    Amount greater than 50 dollars
    Fewer than 3 items purchased
</pre>

For Your Developers
-------------------

### In One File
```C#
class Program
    {
        static void Main(string[] args)
        {
            bool result = DaisyCompiler.Compile<Widget>("Is Verified",
                new StatementSet().FromCurrentAssembly())
              .Execute(
                new Widget() { Verifier = new User() { UserName = "Jim" } })
                .Result;
        }

        public class WidgetController : StatementController<Widget>
        {
            public bool IsVerified()
            {
                return Scope.Verifier != null && Scope.Verifier.UserName != "Geroge";
                // George is a pretty crummy verifier, so ignore his work :)
            }
        }

        public class Widget
        {
            public User Verifier { get; set; }  
        }

        public class User
        {
            public string UserName { get; set; }
        }
    }
```
### Explained
The developer's job is to translate statements into coded queries against your data model. This is done by writing StatementControllers, such as the one below:

```C#
public class UserController : StatementController<User>
{
    public bool IsDeactivated()
    {
        return !Scope.Activated || Scope.Expiration <= DateTime.Now;
    }

    [Matches("Last use is greater than (\d+) days ago")]
    public bool LastUseGreaterThan(int days)
    {
        return (Scope.LastUse - DateTime.Now).TotalDays > days;
    }
}
```

A statement controller has a number of methods, each of which maps to a bit of code that can match a line in Daisy. A statement controller also declares a type as a generic arguement. This type is the Scope, or the object that the rule is examining when a statement is called. Each statement may then access the scope through the inherited Scope property. Each public method that returns a bool in a statement controller is eligible to match Daisy lines. It is made up of two parts:

1.  Matching a Daisy line
2.  The method definition

### Matching
Statement controller methods may be annotated with the Matches attribute. This attribute specifies a regular expression that, is a line in the Daisy rule matches, the rule will execute. The regular expression may specify capture groups to be passed as paramters to the method. If there are capture groups, the regular expression must have the same number of captures as paramters in the method, and the method parameters must be either strings or ints.

Methods that do not have a Matches attribute will match if the method name, with spaces between the humps of the camel, matches the Daisly line.  Clearly method that do not provide a Matches attribute cannot have paramters.  All regular expression evaluation is case insensitive.  In the example above, the IsDeactivated method provides no attribute, and so would match either "Is deactivated", or "Is       Deactivated".

### Statement methods
Statement method are executed with paramters as parsed from capture groups of the regular expressions. They must return a boolean, and have access to the scope via the inherited Scope variable.

### Changing scope
Some statement methods need to alter the scope, to allow groups to execute on sub-objects. For example, in this rule: 
```C#
All purchases
    Amount greater than 50 dollars
    Fewer than 3 items purchased
```

whatever statement method implements the All purchases line would need to to be able to iterate over every purchase, invoke the sub-statements on each purchase, and determine if all of them match the sub-statements.  A method can do this by requesting a proceed function that runs sub-statements, as below: 

```C#
public class UserController : StatementController<User>
{
    public bool AllPurchases(Func<Purchase,bool> proceed)
    {
        return Scope.Purchases.All(x => proceed(x));
    }
}
```
The Daisy runtime will handle calling sub-statements when the proceed function is invoked. Notice that the proceed function calls requires a single paramter, which will be the new scope object for the sub rules.

### Invoking Daisy
Daisy programs can be compiled by calling DaisyCompiler.Compile. This method requires:

1.  The type of the initial scope for a rule
2.  The Daisy code
3.  A StatementSet, which is the set of controllers/methods

The following example will compile a Daisy rule using all rules found in the current assembly:
```C#
var daisyProgram = DaisyCompiler.Compile<User>(@"Is Deactivated
OR
    Last use is greater than 180 days ago
    Unsubscribed", new StatementSet().FromCurrentAssembly());
```

Once a Daisy program has been compiled, it can by it's execute method, and the result gotten via the Result property on the resulting execution object:
```C#
daisyProgram.Execute(initialScopeValue).Result;
```

### Context
Some rules may require more input than a single scope value. Context allows sending more values via a ContextBundle, which is essentially a Dictionary. Pass it in as the second value of the execute method, and statement methods can access via the inherited Context property.
```C#
var context = new ContextBundle();
context["family"] = new Family();
DaisyCompiler.Compile<Person>(myRule,myStatementSet)
    .Execute(new Person(), context);
    
public class PersonController : StatementController<Person> {
  public void IsInFamily() {
    return Context.Get<Family>("family").IsMember(Scope);
  }
}
```

### Attachments
Some rules may require more output that just a boolean. Attachments allow returning more values. Statements may add results to the Attachments object (essentially a Dictionary) and the results are available in the excution.
```C#
    
public class WidgetSetController : StatementController<WidgetSet> {
  public void AnyWidget(Func<Widget,bool> proceed) {
    foreach(var widget in Scope) {
      if(proceed(widget)) {
        Attachments["selected"] = widget;
        return true;
      }
    }
    return false;
  }
  ...
}

var execution = DaisyCompiler.Compile<Widger>.Compile(
@"Any Widget
  Is Broken
  OR Is Expired", myStatementSet)
.Execute()

var selectedWidget = execution.Attachments.Get<Widget>("selected");
```



License
-------
MIT

 [business writeable]: http://martinfowler.com/bliki/BusinessReadableDSL.html
 [nuget package]: https://www.nuget.org/packages/Ancestry.Daisy/
