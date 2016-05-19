﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GUC_Attendance
{
	public class MinLengthValidatorBehavior : Behavior<Entry> 
	{
		// Creating BindableProperties with Limited write access: http://iosapi.xamarin.com/index.aspx?link=M%3AXamarin.Forms.BindableObject.SetValue(Xamarin.Forms.BindablePropertyKey%2CSystem.Object) 

		static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(NumberValidatorBehavior), false);

		public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

		public bool IsValid
		{
			get { return (bool)base.GetValue(IsValidProperty); }
			private set { base.SetValue(IsValidPropertyKey, value); }
		}

		protected override void OnAttachedTo(Entry bindable)
		{
			bindable.TextChanged += bindable_TextChanged;
		}

		private void bindable_TextChanged(object sender, TextChangedEventArgs e)
		{
			IsValid = (e.NewTextValue.Length > 7);
			((Entry)sender).TextColor = IsValid ? Color.Default : Color.FromHex ("#e75d5d");

		}

		protected override void OnDetachingFrom(Entry bindable)
		{
			bindable.TextChanged -= bindable_TextChanged;
		}
	}
}