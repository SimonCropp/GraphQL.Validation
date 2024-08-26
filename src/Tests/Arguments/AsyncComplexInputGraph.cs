public class AsyncComplexInputGraph :
    InputObjectGraphType
{
    public AsyncComplexInputGraph()
    {
        Field<ComplexInputInnerGraph>("inner");

        Field<ListGraphType<NonNullGraphType<ComplexInputListItemGraph>>>("items");
    }
}