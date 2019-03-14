using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.FluentValidation;
#pragma warning disable 649

class QueryExecution
{
    string queryString;
    Inputs inputs;
    Schema schema;
    ValidatorTypeCache validatorTypeCache;
    DocumentExecuter executer;

    void ExecuteQuery(Assembly assemblyContainingValidators)
    {
        #region StartConfig

        var validatorTypeCache = new ValidatorTypeCache();
        validatorTypeCache.AddValidatorsFromAssembly(assemblyContainingValidators);
        var schema = new Schema();
        var executer = new DocumentExecuter();

        #endregion
    }

    async Task ExecuteQuery()
    {
        #region UseFluentValidation

        var options = new ExecutionOptions
        {
            Schema = schema,
            Query = queryString,
            Inputs = inputs
        };
        options.UseFluentValidation(validatorTypeCache);

        var executionResult = await executer.ExecuteAsync(options);

        #endregion
    }

    #region ContextImplementingDictionary

    public class MyUserContext :
        Dictionary<string, object>
    {
        public string MyProperty { get; set; }
    }

    #endregion

    void ExecuteQueryWithContextImplementingDictionary()
    {
        #region ExecuteQueryWithContextImplementingDictionary

        var options = new ExecutionOptions
        {
            Schema = schema,
            Query = queryString,
            Inputs = inputs,
            UserContext = new MyUserContext
            {
                MyProperty = "the value"
            }
        };
        options.UseFluentValidation(validatorTypeCache);

        #endregion
    }

    void ExecuteQueryWithContextInsideDictionary()
    {
        #region ExecuteQueryWithContextInsideDictionary

        var options = new ExecutionOptions
        {
            Schema = schema,
            Query = queryString,
            Inputs = inputs,
            UserContext = new Dictionary<string, object>
            {
                {
                    "MyUserContext",
                    new MyUserContext
                    {
                        MyProperty = "the value"
                    }
                }
            }
        };
        options.UseFluentValidation(validatorTypeCache);

        #endregion
    }

    void NoContext()
    {
        #region NoContext

        var options = new ExecutionOptions
        {
            Schema = schema,
            Query = queryString,
            Inputs = inputs
        };
        options.UseFluentValidation(validatorTypeCache);

        #endregion
    }
}