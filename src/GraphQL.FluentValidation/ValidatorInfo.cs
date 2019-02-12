using System.Collections.Generic;
using FluentValidation;

class ValidatorInfo
{
    public bool HasValidators;
    public List<IValidator> Validators;
}