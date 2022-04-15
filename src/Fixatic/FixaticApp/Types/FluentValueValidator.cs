﻿using FluentValidation;

namespace FixaticApp.Types
{

	/// <summary>
	/// A glue class to make it easy to define validation rules for single values using FluentValidation
	/// </summary>
	public class FluentValueValidator<T> : AbstractValidator<T>
	{
		public FluentValueValidator(Action<IRuleBuilderInitial<T, T>> rule)
		{
			rule(RuleFor(x => x));
		}

		private IEnumerable<string> ValidateValue(T arg)
		{
			var result = Validate(arg);
			if (result.IsValid)
				return new string[0];
			return result.Errors.Select(e => e.ErrorMessage);
		}

		public Func<T, IEnumerable<string>> Validation => ValidateValue;
	}
}