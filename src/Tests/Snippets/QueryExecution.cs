using System.Reflection;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.FluentValidation;

class QueryExecution
{
    async Task ExecuteQuery(string queryString, Inputs inputs)
    {
        #region UseFluentValidation

        using (var schema = new Schema())
        {
            var executer = new DocumentExecuter();

            var options = new ExecutionOptions
            {
                Schema = schema,
                Query = queryString,
                UserContext = new MyUserContext(),
                Inputs = inputs
            };
            options.UseFluentValidation();

            var executionResult = await executer.ExecuteAsync(options)
                .ConfigureAwait(false);
        }

        #endregion
    }

    void AddValidators(Assembly assemblyContainingValidators)
    {
        #region AddValidators
        ValidatorTypeCache.AddValidatorsFromAssembly(assemblyContainingValidators);
        #endregion
    }
}