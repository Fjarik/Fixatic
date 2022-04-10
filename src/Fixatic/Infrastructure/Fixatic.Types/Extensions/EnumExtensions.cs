using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fixatic.Types.Extensions
{
	public static class EnumExtensions
	{
		public static string GetName<T>(this T enumerationValue) where T : struct
		{
			var type = enumerationValue.GetType();
			if (type == null || !type.IsEnum)
			{
				throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumerationValue));
			}

			//Tries to find a DescriptionAttribute for a potential friendly name for the enum
			var memberInfo = type.GetMember(enumerationValue.ToString());
			if (memberInfo != null && memberInfo.Length > 0)
			{
				var attrs = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
				if (attrs != null && attrs.Length > 0)
				{
					//Pull out the description value
					return ((DisplayAttribute)attrs[0]).Name ?? string.Empty;
				}
			}
			//If we have no description attribute, just return the ToString of the enum
			return enumerationValue.ToString() ?? string.Empty;
		}

		public static string GetDescription<T>(this T enumerationValue) where T : struct
		{
			var type = enumerationValue.GetType();
			if (type == null || !type.IsEnum)
			{
				throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumerationValue));
			}

			//Tries to find a DescriptionAttribute for a potential friendly name for the enum
			var memberInfo = type.GetMember(enumerationValue.ToString());
			if (memberInfo != null && memberInfo.Length > 0)
			{
				var attrs = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
				if (attrs != null && attrs.Length > 0)
				{
					//Pull out the description value
					return ((DisplayAttribute)attrs[0]).Description ?? string.Empty;
				}
			}
			//If we have no description attribute, just return the ToString of the enum
			return enumerationValue.ToString() ?? string.Empty;
		}
	}
}
