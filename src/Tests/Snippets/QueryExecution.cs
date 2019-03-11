using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.FluentValidation;

class QueryExecution
{
    void ExecuteQuery(Assembly assemblyContainingValidators)
    {
        #region StartConfig

        var validatorTypeCache = new ValidatorTypeCache();
        validatorTypeCache.AddValidatorsFromAssembly(assemblyContainingValidators);
        var schema = new Schema();
        var executer = new DocumentExecuter();

        #endregion
    }

    async Task ExecuteQuery(string queryString, Inputs inputs, Schema schema, ValidatorTypeCache validatorTypeCache, DocumentExecuter executer)
    {
        #region UseFluentValidation

        var options = new ExecutionOptions
        {
            Schema = schema,
            Query = queryString,
            UserContext = new Dictionary<string, object>
            {
                {"MyContext", new MyUserContext()}
            },
            Inputs = inputs
        };
        options.UseFluentValidation(validatorTypeCache);

        var executionResult = await executer.ExecuteAsync(options);

        #endregion
    }
}