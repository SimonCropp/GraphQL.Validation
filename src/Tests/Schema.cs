public class Schema :
    GraphQL.Types.Schema
{
    public Schema()
    {
        Query = new Query();
    }
}