﻿public class ResultGraph :
    ObjectGraphType<Result>
{
    public ResultGraph() =>
        Field(h => h.Data);
}