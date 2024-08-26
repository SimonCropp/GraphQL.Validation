public class AsyncInputGraph :
    InputObjectGraphType
{
    public AsyncInputGraph() =>
        Field<StringGraphType>("content");
}