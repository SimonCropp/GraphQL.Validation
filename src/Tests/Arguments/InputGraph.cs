﻿public class InputGraph :
    InputObjectGraphType
{
    public InputGraph() =>
        Field<StringGraphType>("content");
}