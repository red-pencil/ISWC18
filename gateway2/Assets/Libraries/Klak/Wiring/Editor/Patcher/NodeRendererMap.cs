using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Klak.Wiring.Patcher;

public class NodeRendererAttribute:Attribute
{
	public NodeRendererAttribute (Type baseType)
	{
		BaseType = baseType;
	}
	public Type BaseType {
		set;
		get;
	}
}

public static class NodeRendererMap {


	static Dictionary<Type,Type> _typeMap=new Dictionary<Type, Type>();
	static bool _enumed=false;

	static void _EnumTypes()
	{

		// Scan all assemblies in the current domain.
		foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
		{
			// Scan all types in the assembly.
			foreach(var type in assembly.GetTypes())
			{
				if (!(typeof(Node).IsAssignableFrom(type) ))
					continue;
				
				// Retrieve AddComponentMenu attributes.
				var attr = type.GetCustomAttributes(typeof(NodeRendererAttribute), true);
				if (attr.Length == 0) continue;

				_typeMap [((NodeRendererAttribute)attr[0]).BaseType] = type;
				Debug.Log ("Adding Node:"+type.ToString());
			}
		}
		_enumed = true;
	}

	public static bool AddType(Type obj,Type renderer)
	{
		if (!renderer.IsAssignableFrom (typeof(Node)))
			return false;

		_typeMap [obj] = renderer;
		return true;
	}

	public static Type GetRenderer(Type obj)
	{
		
		if (!_enumed)
			_EnumTypes ();
		if (_typeMap.ContainsKey (obj))
			return _typeMap [obj];
		return typeof(Node);
	}
}
