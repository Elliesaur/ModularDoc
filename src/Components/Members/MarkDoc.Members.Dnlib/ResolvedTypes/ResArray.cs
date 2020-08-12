﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using MarkDoc.Members.ResolvedTypes;
using MarkDoc.Members.Types;

namespace MarkDoc.Members.Dnlib.ResolvedTypes
{
  /// <summary>
  /// Class for representing resolved arrays
  /// </summary>
  [DebuggerDisplay(nameof(ResArray) + ": {" + nameof(DisplayName) + "}")]
  public class ResArray
    : IResArray, IResType
  {
    #region Properties

    /// <summary>
    /// Type resolver
    /// </summary>
    private IResolver Resolver { get; }

    /// <inheritdoc />
    public IResType ArrayType { get; }

    /// <inheritdoc />
    public bool IsJagged { get; }

    /// <inheritdoc />
    public int Dimension { get; }

    /// <inheritdoc />
    public string DisplayName
      => ArrayType.DisplayName;

    /// <inheritdoc />
    public string DocumentationName
    {
      get
      {
        if (IsJagged)
          return ArrayType.DocumentationName + string.Join(string.Empty, Enumerable.Repeat("[]", Dimension));
        return ArrayType.DocumentationName + $"[{string.Concat(Enumerable.Repeat(",", Dimension - 1))}]";
      }
    }

    /// <inheritdoc />
    public string TypeNamespace
      => ArrayType.TypeNamespace;

    /// <inheritdoc />
    public Lazy<IType?> Reference { get; }

    /// <inheritdoc />
    public string RawName { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="generics">List of known generics</param>
    internal ResArray(IResolver resolver, dnlib.DotNet.TypeSig source, IReadOnlyDictionary<string, string>? generics)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));
      // If the resolver is null..
      if (resolver is null)
        // throw an exception
        throw new ArgumentNullException(nameof(resolver));

      Resolver = resolver;
      RawName = source.FullName;
      IsJagged = source.ElementType == dnlib.DotNet.ElementType.SZArray;

      var arrayType = ResolveArrayType(source, IsJagged);
      ArrayType = Resolver.Resolve(arrayType, generics);
      Dimension = ResolveDimension(source, arrayType);
      Reference = new Lazy<IType?>(() => Resolver.FindReference(source, this), LazyThreadSafetyMode.PublicationOnly);
    }

    #region Methods

    /// <summary>
    /// Strip away the array braces to find the array type
    /// </summary>
    /// <param name="source">Array to process</param>
    /// <param name="isJagged">Is the provided <paramref name="source"/> a jagged array</param>
    /// <returns>Array type</returns>
    private static dnlib.DotNet.TypeSig ResolveArrayType(dnlib.DotNet.TypeSig source, bool isJagged)
    {
      // Get the array
      dnlib.DotNet.TypeSig current = source;
      // Get the array with one level lower braces
      dnlib.DotNet.TypeSig? next = source.Next;

      // While the next array still has braces..
      while (next?.ElementType == (isJagged ? dnlib.DotNet.ElementType.SZArray : dnlib.DotNet.ElementType.Array))
      {
        // Track the next level
        current = next;
        // Strip another level of braces
        next = current.Next;
      }

      // Return the result
      return next ?? current;
    }

    private int ResolveDimension(dnlib.DotNet.TypeSig source, dnlib.DotNet.TypeSig array)
    {
      static int Count(ReadOnlySpan<char> span, char find)
      {
        var i = 0;
        foreach (var c in span)
          if (c.Equals(find))
            ++i;

        return i;
      }

      // Get the array fullname
      var thisType = source.FullName;
      // Get the array type name
      var arrayType = array.FullName;

      // Subtract the names to get the array braces
      var braces = thisType.AsSpan(arrayType.Length);

      // Return the dimension of the array based on the braces count
      return IsJagged
        ? Count(braces, '[')
        : Count(braces, ',') + 1;
    }

    #endregion
  }
}
