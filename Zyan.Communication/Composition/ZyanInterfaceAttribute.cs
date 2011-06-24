﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace Zyan.Communication.Composition
{
	/// <summary>
	/// Specifies that all classes implementing the interface are Zyan components
	/// </summary>
	[MetadataAttribute, AttributeUsage(AttributeTargets.Interface, Inherited = true)]
	public class ZyanInterfaceAttribute : InheritedExportAttribute
	{
		/// <summary>
		/// Initializes ZyanInterfaceAttribute instance.
		/// </summary>
		/// <param name="componentInterface">Interface type attached to ZyanComponent</param>
		public ZyanInterfaceAttribute(Type componentInterface)
			: base(componentInterface)
		{
			Initialize(componentInterface);
		}

		/// <summary>
		/// Initializes ZyanInterfaceAttribute instance.
		/// </summary>
		/// <param name="uniqueName">Unique name of the ZyanComponent</param>
		/// <param name="componentInterface">Interface type attached to ZyanComponent</param>
		public ZyanInterfaceAttribute(string uniqueName, Type componentInterface)
			: base(uniqueName, componentInterface)
		{
			Initialize(componentInterface);
		}

		private void Initialize(Type componentInterface)
		{
			if (!componentInterface.IsInterface)
			{
				throw new InvalidOperationException("Interface type required: " + componentInterface);
			}

			ComponentInterface = componentInterface;
			IsPublished = true;
		}

		public Type ComponentInterface { get; private set; }

		public bool IsPublished { get; set; }
	}
}