using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

public class Singleton<T> where T : class, new()
{
    private static readonly Lazy<T> instance = new(() => new T());
    public static T Instance => instance.Value;
}
