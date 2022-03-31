using System;

namespace SOTS
{
	public static class SOTSUtils
	{
		public static string GetPath<T>()
		{
			return GetPath(typeof(T));
		}
		public static string GetPath<T>(string extra)
		{
			return GetPath(typeof(T), extra);
		}
		public static string GetPath(this object o)
		{
			return GetPath(o.GetType());
		}
		public static string GetPath(this object o, string extra)
		{
			return GetPath(o.GetType(), extra);
		}
		public static string GetPath(Type t)
		{
			return t.Namespace.Replace('.', '/') + "/" + t.Name;
		}
		public static string GetPath(Type t, string extra)
		{
			return GetPath(t) + extra;
		}
	}
}