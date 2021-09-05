using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.FluentValidation;
#pragma warning disable 649

class QueryExecution
{
    string queryString = null!;
    Inputs inputs = null!;
    Schema schema = null!;
    ValidatorInstanceCache validatorCache = null!;
    DocumentExecuter executer = null!;

    void ExecuteQuery(Assembly assemblyContainingValidators)
    {
        #region StartConfig

        ValidatorInstanceCache validatorCache = new();
        validatorCache.AddValidatorsFromAssembly(assemblyContainingValidators);
        Schema schema = new();
        schema.UseFluentValidation();
        DocumentExecuter executer = new();

        #endregion
    }

    async Task ExecuteQuery()
    {
        #region UseFluentValidation

        ExecutionOptions options = new()
        {
            Schema = schema,
            Query = queryString,
            Inputs = inputs
        };
        options.UseFluentValidation(validatorCache);

        var executionResult = await executer.ExecuteAsync(options);

        #endregion
    }

    #region ContextImplementingDictionary

    public class MyUserContext :
        Dictionary<string, object?>
    {
        public MyUserContext(string myProperty)
        {
            MyProperty = myProperty;
        }

        public string MyProperty { get; }
    }

    #endregion

    void ExecuteQueryWithContextImplementingDictionary()
    {
        #region ExecuteQueryWithContextImplementingDictionary

        ExecutionOptions options = new()
        {
            Schema = schema,
            Query = queryString,
            Inputs = inputs,
            UserContext = new MyUserContext
            (
                myProperty: "the value"
            )
        };
        options.UseFluentValidation(validatorCache);

        #endregion
    }

    void ExecuteQueryWithContextInsideDictionary()
    {
        #region ExecuteQueryWithContextInsideDictionary

        ExecutionOptions options = new()
        {
            Schema = schema,
            Query = queryString,
            Inputs = inputs,
            UserContext = new Dictionary<string, object?>
            {
                {
                    "MyUserContext",
                    new MyUserContext
                    (
                        myProperty: "the value"
                    )
                }
            }
        };
        options.UseFluentValidation(validatorCache);

        #endregion
    }

    void NoContext()
    {
        #region NoContext

        ExecutionOptions options = new()
        {
            Schema = schema,
            Query = queryString,
            Inputs = inputs
        };
        options.UseFluentValidation(validatorCache);

        #endregion
    }
}